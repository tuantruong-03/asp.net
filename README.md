# Portfolio Management Web API

## Overview
This is a basic Web API application built with ASP.NET Core for managing portfolios. It includes features for authentication and authorization, and supports CRUD operations for portfolios, stocks, and comments. The application uses DTOs and AutoMapper to handle data transfer efficiently.

## Features
- **JWT Authentication and Authorization**: Secures resources with JWT tokens.
- **CRUD Operations**: Supports creating, reading, updating, and deleting portfolios, stocks, and comments.
- **Data Transfer Objects (DTOs)**: Streamlines data communication between the server and clients.
- **AutoMapper**: Automates the mapping between DTOs and entities.

## Prerequisites
- .NET 6 SDK or later
- SQL Server or any compatible database

## Installation
- Clone the repository, then cd portfolio/api
- Update the connection string in appsettings.json:
- Apply migrations: dotnet ef database update
- Run: dotnet watch run
