using ScottPlot;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;
using ScottPlot.WinForms;

namespace Interactivity_Inspector;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
        formsPlot1.Interaction.Disable();
        formsPlot1.UserInputProcessor.IsEnabled = true;
        formsPlot1.Plot.Title("New UserInputProcessor System");

        formsPlot2.Plot.Add.Signal(Generate.Sin());
        formsPlot2.Plot.Add.Signal(Generate.Cos());
        formsPlot2.Plot.Title("Old Interaction System");
    }

    /// <summary>
    ///     Required because arrow key presses do not invoke KeyDown
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        Key key = keyData.GetKey();

        if (!StandardKeys.IsArrowKey(key))
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        IUserAction keyDownAction = new KeyDown(key);
        formsPlot1.UserInputProcessor.Process(keyDownAction);

        return true;
    }
}
