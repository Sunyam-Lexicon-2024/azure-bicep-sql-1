namespace Persons.CreatePerson;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Post("/Persons");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        string connectionString = Config["ConnectionStrings:AZURE_SQL_CONNECTIONSTRING"]
            ?? throw new InvalidOperationException("Could get azure sql connection string from configuration");

        var rows = new List<string>();

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var createCommand = new SqlCommand(
            "INSERT INTO Persons (firstName, lastName) VALUES (@firstName, @lastName)",
            conn);

        createCommand.Parameters.Clear();
        createCommand.Parameters.AddWithValue("@firstName", req.FirstName);
        createCommand.Parameters.AddWithValue("@lastName", req.LastName);

        using SqlDataReader reader = await createCommand.ExecuteReaderAsync(ct);

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                rows.Add($"{reader.GetInt32(0)},{reader.GetString(1)},{reader.GetInt32(2)}");
            }
        }

        return TypedResults.Ok(new Response() { Persons = rows });
    }
}