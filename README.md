[![Build](https://github.com/Lucas-Alf/resoar-api/actions/workflows/build.yml/badge.svg)](https://github.com/Lucas-Alf/resoar-api/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Lucas-Alf_resoar-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Lucas-Alf_resoar-api)

# RESOAR - API

## Research Open Access Repository

RESOAR is a short for Research Open Access Repository.

Frontend repo: https://github.com/Lucas-Alf/resoar 

This API follows the Repository Pattern with the following layers:

- Core: contains the domain that will be used for modeling the database and also the service layer of our application, integrating what we want to expose from the Infrastructure layer to the Presentation layer.

- Infrastructure: will be responsible for all the access layer and operations in the database.

- Presentation: consists of a REST API, being the communication point between the end user and the application.

- Integration Tests: integration testing project, not unit testing. The integration tests will aim to test the real implementations of the repositories, performing CRUD operations.

More information about the API structure can be found in this [Medium post](https://medium.com/@adlerpagliarini/c-net-core-criando-uma-aplica%C3%A7%C3%A3o-utilizando-repository-pattern-com-dois-orms-diferentes-dapper-97e8aa6ca35).

## Environment Variables
| Name                       | Description                                            |Required|
|----------------------------|--------------------------------------------------------|--------|
| `RESOAR_CONNECTION`        | Database connection string.                            | Yes    |
| `RESOAR_CONNECTION_TESTS`  | Tests database connection string.                      | No     |
| `RESOAR_JWT_SECRET`        | Secret key for the JWT authentication.                 | Yes    |
| `RESOAR_JWT_ISSUER`        | Issuer for the JWT authentication. (backend url)       | Yes    |
| `RESOAR_JWT_AUDIENCE`      | Audience for the JWT authentication. (frontend url)    | Yes    |
| `RESOAR_CAPTCHA_SITE_KEY`  | hCAPTCHA site key. (the same used on frontend)         | Yes    |
| `RESOAR_CAPTCHA_SECRET`    | hCAPTCHA secret key.                                   | Yes    |
| `RESOAR_STORAGE_ACCESS_KEY`| Storage Access Key.                                    | Yes    |
| `RESOAR_STORAGE_SECRET_KEY`| Storage Secret Key.                                    | Yes    |
| `RESOAR_STORAGE_REGION`    | Storage Region. Ex: `us-west-1`                        | Only S3|
| `RESOAR_STORAGE_ENDPOINT`  | Storage Endpoint URL for Digital Ocean Spaces.         | Only DO|
| `RESOAR_SMTP_HOST`         | E-mail SMTP host.                                      | Yes    |
| `RESOAR_SMTP_PORT`         | E-mail SMTP port.                                      | Yes    |
| `RESOAR_SMTP_USER`         | E-mail SMTP user.                                      | Yes    |
| `RESOAR_SMTP_EMAIL`        | E-mail SMTP (sender e-mail).                           | Yes    |
| `RESOAR_SMTP_PASSWORD`     | E-mail SMTP password.                                  | Yes    |

