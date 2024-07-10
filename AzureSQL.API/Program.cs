var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
                .SwaggerDocument();

var app = builder.Build();

app.UseHttpsRedirection();

string connectionString = app.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
try
{
    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "CREATE TABLE Persons (ID int NOT NULL PRIMARY KEY IDENTITY, FirstName varchar(255), LastName varchar(255));",
        conn);
    using SqlDataReader reader = command.ExecuteReader();
}
catch (Exception e)
{
    // Table may already exist
    Console.WriteLine(e.Message);
}

app.UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
    })
    .UseSwaggerGen();

app.Run();