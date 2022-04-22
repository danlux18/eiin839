// Liste des stations pour un contrat choisi
var stations = [];

function goBike() {
    var targetUrl = "http://localhost:8733/Design_Time_Addresses/Routing/Service1/path";
    var requestType = "GET";
    var params = ["start=" + document.getElementById("startPoint").value, "end=" + document.getElementById("endPoint").value];
    var onFinish = displayResult;

    callAPI(targetUrl, requestType, params, onFinish);
}

function callAPI(url, requestType, params, finishHandler) {
    var fullUrl = url;

    // Ajout des paramètres dans l'url
    if (params) {
        fullUrl += "?" + params.join("&");
    }

    // Appel aux serveurs externes : XMLHttpRequest
    var caller = new XMLHttpRequest();
    caller.open(requestType, fullUrl, true);
    // Seulement les requêtes dont la réponse est OK
    caller.setRequestHeader("Accept", "application/json");
    // Onload ==> fonction à appeler à la fin
    caller.onload = finishHandler;

    caller.send();
}

function displayResult() {
    if (this.status !== 200) {
        console.log("Error, didn't succed with th request.");
        console.log(this);
    } else {
        coordinates = JSON.parse(this.responseText);
        console.log("Success");
        //coordinates[0] = (x,y) ==> x : indice des coordonées du début (pied-vélo), y : indice de fin (vélo-pied)
        console.log(coordinates);
    }
}



function retrieveContractStations() {
    // Contrat selectionné stocker dans la valeur de la liste
    var selectedContract = document.getElementById("chosenContract").value;

    var targetUrl = "https://api.jcdecaux.com/vls/v3/stations";
    var requestType = "GET";
    var params = ["apiKey=" + apiKey, "contract=" + selectedContract];

    //Remplissage de la liste des contrats
    var onFinish = feedStationList;

    callAPI(targetUrl, requestType, params, onFinish);

}
function deg2rad(deg) {
    return deg * (Math.PI / 180);
}