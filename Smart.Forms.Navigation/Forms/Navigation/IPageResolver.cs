namespace Smart.Forms.Navigation
{
    using System;

    public interface IPageResolver
    {
        string NormalizeName(string current, string name);

        Type ResolveType(string name);

        string ResolveName(Type type);
    }
}
