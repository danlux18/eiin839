using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MathsLibrary
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IMathsOperations
    {
        //WebMessageFormat.Json as ResponseFormat, WebMessageBodyStyle.Wrapped as BodyStyle, and name the UriTemplate however you like (don't forget to include the 2 parameters in your format).
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Add?x={x}&y={y}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        int Add(int x, int y);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Mult?x={x}&y={y}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        int Multiply(int x, int y);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Sub?x={x}&y={y}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        int Subtract(int x, int y);

        // TODO: ajoutez vos opérations de service ici
    }

}
