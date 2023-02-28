# Authentification
This repository contains a sample .NET Core application that uses Clean Architecture with Token JWT Authentication, Entity Framework Core, Docker SQL and several other frameworks.

## **Clean Architecture**

This project follows the principles of Clean Architecture. It is organized into four main layers:

1. Presentation Layer
2. Application Layer
3. Domain Layer
4. Infrastructure Layer

Each layer has its own responsibilities, and the dependencies between the layers are always inwards.

## **Authentication Token JWT**

The application uses Token JWT Authentication to secure the APIs. The token is generated during the login process and is used to authenticate and authorize requests.

## **Creation of the SQL database with Entity Framework CORE**

Entity Framework Core is used to create the SQL database. It is responsible for generating the tables, relationships, and constraints based on the domain model.

## **Frameworks Used**

- JWT (JSON Web Token) for authentication and authorization
- MediatR for implementing CQRS pattern
- FluentValidation for validating requests and responses
- ErrorOr for handling errors and returning results
- Mapster for mapping between entities and DTOs
- AutoFixture for generating test data

## **Getting Started**

To get started with the application, follow these steps:

1. Clone the repository.
2. Configure the database connection in the appsettings.json file.
3. Run the database migration using Entity Framework Core commands.
4. Start the application.

### **Configuring the database connection**

In the appsettings.json file, update the connection string with your SQL Server instance information:

```
"ConnectionStrings": {
    "DefaultConnection": "Server=<server>;Database=<database>;User=<username>;Password=<password>;Trusted_Connection=True;MultipleActiveResultSets=true"
  }

```

### **Running the database migration**

In the Package Manager Console, run the following command to apply the migration and create the database:

```
Update-Database
```

### **Starting the application**

In the command prompt, navigate to the project directory and run the following command:

```
dotnet run
```

## **Testing**

The application has unit tests for the domain layer, application layer, and infrastructure layer. The tests are written using TDD principles and can be run using the command:

```
dotnet test
```

## **License**

This project is licensed under the MIT License