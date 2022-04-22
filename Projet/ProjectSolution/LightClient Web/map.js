var map = new ol.Map({
    target: 'map', // <-- This is the id of the div in which the map will be built.
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        })
    ],
        //7.0985774, 43.6365619
    view: new ol.View({             //43.615451, 7.071864
        center: ol.proj.fromLonLat([7.071864, 43.615451]), // <-- Those are the GPS coordinates to center the map to.
        zoom: 15 // You can adjust the default zoom.
    })

});