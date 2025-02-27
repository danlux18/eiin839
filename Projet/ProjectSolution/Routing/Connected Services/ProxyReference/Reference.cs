﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Routing.ProxyReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProxyReference.IJCDecauxData")]
    public interface IJCDecauxData {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxData/GetStations", ReplyAction="http://tempuri.org/IJCDecauxData/GetStationsResponse")]
        Station[] GetStations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxData/GetStations", ReplyAction="http://tempuri.org/IJCDecauxData/GetStationsResponse")]
        System.Threading.Tasks.Task<Station[]> GetStationsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxData/GetStationsFromAContract", ReplyAction="http://tempuri.org/IJCDecauxData/GetStationsFromAContractResponse")]
        Station[] GetStationsFromAContract(string contractName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxData/GetStationsFromAContract", ReplyAction="http://tempuri.org/IJCDecauxData/GetStationsFromAContractResponse")]
        System.Threading.Tasks.Task<Station[]> GetStationsFromAContractAsync(string contractName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxData/GetAStation", ReplyAction="http://tempuri.org/IJCDecauxData/GetAStationResponse")]
        Station GetAStation(string stationName, string contractName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxData/GetAStation", ReplyAction="http://tempuri.org/IJCDecauxData/GetAStationResponse")]
        System.Threading.Tasks.Task<Station> GetAStationAsync(string stationName, string contractName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJCDecauxDataChannel : Routing.ProxyReference.IJCDecauxData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JCDecauxDataClient : System.ServiceModel.ClientBase<Routing.ProxyReference.IJCDecauxData>, Routing.ProxyReference.IJCDecauxData {
        
        public JCDecauxDataClient() {
        }
        
        public JCDecauxDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JCDecauxDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JCDecauxDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JCDecauxDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Station[] GetStations() {
            return base.Channel.GetStations();
        }
        
        public System.Threading.Tasks.Task<Station[]> GetStationsAsync() {
            return base.Channel.GetStationsAsync();
        }
        
        public Station[] GetStationsFromAContract(string contractName) {
            return base.Channel.GetStationsFromAContract(contractName);
        }
        
        public System.Threading.Tasks.Task<Station[]> GetStationsFromAContractAsync(string contractName) {
            return base.Channel.GetStationsFromAContractAsync(contractName);
        }
        
        public Station GetAStation(string stationName, string contractName) {
            return base.Channel.GetAStation(stationName, contractName);
        }
        
        public System.Threading.Tasks.Task<Station> GetAStationAsync(string stationName, string contractName) {
            return base.Channel.GetAStationAsync(stationName, contractName);
        }
    }
}
