DevicesAPI – RESTful API for Managing Devices

This project is a simple and clean RESTful API built with ASP.NET Core 8.0.
It allows creating, updating, fetching, and deleting devices while following proper software development and containerization practices.

**Project Overview**

The purpose of this API is to manage a list of devices with basic properties and rules:

Each device has an Id, Name, Brand, State (Available, InUse, Inactive), and CreatedAt time.

The API performs all CRUD operations (Create, Read, Update, Delete).

Business rules are applied, such as:

CreatedAt cannot be updated.

Devices in use cannot be deleted or have their name/brand modified.

 **Key Functionalities**

 Create a new device
 Fully or partially update an existing device
 Fetch a single device by Id
 Fetch all devices
 Fetch devices by brand
 Fetch devices by state
 Delete a device (only if it’s not in use)

 **Technologies Used**

C# 12 / .NET 8.0

Entity Framework Core (Code-First Approach)

SQL Server

Swagger UI for API documentation

Docker & Docker Compose for containerization


**Project Structure**
DevicesAPI/

Controllers/            Contains API controllers (DevicesController.cs)
Data/                   EF Core database context (DevicesDbContext.cs)
Domain/                 Device entity and DeviceState enum
DTOs/                   Data transfer objects (CreateDeviceDto, UpdateDeviceDto)
Services/               Business logic layer (DeviceService, IDeviceService)
Migrations/             Database migrations folder
appsettings.json        Database connection string
Dockerfile              Docker build configuration for the API
docker-compose.yml      Combined setup for API + SQL Server containers
Program.cs              Application startup file

**Docker Setup**

This project is fully containerized.
It includes both the API and SQL Server database running through Docker Compose.

How to run:

Make sure Docker Desktop is running.

Open a terminal in the project root folder.

Run the following command:

First Command = docker-compose up --build
Once the build completes, open your browser and go to:
Second Command = http://localhost:8080/swagger  Explanation = to access Swagger UI and test all endpoints.


**API Endpoints**
Method	   Endpoint                        Description
GET	/api/devices/                          ping	Check if API is running
POST	/api/devices	                       Add a new device
GET	/api/devices	                         Get all devices
GET	/api/devices/{id}	                     Get a specific device
GET	/api/devices/brand/{brand}	           Get devices by brand
GET	/api/devices/state/{state}	           Get devices by state
PUT	/api/devices/{id}                    	 Update device fully
PATCH	/api/devices/{id}	                   Partially update device
DELETE	/api/devices/{id}	                 Delete device (only if not in use)

**Notes**

Built using clean code structure and layered architecture.

Follows best practices for naming conventions and business logic separation.

Tested locally in Docker and verified that all endpoints work successfully.

**Author**

Hafiz Muhammad Qasim
Master’s in Information Systems – Uppsala University, Sweden
Backend Developer | ASP.NET Core | SQL Server | Docker
