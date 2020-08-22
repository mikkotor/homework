INSERT INTO Users (Email, PasswordHash)
VALUES (@Email, @PasswordHash);
SELECT MAX(Id) AS Id FROM Users;
