﻿using NUnit.Framework.Internal;

namespace ScottPlotCookbook.Recipes;

//#pragma warning disable IDE1006 // Ignore lowercase public variable names

/// <summary>
///     Individual recipes can inherit this so they double as <see cref="IRecipe" /> and
///     test cases that have a function decorated with the <see cref="Test" /> attribute.
/// </summary>
public abstract class RecipeBase : IRecipe
{
    public static int ImageWidth { get; set; } = 400;

    public static int ImageHeight { get; set; } = 300;

    public abstract string Name { get; }

    public abstract string Description { get; }

    // keep this lowercase because it should be lowercase in recipe source code
    public Plot MyPlot { get; private set; } = new Plot();

    /// <summary>
    ///     This function is called by code interacting with <see cref="IRecipe" />
    /// </summary>
    public void Execute(Plot plot)
    {
        MyPlot = plot;
        Execute();
    }

    /// <summary>
    ///     This function is called from within the test system
    /// </summary>
    [Test]
    public abstract void Execute();

    [SetUp]
    public void ResetRandomNumberGenerator()
    {
        Generate.RandomData.Seed(0);
    }

    [TearDown]
    public void SaveRecipeImage()
    {
        string recipeClassName = GetType().Name;
        string imageFilename = $"{recipeClassName}.png";
        string saveAs = Path.Combine(Paths.OutputImageFolder, imageFilename);
        Directory.CreateDirectory(Paths.OutputImageFolder);
        MyPlot.SavePng(saveAs, ImageWidth, ImageHeight);
        TestContext.Out.WriteLine($"{saveAs}");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        MyPlot.Dispose();
    }
}
