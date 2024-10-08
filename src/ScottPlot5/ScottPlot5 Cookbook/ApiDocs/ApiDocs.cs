﻿using System.Reflection;
using Version = ScottPlot.Version;

namespace ScottPlotCookbook.ApiDocs;

internal class ApiDocs(Type typeInAssembly, string xmlFilePath)
{
    private const string _scottPlotPrefix = nameof(ScottPlot) + ".";
    private const string _namedColorsPrefix = $"{_scottPlotPrefix}{nameof(ScottPlot.NamedColors)}.";

    private readonly XmlDocsDB _xmlDocsDB = new XmlDocsDB(xmlFilePath);
    private readonly Type[] _assemblyTypes = Assembly.GetAssembly(typeInAssembly)
                                                     ?.GetTypes()
                                                     .Where(static x => x.FullName is string name
                                                                        && name.StartsWith(_scottPlotPrefix)
                                                                        && !name.StartsWith(_namedColorsPrefix))
                                                     .ToArray()
                                             ?? [];

    //private static string GetTypeName(Type type) => GetName(type.FullName ?? type.Name);

    //private static string GetParameterTypeName(ParameterInfo pi)
    //{
    //    return Nullable.GetUnderlyingType(pi.ParameterType) is Type nullableType ? GetName(nullableType.Name) + "?" : GetName(pi.Name);
    //}

    //private static string GetParameterName(ParameterInfo pi) => GetName(pi.Name ?? "unknown");

    //private static string GetName(string? name)
    //{
    //    return name switch
    //    {
    //        null => "unknown",
    //        "System.Void" => "void",
    //        "System.Int32" => "int",
    //        "System.Double" => "double",
    //        "System.Float" => "float",
    //        "System.Boolean" => "bool",
    //        "System.String" => "string",
    //        "System.Object" => "object",
    //        _ => name.Replace("`1", "&lt;T&gt;").Split("+")[0].Replace("`2", "&lt;T1, T2&gt;").Split("+")[0].Replace("`3", "&lt;T1, T2, T3&gt;").Split("+")[0],
    //    };
    //}

    public string GetMarkdown()
        => $"""
            ---
            Title: ScottPlot 5.0 API
            Description: All classes, fields, properties, and methods provided by the ScottPlot package
            URL: /api/5.0/
            Date: {DateTime.Now.Year:0000}-{DateTime.Now.Month:00}-{DateTime.Now.Day:00}
            ShowEditLink: false
            ---

            # ScottPlot {Version.VersionString} API

            _Generated {DateTime.Now}_

            <div class='my-5'>&nbsp;</div>

            {GetHtml()}
            """;

    public string GetHtml()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<style>")
          .AppendLine("body {font-family: sans-serif;}")
          .AppendLine(".title{font-family: monospace; font-size: 1.5em; font-weight: 600;}")
          .AppendLine(".otherType{color: blue; font-family: monospace;}")
          .AppendLine(".scottPlotType{color: blue; font-family: monospace;}")
          .AppendLine(".name{color: black; font-family: monospace;}")
          .AppendLine(".docs{color: green; font-family: monospace;}")
          .AppendLine("a {text-decoration: none;}")
          .AppendLine("a:hover {text-decoration: underline;}")
          .AppendLine(".break-container{width:95vw;position:relative;left:calc(-1 * (95vw - 95%)/2);}")
          .AppendLine("</style>");

        sb.AppendLine("<div class='break-container'>");

        foreach (ClassDocs classDocs in _assemblyTypes.Select(type => new ClassDocs(type, _xmlDocsDB)))
        {
            sb.AppendLine("<div style='margin-top: 2em'>");

            sb.Append("<div class='title' id='")
              .Append(classDocs.TypeName.CleanNameHtml)
              .Append("'><a style='color: black;' href='#")
              .Append(classDocs.TypeName.CleanNameHtml)
              .Append("'>")
              .Append(classDocs.TypeName.CleanNameHtml)
              .AppendLine("</a></div>");

            sb.Append("<div class='docs'>").Append(classDocs.Docs).AppendLine("</div>");
            sb.AppendLine("</div>");

            foreach (PropertyDocs propDocs in classDocs.GetPropertyDocs(_xmlDocsDB))
            {
                if (propDocs.TypeName.CleanNameHtml.StartsWith(_scottPlotPrefix))
                {
                    sb.Append("<div><a class='scottPlotType' href='#")
                      .Append(propDocs.TypeName.CleanNameHtml)
                      .Append("'>")
                      .Append(propDocs.TypeName.CleanNameHtml)
                      .Append("</a> <span class='name'>")
                      .Append(propDocs.Name)
                      .Append("</span> <span class='docs'>")
                      .Append(propDocs.Docs)
                      .AppendLine("</span></div>");
                }
                else
                {
                    sb.Append("<div><span class='otherType'>")
                      .Append(propDocs.TypeName.CleanNameHtml)
                      .Append("</span> <span class='name'>")
                      .Append(propDocs.Name)
                      .Append("</span> <span class='docs'>")
                      .Append(propDocs.Docs)
                      .AppendLine("</span></div>");
                }
            }

            foreach (MethodDocs methodDocs in classDocs.GetMethodDocs(_xmlDocsDB))
            {
                List<string> argsHtml = [];

                foreach (MethodParameterDocs p in methodDocs.Parameters)
                {
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (p.TypeName.CleanNameHtml.StartsWith(_scottPlotPrefix))
                    {
                        argsHtml.Add($"<a class='scottPlotType' href='#{p.TypeName.CleanNameHtml}'>{p.TypeName.CleanNameHtml}</a> <span class='name'>{p.Name}</span>");
                    }
                    else
                    {
                        argsHtml.Add($"<span class='otherType'>{p.TypeName.CleanNameHtml}</span> <span class='name'>{p.Name}</span>");
                    }
                }

                string argLine = string.Join(", ", argsHtml);

                if (methodDocs.ReturnTypeName.CleanNameHtml.StartsWith(_scottPlotPrefix))
                {
                    sb.Append("<div><a class='scottPlotType' href='#")
                      .Append(methodDocs.ReturnTypeName.CleanNameHtml)
                      .Append("'>")
                      .Append(methodDocs.ReturnTypeName.CleanNameHtml)
                      .Append("</a> <span class='name'>")
                      .Append(methodDocs.Name)
                      .Append('(')
                      .Append(argLine)
                      .Append(")</span> <span class='docs'>")
                      .Append(methodDocs.Docs)
                      .AppendLine("</span></div>");
                }
                else
                {
                    sb.Append("<div><span class='otherType'>")
                      .Append(methodDocs.ReturnTypeName.CleanNameHtml)
                      .Append("</span> <span class='name'>")
                      .Append(methodDocs.Name)
                      .Append('(')
                      .Append(argLine)
                      .Append(")</span> <span class='docs'>")
                      .Append(methodDocs.Docs)
                      .AppendLine("</span></div>");
                }
            }
        }

        sb.AppendLine("</div>");

        return sb.ToString();
    }
}
