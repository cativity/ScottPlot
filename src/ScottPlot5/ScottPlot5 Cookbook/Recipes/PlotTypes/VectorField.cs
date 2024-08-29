using System.Numerics;
using JetBrains.Annotations;
using ScottPlot.Colormaps;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class VectorField : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Vector Field";

    public string CategoryDescription => "Vector fields display a collection of vectors rooted at points in coordinate space";

    public class VectorFieldQuickstart : RecipeBase
    {
        public override string Name => "Vector Field Quickstart";

        public override string Description
            => "Vectors (representing a magnitude and direction) can be placed at specific points in coordinate space to display as a vector field.";

        [Test]
        public override void Execute()
        {
            // generate a grid of positions
            double[] xs = Generate.Consecutive(10);
            double[] ys = Generate.Consecutive(10);

            // create a collection of vectors
            List<RootedCoordinateVector> vectors = xs.SelectMany(_ => ys,
                                                                 (cx, cy) => new RootedCoordinateVector(new Coordinates(cx, cy),
                                                                                                        new Vector2((float)cy, -19.62f * (float)Math.Sin(cx))))
                                                     .ToList();
            //vectors.AddRange(xs.SelectMany(cx => ys,
            //                               (cx, cy) => new RootedCoordinateVector(new Coordinates(cx, cy),
            //                                                                      new Vector2((float)cy, -19.62f * (float)Math.Sin(cx)))));

            // point on the grid
            // direction & magnitude
            //Vector2 v = new Vector2((float)cy, (-9.81f / 0.5f) * (float)Math.Sin(cx));
            // add coordinate and vector to the collection
            //foreach (double cx in xs)
            //{
            //    foreach (double cy in ys)
            //    {
            //        RootedCoordinateVector vector = new RootedCoordinateVector(new Coordinates(cx, cy), new Vector2((float)cy, -19.62f * (float)Math.Sin(cx)));
            //        vectors.Add(vector);
            //    }
            //}
            // plot the collection of rooted vectors as a vector field
            MyPlot.Add.VectorField(vectors);
        }
    }

    public class VectorFieldColormap : RecipeBase
    {
        public override string Name => "Vector Field Colormap";

        public override string Description => "Vector field arrows can be colored according to their magnitude.";

        [Test]
        public override void Execute()
        {
            RootedCoordinateVector[] vectors = Generate.SampleVectors();
            ScottPlot.Plottables.VectorField vf = MyPlot.Add.VectorField(vectors);
            vf.Colormap = new Turbo();
        }
    }
}
