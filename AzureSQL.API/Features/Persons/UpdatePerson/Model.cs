namespace Persons.UpdatePerson;

public class Request
{
    public int PersonId { get; set;}
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

public class Response
{
    public int PersonId { get; set; }
}