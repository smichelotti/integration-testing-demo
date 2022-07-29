# Integration Testing Demo

This repository highlights how we do Integration testing.

## Standards

* At least 1 integration test per Web API endpoint
* Live database calls (for DBs owned by our app) *are* tested (Cosmos DB emulator)
* True 3rd party External dependencies are mocked (via WireMock.Net)

## Frameworks

* [xUnit](https://xunit.net/) - Core testing framework
* [Fluent Assertions](https://fluentassertions.com/) - More naturally specify expected outcome
* [Abla](https://jasperfx.github.io/alba/) - Built on [built-in in-memory test server](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0) in asp.net core
* [WireMock.Net](https://github.com/WireMock-Net/WireMock.Net) - Preferred for Integration testing
* [Moq](https://github.com/Moq/moq) - Only when necessary (favor more for unit testing)

## Principles

* Combination of Unit testing and **Integration testing**, but favor Integration testing because:
  * Yield much larger code coverage for less test code (DB, etc.)
  * Better testing of real scenarios
  * Enable easy testing of (hard to unit test) middleware, etc.
  * Enable easy testing of entire asp.net pipeline (serialization, validation, etc.)
* Favor **Unit testing** over Integration testing for isolated logic-heavy components need several different conditions/scenarios verified

## Running the code

* Obtain an Google Maps API Key from the [Google developer console](https://console.cloud.google.com/apis/dashboard)
* `cd src\ContactsApi`
* `dotnet user-secrets set MapsApiKey <your-google-maps-api-key>`
* `Ctrl-F5` to run from Visual Studio
