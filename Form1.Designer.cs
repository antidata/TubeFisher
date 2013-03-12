
partial class form1
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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form1));
        this.txtUrl = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.btnAudio = new System.Windows.Forms.Button();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.lblResult = new System.Windows.Forms.Label();
        this.lblExample = new System.Windows.Forms.Label();
        this.btnAll = new System.Windows.Forms.Button();
        this.tooltip = new System.Windows.Forms.ToolTip(this.components);
        this.btnPaste = new System.Windows.Forms.Button();
        this.lnkAbout = new System.Windows.Forms.LinkLabel();
        this.lnkHelp = new System.Windows.Forms.LinkLabel();
        this.lnkMore = new System.Windows.Forms.LinkLabel();
        this.lnkCancel = new System.Windows.Forms.LinkLabel();
        this.SuspendLayout();
        // 
        // txtUrl
        // 
        this.txtUrl.Location = new System.Drawing.Point(3, 32);
        this.txtUrl.MaxLength = 500;
        this.txtUrl.Name = "txtUrl";
        this.txtUrl.Size = new System.Drawing.Size(291, 20);
        this.txtUrl.TabIndex = 0;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.BackColor = System.Drawing.Color.Transparent;
        this.label1.Font = new System.Drawing.Font("Book Antiqua", 9.5F);
        this.label1.ForeColor = System.Drawing.Color.DarkBlue;
        this.label1.Location = new System.Drawing.Point(3, 13);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(112, 17);
        this.label1.TabIndex = 1;
        this.label1.Text = "Youtube video url";
        // 
        // btnAudio
        // 
        this.btnAudio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
        this.btnAudio.Location = new System.Drawing.Point(167, 80);
        this.btnAudio.Name = "btnAudio";
        this.btnAudio.Size = new System.Drawing.Size(144, 23);
        this.btnAudio.TabIndex = 4;
        this.btnAudio.Text = "extract Audio";
        this.btnAudio.UseVisualStyleBackColor = true;
        this.btnAudio.Click += new System.EventHandler(this.btn_Click);
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new System.Drawing.Point(1, 129);
        this.progressBar1.MarqueeAnimationSpeed = 25;
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(317, 18);
        this.progressBar1.TabIndex = 0;
        // 
        // lblResult
        // 
        this.lblResult.BackColor = System.Drawing.Color.Transparent;
        this.lblResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
        this.lblResult.Location = new System.Drawing.Point(0, 131);
        this.lblResult.Margin = new System.Windows.Forms.Padding(0);
        this.lblResult.Name = "lblResult";
        this.lblResult.Size = new System.Drawing.Size(316, 18);
        this.lblResult.TabIndex = 0;
        this.lblResult.Text = "*** lblResult ***";
        this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // lblExample
        // 
        this.lblExample.AutoSize = true;
        this.lblExample.BackColor = System.Drawing.Color.Transparent;
        this.lblExample.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
        this.lblExample.ForeColor = System.Drawing.Color.DimGray;
        this.lblExample.Location = new System.Drawing.Point(3, 52);
        this.lblExample.Name = "lblExample";
        this.lblExample.Size = new System.Drawing.Size(268, 13);
        this.lblExample.TabIndex = 0;
        this.lblExample.Text = "Example: http://youtube.com/watch?v=NXabvZzSDNI";
        // 
        // btnAll
        // 
        this.btnAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
        this.btnAll.Location = new System.Drawing.Point(3, 80);
        this.btnAll.Name = "btnAll";
        this.btnAll.Size = new System.Drawing.Size(165, 23);
        this.btnAll.TabIndex = 3;
        this.btnAll.Text = "download Video";
        this.btnAll.UseVisualStyleBackColor = true;
        this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
        // 
        // btnPaste
        // 
        this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.btnPaste.FlatAppearance.BorderSize = 0;
        this.btnPaste.Image = global::YouTubeFisher.Properties.Resources.FromClipboard;
        this.btnPaste.Location = new System.Drawing.Point(287, 30);
        this.btnPaste.Margin = new System.Windows.Forms.Padding(0);
        this.btnPaste.Name = "btnPaste";
        this.btnPaste.Size = new System.Drawing.Size(24, 24);
        this.btnPaste.TabIndex = 2;
        this.tooltip.SetToolTip(this.btnPaste, "Paste text from clipboard");
        this.btnPaste.UseVisualStyleBackColor = false;
        this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
        // 
        // lnkAbout
        // 
        this.lnkAbout.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.lnkAbout.AutoSize = true;
        this.lnkAbout.BackColor = System.Drawing.Color.Transparent;
        this.lnkAbout.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkAbout.Location = new System.Drawing.Point(270, 2);
        this.lnkAbout.Name = "lnkAbout";
        this.lnkAbout.Size = new System.Drawing.Size(35, 13);
        this.lnkAbout.TabIndex = 6;
        this.lnkAbout.TabStop = true;
        this.lnkAbout.Text = "About";
        this.lnkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAbout_LinkClicked);
        // 
        // lnkHelp
        // 
        this.lnkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.lnkHelp.AutoSize = true;
        this.lnkHelp.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkHelp.Location = new System.Drawing.Point(238, 2);
        this.lnkHelp.Name = "lnkHelp";
        this.lnkHelp.Size = new System.Drawing.Size(29, 13);
        this.lnkHelp.TabIndex = 5;
        this.lnkHelp.TabStop = true;
        this.lnkHelp.Text = "Help";
        this.lnkHelp.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
        // 
        // lnkMore
        // 
        this.lnkMore.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.lnkMore.AutoSize = true;
        this.lnkMore.BackColor = System.Drawing.Color.Transparent;
        this.lnkMore.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkMore.Location = new System.Drawing.Point(222, 111);
        this.lnkMore.Name = "lnkMore";
        this.lnkMore.Size = new System.Drawing.Size(87, 13);
        this.lnkMore.TabIndex = 7;
        this.lnkMore.TabStop = true;
        this.lnkMore.Text = "Download more..";
        this.lnkMore.UseWaitCursor = true;
        this.lnkMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMore_LinkClicked);
        // 
        // lnkCancel
        // 
        this.lnkCancel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkCancel.AutoSize = true;
        this.lnkCancel.BackColor = System.Drawing.Color.Transparent;
        this.lnkCancel.Enabled = false;
        this.lnkCancel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lnkCancel.Location = new System.Drawing.Point(3, 111);
        this.lnkCancel.Name = "lnkCancel";
        this.lnkCancel.Size = new System.Drawing.Size(40, 13);
        this.lnkCancel.TabIndex = 8;
        this.lnkCancel.TabStop = true;
        this.lnkCancel.Text = "Cancel";
        this.lnkCancel.UseWaitCursor = true;
        this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCancel_LinkClicked);
        // 
        // form1
        // 
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
        this.ClientSize = new System.Drawing.Size(312, 148);
        this.Controls.Add(this.lnkAbout);
        this.Controls.Add(this.lnkCancel);
        this.Controls.Add(this.lnkMore);
        this.Controls.Add(this.lnkHelp);
        this.Controls.Add(this.btnPaste);
        this.Controls.Add(this.btnAll);
        this.Controls.Add(this.lblExample);
        this.Controls.Add(this.lblResult);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.txtUrl);
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.btnAudio);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.Name = "form1";
        this.Text = "youtubeFisher";
        this.ResumeLayout(false);
        this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.TextBox txtUrl;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.Button btnAudio;
	private System.Windows.Forms.ProgressBar progressBar1;
	private System.Windows.Forms.Label lblResult;
	private System.Windows.Forms.Label lblExample;
	private System.Windows.Forms.Button btnAll;
	private System.Windows.Forms.Button btnPaste;
	private System.Windows.Forms.ToolTip tooltip;
	private System.Windows.Forms.LinkLabel lnkAbout;
	private System.Windows.Forms.LinkLabel lnkHelp;
    private System.Windows.Forms.LinkLabel lnkMore;
    private System.Windows.Forms.LinkLabel lnkCancel;
}

