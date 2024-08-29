namespace WinForms_Demo;

public class DemoWindowScrollList : UserControl
{
    public DemoWindowScrollList()
    {
        Update(string.Empty);
    }

    public void Update(string search, int? width = 500)
    {
        FlowLayoutPanel layoutPanel =
            new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = false, FlowDirection = FlowDirection.TopDown, };

        foreach (IDemoWindow demoForm in DemoWindows.GetDemoWindows()
                                                    .Where(demoForm => string.IsNullOrWhiteSpace(search)
                                                                       || demoForm.Title.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                                                       || demoForm.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase)))
        {
            layoutPanel.Controls.Add(new DemoWindowInfo(demoForm, demoForm.GetType()) { Margin = new Padding(10, 10, 10, 10), Width = width ?? Width - 40, });
        }

        Controls.Add(layoutPanel);

        while (Controls.Count > 1)
        {
            Controls.RemoveAt(0);
        }
    }
}
