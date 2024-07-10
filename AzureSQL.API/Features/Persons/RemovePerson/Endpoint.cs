
namespace Persons.RemovePerson;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{

    public override void Configure()
    {
        Delete("/Persons/{PersonId}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {

        string connectionString = Config["ConnectionStrings:AZURE_SQL_CONNECTIONSTRING"]
            ?? throw new InvalidOperationException("Could get azure sql connection string from configuration");

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var createCommand = new SqlCommand(
            "DELETE FROM Persons WHERE ID = @PersonId",
            connection);

        createCommand.Parameters.Clear();
        createCommand.Parameters.AddWithValue("@PersonId", req.PersonId);

        await createCommand.ExecuteNonQueryAsync(ct);

        return TypedResults.Ok(new Response()
        {
            PersonId = req.PersonId
        });

    }
}