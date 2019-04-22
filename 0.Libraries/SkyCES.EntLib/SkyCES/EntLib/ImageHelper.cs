namespace SkyCES.EntLib
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public sealed class ImageHelper
    {
        public static void AddImageWater(string oldImage, string newImage, int waterPossition, string waterImage)
        {
            using (Image image = Image.FromFile(oldImage))
            {
                Guid guid = image.FrameDimensionsList[0];
                FrameDimension dimension = new FrameDimension(guid);
                if (image.GetFrameCount(dimension) == 1)
                {
                    using (Bitmap bitmap = new Bitmap(image))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.InterpolationMode = InterpolationMode.High;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            using (Image image2 = Image.FromFile(waterImage))
                            {
                                if ((image.Width > image2.Width) && (image.Height > image2.Height))
                                {
                                    int x = 0;
                                    int y = 0;
                                    switch (waterPossition)
                                    {
                                        case 2:
                                            y = (image.Height / 2) - (image2.Height / 2);
                                            break;

                                        case 3:
                                            y = (image.Height - image2.Height) - 10;
                                            break;

                                        case 4:
                                            x = (image.Width / 2) - (image2.Width / 2);
                                            break;

                                        case 5:
                                            x = (image.Width / 2) - (image2.Width / 2);
                                            y = (image.Height / 2) - (image2.Height / 2);
                                            break;

                                        case 6:
                                            x = (image.Width / 2) - (image2.Width / 2);
                                            y = (image.Height - image2.Height) - 10;
                                            break;

                                        case 7:
                                            x = (image.Width - image2.Width) - 20;
                                            break;

                                        case 8:
                                            x = (image.Width - image2.Width) - 20;
                                            y = (image.Height / 2) - (image2.Height / 2);
                                            break;

                                        case 9:
                                            x = (image.Width - image2.Width) - 20;
                                            y = (image.Height - image2.Height) - 10;
                                            break;
                                    }
                                    Rectangle rec = new Rectangle(x, y, image2.Width, image2.Height);
                                    //graphics.DrawImage(image2, x, y);
                                    graphics.DrawImage(image2, rec);

                                    ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                                    ImageCodecInfo encoder = null;
                                    foreach (ImageCodecInfo info3 in imageEncoders)
                                    {
                                        if (info3.MimeType == "image/jpeg")
                                        {
                                            encoder = info3;
                                        }
                                    }
                                    EncoderParameters encoderParams = new EncoderParameters();
                                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                                    bitmap.Save(newImage, encoder, encoderParams);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void AddTextWater(string oldImage, string newImage, int waterPossition, string waterText, string textFont, string textColor, int textSize)
        {
            using (Image image = Image.FromFile(oldImage))
            {
                Guid guid = image.FrameDimensionsList[0];
                FrameDimension dimension = new FrameDimension(guid);
                if (image.GetFrameCount(dimension) == 1)
                {
                    using (Bitmap bitmap = new Bitmap(image))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.InterpolationMode = InterpolationMode.High;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            Font font = new Font(textFont, (float)textSize, GraphicsUnit.Pixel);
                            Brush brush = new SolidBrush(ColorTranslator.FromHtml(textColor));
                            float x = 0f;
                            float y = 0f;
                            int num4 = 0;
                            char[] chArray = waterText.ToCharArray();
                            for (int i = 0; i < chArray.Length; i++)
                            {
                                if ((chArray[i] >= '一') && (chArray[i] <= 0x9fa5))
                                {
                                    num4++;
                                }
                            }
                            int num6 = textSize * (waterText.Length - num4);
                            num6 = (num6 / 2) + ((textSize + 2) * num4);
                            if (bitmap.Width > num6)
                            {
                                switch (waterPossition)
                                {
                                    case 2:
                                        y = (bitmap.Height / 2) - (textSize / 2);
                                        break;

                                    case 3:
                                        y = (bitmap.Height - textSize) - 10;
                                        break;

                                    case 4:
                                        x = (bitmap.Width / 2) - (num6 / 2);
                                        break;

                                    case 5:
                                        x = (bitmap.Width / 2) - (num6 / 2);
                                        y = (bitmap.Height / 2) - (textSize / 2);
                                        break;

                                    case 6:
                                        x = (bitmap.Width / 2) - (num6 / 2);
                                        y = (bitmap.Height - textSize) - 10;
                                        break;

                                    case 7:
                                        x = (bitmap.Width - num6) - 20;
                                        break;

                                    case 8:
                                        x = (bitmap.Width - num6) - 20;
                                        y = (bitmap.Height / 2) - (textSize / 2);
                                        break;

                                    case 9:
                                        x = (bitmap.Width - num6) - 20;
                                        y = (bitmap.Height - textSize) - 10;
                                        break;
                                }
                                graphics.DrawString(waterText, font, brush, x, y);
                                bitmap.Save(newImage, GetImageFormat(Path.GetExtension(oldImage)));
                            }
                        }
                    }
                }
            }
        }

        private static ImageFormat GetImageFormat(string extension)
        {
            ImageFormat memoryBmp = ImageFormat.MemoryBmp;
            switch (extension.ToLower())
            {
                case ".bmp":
                    return ImageFormat.Bmp;

                case ".emf":
                    return ImageFormat.Emf;

                case ".exif":
                    return ImageFormat.Exif;

                case ".gif":
                    return ImageFormat.Gif;

                case ".icon":
                    return ImageFormat.Icon;

                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".jpg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".tiff":
                    return ImageFormat.Tiff;

                case ".wmf":
                    return ImageFormat.Wmf;
            }
            return memoryBmp;
        }

        public static void MakeThumbnailImage(string bigImage, string smallImage, int imageWidth, int imageHeight, ThumbnailType thumbnailType)
        {
            if (File.Exists(bigImage))
            {
                try
                {
                    DirectoryInfo info = new DirectoryInfo(smallImage.Substring(0, smallImage.LastIndexOf(@"\")) + @"\");
                    if (!info.Exists)
                    {
                        info.Create();
                    }
                }
                catch
                {
                }
                using (Image image = Image.FromFile(bigImage))
                {
                    switch (thumbnailType)
                    {
                        case ThumbnailType.WidthFix:
                            imageHeight = Convert.ToInt32((double)((Convert.ToDouble(image.Height) * Convert.ToDouble(imageWidth)) / Convert.ToDouble(image.Width)));
                            goto Label_01C9;

                        case ThumbnailType.HeightFix:
                            imageWidth = Convert.ToInt32((double)((Convert.ToDouble(image.Width) * Convert.ToDouble(imageHeight)) / Convert.ToDouble(image.Height)));
                            goto Label_01C9;

                        case ThumbnailType.InBox:
                            if ((Convert.ToDouble(image.Width) / ((double)image.Height)) >= (Convert.ToDouble(imageWidth) / ((double)imageHeight)))
                            {
                                imageHeight = Convert.ToInt32((double)((Convert.ToDouble(image.Height) * Convert.ToDouble(imageWidth)) / Convert.ToDouble(image.Width)));
                            }
                            else
                            {
                                imageWidth = Convert.ToInt32((double)((Convert.ToDouble(image.Width) * Convert.ToDouble(imageHeight)) / Convert.ToDouble(image.Height)));
                            }
                            goto Label_01C9;

                        case ThumbnailType.OutBox:
                            if ((Convert.ToDouble(image.Width) / ((double)image.Height)) < (Convert.ToDouble(imageWidth) / ((double)imageHeight)))
                            {
                                break;
                            }
                            imageWidth = Convert.ToInt32((double)((Convert.ToDouble(image.Width) * Convert.ToDouble(imageHeight)) / Convert.ToDouble(image.Height)));
                            goto Label_01C9;

                        default:
                            goto Label_01C9;
                    }
                    imageHeight = Convert.ToInt32((double)((Convert.ToDouble(image.Height) * Convert.ToDouble(imageWidth)) / Convert.ToDouble(image.Width)));
                Label_01C9:
                    using (Bitmap bitmap = new Bitmap(imageWidth, imageHeight))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.InterpolationMode = InterpolationMode.High;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.Clear(Color.White);
                            graphics.DrawImage(image, new Rectangle(0, 0, imageWidth, imageHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                            ImageCodecInfo encoder = null;
                            foreach (ImageCodecInfo info3 in imageEncoders)
                            {
                                if (info3.MimeType == "image/jpeg")
                                {
                                    encoder = info3;
                                }
                            }
                            EncoderParameters encoderParams = new EncoderParameters();
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 95L);
                            bitmap.Save(smallImage, encoder, encoderParams);
                        }
                        return;
                    }
                }
            }
        }
    }
}

