namespace Softylines.OrleansApp.Abstractions.Models;

[GenerateSerializer]
public class UserDetails
{
    [Id(0)]
    public string? Name { get; set; }

    [Id(1)]
    public string? LastName { get; set; }
}