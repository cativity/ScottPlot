using System.Reflection;

namespace ScottPlotCookbook.ApiDocs;

public class MethodDocs(MethodInfo fi, XmlDocsDB docsDb)
{
    public string Name { get; } = fi.Name;

    public TypeName ReturnTypeName { get; } = new TypeName(fi.ReturnType);

    public string? Docs { get; } = docsDb.GetSummary(fi);

    public MethodParameterDocs[] Parameters { get; } = fi.GetParameters().Select(static x => new MethodParameterDocs(x)).ToArray();
}
