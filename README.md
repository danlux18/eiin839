# SOC - Dan Nakache
## TP NOTE - Part II Session 2

Le fichier solution .sln se trouve dans le dossier TD6 et se nomme : "TP_NOTE_REST_AND_SOAP_WEB_SERVICE.sln".

### Structure :
Le projet est composé des 3 composants suivants :
- MathsLibrarySoap : serveur SOAP
- MathsLibraryRest : serveur REST
- Soap_Client : client SOAP

### URLs à appeler pour tester le serveur REST (avec POSTMAN) :
- URL pour l'addition : "http://localhost:8733/Design_Time_Addresses/MathsLibraryRest/Service1/Add?x=1&y=2", Résultat attendu : 3

- URL pour la soustraction : "http://localhost:8733/Design_Time_Addresses/MathsLibraryRest/Service1/Sub?x=50&y=5", Résultat attendu : 45

- URL pour la multiplication : "http://localhost:8733/Design_Time_Addresses/MathsLibraryRest/Service1/Mult?x=4&y=6", Résultat attendu : 24

___
TD1:


Question 1:
Parce qu'il est d�j� utilis�, bloqu� par l'univ.

Question 2:
<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML 2.0//EN">
<html>
    <head>
        <title>302 Found</title>
    </head>
    <body>
        <h1>Found</h1>
        <p>The document has moved <a href="http://sparks.i3s.unice.fr/">here</a>.</p>
        <hr>
        <address>Apache/2.4.41 (Ubuntu) Server at erebe-vm5.i3s.unice.fr Port 80</address>
    </body>
</html>

Question 3:
30 � 60 (56 ici)
11,90s pour 43 requ�tes
9,70s pour 43 requetes
