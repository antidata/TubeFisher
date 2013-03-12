#region license
/***************************************************************************
 *		youtubeFisher
 *		Copyright(C) 2009-2011 by fremyd
 *
 *		This program is free software; you can redistribute it and/or
 *		modify it under the terms of the GNU General Public License as
 *		published by the Free Software Foundation; either version 2 of
 *		the License, or (at your option) any later version.
 *
 *		This program is distributed WITHOUT ANY WARRANTY; without even the
 *		implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *		See the GNU General Public License for more details.
 *
 *****************************************************************************/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using YouTubeFisher;

public partial class form1 : Form
{
	private bool _audio = false;
	private string dwnFile, title;
	private static string filePath;
	private WebClient wc;
	private YouTubeService yts = null;

	public form1()
	{
		InitializeComponent();
		//--> for high DPI compatibility
		this.SuspendLayout();
		this.Font = new Font("Tahoma", 8.25f);
		if (this.Font.Name != "Tahoma") this.Font = new Font("Arial", 8.25f);
		this.AutoScaleMode = AutoScaleMode.Font;
		this.AutoScaleDimensions = new SizeF(6f, 13f);
		this.ResumeLayout(false);
		//--> end

		this.Text = Application.ProductName + "  [v" + Application.ProductVersion.Substring(0, 3) + "]";
		lblResult.Text = "";
		if (Clipboard.GetText().Contains("youtube.com/") || Clipboard.GetText().Contains("youtu.be/") )
			txtUrl.Text = Clipboard.GetText().Trim();

		/*	// for beta versions only...
			int rand = new Random().Next(3);
			if (rand == 0 && System.DateTime.Now.Month > 3)
				MessageBox.Show(" A new version of youtubeFisher was released. Go to " + Environment.NewLine
							+ "             http://youtubefisher.codeplex.com " + Environment.NewLine
							+ "                          and update now!! ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		 */
	}

	private void btn_Click(object sender, EventArgs e)
	{
		_audio = true;
		btnAll_Click(sender, e);
	}

	private void btnAll_Click(object sender, EventArgs e)
	{
		filePath = null;
		lockGui();
		lblResult.Text = "Check video url..";
		lblResult.Refresh();

		// check youtube url
		string url = txtUrl.Text.Trim();

		if (url.StartsWith("https://")) url = "http://" + url.Substring(8);
		else if (!url.StartsWith("http://")) url = "http://" + url;

		url = url.Replace("youtu.be/", "youtube.com/watch?v=");
		url = url.Replace("www.youtube.com", "youtube.com");

		if (url.StartsWith("http://youtube.com/v/"))
			url = url.Replace("youtube.com/v/", "youtube.com/watch?v=");
		else if (url.StartsWith("http://youtube.com/watch#"))
			url = url.Replace("youtube.com/watch#", "youtube.com/watch?");

		if (!url.StartsWith("http://youtube.com/watch"))
		{
			MessageBox.Show("Invalid youtube url!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			unlockGui();
			return;
		}

		string link = null;
		try
		{
			yts = YouTubeService.Create(url);
			title = yts.VideoTitle;
			if ( (_audio && yts.AvailableAudioFormat.Length==0) ||
				 ( !_audio && yts.AvailableVideoFormat.Length==0)
			   )
				   throw new Exception("No videos found!");
		}
		catch (Exception exc)
		{
			MessageBox.Show(exc.Message, Application.ProductName + " error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			unlockGui();
			return;
		}

		int format = 0;
		filePath = chooseFormat(out format);

		if (format == 0)
		{
			unlockGui();
			return;
		}

		if (_audio)		// audio only
		{
			try
			{
				File.Create(filePath).Close();  //TODO: Close when end download and not here..
			}
			catch (Exception) { }
			link = yts.AvailableAudioFormat[format - 1].VideoUrl;
			dwnFile = Path.GetTempFileName();
		}
		else			// Video
		{
			link = yts.AvailableVideoFormat[format - 1].VideoUrl;
			dwnFile = filePath;
		}

		lblResult.Text = "Download file...";
		lblResult.Refresh();
		try
		{
			wc = new WebClient();
			wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
			wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
			lnkCancel.Enabled = true;
			wc.DownloadFileAsync(new Uri(link), dwnFile);
			lblResult.Visible = false;
		}
		catch (Exception)
		{
			MessageBox.Show("  Unexpected error occured.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			try { File.Delete(dwnFile); if (_audio) File.Delete(filePath); }catch (Exception) { }
			unlockGui();
		}
		lblResult.Text = "";
	}

	void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
	{
		lnkCancel.Enabled = false;

		if (e.Cancelled)
		{
			MessageBox.Show("Download canceled.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			try
			{
				File.Delete(dwnFile);
				if (_audio) File.Delete(filePath);
			}
			catch (Exception) { }
			unlockGui();
			return;
		}
		wc = null;
		try
		{
			FileInfo f = new FileInfo(dwnFile);
			if (f.Length < 4)
			{
				MessageBox.Show("  Error occured during download.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				f = null;
				try
				{
					if (_audio) File.Delete(filePath); File.Delete(dwnFile);
				}
				catch (Exception) { }
			}
			else
			{
				f = null;
				progressBar1.Value = 0;
				if (_audio)
				{
					toMp3(dwnFile);
					try { File.Delete(dwnFile); }
					catch (Exception) { }
				}
				this.Activate();
			}
		}
		catch (Exception ex)
		{
			this.Activate();
			MessageBox.Show("Error: " + ex.Message, Application.ProductName);
		}
		unlockGui();
		lblResult.Text = "Done ";
	}

	void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		progressBar1.Value = e.ProgressPercentage;
		string kb = null;
		if (!_audio) kb = "  [" + (e.BytesReceived / 1024).ToString() + "/" + (e.TotalBytesToReceive / 1024).ToString() + " Kb]";
		tooltip.SetToolTip(progressBar1, progressBar1.Value.ToString() + "% "+kb);
	}

	private void lockGui()
	{
		btnAudio.Enabled = false;
		btnAll.Enabled = false;
		txtUrl.Enabled = false;
		this.Cursor = Cursors.WaitCursor;
	}

	private void unlockGui()
	{
		_audio = false;
		btnAudio.Enabled = true;
		btnAll.Enabled = true;
		txtUrl.Enabled = true;
		txtUrl.Text = "";
		lblResult.Text = "";
		lblResult.Visible = true;
		this.Cursor = Cursors.Default;
		txtUrl.Focus();
	}

	private string getContent(string url)
	{
		string buffer;
		try
		{
			lblResult.Text = "Send request to youtube..";
			lblResult.Refresh();
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			lblResult.Text = "Get youtube response..";
			lblResult.Refresh();
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			StreamReader sr = new StreamReader(resp.GetResponseStream());
			buffer = sr.ReadToEnd();
			sr.Close();

			return buffer;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private void toMp3(string inputPath)
	{
		try
		{
			using (FLVFile flvFile = new FLVFile(Path.GetFullPath(inputPath)))
			{
				flvFile.ExtractStreams(PromptOverwrite);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message + Environment.NewLine, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	/// <returns>True if the file must be saved in the path specified</returns>
	private static bool PromptOverwrite(ref string path)
	{
		try { File.Delete(filePath); }
		catch (Exception) { }
		path = filePath;
		return true;
	}

	private void btnPaste_Click(object sender, EventArgs e)
	{
		txtUrl.Text = Clipboard.GetText().Trim();
		if (String.IsNullOrEmpty(txtUrl.Text))
			txtUrl.Focus();
	}

	private string chooseFormat(out int index)
	{
		SaveFileDialog dialog = new SaveFileDialog();
		if (_audio)
		{
			dialog.FileName = String.IsNullOrEmpty(title) ? "AudioTrack [by youtubeFisher]" : title;
			string t = "";
			foreach (YouTubeVideoFile v in yts.AvailableVideoFormat)
			{
				t += MapFormatCodeToFilter(v.FormatCode);
			}
			t = t.Substring(0, t.Length - 1);
			dialog.Filter = t;
			dialog.DefaultExt = "mp3";
		}
		else
		{
			dialog.FileName = String.IsNullOrEmpty(title) ? "video  [by youtubeFisher]" : title;
			string t = "";
			int filterIndex = 0;
			foreach (YouTubeVideoFile v in yts.AvailableVideoFormat)
			{
				t += MapFormatCodeToFilter(v.FormatCode);
				filterIndex++;

				// Select 480p Flash Video (if available) as the default option
				if (v.FormatCode == 35)
					dialog.FilterIndex = filterIndex;
			}
			if (t.Length>0) t=t.Substring(0, t.Length - 1);
			dialog.Filter = t;

			dialog.DefaultExt = "mp4";
		}
		dialog.AddExtension = true;
		dialog.RestoreDirectory = true;
		if (dialog.ShowDialog() == DialogResult.OK)
		{
			index = dialog.FilterIndex;
			return dialog.FileName;
		}
		else
		{
			index = 0;
			return null;
		}
	}

	private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		MessageBox.Show(
			String.Format(
				"    {0} v.{1} {2}{2} http://youtubefisher.codeplex.com {2}{2} Copyright © YouTubeFisher Contributors 2009 - {3} {2}{2} Developed by:{2}    * Giuseppe Cazzato {2}    * Essam Almohammadi",
				Application.ProductName,
				Application.ProductVersion,
				Environment.NewLine,
				DateTime.Now.ToString("yyyy")),
			"About",
			MessageBoxButtons.OK,
			MessageBoxIcon.Information
		);
	}

	private void lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			System.Diagnostics.Process.Start("http://youtubefisher.codeplex.com");
		}
		catch (Exception w)
		{
			MessageBox.Show(w.Message, Application.ProductName);
		}
	}

	private void lnkMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			System.Diagnostics.Process.Start(Application.ExecutablePath);
		}
		catch (Exception ew)
		{
			MessageBox.Show(ew.Message, Application.ProductName);
		}
	}

	private void lnkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (wc != null) wc.CancelAsync();
		wc = null;
	}

	private string MapFormatCodeToFilter(byte formatCode)
	{
		string formatDescription = "";

		switch (formatCode)
		{
			case 43:
				formatDescription = (_audio) ? "" : "WebM Video 360p (*.webm)|*.webm|";
				break;

			case 44:
				formatDescription = (_audio) ? "" : "WebM Video 480p (*.webm)|*.webm|";
				break;

			case 45:
				formatDescription = (_audio) ? "" : "WebM HD Video 720p (*.webm)|*.webm|";
				break;

			case 46:
				formatDescription = (_audio) ? "" : "WebM Full HD Video 1080p (*.webm)|*.webm|";
				break;

			case 38:
				formatDescription = (_audio) ? "" : "4K Resolution (*.mp4)|*.mp4|";
				break;

			case 37:
				formatDescription = (_audio) ? "" : "Full HD 1080p (*.mp4)|*.mp4|";
				break;

			case 22:
				formatDescription = (_audio) ? "" : "HD 720p (*.mp4)|*.mp4|";
				break;

			case 82:
				formatDescription = (_audio) ? "" : "3D Standard Youtube Qualty 360p (*.mp4)|*.mp4|";
				break;

			case 84:
				formatDescription = (_audio) ? "" : "3D HD 720p (*.mp4)|*.mp4|";
				break;

			case 35:
				formatDescription = (_audio) ? "[HQ] Advanced Audio Coding [44KHz] (*.aac)|*.aac|" : "HQ Flash Video 480p (*.flv)|*.flv|";
				break;

			case 34:
				formatDescription = (_audio) ? "[HQ] Advanced Audio Coding [22KHz] (*.aac)|*.aac|" : "LQ Flash Video 360p [AAC] (*.flv)|*.flv|";
				break;

			case 18:
				formatDescription = (_audio) ? "" : "Standard Youtube Qualty 360p (*.mp4)|*.mp4|";
				break;

			case 6:
				formatDescription = (_audio) ? "MP3 Audio [44KHz] (*.mp3)|*.mp3|" : "LQ Flash Video [MP3.44KHz] (*.flv)|*.flv|";
				break;

			case 5:
				formatDescription = (_audio) ? "MP3 Audio [22KHz] (*.mp3)|*.mp3|" : "LQ Flash Video [MP3.22KHz] (*.flv)|*.flv|";
				break;

			case 13:
				formatDescription = (_audio) ? "" : "Mobile Video XX-Small (*.3gp)|*.3gp|";
				break;

			case 17:
				formatDescription = (_audio) ? "" : "Mobile Video X-Small (*.3gp)|*.3gp|";
				break;

			case 36:
				formatDescription = (_audio) ? "" : "Mobile Video Small (*.3gp)|*.3gp|";
				break;

			default:
				// New Format?
				formatDescription = "";
				break;
		}

		return formatDescription;
	}
}
