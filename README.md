[![Lifecycle:Experimental](https://img.shields.io/badge/Lifecycle-Experimental-339999)]

# Spatial Data POC

This mini project is a POC that shows how to perform different tasks to manipulate or act on spatial geometry using the NetTopologySuite in-memory operations, PostgreSQL + PostGIS EF database operations & (still a WIP) Oracle EF database operations.

## Libraries Used
- [NetTopologySuite](https://github.com/NetTopologySuite/NetTopologySuite) :  This library is used in this project to perform GIS functions that are absent from SQL Server in-memory.
- [ProjNet](https://github.com/NetTopologySuite/ProjNet4GeoAPI) : This library is used to perform Coordinate System Transformations (Reprojections).
- [NPGSQL](https://github.com/npgsql/efcore.pg) : Consistes of 2 libraries that are used to perform EF database operations on a PostgreSQL DB with PostGIS. Full documenation on the library, it's usage and supported PostGIS functions can be found [here](https://www.npgsql.org/efcore/mapping/nts.html)

# External Documentation/References
- [EF Core Database Providers](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
- [EF Core Spatial Modelling](https://docs.microsoft.com/en-us/ef/core/modeling/spatial)

# Getting Started

Assumes you have Visual Studio & Docker installed.
- Fork & Clone the Repo
- Execute the docker-compose in ./docker/postgis
- Execute the scripts in ./ddl to create the required schema/table(s).
- Open the solution and ensure the required libraries are installed.
- You can rebuild the EF Context/Models with the following command 
    ```Scaffold-DbContext -Connection "host=localhost;port=5432;database=gis;user id=docker;password=docker" -Provider Npgsql.EntityFrameworkCore.PostgreSQL -o Models -ContextDir Context -Context MyDbContext -Force```