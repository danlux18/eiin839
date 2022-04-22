using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proxy
{
    public class JCDecauxData : IJCDecauxData
    {
        ProxyCache<JCDecauxItem> cache = new ProxyCache<JCDecauxItem>(JCDecauxItem.creationAsync);
        public async Task<List<Station>> GetStations()
        {
            JCDecauxItem jCDecauxItem = await cache.Get("AllStations");
            return (List<Station>) jCDecauxItem.item;
        }

        public async Task<List<Station>> GetStationsFromAContract(string contractName)
        {
            if(contractName == null)
            {
                throw new ArgumentNullException(nameof(contractName));
            }
            JCDecauxItem jCDecauxItem = await cache.Get(contractName, 60);
            // Stations from a contract expire after 1 minute
            return (List<Station>) jCDecauxItem.item;
        }

        public async Task<Station> GetAStation(string stationName, string contractName)
        {
            if (stationName==null || contractName == null)
            {
                throw new ArgumentNullException(nameof(contractName));
            }
            List<Station> stations = await GetStationsFromAContract(contractName);
            Station station = null;
            foreach (Station stationItem in stations)
            {
                if (stationItem.name == stationName)
                {
                    station = stationItem;
                    break;
                }
            }
            return station;
        }
    }
}
