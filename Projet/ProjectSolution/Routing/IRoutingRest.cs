using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ClassLibrary;
using System.Threading.Tasks;

namespace Routing
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRoutingRest
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "path?start={start}&end={end}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<ResultObject> ComputePath(string start, string end);

    }
}
