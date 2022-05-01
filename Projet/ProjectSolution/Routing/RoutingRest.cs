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
    public class RoutingRest : IRoutingRest
    {
        private static string openRouteServiceKey = "5b3ce3597851110001cf624837f5baa075ce448bbee78662285a971d";
        private static string openRouteServiceBase = "https://api.openrouteservice.org/";
        private HttpClient httpClient = new HttpClient();
        private ProxyReference.JCDecauxDataClient proxyClient = new ProxyReference.JCDecauxDataClient();

        public async Task<ResultObject> ComputePath(string start, string end)
        {
            Console.WriteLine("From " + start + " to " + end);
            string startUrl = openRouteServiceBase + "geocode/search?api_key=" + openRouteServiceKey + "&text=" + start + "&size=1";
            string endUrl = openRouteServiceBase + "geocode/search?api_key=" + openRouteServiceKey + "&text=" + end + "&size=1";

            Position footStartPosition = await stringToPosition(startUrl);
            Position footEndPosition = await stringToPosition(endUrl);

            
            Station stationStart = findClosestStation(footStartPosition, false);
            Station stationEnd = findClosestStation(footEndPosition, true);
            Console.WriteLine("Closest station from the start : " + stationStart.ToString());
            Console.WriteLine("Closest station from the end : " + stationEnd.ToString());

            (PositionInstruction footToBikeList, double footToBikeDistance) = await footToBike(footStartPosition, stationStart.position);
            PositionInstruction bikeToBike = await stationToStation(stationStart.position, stationEnd.position);
            (PositionInstruction bikeToFootList, double bikeToFootListDistance) = await BikeToFoot(stationEnd.position, footEndPosition);
            
            if(footToBikeList.positions.Count < 1 || bikeToBike.positions.Count < 1 || bikeToFootList.positions.Count < 1)
            {
                Console.WriteLine("Problem for the request "+start + " to " + end+", a piece of the path is inexisting !");
            }

            ResultObject result = await computeList(footToBikeList, bikeToBike, bikeToFootList, footToBikeDistance, bikeToFootListDistance);
            result.startPosition = footStartPosition;

            return result;
        }

        private async Task<(PositionInstruction, double)> footToBike(Position startPos, Position destination)
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
            PositionInstruction result = new PositionInstruction();
            //double time = 0;
            double distance = 0;
            if (response.IsSuccessStatusCode)
            {
                var response_content = await response.Content.ReadAsStringAsync();
                geoJSON = JsonSerializer.Deserialize<GeoJSON>(response_content);
                //time += geoJSON.features[0].properties.summary.duration;
                Feature feature = geoJSON.features[0];
                List<Position> positions = new List<Position>();
                distance = feature.properties.summary.distance;
              
                double[][] coordinates = geoJSON.features[0].geometry.coordinates;
                for (int i = 0; i < coordinates.Length; i++)
                {
                    positions.Add(fromGeoCoordinate(coordinates[i]));
                }
                result.instructions = feature.properties.segments[0].steps;  
                result.positions = positions;
            }
            else
            {
                throw new Exception("footToBike error in the request !");
            }

            return (result,  distance);
        }

        private async Task<PositionInstruction> stationToStation(Position startPos, Position destination)
        {
            
            string api = "v2/directions/";
            string transportType = "cycling-regular";
            string extra = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", startPos.ToString());
            parameters.Add("end", destination.ToString());
            string url = urlCreation(api, transportType, extra, parameters);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            GeoJSON geoJSON = null;
            PositionInstruction result = new PositionInstruction();
            //double time = 0;
            double distance = 0;
            if (response.IsSuccessStatusCode)
            {
                var response_content = await response.Content.ReadAsStringAsync();
                geoJSON = JsonSerializer.Deserialize<GeoJSON>(response_content);
                //time += geoJSON.features[0].properties.summary.duration;
                Feature feature = geoJSON.features[0];
                List<Position> positions = new List<Position>();
                distance = feature.properties.summary.distance;

                double[][] coordinates = geoJSON.features[0].geometry.coordinates;
                for (int i = 0; i < coordinates.Length; i++)
                {
                    positions.Add(fromGeoCoordinate(coordinates[i]));
                }
                result.instructions = feature.properties.segments[0].steps;
                result.positions = positions;
            }
            else
            {
                throw new Exception("stationToStation error in the request !");
            }

            return result;
        }

        private async Task<(PositionInstruction, double)> BikeToFoot(Position startPos, Position destination)
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
            PositionInstruction result = new PositionInstruction();
            //double time = 0;
            double distance = 0;
            if (response.IsSuccessStatusCode)
            {
                var response_content = await response.Content.ReadAsStringAsync();
                geoJSON = JsonSerializer.Deserialize<GeoJSON>(response_content);
                //time += geoJSON.features[0].properties.summary.duration;
                Feature feature = geoJSON.features[0];
                List<Position> positions = new List<Position>();
                distance = feature.properties.summary.distance;

                double[][] coordinates = geoJSON.features[0].geometry.coordinates;
                for (int i = 0; i < coordinates.Length; i++)
                {
                    positions.Add(fromGeoCoordinate(coordinates[i]));
                }
                result.instructions = feature.properties.segments[0].steps;
                result.positions = positions;
            }
            else
            {
                throw new Exception("bikeToFoot error in the request !");
            }

            return (result, distance);
        }

        private async Task<ResultObject> computeList(PositionInstruction first, PositionInstruction second, PositionInstruction third, double distanceFirst, double distanceSecond)
        { 
            //Check if taking a bike is worth than walking : check for the smallest distance amount to walk
            ResultObject result = new ResultObject();
            (bool worth, PositionInstruction directFoot) = await worthIt(first.positions, second.positions, third.positions, distanceFirst, distanceSecond);
            result.worthIt = worth;
            result.footToFoot = directFoot;
            result.footToStation = first;
            result.stationToSation = second;
            result.sationToFoot = third;
            return result;
        }

        private async Task<(bool, PositionInstruction)> worthIt(List<Position> first, List<Position> second, List<Position> third, double distanceFirst, double distanceSecond)
        {
            //Plutôt comparer la distance de marche entre les 2
            PositionInstruction positionInstruction = new PositionInstruction();
            string api = "v2/directions/";
            string transportType = "foot-walking";
            string extra = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", first[0].ToString());
            parameters.Add("end", third[third.Count-1].ToString());
            string url = urlCreation(api, transportType, extra, parameters);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            //HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Error request for api.openrouteservice.org !");
            }
            GeoJSON geoJSON = JsonSerializer.Deserialize<GeoJSON>(await response.Content.ReadAsStringAsync());
            Feature feature = geoJSON.features[0];
            List<Position> positions = new List<Position>();
            double distanceDirect = feature.properties.summary.distance;
            bool worth = distanceFirst + distanceSecond < distanceDirect;
            
            double[][] coordinates = feature.geometry.coordinates;
            for (int i = 0; i < coordinates.Length; i++)
            {
                positions.Add(fromGeoCoordinate(coordinates[i]));
            }
            positionInstruction.instructions = feature.properties.segments[0].steps;
            positionInstruction.positions = positions;

            return (worth, positionInstruction);
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
                if ((takeABike && updateStation.mainStands.availabilities.bikes > 0) ||
                    !takeABike && updateStation.mainStands.availabilities.stands > 0)
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
                GeoJSONSearch geo = JsonSerializer.Deserialize<GeoJSONSearch>(await response.Content.ReadAsStringAsync());
                //GeoJSONSearch geo = JsonSerializer.Deserialize<GeoJSONSearch>(response.Content.ReadAsStringAsync().Result);

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
