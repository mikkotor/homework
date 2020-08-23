# Simple Blazor app to register an email address with a password

## Description

The frontend app requires the user to enter a valid email and a password that is strong enough (8 characters long minimum and contains letters, numbers and symbols). It also requires the user to retype the password to reduce the possiblity of mistyping.

Password strength is checked using [PasswordValidator](https://github.com/havardt/PasswordValidator).

When all validation checks are completed, the 'Register' button is enabled. When the user presses this button, the following happens:

1. The email address is checked against the backend service to verify it isn't already found in the database. If it is, the user is notified
2. Next, the password is encrypted using [BCrypt](https://github.com/caetanoharyon/bcrypt-core) before sending a POST request to the backend server
3. Lastly, the form is cleared and a message is shown to the user of whether the registration was a success or not

## The backend service and database

The backend service consists of single controller called Users that provides a GET endpoint to check for an existing email address and POST endpoint for adding a new email/password combination.

The database is implemented using SQLite and is created at launch if not present. All SQL scripts for creating the Users table, querying for users and inserting new ones are attached as embedded resources to the backend DLL.
