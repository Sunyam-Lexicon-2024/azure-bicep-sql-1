namespace Persons.CreatePerson;

public class Request
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

public class Response
{
    public List<string> Persons { get; set; } = [];
}