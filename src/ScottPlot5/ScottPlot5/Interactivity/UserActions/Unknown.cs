namespace ScottPlot.Interactivity.UserActions;

public record struct Unknown(string Device, string? Description) : IUserAction
{
    public string Device { get; } = Device;

    public string Description { get; } = Description ?? string.Empty;

    public DateTime DateTime { get; set; } = DateTime.Now;
}
