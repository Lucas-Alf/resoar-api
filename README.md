[![Build](https://github.com/Lucas-Alf/resoar-api/actions/workflows/build.yml/badge.svg)](https://github.com/Lucas-Alf/resoar-api/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Lucas-Alf_resoar-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Lucas-Alf_resoar-api)

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
| Name                      | Description                            |
|---------------------------|----------------------------------------|
| `RESOAR_CONNECTION`       | Database connection string.            |
| `RESOAR_CONNECTION_TESTS` | Tests database connection string.      |
| `RESOAR_JWT_SECRET`       | Secret key for the JWT authentication. |
| `RESOAR_JWT_ISSUER`       | Issuer for the JWT authentication.     |
| `RESOAR_JWT_AUDIENCE`     | Audience for the JWT authentication.   |
| `RESOAR_RECAPTCHA_SECRET` | reCAPTCHA secret key.                  |
