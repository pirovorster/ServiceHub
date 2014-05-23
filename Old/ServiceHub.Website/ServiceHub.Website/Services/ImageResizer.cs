using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing.Drawing2D;
using System;
using System.IO;

namespace ServiceHub.Website
{
	internal sealed class ImageResizer
	{
		private readonly int _widthThreshold;
		private readonly int _heightThreshold;
		private readonly int _compression;


		internal ImageResizer(int widthThreshold, int heightThreshold, int compression)
		{
			_widthThreshold = widthThreshold;
			_heightThreshold = heightThreshold;
			_compression = compression;
		}

		internal void ResizeImage(Image source, Stream stream)
		{
			if (source == null)
				throw new ArgumentNullException("source");


			Size size = DetermineScalingSize(source, _widthThreshold, _heightThreshold);

			ImageConverter imageConverter = new ImageConverter();

			using (Image resized = source.GetThumbnailImage(size.Width, size.Height, () => false, IntPtr.Zero))
			{
				ImageCodecInfo jpgEncoder = GetEncoderInfo("image/jpeg");
				using (EncoderParameters encoderParameters = GetEncodeParamaters(_compression))
				{
					resized.Save(stream, jpgEncoder, encoderParameters);
				}
			}


		}

		private Size DetermineScalingSize(Image image, int widthThreshold, int heightThreshold)
		{
			if (image.Width < widthThreshold && image.Height < heightThreshold)
				return new Size(image.Width, image.Height);

			double ratioWidth = ((double)widthThreshold / (double)image.Width);
			double ratioHeight = ((double)heightThreshold / (double)image.Height);
			double ratio = Math.Min(ratioWidth, ratioHeight);

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			return new Size(newWidth, newHeight);
		}

		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			return ImageCodecInfo.GetImageEncoders().Single(o => o.MimeType == mimeType);
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "I will dispose it in the callin function")]
		private static EncoderParameters GetEncodeParamaters(int compression)
		{
			EncoderParameters encoderParameters = new EncoderParameters(1);
			Encoder encoder = Encoder.Quality;
			EncoderParameter encoderParameter = new EncoderParameter(encoder, compression);
			encoderParameters.Param[0] = encoderParameter;

			return encoderParameters;

		}
	}
}
