# RESOAR - API

## Research Open Access Repository

RESOAR is a short for Research Open Access Repository. It is a repository for research data that is open to the public.

This API follows the Repository Pattern with the following layers:

- Core: contains the domain that will be used for modeling the database and also the service layer of our application, integrating what we want to expose from the Infrastructure layer to the Presentation layer.

- Infrastructure: will be responsible for all the access layer and operations in the database.

- Presentation: consists of a REST API, being the communication point between the end user and the application.

- Integration Tests: integration testing project, not unit testing. The integration tests will aim to test the real implementations of the repositories, performing CRUD operations.

More information about the API structure can be found in this [Medium post](https://medium.com/@adlerpagliarini/c-net-core-criando-uma-aplica%C3%A7%C3%A3o-utilizando-repository-pattern-com-dois-orms-diferentes-dapper-97e8aa6ca35).

## Environment Variables

The following environment variables are used in the application:

- `Resoar:Connection`: connection string to the database.
- `Resoar:ConnectionTests`: connection string to the database for integration tests.
- `Resoar:JwtSecret`: secret key for the JWT authentication.
- `Resoar:JwtIssuer`: issuer for the JWT authentication.
- `Resoar:JwtAudience`: audience for the JWT authentication.
