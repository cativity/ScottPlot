using System.Diagnostics;
using ScottPlot.Testing;
using ScottPlot.WinForms;
using Image = ScottPlot.Image;

namespace GraphicalTestRunner;

public partial class ImageComparer : UserControl
{
    private Bitmap? _bmp1;
    private Bitmap? _bmp2;
    private ImageDiff? _imgDiff;
    private int _imageMode;

    private string _path1 = string.Empty;
    private string _path2 = string.Empty;
    private readonly Color _selectedBackgroundColor = Color.FromArgb(50, SystemColors.ControlDark);

    public ImageComparer()
    {
        InitializeComponent();

        timer1.Enabled = !DesignMode;
        timer1.Tick += (s, e) => SwitchImages(1);
        timer1.Interval = 100;
        pictureBox1.MouseWheel += (s, e) => SwitchImages(e.Delta > 0 ? 1 : -1);

        checkBox1.CheckedChanged += (s, e) => timer1.Enabled = checkBox1.Checked;
        checkBox2.CheckedChanged += (s, e) => UpdateDiffBitmap();

        pictureBox1.DoubleClick += (s, e) => Process.Start("explorer.exe", _path1);
        pictureBox2.DoubleClick += (s, e) => Process.Start("explorer.exe", _path2);
    }

    private void SwitchImages(int delta)
    {
        _imageMode += delta;

        if (_imageMode % 2 == 0)
        {
            SetBitmap1();
        }
        else
        {
            SetBitmap2();
        }
    }

    private void SetBitmap1()
    {
        label1.BackColor = _bmp1 is not null ? _selectedBackgroundColor : SystemColors.Control;
        label2.BackColor = SystemColors.Control;
        pictureBox1.Image = _bmp1;
    }

    private void SetBitmap2()
    {
        label2.BackColor = _bmp2 is not null ? _selectedBackgroundColor : SystemColors.Control;
        label1.BackColor = SystemColors.Control;
        pictureBox1.Image = _bmp2;
    }

    private void UpdateDiffBitmap()
    {
        if (_imgDiff is null)
        {
            return;
        }

        Image? diffImage = checkBox2.Checked ? _imgDiff.DifferenceImage?.GetAutoscaledImage() : _imgDiff.DifferenceImage;

        pictureBox2.Image = diffImage?.GetBitmap();
    }

    public void SetImages(string path1, string path2)
    {
        _path1 = path1;
        _path2 = path2;
        Image? img1 = null;
        Image? img2 = null;

        if (File.Exists(path1))
        {
            img1 = new Image(path1);
            _bmp1 = img1.GetBitmap();
        }
        else
        {
            _bmp1 = null;
        }

        if (File.Exists(path2))
        {
            img2 = new Image(path2);
            _bmp2 = img2.GetBitmap();
        }

        if (img1 is not null && img2 is not null)
        {
            _imgDiff = new ImageDiff(img1, img2);
        }
        else
        {
            _imgDiff = null;
        }

        SwitchImages(0);
        UpdateDiffBitmap();
        pictureBox1.BackColor = SystemColors.Control;
        pictureBox2.BackColor = SystemColors.Control;
    }
}
