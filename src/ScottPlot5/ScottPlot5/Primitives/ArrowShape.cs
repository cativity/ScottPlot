using ScottPlot.ArrowShapes;
using Double = ScottPlot.ArrowShapes.Double;
using Single = ScottPlot.ArrowShapes.Single;

namespace ScottPlot;

public enum ArrowShape
{
    Single,
    SingleLine,
    Double,
    DoubleLine,
    Arrowhead,
    ArrowheadLine,
    Pentagon,
    Chevron,
}

public static class ArrowShapeExtensions
{
    public static IArrowShape GetShape(this ArrowShape shape)
    {
        return shape switch
        {
            ArrowShape.Single => new Single(),
            ArrowShape.SingleLine => new SingleLine(),
            ArrowShape.Double => new Double(),
            ArrowShape.DoubleLine => new DoubleLine(),
            ArrowShape.Arrowhead => new Arrowhead(),
            ArrowShape.ArrowheadLine => new ArrowheadLine(),
            ArrowShape.Pentagon => new Pentagon(),
            ArrowShape.Chevron => new Chevron(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }
}
