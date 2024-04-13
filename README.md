# WebApi
Just a simple web api implementation using .NET framework in c# 
The instructions were:
build a WebApi, using .NET 8. The WebApi should have a Controller, named "PositionController". The Controller will have 2 endpoints, one named "InsertPosition" and will be HTTP Post, and one named "GetPositions" and will be HTTP GET.

The first endpoint will read from the HTTP Request Body an "object", which will have the following properties:

stringName,
double Longitude,
double Latitude

This object will be called "Position". Once the endpoint "reads" it, it will go to put it in a table in the base, which  will have the following columns:

name NVARCHAR(128) UNIQUE NOT NULL PRIMARY KEY,
latitude FLOAT NOT NULL,
longitude FLOAT NOT NULL

The array name will be "positions". Once the position is entered into the database, the API will return "200 OK" (HTTP Status code).


The second endpoint will not expect anything from the user, it will simply connect to the base, read from the positions table (SELECT all that is), and return them in JSON format.
The third endpoint, which will take 2 names (from the positions you have put in the base) and read them from the base, calculate the distance between them (with Haversine), and return it.
