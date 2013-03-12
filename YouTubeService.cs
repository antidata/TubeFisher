using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace YouTubeFisher
{
	internal class YouTubeService
	{
		private const string VideoInfoPageUrl = "http://www.youtube.com/get_video_info?&video_id={0}&el=detailpage&ps=default&eurl=&gl=US&hl=en";
		private const string VideoUrlsSeparator = ",";

		private string videoId;
		private NameValueCollection videoInfoCollection;
		private List<YouTubeVideoFile> availableVideoFormat = new List<YouTubeVideoFile>();

		private YouTubeService()
		{
			// Does nothing more than preventing the class from being instantiated.
		}

		public string VideoUrl { get; private set; }

		public string VideoTitle { get; private set; }

		public YouTubeVideoFile[] AvailableVideoFormat
		{
			// Get (the format of) all videos available..
			get { return this.availableVideoFormat.ToArray(); }
		}

		public YouTubeVideoFile[] AvailableAudioFormat
		{
			get
			{
				return this.availableVideoFormat.FindAll(
					delegate(YouTubeVideoFile v)
						{
							return v.VideoFormat == YouTubeVideoType.Flash;
						}).ToArray();
			}
		}

		public static YouTubeService Create(string youTubeVideoUrl)
		{
			YouTubeService service = new YouTubeService();
			service.VideoUrl = youTubeVideoUrl;
			service.videoId = HttpUtility.ParseQueryString(new Uri(service.VideoUrl).Query)["v"];

			service.GetVideoInfo();
			service.GetVideoTitle();
			service.PopulateAvailableVideoFormatList();

			return service;
		}

		private void GetVideoInfo()
		{
			string videoInfoPageSource = this.DownloadString(string.Format(VideoInfoPageUrl, this.videoId));
			this.videoInfoCollection = HttpUtility.ParseQueryString(videoInfoPageSource);
		}

		private void GetVideoTitle()
		{
			this.VideoTitle = this.videoInfoCollection["title"];

			// Remove the invalid characters in file names
			// In Windows they are: \ / : * ? " < > |
			this.VideoTitle = Regex.Replace(this.VideoTitle, @"[:\*\?""\<\>\|]", string.Empty);
			this.VideoTitle = this.VideoTitle.Replace("\\", "-").Replace("/", "-").Trim();

			if (string.IsNullOrEmpty(this.VideoTitle))
			{
				this.VideoTitle = this.videoId;
			}
		}

		private string DownloadString(string url)
		{
			WebRequest req = WebRequest.Create(url);
			HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
			string source = new StreamReader(resp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
			resp.Close();

			return source;
		}

		private void PopulateAvailableVideoFormatList()
		{
			var availableFormats = this.videoInfoCollection["url_encoded_fmt_stream_map"];

			if (availableFormats == string.Empty) { return; }

			var formatList = new List<string>(Regex.Split(availableFormats, VideoUrlsSeparator));

			formatList.ForEach(
				delegate(string format)
				{
					if (string.IsNullOrEmpty(format.Trim())) { return; }

					var formatInfoCollection = HttpUtility.ParseQueryString(format);
					var urlEncoded = formatInfoCollection["url"];
					var itag = formatInfoCollection["itag"];
					var signature = formatInfoCollection["sig"];
					var fallbackHost = formatInfoCollection["fallback_host"];

					var formatCode = byte.Parse(itag);

					urlEncoded = string.Format("{0}&fallback_host={1}&signature={2}", urlEncoded, fallbackHost, signature);

					var url = new Uri(HttpUtility.UrlDecode(HttpUtility.UrlDecode(urlEncoded)));

					// for this version, only use the download URL
					this.availableVideoFormat.Add(new YouTubeVideoFile(url.ToString(), formatCode));
				});
		}
	}
}
