# RESOAR - API
Research Open Access Repository

This API uses the Repository Pattern with the following layers:

Core: contains the domain that will be used for modeling the database and also the service layer of our application, integrating what we want to expose from the Infrastructure layer to the Presentation layer.

Infrastructure: will be responsible for all the access layer and operations in the database.

Presentation: consists of a REST API, being the communication point between the end user and the application.

Integration Tests: integration testing project, not unit testing. The integration tests will aim to test the real implementations of the repositories, performing CRUD operations without the use of MOQ.

