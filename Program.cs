using Microsoft.AspNetCore.Http.HttpResults;
using MySqlConnector;


var builder = WebApplication.CreateBuilder(args);//skapar en byggare för appen


//hämtar connection string från appsettings.json håller lösenord utanför koden
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build(); //bygger applikationen utifrån konfigurationen

app.MapGet("/todos", HämtaTodos);
app.MapPost("/todos", SkapaTodo);
app.MapPut("/todos/{id}", UppdateraTodo);
app.MapDelete("/todos/{id}", TaBortTodo);

app.Run();

//get - hämtar alla todos från db 
IResult HämtaTodos()
{
    using var db = new MySqlConnection(connectionString); //skapar en ny dataanslutning
    db.Open(); //öppnar anslutningen till MYsql
    var todos = new List<object>(); //skapar en ny lista som jag fyller i med todos
    var command = db.CreateCommand();//skappar ett SQL kommando
    command.CommandText = "SELECT * FROM todos"; //SQL fråga som jag kör
    var reader = command.ExecuteReader(); // kör frågan och får tillbaka en läsare
    while (reader.Read()) // loopar igenom varje rad i resultatet
    {
        todos.Add(new //lägger till en nya rad som ett objekt i listan
        {
            id = reader.GetInt32("id"), // hämtar id so, heltad
            title = reader.GetString("title"), //hämtar title som sträng
            isCompleted = reader.GetBoolean("isCompleted"), // hämtar bool
            createdAt = reader.GetDateTime("createdAt") // hämtar fatum
        });
    }
    return Results.Ok(todos); // skickar tillbaka listan med status 200
}
;

// POST skapa en ny todo i databasen
IResult SkapaTodo(Todo todo)
{
    using var db = new MySqlConnection(connectionString);
    db.Open();
    var command = db.CreateCommand();
    command.CommandText = "INSERT INTO todos (title) VALUES (@title)";
    command.Parameters.AddWithValue("@title", todo.Title); //sätter värdet
    command.ExecuteNonQuery(); // kör INSERT returnerar inga rader
    return Results.Ok("Todo skapad!");
} // tar emot ett Todo-objekt från request body

IResult UppdateraTodo(int id, Todo todo)
{
    using var db = new MySqlConnection(connectionString);
    db.Open();
    var command = db.CreateCommand();
    command.CommandText = "UPDATE todos SET title = @title, isCompleted = @isCompleted WHERE id = @id";
    command.Parameters.AddWithValue("@title", todo.Title);
    command.Parameters.AddWithValue("@isCompleted", todo.IsCompleted);
    command.Parameters.AddWithValue("@id", id);
    command.ExecuteNonQuery();
    return Results.Ok("Todo uppdaterad!");

}

IResult TaBortTodo(int id)
{
    using var db = new MySqlConnection(connectionString);
    db.Open();
    var command = db.CreateCommand();
    command.CommandText = "DELETE FROM todos WHERE id = @id";
    command.Parameters.AddWithValue("@id", id);
    command.ExecuteNonQuery();
    return Results.Ok("TODO deleted!");
}


record Todo(string Title, bool IsCompleted = false);