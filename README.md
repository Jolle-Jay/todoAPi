Todo API

My first REST API built from scratch using C# Minimal API and MySQL.

Tech Stack
- C# / .NET 10 - Minimal API
- MySQL - Database
- MySqlConnector - Database driver

Endpoints
| Method | URL | Description |
|--------|-----|-------------|
| GET | /todos | Get all todos |
| POST | /todos | Create a new todo |
| PUT | /todos/{id} | Update a todo |
| DELETE | /todos/{id} | Delete a todo |

Setup

1. Clone the repo
2. Create a MySQL database:
```sql
CREATE DATABASE tododb;
USE tododb;

CREATE TABLE todos (
    id INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(255) NOT NULL,
    isCompleted BOOLEAN DEFAULT FALSE,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

3. Copy `appsettings.example.json` to `appsettings.json` and fill in your MySQL credentials
4. Run the API:
```bash
dotnet run
```