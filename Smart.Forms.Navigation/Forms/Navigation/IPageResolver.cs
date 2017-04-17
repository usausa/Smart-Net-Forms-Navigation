namespace Smart.Forms.Navigation
{
    using System;

    public interface IPageResolver
    {
        string NormarizeName(string current, string name);

        Type ResolveType(string name);

        string ResolveName(Type type);
    }
}
