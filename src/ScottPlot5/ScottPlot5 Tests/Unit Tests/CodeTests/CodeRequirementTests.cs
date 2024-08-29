using System.Reflection;

namespace ScottPlotTests.CodeTests;

internal class CodeRequirementTests
{
    [Test]
    public void TestAllTestMethodsHaveTestAttribute()
    {
        IEnumerable<MethodInfo> testMethods = Assembly.GetAssembly(typeof(CodeRequirementTests))?.GetTypes()
                                                      .SelectMany(static t => t.GetMethods())
                                                      .Where(static x => x.Name.StartsWith("Test_")) ?? [];

        foreach (MethodInfo mi in testMethods)
        {
            bool hasTestAttribute = mi.CustomAttributes.Select(static x => x.AttributeType).Contains(typeof(TestAttribute));

            bool hasParameterizedTestAttribute = mi.CustomAttributes.Select(static x => x.AttributeType).Contains(typeof(TestCaseAttribute));

            bool hasRequireAttribute = hasTestAttribute || hasParameterizedTestAttribute;

            if (!hasRequireAttribute)
            {
                string name = $"{mi.DeclaringType}." + mi.ToString()?.Split(" ")[1];
                string message = $"{name} is missing the [Test] attribute.";
                Assert.Fail(message);
            }
        }
    }

    [Test]
    public void TestPlottablesRenderMethodIsVirtual()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3693

        foreach (Type type in Assembly.GetAssembly(typeof(Plot))?.GetTypes().Where(static x => x.IsAssignableTo(typeof(IPlottable)) && x.IsClass) ?? [])
        {
            foreach (MethodInfo mi in type.GetMethods().Where(static x => x.Name == "Render").ToArray())
            {
                ParameterInfo[] pis = mi.GetParameters();

                if (pis.Length != 1)
                {
                    continue;
                }

                ParameterInfo pi = pis[0];

                if (pi.ParameterType.Name != "RenderPack")
                {
                    continue;
                }

                if (mi.IsFinal)
                {
                    Assert.Fail($"{type.Namespace}.{type.Name}.Render() must be virtual void");
                }
            }
        }
    }

    [Test]
    public void TestRenderActionsArePublic()
    {
        IEnumerable<Type> actionTypes =
            Assembly.GetAssembly(typeof(Plot))?.GetTypes().Where(static x => x.IsAssignableTo(typeof(IRenderAction)) && x.IsClass) ?? [];

        foreach (Type type in actionTypes.Where(static type => !type.GetTypeInfo().IsVisible))
        {
            throw new InvalidOperationException($"{type.Namespace}.{type.Name} should be public");
        }
    }
}
