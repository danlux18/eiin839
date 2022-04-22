using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IJCDecauxData
    {
        [OperationContract]
        Task<List<Station>> GetStations();
        [OperationContract]

        Task<List<Station>> GetStationsFromAContract(string contractName);

        [OperationContract]
        Task<Station> GetAStation(string stationName, string contractName);
    }
}
