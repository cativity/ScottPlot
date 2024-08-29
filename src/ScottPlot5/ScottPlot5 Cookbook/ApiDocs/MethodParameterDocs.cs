using System.Reflection;

namespace ScottPlotCookbook.ApiDocs;

public class MethodParameterDocs(ParameterInfo pi)
{
    public string Name { get; } = pi.Name ?? "ANONYMOUS";

    public TypeName TypeName { get; } = new TypeName(pi.ParameterType);
}
