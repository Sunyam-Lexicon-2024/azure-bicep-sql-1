namespace Persons.GetAllPersons;

public class Endpoint : EndpointWithoutRequest<Response>
{

    public override void Configure()
    {
        Get("/Persons");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string connectionString = Config.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
         ?? throw new InvalidOperationException("Could get azure sql connection string from configuration");

        var rows = new List<string>();

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT * FROM Persons", connection);
        using SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}");
            }
        }

        await SendAsync(new()
        {
            Persons = rows
        }, cancellation: ct);
    }
}