using System.ServiceModel;
using System.ServiceModel.Web;
using ClassLibrary;
using System.Threading.Tasks;

namespace Routing
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRoutingSoap
    {
        [OperationContract]
        Task<ResultObject> computePath(string start, string end);
    }
}
