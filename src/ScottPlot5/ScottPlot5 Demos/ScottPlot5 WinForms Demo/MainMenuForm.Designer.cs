﻿namespace WinForms_Demo;

partial class MainMenuForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        pictureBox1 = new PictureBox();
        label2 = new Label();
        label1 = new Label();
        tbSearch = new TextBox();
        label3 = new Label();
        demoWindowScrollList1 = new DemoWindowScrollList();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.ErrorImage = null;
        pictureBox1.Image = Properties.Resources.scottplot_64;
        pictureBox1.Location = new Point(14, 16);
        pictureBox1.Margin = new Padding(3, 4, 3, 4);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(73, 85);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI Semilight", 12F);
        label2.Location = new Point(98, 65);
        label2.Name = "label2";
        label2.Size = new Size(222, 28);
        label2.TabIndex = 2;
        label2.Text = "ScottPlot.WinForms 5.0.0";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 20.25F);
        label1.Location = new Point(94, 16);
        label1.Name = "label1";
        label1.Size = new Size(257, 46);
        label1.TabIndex = 1;
        label1.Text = "ScottPlot Demo";
        // 
        // tbSearch
        // 
        tbSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        tbSearch.BackColor = SystemColors.Control;
        tbSearch.Location = new Point(447, 63);
        tbSearch.Margin = new Padding(3, 4, 3, 4);
        tbSearch.Name = "tbSearch";
        tbSearch.Size = new Size(190, 27);
        tbSearch.TabIndex = 3;
        // 
        // label3
        // 
        label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        label3.AutoSize = true;
        label3.Location = new Point(389, 67);
        label3.Name = "label3";
        label3.Size = new Size(56, 20);
        label3.TabIndex = 4;
        label3.Text = "Search:";
        // 
        // demoWindowScrollList1
        // 
        demoWindowScrollList1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        demoWindowScrollList1.BackColor = SystemColors.Control;
        demoWindowScrollList1.Location = new Point(0, 111);
        demoWindowScrollList1.Margin = new Padding(3, 4, 3, 4);
        demoWindowScrollList1.Name = "demoWindowScrollList1";
        demoWindowScrollList1.Size = new Size(687, 747);
        demoWindowScrollList1.TabIndex = 5;
        // 
        // MainMenuForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        ClientSize = new Size(689, 860);
        Controls.Add(demoWindowScrollList1);
        Controls.Add(label3);
        Controls.Add(tbSearch);
        Controls.Add(label2);
        Controls.Add(pictureBox1);
        Controls.Add(label1);
        Margin = new Padding(3, 4, 3, 4);
        Name = "MainMenuForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot Demo";
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private PictureBox pictureBox1;
    private Label label2;
    private Label label1;
    private TextBox tbSearch;
    private Label label3;
    private DemoWindowScrollList demoWindowScrollList1;
}
