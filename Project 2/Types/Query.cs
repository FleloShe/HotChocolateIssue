using Project_2.Types;

namespace Project2.Types;

[QueryType]
public static class Query
{
    public static Author GetAuthor()
        => new Author("Jon Skeet");
}
