using ScottPlotCookbook;
using Version = ScottPlot.Version;

namespace WinForms_Demo.Demos;

public partial class CookbookViewer : Form, IDemoWindow
{
    public string Title => $"{Version.LongString} Cookbook";

    public string Description => "Common ScottPlot features demonstrated as interactive graphs displayed next to the code used to create them";

    private readonly Dictionary<ICategory, IEnumerable<IRecipe>> _recipesByCategory = Query.GetRecipesByCategory();

    private readonly JsonCookbookInfo? _jsonInfo;

    public CookbookViewer()
    {
        InitializeComponent();

        string jsonFilePathInRepo = Path.GetFullPath("../../../../../../../dev/www/cookbook/5.0/recipes.json");
        string jsonFilePathHere = Path.GetFullPath("recipes.json");

        if (File.Exists(jsonFilePathInRepo))
        {
            _jsonInfo = JsonCookbookInfo.FromJsonFile(jsonFilePathInRepo);
        }
        else if (File.Exists(jsonFilePathHere))
        {
            _jsonInfo = JsonCookbookInfo.FromJsonFile(jsonFilePathHere);
        }
    }

    private void CookbookViewerLoad(object sender, EventArgs e)
    {
        UpdateRecipeList();
        listView1.Items[0].Selected = true;
        tbFilter.Select();
    }

    private void UpdateRecipeList(string match = "")
    {
        listView1.Items.Clear();
        listView1.Groups.Clear();

        foreach (string chapter in Query.GetChapterNamesInOrder())
        {
            foreach (ICategory category in _recipesByCategory.Keys.Where(x => x.Chapter == chapter))
            {
                List<IRecipe> matchingRecipes = [];

                foreach (IRecipe recipe in _recipesByCategory[category])
                {
                    if (!string.IsNullOrEmpty(match))
                    {
                        bool matches = recipe.Name.Contains(match, StringComparison.InvariantCultureIgnoreCase)
                                       || recipe.Description.Contains(match, StringComparison.InvariantCultureIgnoreCase);

                        if (!matches)
                        {
                            continue;
                        }
                    }

                    matchingRecipes.Add(recipe);
                }

                if (matchingRecipes.Count == 0)
                {
                    continue;
                }

                ListViewGroup group = new ListViewGroup { HeaderAlignment = HorizontalAlignment.Center, Header = category.CategoryName, };

                listView1.Groups.Add(group);
                //listView1.Items.AddRange(matchingRecipes.Select(recipe => new ListViewItem { Text = recipe.Name, Group = group, }).ToArray());

                foreach (ListViewItem item in matchingRecipes.Select(recipe => new ListViewItem { Text = recipe.Name, Group = group, }))
                {
                    listView1.Items.Add(item);
                }
            }
        }
    }

    private void ListView1SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 0)
        {
            return;
        }

        IRecipe selectedRecipe = _recipesByCategory.SelectMany(static x => x.Value).Single(x => x.Name == listView1.SelectedItems[0].Text);

        formsPlot1.Reset();
        selectedRecipe.Execute(formsPlot1.Plot);
        formsPlot1.Refresh();

        if (_jsonInfo is null)
        {
            rtbCode.Text = "Source code not found.\nRun test suite to generate JSON file.";

            return;
        }

        List<JsonCookbookInfo.JsonRecipeInfo> recipeInfos = _jsonInfo.Recipes.Where(x => x.Name == selectedRecipe.Name).ToList();

        if (recipeInfos.Count == 0)
        {
            rtbCode.Text = "Source code not found.\nRun test suite to generate JSON file.";

            return;
        }

        if (recipeInfos.Count > 1)
        {
            throw new InvalidOperationException($"multiple recipes with same name: {selectedRecipe.Name}");
        }

        rtbDescription.Rtf = @"{\rtf1\ansi \b NAME \b0 - DESC}".Replace("NAME", recipeInfos.Single().Name).Replace("DESC", recipeInfos.Single().Description);

        rtbCode.Text = recipeInfos.Single().Source;
    }

    private void TbFilterTextChanged(object sender, EventArgs e)
    {
        UpdateRecipeList(tbFilter.Text);
    }
}
