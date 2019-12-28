# Event Sourcing Sample With CQRS and MediatR

> .Net Core 3.0, PostgreSql, EntityFrameworkCore, Xunit, MiniCover, Docker ,Swagger , ProblemDetails, CorrelationId, Prometheus

## Description
This project aims to be an example of event sourcing in .net core. 
Case Study:
Most common sample of event sourcing is bank account transactions. But in this sample I used a football match; 
- Create a match (requires information like home and away teams info )
- Start the match (requires game id )
- Add score for one team
- Add card to a game statistics
- Add faul to game statistics
- Get score board
- Get game details

All the data is storaged in different tables in postgreSql database. Event sourcing is handled with cqrs and mediatr patterns.No external tool is used and Marten is planned to use in future

## Requirements
- .Net Core >= 3.0
- PostgreSql
- Docker
## Features
##### Framework
- [.Net Core](https://github.com/dotnet/core)
##### Storage
- [PostgreSql](https://www.postgresql.org/)
- [InMemoryDb](https://entityframeworkcore.com/providers-inmemory)
##### Data Access
- [EntityFrameworkCore](https://docs.microsoft.com/en-us/ef/core/)
##### Integration testing
- [Xunit](https://xunit.net/)
- [Fluent Assertions](https://fluentassertions.com/)
##### Logging
- [NLog](https://nlog-project.org/)
##### Metrics
- [Prometheus](https://prometheus.io/)
##### Api Documentation
- [Swagger](https://swagger.io/)
##### Test Coverage
- [MiniCover](https://github.com/lucaslorentz/minicover)

## Running the API
### Development
To start the application in development mode, run:

```cmd
dotnet build
cd src\EventSourcingSampleWithCQRSandMediatr
dotnet run
```
Application will be served on route: 
http://localhost:5000

To start the application in docker container:
```cmd
docker-compose up
```
Docker will spin up application, postgreSql container and PgAdmin to manage database and will be served on route : 
http://localhost:5001

## Swagger
Swagger documentation will be available on route: 
```bash
http://localhost:5000/swagger
```

### Testing
To run tests: 
```bash
dotnet test ./tests/EventSourcingSampleWithCQRSandMediatr.Tests/EventSourcingSampleWithCQRSandMediatr.Tests.csproj
```
 
To run unit and integration tests with script: 
```bash
scripts/tests.sh
```

### Test Coverage
```bash
scripts/coverage.sh
```
Coverage file can be found in coverage-html/index.html file

## Set up environment
Keys and the secrets are defined in user secret file. More information can be found [.net core user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows)
This application uses "8c43a081-db6b-43eb-8376-df1651b2d72a" as secret key id.

#### *To configure PostgreSql db:
Application uses InMemory database for local development and PostgreSql is also configurable.To configure: 
Connection string needs to be defined in appsettings.json
 
```bash
 "Db": {
	"ConnectionString": "Your connection string"
	"UseMemoryDb": false,
  }
```

#### To configure InMemory storage:
appsettings.json
```bash
 "Db": {
    "UseMemoryDb": true
  }
```

#### *To configure db docker-compose.yml:
docker-compose.yml
```bash
  environment:
	Db__ConnectionString: 'Your Connection string'
	Db__UseMemoryDb: 'your choice true/false'
```


## *TODO-Nice to have list:
- Use Marten as event sourcing tool
- Add more statistics like injuries and player substitution 
- Add more business validation