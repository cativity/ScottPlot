using ScottPlot;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActionResponses;
using ScottPlot.Interactivity.UserActions;
using ScottPlot.WinForms;

namespace WinForms_Demo.Demos;

public partial class CustomMouseActions : Form, IDemoWindow
{
    public string Title => "Custom Mouse Actions";

    public string Description => "Demonstrates how to disable the mouse or changes what the button actions are";

    public CustomMouseActions()
    {
        InitializeComponent();

        // Disable the old input system and enable the new one.
        // The new one will be enabled by default in a future release.
        formsPlot1.Interaction.IsEnabled = false;
        formsPlot1.UserInputProcessor.IsEnabled = true;

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        btnDefault.Click += (_, _) =>
        {
            richTextBox1.Text = "left-click-drag pan, right-click-drag zoom, middle-click autoscale, "
                                + "middle-click-drag zoom rectangle, alt+left-click-drag zoom rectangle, right-click menu, "
                                + "double-click benchmark, scroll wheel zoom, arrow keys pan, "
                                + "shift or alt with arrow keys pans more or less, ctrl+arrow keys zoom";

            formsPlot1.UserInputProcessor.IsEnabled = true;
            formsPlot1.UserInputProcessor.Reset();
        };

        btnDisable.Click += (_, _) =>
        {
            richTextBox1.Text = "Mouse and keyboard events are disabled";
            formsPlot1.UserInputProcessor.IsEnabled = false;
        };

        btnCustom.Click += (_, _) =>
        {
            richTextBox1.Text = "middle-click-drag pan, right-click-drag zoom rectangle, "
                                + "right-click autoscale, left-click menu, Q key autoscale, WASD keys pan";

            formsPlot1.UserInputProcessor.IsEnabled = true;

            // remove all existing responses so we can create and add our own
            formsPlot1.UserInputProcessor.UserActionResponses.Clear();

            // middle-click-drag pan
            MouseButton panButton = StandardMouseButtons.Middle;
            MouseDragPan panResponse = new MouseDragPan(panButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(panResponse);

            // right-click-drag zoom rectangle
            MouseButton zoomRectangleButton = StandardMouseButtons.Right;
            MouseDragZoomRectangle zoomRectangleResponse = new MouseDragZoomRectangle(zoomRectangleButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(zoomRectangleResponse);

            // right-click autoscale
            MouseButton autoscaleButton = StandardMouseButtons.Right;
            SingleClickAutoscale autoscaleResponse = new SingleClickAutoscale(autoscaleButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(autoscaleResponse);

            // left-click menu
            MouseButton menuButton = StandardMouseButtons.Left;
            SingleClickContextMenu menuResponse = new SingleClickContextMenu(menuButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(menuResponse);

            // Q key autoscale too
            Key autoscaleKey = new Key("Q");
            KeyPressResponse autoscaleKeyResponse = new KeyPressResponse(autoscaleKey, AutoscaleAction);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(autoscaleKeyResponse);

            // WASD keys pan
            KeyboardPanAndZoom keyPanResponse = new KeyboardPanAndZoom
            {
                PanUpKey = new Key("W"),
                PanLeftKey = new Key("A"),
                PanDownKey = new Key("S"),
                PanRightKey = new Key("D"),
            };

            formsPlot1.UserInputProcessor.UserActionResponses.Add(keyPanResponse);

            return;

            static void AutoscaleAction(Plot plot, Pixel pixel) => plot.Axes.AutoScale();
        };

        Load += (_, _) => btnDefault.PerformClick();
    }

    /// <summary>
    ///     Required because arrow key presses do not invoke KeyDown
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        Key key = keyData.GetKey();

        if (StandardKeys.IsArrowKey(key))
        {
            KeyDown keyDownAction = new KeyDown(key);
            formsPlot1.UserInputProcessor.Process(keyDownAction);

            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
