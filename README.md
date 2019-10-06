Progress of skeleton for case 1:


| Docker            | FrontEnd - Animal | WebAPI        | Other |
|:-------------------|:-----------------|:--------------|:--------------|
| ✓ - FrontEnd        | ✓ - HttpGet     | ✓ - HttpGet  |  X - FrontEnd Pipeline |
| ✓ - WebAPI          | X - HttpPost    | ✓ - Insert    |  X - BackEnd Pipeline |
| ✓ - Database        | X - HttpPut     | ✓ - Edit      | 
| ✓ - docker-compose  | X - Views       | ✓ - Delete    |
|                     |                 | ✓ - Swashbuckle |
|                     |                 | X - DataMapper 

___
#Docker
>Description of the dockerfiles and docker compose                                           


The FrontEnd and WebAPI each make use of their own Dockerfile building from:   <br>
*mcr.microsoft.com/dotnet/core/aspnet:3.0 <br>*

It's important to note that they expect the project to be published to <br>
*./bin/Release/netcoreapp3.0  <br>*

The database makes use of the following base image: <br>
*mcr.microsoft.com/mssql/server:2017-latest-ubuntu* <br>

Images and containers built will be prefixed with my initials: so

___
#FrontEnd
>.NET Core 3.0 MVC   


The FrontEnd is exposed via the docker-compose via port: <br>
*52020:80*

Controllers make use of agents to retrieve the model to view. <br> 
Agents are injected in the ConfigureServices method.<br>
This allows models to be mocked for testing.
___
#WebAPI
>.NET Core 3.0 MVC   


The WebAPI is exposed via the docker-compose via port: <br>
*52019:80* <br> 
It's important to note that this is intended for Swashbuckle use only. <br> 

Oh right, the WebAPI makes use of Swashbuckle!
Access the GUI via <br>
https://localhost:52019/swagger  
<br> 
Access the Json via  <br>
https://localhost:52019/swagger/v1/swagger.json

Important to note that Swashbuckle is installed with the preview version: <br>
 5.0.0-rc4

 | Action             | HTTP method     | Relative URI       |
|:--------------------|:----------------|:-------------------|
| Get all animals     | GET             | /api/animals       |  
| Create a new animal | POST            | /api/animals       | 
| Get an animal by ID | GET             | /api/animals/*id*  |  
| Update an animal    | PUT             | /api/animals/*id*  | 
| Delete an animal    | DELETE          | /api/animals/*id*  |

___
#Database
>Microsoft SQL Server   


The database makes use of the image provided by microsoft: <br>
*mcr.microsoft.com/mssql/server:2017-latest-ubuntu*  
<br>
___
#Pipeline
>Azure Pipelines


Pipelines are built only for the FrontEnd and WebAPI <br>
They are two distinct Pipelines to prevent cross-contamination if things go bork
&nbsp;
___
#Known issues
1. FrontEnd expects environment variable for the HttpClient Uri which isn't set yet
&nbsp;
___
#Versions
#####V1.0 Setup of FrontEnd and ReadMe 04-10-2019
#####V1.01 Setup of docker-compose 05-10-2019
#####V1.02 WebAPI basic CRUD with Database 06-10-2019
___
