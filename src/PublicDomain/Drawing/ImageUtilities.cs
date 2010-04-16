using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public const long MaxFileSize = 20000000;

        private static Brush s_whiteBrush;

        static ImageUtilities()
        {
            s_whiteBrush = new SolidBrush(Color.White);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="thumbnailWidth"></param>
        /// <param name="thumbnailHeight"></param>
        /// <param name="thumbFileName"></param>
        /// <returns></returns>
        public static bool ResizeImage(
            string fileName,
            int thumbnailWidth,
            int thumbnailHeight,
            string thumbFileName
        )
        {
            return ResizeImage(fileName, thumbnailWidth, thumbnailHeight, thumbFileName, true, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="thumbnailWidth"></param>
        /// <param name="thumbnailHeight"></param>
        /// <param name="thumbFileName"></param>
        /// <param name="highQuality"></param>
        /// <param name="keepAspectRatio"></param>
        /// <param name="cutToAspectRatio"></param>
        /// <returns></returns>
        public static bool ResizeImage(
            string fileName,
            int thumbnailWidth,
            int thumbnailHeight,
            string thumbFileName,
            bool highQuality,
            bool keepAspectRatio,
            bool cutToAspectRatio
        )
        {
            if (thumbnailWidth <= 0)
            {
                throw new ArgumentOutOfRangeException("thumbnailWidth", "Width must be greater than 0");
            }
            if (thumbnailHeight <= 0)
            {
                throw new ArgumentOutOfRangeException("thumbnailHeight", "Height must be greater than 0");
            }
            if (string.IsNullOrEmpty(thumbFileName))
            {
                throw new ArgumentException("thumbFileName", "Output file name of new thumb not specified");
            }

            FileInfo file = new FileInfo(fileName);
            if (!file.Exists)
            {
                throw new Exception("Could not find file " + fileName);
            }

            // There's a maximum file size that we're willing to load an image of into memory. otherwise, we'd
            // go OutOfMemory under simple load
            if (file.Length > MaxFileSize)
            {
                return false;
            }

            if (File.Exists(thumbFileName))
            {
                File.Delete(thumbFileName);
            }

            ImageFormat destinationFormat = null;
            string extension = Path.GetExtension(thumbFileName);
            if (!string.IsNullOrEmpty(extension))
            {
                extension = extension.ToLower();
                if (extension == ".gif")
                {
                    destinationFormat = ImageFormat.Exif;
                }
                else if (extension == ".bmp")
                {
                    destinationFormat = ImageFormat.Bmp;
                }
                else if (extension == ".ico")
                {
                    destinationFormat = ImageFormat.Icon;
                }
                else if (extension == ".png")
                {
                    destinationFormat = ImageFormat.Png;
                }
                else if (extension == ".tiff")
                {
                    destinationFormat = ImageFormat.Tiff;
                }
                else if (extension == ".jpg" || extension == ".jpeg")
                {
                    destinationFormat = ImageFormat.Jpeg;
                }
            }

            using (Image image = Image.FromFile(file.FullName, true))
            {
                float newWidth = thumbnailWidth;
                float newHeight = thumbnailHeight;
                int xOffset = 0;
                int yOffset = 0;

                if (keepAspectRatio)
                {
                    float tempDimension;
                    if (((image.Width / thumbnailWidth)
                            > (image.Width / thumbnailHeight)))
                    {
                        tempDimension = image.Width;
                        newWidth = thumbnailWidth;
                        newHeight = (image.Height
                                    * (thumbnailWidth / tempDimension));
                        if ((newHeight > thumbnailHeight))
                        {
                            newWidth = (newWidth
                                        * (thumbnailHeight / newHeight));
                            newHeight = thumbnailHeight;
                        }
                    }
                    else
                    {
                        tempDimension = image.Height;
                        newHeight = thumbnailHeight;
                        newWidth = (image.Width
                                    * (thumbnailWidth / tempDimension));
                        if ((newWidth > thumbnailWidth))
                        {
                            newHeight = (newHeight
                                        * (thumbnailWidth / newWidth));
                            newWidth = thumbnailWidth;
                        }
                    }

                    if (cutToAspectRatio)
                    {
                        thumbnailWidth = Convert.ToInt32(newWidth);
                        thumbnailHeight = Convert.ToInt32(newHeight);
                    }
                    else
                    {
                        if (newHeight < thumbnailHeight)
                        {
                            yOffset = (int)((thumbnailHeight - newHeight) / 2F);
                        }
                        if (newWidth < thumbnailWidth)
                        {
                            xOffset = (int)((thumbnailWidth - newWidth) / 2F);
                        }
                    }
                }

                try
                {
                    using (Bitmap thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight, PixelFormat.Format24bppRgb))
                    {
                        if (highQuality)
                        {
                            thumbnail.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                        }

                        using (Graphics g = Graphics.FromImage(thumbnail))
                        {
                            if (highQuality)
                            {
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                g.CompositingQuality = CompositingQuality.HighQuality;
                            }

                            if (!cutToAspectRatio)
                            {
                                g.FillRectangle(s_whiteBrush, 0, 0, thumbnail.Width, thumbnail.Height);
                            }

                            g.DrawImage(image, xOffset, yOffset, newWidth, newHeight);

                            if (destinationFormat != null)
                            {
                                if (highQuality && destinationFormat == ImageFormat.Jpeg)
                                {
                                    ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                                    EncoderParameters encoderParameters;
                                    encoderParameters = new EncoderParameters(1);
                                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                                    thumbnail.Save(thumbFileName, info[1], encoderParameters);
                                }
                                else
                                {
                                    thumbnail.Save(thumbFileName, destinationFormat);
                                }
                            }
                            else
                            {
                                thumbnail.Save(thumbFileName);
                            }
                        }
                    }
                }
                catch (OutOfMemoryException)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
