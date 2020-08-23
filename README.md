# Simple [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) app to register an email address with a password

## Description

The frontend app requires the user to enter a valid email and a password that is strong enough (8 characters long minimum and contains letters, numbers and symbols). It also requires the user to retype the password to reduce the possiblity of mistyping.

Email validity is checked using [EmailValidation](https://github.com/jstedfast/EmailValidation).
Password strength is checked using [PasswordValidator](https://github.com/havardt/PasswordValidator).

When all validation checks are completed, the 'Register' button is enabled. When the user presses this button, the following happens:

1. The email address is checked against the backend service to verify it isn't already found in the database. If it is, the user is notified
2. Next, the password is encrypted using [BCrypt](https://github.com/caetanoharyon/bcrypt-core) before sending a POST request to the backend server
3. Lastly, the form is cleared and a message is shown to the user of whether the registration was a success or not

## The backend service and database

The backend service consists of single controller called Users that provides a GET endpoint to check for an existing email address and POST endpoint for adding a new email/password combination.

The database is implemented using SQLite and is created at launch if not present. All SQL scripts for creating the Users table, querying for users and inserting new ones are attached as embedded resources to the backend DLL.

## Requirements and instructions to build

This project is built using and depends on:

* .NET Core 3.1 SDK
* Various NuGet packages mentioned above for providing email and password checks and password encryption
* Xunit and NSubstitute for unit and integration tests

## How to run

* Clone this repository to your machine
* Make sure you have .NET Core 3.1 SDK installed
* If you have Visual Studio on your machine, open the solution file and mark both Backend and Fronted solutions as startup projects, then rebuild and run
* You can also use dotnet CLI tool to run the project. In this mode, Run Backend.csproj and Frontend.csproj on their own terminals.

## Tests

Both projects contain unit tests for the service classes the "business logic" is implemented in.
Furthermore, BackendIntegrationTests verify that the SQLite database can be setup and used properly without having to just run the project and hope for the best.
