namespace Smart.Forms.Navigation.Components
{
    using System;

    public interface IActivator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Ignore")]
        object Get(Type type);
    }
}
