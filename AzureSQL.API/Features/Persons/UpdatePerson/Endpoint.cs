namespace Persons.UpdatePerson;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{

    public override void Configure()
    {
        Put("/Persons/{PersonId}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        string connectionString = Config["ConnectionStrings:AZURE_SQL_CONNECTIONSTRING"]
          ?? throw new InvalidOperationException("Could get azure sql connection string from configuration");
        
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var createCommand = new SqlCommand(
            @"UPDATE Persons SET firstName = @firstName, lastName = @lastName
            WHERE ID = @PersonId",
            connection);

        createCommand.Parameters.Clear();
        createCommand.Parameters.AddWithValue("@firstName", req.FirstName);
        createCommand.Parameters.AddWithValue("@lastName", req.LastName);
        createCommand.Parameters.AddWithValue("@PersonId", req.PersonId);

        using SqlDataReader reader = await createCommand.ExecuteReaderAsync(ct);

        var rows = new List<string>();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                rows.Add($"{reader.GetInt32(0)},{reader.GetString(1)},{reader.GetInt32(2)}");
            }
        }

        return TypedResults.Ok(new Response()
        {
            PersonId = req.PersonId
        });
    }
}