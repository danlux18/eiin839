var vectorsToClear = [];

function goBike() {
    var targetUrl = "http://localhost:8733/Design_Time_Addresses/Routing/ServiceRest/path";
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
        //console.log("Error, didn't succed with th request.");
        //console.log(this);
        window.alert("Invalid request, error code : " + this.status+", error message :" + this.statusText);
    } else {
        result = JSON.parse(this.responseText).ComputePathResult;
        console.log("Success");
        console.log(result);
        var worthIt = result.worthIt;
        var stepList = [];
        var positionList = [];

        map.setView(new ol.View({             //43.615451, 7.071864
        center: ol.proj.fromLonLat([result.startPosition.longitude,result.startPosition.latitude]), // <-- Those are the GPS coordinates to center the map to.
        zoom: 10 // You can adjust the default zoom.
        }));

        if(!worthIt){
            /*for(var i = 0; i < result.footToFoot.instructions.length; i++){
                stepList.push(result.footToFoot.instructions[i].instruction);
            }*/
            stepList.push(result.footToFoot.instructions);
            positionList.push(result.footToFoot.positions);
        }
        else{
            console.log(result.footToStation.instructions);
            console.log(result.footToStation.instructions[0].instruction);
            /*for(var i = 0; i < result.footToStation.instructions.length; i++){
                stepList.push(result.footToStation.instructions[i].instruction);
            }
            for(var i = 0; i < result.stationToSation.instructions.length; i++){
                stepList.push(result.stationToSation.instructions[i].instruction);
            }
            for(var i = 0; i < result.sationToFoot.instructions.length; i++){
                stepList.push(result.sationToFoot.instructions[i].instruction);
            }*/
            stepList.push(result.footToStation.instructions);
            stepList.push(result.stationToSation.instructions);
            stepList.push(result.sationToFoot.instructions);
            positionList.push(result.footToStation.positions);
            positionList.push(result.stationToSation.positions);
            positionList.push(result.sationToFoot.positions);
        }
        displayInstructions(stepList);
        drawMap(worthIt, positionList);
    }
}

function drawMap(worthIt, positionList) {
    // Configure the style of the line
    var footLineStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: 'deepskyblue',
            width: 3
        })
    });

    var cycleLineStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: '#01FF00',
            width: 3
        })
    });

    vectorsToClear.forEach(vec => map.removeLayer(vec));
    vectorsToClear = [];

    if(worthIt){
        var foot1SubList = positionList[0];
        var cycleSubList = positionList[1];
        var foot2SubList = positionList[2];
        var foot1Coordinates  = [];
        var cycleCoordinates = [];
        var foot2Coordinates = [];
        for(var i = 0; i < foot1SubList.length; i++){
            foot1Coordinates.push([foot1SubList[i].longitude,foot1SubList[i].latitude]);
        }
        for(var i = 0; i < cycleSubList.length; i++){
            cycleCoordinates.push([cycleSubList[i].longitude,cycleSubList[i].latitude]);
        }
        for(var i = 0; i < foot2SubList.length; i++){
            foot2Coordinates.push([foot2SubList[i].longitude,foot2SubList[i].latitude]);
        }

        var foot1LineString = new ol.geom.LineString(foot1Coordinates);
        var cycleLineString = new ol.geom.LineString(cycleCoordinates);
        var foot2LineString = new ol.geom.LineString(foot2Coordinates);

        // Transform to EPSG:3857
        foot1LineString.transform('EPSG:4326', 'EPSG:3857');
        cycleLineString.transform('EPSG:4326', 'EPSG:3857');
        foot2LineString.transform('EPSG:4326', 'EPSG:3857');

        // Create the feature
        var foot1Feature = new ol.Feature({
            geometry: foot1LineString,
            name: 'Line'
        });
        var cycleFeature = new ol.Feature({
            geometry: cycleLineString,
            name: 'Line'
        });
        var foot2Feature = new ol.Feature({
            geometry: foot2LineString,
            name: 'Line'
        });

        var foot1Source = new ol.source.Vector({
            features: [foot1Feature]
        });
        var cycleSource = new ol.source.Vector({
            features: [cycleFeature]
        });
        var foot2Source = new ol.source.Vector({
            features: [foot2Feature]
        });

        var foot1Vector = new ol.layer.Vector({
            source: foot1Source,
            style: [footLineStyle]
        });
        var cycleVector = new ol.layer.Vector({
            source: cycleSource,
            style: [cycleLineStyle]
        });
        var foot2Vector = new ol.layer.Vector({
            source: foot2Source,
            style: [footLineStyle]
        });
        vectorsToClear.push(foot1Vector);
        vectorsToClear.push(cycleVector);
        vectorsToClear.push(foot2Vector);
        map.addLayer(foot1Vector);
        map.addLayer(cycleVector);
        map.addLayer(foot2Vector);
    }
    else{
        var subList = positionList[0];
        var directCoordinates = [];
        for(var i = 0; i < subList.length; i++){
            directCoordinates.push([subList[i].longitude,subList[i].latitude])
        }
        var lineString = new ol.geom.LineString(directCoordinates);

        // Transform to EPSG:3857
        lineString.transform('EPSG:4326', 'EPSG:3857');

        // Create the feature
        var feature = new ol.Feature({
            geometry: lineString,
            name: 'Line'
        });

        var source = new ol.source.Vector({
            features: [feature]
        });

        var vector = new ol.layer.Vector({
            source: source,
            style: [footLineStyle]
        });

        vectorsToClear.push(vector);
        map.addLayer(vector);
    }

}

function displayInstructions(stepList) {
    var element1 = document.getElementById("instructionContent1");
    var element2 = document.getElementById("instructionContent2");
    var element3 = document.getElementById("instructionContent3");
    element1.innerHTML = "";
    element2.innerHTML = "";
    element3.innerHTML = "";
    var element;
    var newText;
    /*for(var i = 0; i < stepList.length; i++){
        newText = i+". "+stepList[i];
        //element.textContent = element.textContent + newText;
        element.innerHTML = element.innerHTML + newText + "<br>";
    }*/
    for (var i = 0; i < stepList.length; i++) {
        var list = stepList[i];
        newText = "";
        switch (i) {
            case 0:
                element = element1;
                element.innerHTML = element.innerHTML + "<i> <b>First path with foot :</b> </i>" + "<br>";
                break;
            case 1:
                element = element2;
                element.innerHTML = element.innerHTML + "<i> <b>Cycle path :</b> </i>" + "<br>";
                break;
            default:
                element = element3;
                element.innerHTML = element.innerHTML + "<i> <b>Second path with foot :</b> </i>" + "<br>";
        }
        for (var j = 0; j < list.length; j++) {
            newText = j + ". " + list[j].instruction;
            element.innerHTML = element.innerHTML + newText + "<br>";
        }
    }
    if(stepList.length < 2){
        element2.style.visibility = "hidden";
        element3.style.visibility = "hidden";
    }
    else{
        element2.style.visibility = "visible";
        element3.style.visibility = "visible";
    }
}