namespace Smart.Forms.Navigation.Components
{
    using System;

    public interface IFactory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Ignore")]
        object Create(Type type);
    }
}
