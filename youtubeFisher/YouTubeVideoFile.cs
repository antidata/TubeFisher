namespace YouTubeFisher
{
	internal class YouTubeVideoFile
	{
		private byte formatCode;

		public YouTubeVideoFile(string url, byte formatCode)
		{
			this.VideoUrl = url;
			this.FormatCode = formatCode;
		}

		public string VideoUrl { get; set; }

		public YouTubeVideoType VideoFormat { get; private set; }

		public byte FormatCode
		{
			get { return this.formatCode; }
			set
			{
				this.formatCode = value;

				switch (value)
				{
					case 34:
					case 35:
					case 5:
					case 6:
						this.VideoFormat = YouTubeVideoType.Flash;
						break;

					case 18:
					case 22:
					case 37:
					case 38:
					case 82:
					case 84:
						this.VideoFormat = YouTubeVideoType.MP4;
						break;

					case 13:
					case 17:
					case 36:
						this.VideoFormat = YouTubeVideoType.Mobile;
						break;

					case 43:
					case 45:
					case 46:
						this.VideoFormat = YouTubeVideoType.WebM;
						break;

					default:
						this.VideoFormat = YouTubeVideoType.Unknown;
						break;
				}
			}
		}

		public long VideoSize { get; set; }
	}
}
