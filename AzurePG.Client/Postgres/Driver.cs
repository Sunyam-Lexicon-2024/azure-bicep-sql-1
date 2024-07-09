using Npgsql;

namespace AzurePG.Client.Postgres;

public class Driver
{
    private readonly string _connectionString;

    public string Host { get; } = "lexicon-demoserver-pg.postgres.database.azure.com";
    public string User { get; } = "pgadmin";
    public string DBName { get; } = "postgres";
    public string Password { get; } = Environment.GetEnvironmentVariable("PGPASSWORD");
    public string Port { get; } = "5432";

    public Driver()
    {
        _connectionString = string.Format("Server={0};UserId={1};Database={2};Port={3};Password={4};SSLMode=require",
            Host,
            User,
            DBName,
            Port,
            Password);
    }

    public void Create()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        Console.WriteLine("Establishing Connection...");
        try
        {
            connection.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine("{0}", ex);
        }

        using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS inventory", connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Finished dropping preexisting tables");
        }

        using (var command = new NpgsqlCommand("CREATE TABLE inventory(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Finished creating table");
        }

        using (var command = new NpgsqlCommand("INSERT INTO inventory (name, quantity) VALUES (@n1, @q1), (@n2, @q2), (@n3, @q3)", connection))
        {
            command.Parameters.AddWithValue("n1", "banana");
            command.Parameters.AddWithValue("q1", 150);
            command.Parameters.AddWithValue("n2", "orange");
            command.Parameters.AddWithValue("q2", 154);
            command.Parameters.AddWithValue("n3", "apple");
            command.Parameters.AddWithValue("q3", 100);

            int nRows = command.ExecuteNonQuery();
            Console.WriteLine("Numer of rows inserted: {0} ", nRows);
        }

        Console.WriteLine("Press RETURN to exit.");
        Console.ReadLine();
    }

    public void Read()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            Console.Out.WriteLine("Opening Connection...");
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM inventory", connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(
                        string.Format(
                            "Reading from table: ({0}, {1}, {2})",
                            reader.GetInt32(0).ToString(),
                            reader.GetString(1),
                            reader.GetInt32(2).ToString()
                        )
                    );
                }
                reader.Close();
            }
        }

        Console.WriteLine("Press RETURN to exit.");
        Console.ReadLine();
    }

    public void Update()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {

            Console.Out.WriteLine("Opening connection...");
            connection.Open();

            using (var command = new NpgsqlCommand("UPDATE inventory SET quantity = @q WHERE name = @n", connection))
            {
                command.Parameters.AddWithValue("n", "banana");
                command.Parameters.AddWithValue("q", 200);
                int nRows = command.ExecuteNonQuery();
                Console.Out.WriteLine(String.Format("Number of rows updated={0}", nRows));
            }
        }

        Console.WriteLine("Press RETURN to exit.");
        Console.ReadLine();
    }

    public void Delete()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            Console.Out.WriteLine("Opening connection");
            connection.Open();

            using (var command = new NpgsqlCommand("DELETE FROM inventory WHERE name = @n", connection))
            {
                command.Parameters.AddWithValue("n", "orange");

                int nRows = command.ExecuteNonQuery();
                Console.Out.WriteLine(String.Format("Number of rows deleted={0}", nRows));
            }
        }

        Console.WriteLine("Press RETURN to exit.");
        Console.ReadLine();
    }
}

