using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using ClassLibrary;
using System.Threading.Tasks;

namespace Routing
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Routing : IRouting
    {
        private static string openRouteServiceKey = "5b3ce3597851110001cf624837f5baa075ce448bbee78662285a971d";
        private static string openRouteServiceBase = "https://api.openrouteservice.org/";
        private HttpClient httpClient = new HttpClient();
        private ProxyReference.JCDecauxDataClient proxyClient = new ProxyReference.JCDecauxDataClient();

        public async Task<List<Position>> ComputePath(string start, string end)
        {
            string startUrl = openRouteServiceBase + "geocode/search?api_key=" + openRouteServiceKey + "&text=" + start + "&size=1";
            string endUrl = openRouteServiceBase + "geocode/search?api_key=" + openRouteServiceKey + "&text=" + end + "&size=1";

            Position startPos = await stringToPosition(startUrl);
            Position endPos = await stringToPosition(endUrl);

            Station starting = findClosestStation(startPos, false);
            Station ending = findClosestStation(endPos, true);
            var affiche = "startPos = " + startPos.ToString() + ", station pos = " + starting.position.ToString();
            List<Position> list = new List<Position>();
            Position indexStartEnd = null;
            list.Add(indexStartEnd);
            list.Add(startPos);


            (List<Position> first, double distanceFirst) = await footToBike(startPos, starting.position);
            List<Position> second = await stationToStation(starting.position, ending.position);
            (List<Position> third, double distanceSecond) = await BikeToFoot(ending.position, endPos);
            
            await computeList(list, first, second, third, distanceFirst, distanceSecond);

            return list;
        }

        private async Task<(List<Position>, double)> footToBike(Position startPos, Position destination)
        {
            string api = "v2/directions/";
            string transportType = "foot-walking";
            string extra = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", startPos.ToString());
            parameters.Add("end", destination.ToString());
            string url = urlCreation(api, transportType, extra, parameters);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            GeoJSON geoJSON = null;
            List<Position> listPos = new List<Position>();
            //double time = 0;
            double distance = 0;
            if (response.IsSuccessStatusCode)
            {
                var response_content = await response.Content.ReadAsStringAsync();
                geoJSON = JsonSerializer.Deserialize<GeoJSON>(response_content);
                //time += geoJSON.features[0].properties.summary.duration;
                distance = geoJSON.features[0].properties.summary.distance;
                Coordinate[] coordinates = geoJSON.features[0].geometry.coordinates;
                for (int i = 0; i < coordinates.Length; i++)
                {
                    listPos.Add(fromGeoCoordinate(coordinates[i].values));
                }
            }
            else
            {
                Console.WriteLine("ERROR LORS DE LA REQUÊTE !");
            }

            return (listPos,  distance);
        }

        private async Task<List<Position>> stationToStation(Position startPos, Position destination)
        {
            
            string api = "v2/directions/";
            string transportType = "cycling-regular";
            string extra = "/geojson";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", startPos.ToString());
            parameters.Add("end", destination.ToString());
            string url = urlCreation(api, transportType, extra, parameters);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            GeoJSON geoJSON = null;
            List<Position> listPos = new List<Position>();
            if (response.IsSuccessStatusCode)
            {
                geoJSON = JsonSerializer.Deserialize<GeoJSON>(await response.Content.ReadAsStringAsync());
                Coordinate[] coordinates = geoJSON.features[0].geometry.coordinates;
                for (int i = 0; i < coordinates.Length; i++)
                {
                    listPos.Add(fromGeoCoordinate(coordinates[i].values));
                }
            }
            else
            {
                Console.WriteLine("ERROR LORS DE LA REQUÊTE !");
            }

            return listPos;
        }

        private async Task<(List<Position>, double)> BikeToFoot(Position startPos, Position destination)
        {
            string api = "v2/directions/";
            string transportType = "foot-walking";
            string extra = "/geojson";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", startPos.ToString());
            parameters.Add("end", destination.ToString());
            string url = urlCreation(api, transportType, extra, parameters);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            GeoJSON geoJSON = null;
            List<Position> listPos = new List<Position>();
            double distance = 0;
            if (response.IsSuccessStatusCode)
            {
                geoJSON = JsonSerializer.Deserialize<GeoJSON>(await response.Content.ReadAsStringAsync());
                distance = geoJSON.features[0].properties.summary.distance;
                Coordinate[] coordinates = geoJSON.features[0].geometry.coordinates;
                for (int i = 0; i < coordinates.Length; i++)
                {
                    listPos.Add(fromGeoCoordinate(coordinates[i].values));
                }
            }
            else
            {
                Console.WriteLine("ERROR LORS DE LA REQUÊTE !");
            }

            return (listPos, distance);
        }

        private async Task computeList(List<Position> list, List<Position> first, List<Position> second, List<Position> third, double distanceFirst, double distanceSecond)
        { 
            Position index = list[0];
            //Check if taking a bike is worth than walking : check for the smallest distance amount to walk
            if (await worthIt(first, second, third, distanceFirst, distanceSecond))
            {
                index.latitude = first.Count;
                index.longitude = first.Count + second.Count;
                list.AddRange(first);
                list.AddRange(second);
                list.AddRange(third);
            }
            else
            {
                index.latitude = first.Count + third.Count;
                index.longitude = first.Count + third.Count;
                list.AddRange(first);
                list.AddRange(third);
            }
        }

        private async Task<bool> worthIt(List<Position> first, List<Position> second, List<Position> third, double distanceFirst, double distanceSecond)
        {
            //Plutôt comparer la distance de marche entre les 2
            string api = "v2/directions/";
            string transportType = "foot-walking";
            string extra = "/geojson";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", first[0].ToString());
            parameters.Add("end", third[third.Count-1].ToString());
            string url = urlCreation(api, transportType, extra, parameters);

            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Error request for api.openrouteservice.org !");
            }
            GeoJSON geo = JsonSerializer.Deserialize<GeoJSON>(await response.Content.ReadAsStringAsync());
            double distanceDirect = geo.features[0].properties.summary.distance;
          
            return distanceFirst + distanceSecond < distanceDirect;
        }

        private Station findClosestStation(Position startPos, bool takeABike)
        {
            Station[] stations = GetStations();
            if (stations == null)
            {
                throw new Exception("Null in findClosestStation ! ");
            }
            Station result = null;
            SortedDictionary<double, Station> dic = new SortedDictionary<double, Station>();
            foreach (Station station in stations)
            {
                if(station == null)
                {
                    throw new Exception("Null station !");
                }
                try
                {
                    dic.Add(distance(startPos, station.position), station);
                }
                catch (Exception ex)
                {
                    //Station already existing 
                }
            }
            //TODO : TRIER PAR TEMPS POUR ALLER À LA STATION (PAS À VOL D'OISEAU)
            foreach (Station station in dic.Values)
            {
                if(station.name == null || station.contractName == null)
                {
                    throw new Exception("NAME "+station.ToString());
                }
                Station updateStation = proxyClient.GetAStation(station.name, station.contractName);
                if (updateStation == null)
                {
                    throw new Exception("Null in findClosestStation ! ");
                }
                if ((takeABike && updateStation.totalStands.availabilities.bikes > 0) ||
                    !takeABike && updateStation.totalStands.availabilities.stands > 0)
                {
                    result = updateStation;
                    break;
                }                
            }
            return result;
        }
        private async Task<Position> stringToPosition(string adress)
        {
            Position position = new Position();

            HttpResponseMessage response = await httpClient.GetAsync(adress);

            if (response.IsSuccessStatusCode) {
                GeoJSONSearch geo = JsonSerializer.Deserialize<GeoJSONSearch>(response.Content.ReadAsStringAsync().Result);

                position.latitude = geo.features[0].geometry.coordinates[1];
                position.longitude = geo.features[0].geometry.coordinates[0];
            }
            else
            {
                Console.WriteLine("ERROR LORS DE LA REQUÊTE !");
            }
            return position;
        }

        private Position fromGeoCoordinate(double[] coordinates)
        {
            Position position = new Position();
            position.latitude = coordinates[1];
            position.longitude = coordinates[0];
            return position;
        }
        private Station CloserStationFrom(Position position)
        {
            Station[] list = GetStations();
            if(list == null)
            {
                throw new Exception("Null in CloserStationFrom ! ");
            }
            Station closest = list[0];
            foreach(Station station in list)
            {
                if(distance(position, closest.position) > distance(position, station.position))
                {
                    closest = station;
                }
            }
            return closest;
        }        

        private Station[] GetStations()
        {
            return proxyClient.GetStations();
        }

        private double distance(Position goalPos, Position stationPos)
        {
            return Math.Sqrt(Math.Pow(goalPos.longitude-stationPos.longitude,2) + Math.Pow(goalPos.latitude - stationPos.latitude, 2));
        }

        private string urlCreation(string serviceName, string transportType, string extra, Dictionary<string,string> parameters)
        {
            string url = openRouteServiceBase + serviceName + transportType + extra + "?api_key=" + openRouteServiceKey;
            foreach(KeyValuePair<string,string> pair in parameters)
            {
                url += "&"+pair.Key+"="+pair.Value;
            }
            return url;
        }
    }
}
