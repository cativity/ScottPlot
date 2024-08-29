namespace ScottPlotCookbook;

/// <summary>
///     A recipe category object contains many individual recipes
/// </summary>
public interface ICategory
{
    public string Chapter { get; }

    public string CategoryName { get; } // keep this redundant name to distinguish it from recipe names

    public string CategoryDescription { get; } // keep this redundant name to distinguish it from recipe names
}
