using GraphicalTestRunner;
using System.Data;
using System.Diagnostics;
using Color = System.Drawing.Color;

namespace Graphical_Test_Runner;

public partial class CollectionCompareForm : Form
{
    private FolderComparisonResults? _folderResults;

    public CollectionCompareForm()
    {
        InitializeComponent();
        Width = 890;
        Height = 832;

        string docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string defaultFolder = Path.Combine(docsFolder, @"ScottPlot\TestImageCollections");

        defaultFolder = Directory.Exists(defaultFolder) ? Directory.GetDirectories(defaultFolder).Last() : "C:/path/to/old/images/";

        tbBefore.Text = defaultFolder;
        tbAfter.Text = Path.GetFullPath(@"..\..\..\..\..\..\..\dev\www\cookbook\5.0\images");

        btnHelp.Click += (s, e) => new HelpForm().Show();

        btnAnalyze.Click += (s, e) =>
        {
            if (btnAnalyze.Text == "Analyze")
            {
                Analyze();
            }
            else
            {
                _stop = true;
            }
        };

        dataGridView1.SelectionChanged += (s, e) =>
        {
            if (_folderResults is null)
            {
                return;
            }

            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount == 0)
            {
                return;
            }

            string? selectedFilename = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int index = Array.IndexOf(_folderResults.Filenames, selectedFilename);
            string path1 = _folderResults.GetPath1(index);
            string path2 = _folderResults.GetPath2(index);
            imageComparer1.SetImages(path1, path2);
        };

        dataGridView1.Sorted += (s, e) =>
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Recolor(dataGridView1.Rows[i]);
            }
        };

        btnUT.Click += (s, e) =>
        {
            string path = Path.GetFullPath("../../../../../../../src/ScottPlot5/ScottPlot5 Tests/Unit Tests/bin/Debug/net6.0/test-images");
            Process.Start("explorer.exe", path);
        };

        btnCB.Click += (s, e) =>
        {
            string path = Path.GetFullPath("../../../../../../../dev/www/cookbook/5.0/images");
            Process.Start("explorer.exe", path);
        };

        btn1.Click += (s, e) =>
        {
            string path = Path.GetFullPath(tbBefore.Text);
            Process.Start("explorer.exe", path);
        };

        btn2.Click += (s, e) =>
        {
            string path = Path.GetFullPath(tbAfter.Text);
            Process.Start("explorer.exe", path);
        };
    }

    private bool _stop;

    private void Analyze()
    {
        _folderResults = new FolderComparisonResults(tbBefore.Text, tbAfter.Text);

        _stop = false;
        btnAnalyze.Text = "Stop";

        DataTable table = new DataTable();
        table.Columns.Add("name", typeof(string));
        table.Columns.Add("change", typeof(string));
        table.Columns.Add("total diff", typeof(double));
        table.Columns.Add("max diff", typeof(double));

        dataGridView1.DataSource = table;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.MultiSelect = false;
        dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        const int maxImageCount = int.MaxValue;
        //MAX_IMAGE_COUNT = 20;

        for (int i = 0; i < Math.Min(_folderResults.ImageDiffs.Length, maxImageCount); i++)
        {
            if (_stop)
            {
                break;
            }

            progressBar1.Maximum = _folderResults.ImageDiffs.Length;
            progressBar1.Value = i + 1;
            Text = $"Analyzing {i + 1} of {_folderResults.ImageDiffs.Length}...";
            _folderResults.Analyze(i);
            Application.DoEvents();

            if (checkHideUnchanged.Checked && _folderResults.Summaries[i] == "unchanged")
            {
                continue;
            }

            DataRow row = table.NewRow();
            row.SetField(0, _folderResults.Filenames[i]);
            row.SetField(1, _folderResults.Summaries[i]);
            row.SetField(2, _folderResults.ImageDiffs[i]?.TotalDifference);
            row.SetField(3, _folderResults.ImageDiffs[i]?.MaxDifference);

            table.Rows.Add(row);
            Recolor(dataGridView1.Rows[table.Rows.Count - 1]);
            dataGridView1.AutoResizeColumns();

            if (table.Rows.Count == 1)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        Text = $"Analyzed {_folderResults.ImageDiffs.Length} image pairs";
        progressBar1.Value = 0;
        btnAnalyze.Text = "Analyze";
    }

    private void Recolor(DataGridViewRow row)
    {
        if (row.Cells[1].Value.ToString() == "changed")
        {
            row.Cells[1].Style.BackColor = Color.Yellow;
        }
        else if (row.Cells[1].Value.ToString() == "unchanged")
        {
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                row.Cells[j].Style.BackColor = SystemColors.ControlLight;
                row.Cells[j].Style.ForeColor = SystemColors.ControlDark;
            }
        }
        else
        {
            row.Cells[1].Style.BackColor = Color.LightSteelBlue;
        }
    }
}
