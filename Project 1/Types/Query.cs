namespace Project1.Types;

[QueryType]
public static class Query
{
    public static Book GetBook()
        => new Book("C# in depth.");
}
