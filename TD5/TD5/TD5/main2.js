// Liste des stations pour un contrat choisi
var stations = [];

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

function retrieveAllContracts() {
    var targetUrl = "https://api.jcdecaux.com/vls/v3/contracts";
    var requestType = "GET";
    var params = ["apiKey=" + getApiKey()];

    //Remplissage de la liste des contrats
    var onFinish = feedContractList;

    callAPI(targetUrl, requestType, params, onFinish);
}

function retrieveContractStations() {
    // Contrat selectionné stocker dans la valeur de la liste
    var selectedContract = document.getElementById("chosenContract").value;

    var targetUrl = "https://api.jcdecaux.com/vls/v3/stations";
    var requestType = "GET";
    var params = ["apiKey=" + getApiKey(), "contract=" + selectedContract];

    //Remplissage de la liste des contrats
    var onFinish = feedStationList;

    callAPI(targetUrl, requestType, params, onFinish);
}

// Appel quand l'appel au xml est fini, ici, c'est la reponse de l'api
function feedContractList() {
    // 200 = OK
    if (this.status !== 200) {
        console.log("Contracts not retrieved. Check the error in the Network or Console tab.");
    } else {
        // The result is contained in "this.responseText". First step: transform it into a js object.
        var responseObject = JSON.parse(this.responseText);
        // Second step: extract the contract names from the list.
        var contracts = responseObject.map(function (contract) {
            return contract.name;
        });
        // Third step: fill the datalist in the html.
        var listContainer = document.getElementById("contractsList");
        // Empty the list in case it had already been filled.
        listContainer.innerHTML = "";
        for (var i = 0; i < contracts.length; i++) {
            var currentContract = contracts[i];
            // Create a <option> tag that will contain the contract value.
            var option = document.createElement("option");
            // <option>'s value needs to be in a "value" attribute.
            option.setAttribute("value", currentContract);
            // When the <option> is complete, add it to the list.
            listContainer.appendChild(option);
        }

        // When the list is filled, display the Step 2:
        document.getElementById("step2").style.display = "block";
    }
}

// This function is called when a XML call is finished. In this context, "this" refers to the API response.
function feedStationList() {
    // First of all, check that the call went through OK:
    if (this.status !== 200) {
        console.log("Stations not retrieved. Check the error in the Network or Console tab.");
    } else {
        // Let's fill the stations variable with the list we got from the API:
        stations = JSON.parse(this.responseText);
        // Then let's display the Step 3:
        document.getElementById("step3").style.display = "block";
    }
}

function getApiKey() {
    // Let's first retrieve the input:
    var input = document.getElementById("apiKey");
    // And then return its value:
    return input.value;
}

function getClosestStation() {
    var latitude = document.getElementById("latitude").value;
    var longitude = document.getElementById("longitude").value;

    var currentMinDistance = -1;
    var currentMinDistanceStation = null;

    for (var i = 0; i < stations.length; i++) {
        var currentStation = stations[i];
        var distance = getDistanceFrom2GpsCoordinates(latitude, longitude, currentStation.position.latitude, currentStation.position.longitude);
        if (currentMinDistance === -1 || currentMinDistance > distance) {
            currentMinDistance = distance;
            currentMinDistanceStation = currentStation.name;
        }
    }

    document.getElementById("closestStation").innerHTML = currentMinDistanceStation;
    // Then let's display the final Step:
    document.getElementById("step4").style.display = "block";
}

function getDistanceFrom2GpsCoordinates(lat1, lon1, lat2, lon2) {
    // Radius of the earth in km
    var earthRadius = 6371;
    var dLat = deg2rad(lat2 - lat1);
    var dLon = deg2rad(lon2 - lon1);
    var a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
        Math.sin(dLon / 2) * Math.sin(dLon / 2)
        ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = earthRadius * c; // Distance in km
    return d;
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}