namespace SkyCES.EntLib
{
    using System;
    using System.IO;
    using System.Web;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public class UploadHelper
    {
        private string fileExtension;
        private SkyCES.EntLib.FileNameType fileNameType = SkyCES.EntLib.FileNameType.Date;
        private string fileType = string.Empty;
        private long localFileLength;
        private string localFileName;
        private string localFilePath;
        private string path = string.Empty;
        private HttpPostedFile postedFile;
        private string saveFileFolderPath;
        private string saveFileFullPath;
        private string saveFileName;
        //private int sizes = 0x100000;
        private int sizes = 1024;

        private int allImageIsNail = 0;
        private int maxWidth = 750;

        private string GetSaveFileFolderPath()
        {
            string path = string.Empty;
            path = ServerHelper.MapPath(this.path);
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                info.Create();
            }
            return path;
        }

        public FileInfo SaveAs()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            FileInfo info = null;
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    this.postedFile = files[i];
                    this.localFilePath = this.postedFile.FileName;
                    if ((this.localFilePath == null) || (this.localFilePath == ""))
                    {
                        throw new Exception("不能上传空文件");
                    }
                    this.localFileLength = this.postedFile.ContentLength;
                    if (this.localFileLength >= (this.sizes * 0x400))
                    {
                        throw new Exception("上传的文件不能大于:" + this.sizes + "KB");
                    }
                    this.saveFileFolderPath = this.GetSaveFileFolderPath();
                    this.localFileName = System.IO.Path.GetFileName(this.postedFile.FileName);
                    this.fileExtension = FileHelper.GetFileExtension(this.localFileName);
                    if (this.fileType.ToLower().IndexOf(this.fileExtension.ToLower()) == -1)
                    {
                        throw new Exception("目前本系统支持的格式为:" + this.fileType);
                    }
                    this.saveFileName = FileHelper.CreateFileName(this.fileNameType, this.localFileName, this.fileExtension);
                    this.saveFileFullPath = this.saveFileFolderPath + this.saveFileName;
                    this.postedFile.SaveAs(this.saveFileFullPath);


                    if (allImageIsNail == 1)//如果网站设置开启压缩图片
                    {
                        if (this.fileExtension == ".jpg" || this.fileType.ToLower() == ".jpeg" || this.fileType.ToLower() == ".gif" || this.fileType.ToLower() == ".png" || this.fileType.ToLower() == ".bmp")//如果后缀名为图片格式则进行压缩
                        {
                            Image image = Image.FromFile(this.saveFileFullPath);
                            if (image.Width > maxWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                            {
                                string filePath = this.saveFileFolderPath + this.saveFileName;
                                string nailPath = this.saveFileFolderPath + "temp_nail" + this.fileExtension;
                                ImageHelper.MakeThumbnailImage(this.saveFileFullPath, nailPath, maxWidth, maxWidth, ThumbnailType.WidthFix);
                                image.Dispose();
                                System.IO.File.Delete(filePath);//删除原图
                                System.IO.File.Move(nailPath, filePath);//重命名压缩后的文件                            
                            }
                            image.Dispose();
                        }
                    }

                    info = new FileInfo(this.saveFileFolderPath + this.saveFileName);
                }
            }
            catch
            {
                throw;
            }
            return info;
        }

        public FileInfo SaveAs(int waterType, int waterPossition, string text, string textFont, int textSize, string textColor, string waterPhoto)
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            FileInfo info = null;
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    this.postedFile = files[i];
                    this.localFilePath = this.postedFile.FileName;
                    if ((this.localFilePath == null) || (this.localFilePath == ""))
                    {
                        throw new Exception("不能上传空文件");
                    }
                    this.localFileLength = this.postedFile.ContentLength;
                    if (this.localFileLength >= (this.sizes * 0x400))
                    {
                        throw new Exception("上传的文件不能大于:" + this.sizes + "KB");
                    }
                    this.saveFileFolderPath = this.GetSaveFileFolderPath();
                    this.localFileName = System.IO.Path.GetFileName(this.postedFile.FileName);
                    this.fileExtension = FileHelper.GetFileExtension(this.localFileName);
                    if (this.fileType.ToLower().IndexOf(this.fileExtension.ToLower()) == -1)
                    {
                        throw new Exception("目前本系统支持的格式为:" + this.fileType);
                    }
                    this.saveFileName = FileHelper.CreateFileName(this.fileNameType, this.localFileName, this.fileExtension);
                    this.saveFileFullPath = this.saveFileFolderPath + this.saveFileName;
                    this.postedFile.SaveAs(this.saveFileFullPath);

                    var imgTypes = ".jpg|.gif|.bmp|.png|.jpeg";
                    if (imgTypes.IndexOf(this.fileExtension.ToLower()) != -1)//必须是图片类型才能加压缩
                    {
                        if (allImageIsNail == 1)//如果网站设置开启压缩图片
                        {
                            Image image = Image.FromFile(this.saveFileFullPath);
                            if (image.Width > maxWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                            {
                                string filePath = this.saveFileFolderPath + this.saveFileName;
                                string nailPath = this.saveFileFolderPath + "temp_nail" + this.fileExtension;
                                ImageHelper.MakeThumbnailImage(this.saveFileFullPath, nailPath, maxWidth, maxWidth, ThumbnailType.WidthFix);
                                image.Dispose();
                                System.IO.File.Delete(filePath);//删除原图
                                System.IO.File.Move(nailPath, filePath);//重命名压缩后的文件                            
                            }
                            image.Dispose();
                        }

                        //添加水印
                        string sFileName = System.IO.Path.GetFileNameWithoutExtension(saveFileName) + "_wm" + this.fileExtension;
                        string newPath = System.IO.Path.Combine(this.saveFileFolderPath, sFileName);
                        if (waterType == 2)
                        {
                            ImageHelper.AddTextWater(saveFileFullPath, newPath, waterPossition, text, textFont, textColor, textSize);
                        }
                        else
                        {
                            ImageHelper.AddImageWater(saveFileFullPath, newPath, waterPossition, waterPhoto);
                        }
                        //删除没上水印的老图
                        //if (System.IO.File.Exists(saveFileFullPath))
                        //{
                        //    System.IO.File.Delete(saveFileFullPath);
                        //}

                        info = new FileInfo(newPath);
                    }
                }
            }
            catch
            {
                throw;
            }
            return info;
        }

        public FileInfo SaveFromTaobao(int waterType, int waterPossition, string text, string textFont, int textSize, string textColor, string waterPhoto,string fileName)
        {
            this.saveFileFolderPath = ServerHelper.MapPath("~/Upload/TaoBaoPhoto/Original/");
            FileInfo info = new FileInfo(ServerHelper.MapPath(fileName));
            try
            {
                if (fileName == "")
                {
                    throw new Exception("不能上传空文件");
                }
                this.localFileLength = info.Length;
                if (this.localFileLength >= (this.sizes * 0x400))
                {
                    throw new Exception("上传的图片不能大于:" + this.sizes + "KB");
                }
                this.fileExtension = FileHelper.GetFileExtension(fileName);
                if (this.fileType.ToLower().IndexOf(this.fileExtension) == -1)
                {
                    throw new Exception("目前本系统支持的格式为:" + this.fileType);
                }

                this.saveFileFullPath = ServerHelper.MapPath(fileName);

                var imgTypes = ".jpg|.gif|.bmp|.png|.jpeg";
                if (imgTypes.IndexOf(this.fileExtension.ToLower()) != -1)//必须是图片类型才能加压缩
                {
                    if (allImageIsNail == 1)//如果网站设置开启压缩图片
                    {
                        Image image = Image.FromFile(this.saveFileFullPath);
                        if (image.Width > maxWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                        {
                            string filePath = this.saveFileFullPath;
                            string nailPath = this.saveFileFolderPath + "temp_nail" + this.fileExtension;
                            ImageHelper.MakeThumbnailImage(this.saveFileFullPath, nailPath, maxWidth, maxWidth, ThumbnailType.WidthFix);
                            image.Dispose();
                            System.IO.File.Delete(filePath);//删除原图
                            System.IO.File.Move(nailPath, filePath);//重命名压缩后的文件                            
                        }
                        image.Dispose();
                    }

                    //添加水印
                    string sFileName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_wm" + this.fileExtension;
                    string newPath = System.IO.Path.Combine(this.saveFileFolderPath, sFileName);
                    if (waterType == 2)
                    {
                        ImageHelper.AddTextWater(saveFileFullPath, newPath, waterPossition, text, textFont, textColor, textSize);
                    }
                    else
                    {
                        ImageHelper.AddImageWater(saveFileFullPath, newPath, waterPossition, waterPhoto);
                    }
                    //删除没上水印的老图
                    //if (System.IO.File.Exists(saveFileFullPath))
                    //{
                    //    System.IO.File.Delete(saveFileFullPath);
                    //}

                    info = new FileInfo(newPath);
                }
            }
            catch
            {
                throw;
            }
            return info;
        }
        /// <summary>
        /// 保留原文件名上传文件
        /// </summary>
        /// <param name="fileName">原文件名，如果为空则生成文件名</param>
        /// <returns></returns>
        public FileInfo SaveAs(string fileName)
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            FileInfo info = null;
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    this.postedFile = files[i];
                    this.localFilePath = this.postedFile.FileName;
                    if ((this.localFilePath == null) || (this.localFilePath == ""))
                    {
                        throw new Exception("不能上传空文件");
                    }
                    this.localFileLength = this.postedFile.ContentLength;
                    if (this.localFileLength >= (this.sizes * 0x400))
                    {
                        throw new Exception("上传的图片不能大于:" + this.sizes + "KB");
                    }
                    this.saveFileFolderPath = this.GetSaveFileFolderPath();
                    this.localFileName = System.IO.Path.GetFileName(this.postedFile.FileName);
                    this.fileExtension = FileHelper.GetFileExtension(this.localFileName);
                    if (this.fileType.ToLower().IndexOf(this.fileExtension) == -1)
                    {
                        throw new Exception("目前本系统支持的格式为:" + this.fileType);
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        this.saveFileName = fileName;
                    }
                    else
                    {
                        this.saveFileName = FileHelper.CreateFileName(this.fileNameType, this.localFileName, this.fileExtension);
                    }
                    this.saveFileFullPath = this.saveFileFolderPath + this.saveFileName;
                    this.postedFile.SaveAs(this.saveFileFullPath);

                    if (allImageIsNail == 1)//如果网站设置开启压缩图片
                    {
                        if (this.fileExtension == ".jpg" || this.fileType.ToLower() == ".jpeg" || this.fileType.ToLower() == ".gif" || this.fileType.ToLower() == ".png" || this.fileType.ToLower() == ".bmp")//如果后缀名为图片格式则进行压缩
                        {
                            Image image = Image.FromFile(this.saveFileFullPath);
                            if (image.Width > maxWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                            {
                                string filePath = this.saveFileFolderPath + this.saveFileName;
                                string nailPath = this.saveFileFolderPath + "temp_nail" + this.fileExtension;
                                ImageHelper.MakeThumbnailImage(this.saveFileFullPath, nailPath, maxWidth, maxWidth, ThumbnailType.WidthFix);
                                image.Dispose();
                                System.IO.File.Delete(filePath);//删除原图
                                System.IO.File.Move(nailPath, filePath);//重命名压缩后的文件                            
                            }
                            image.Dispose();
                        }
                    }
                    info = new FileInfo(this.saveFileFolderPath + this.saveFileName);
                }
            }
            catch
            {
                throw;
            }
            return info;
        }


        public FileInfo SaveFromTaobao(string fileName)
        {
            this.saveFileFolderPath = ServerHelper.MapPath("~/Upload/TaoBaoPhoto/Original/");
            FileInfo info = new FileInfo(ServerHelper.MapPath(fileName));
            try
            {
                if (fileName == "")
                {
                    throw new Exception("不能上传空文件");
                }
                this.localFileLength = info.Length;
                if (this.localFileLength >= (this.sizes * 0x400))
                {
                    throw new Exception("上传的图片不能大于:" + this.sizes + "KB");
                }
                this.fileExtension = FileHelper.GetFileExtension(fileName);
                if (this.fileType.ToLower().IndexOf(this.fileExtension) == -1)
                {
                    throw new Exception("目前本系统支持的格式为:" + this.fileType);
                }

                this.saveFileFullPath = ServerHelper.MapPath(fileName);

                if (this.allImageIsNail == 1)//如果网站设置开启压缩图片
                {
                    if (this.fileExtension == ".jpg" || this.fileType.ToLower() == ".jpeg" || this.fileType.ToLower() == ".gif" || this.fileType.ToLower() == ".png" || this.fileType.ToLower() == ".bmp")//如果后缀名为图片格式则进行压缩
                    {
                        Image image = Image.FromFile(this.saveFileFullPath);
                        if (image.Width > maxWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                        {
                            string filePath = this.saveFileFullPath;
                            string nailPath = this.saveFileFolderPath + "temp_nail" + this.fileExtension;
                            ImageHelper.MakeThumbnailImage(this.saveFileFullPath, nailPath, maxWidth, maxWidth, ThumbnailType.WidthFix);
                            image.Dispose();
                            System.IO.File.Delete(filePath);//删除原图
                            System.IO.File.Move(nailPath, filePath);//重命名压缩后的文件                            
                        }
                        image.Dispose();
                    }
                }
                info = new FileInfo(saveFileFullPath);
            }
            catch
            {
                throw;
            }
            return info;
        }

        /// <summary>
        /// 生成缩略水印图
        /// </summary>
        /// <param name="waterType"></param>
        /// <param name="waterPossition"></param>
        /// <param name="text"></param>
        /// <param name="textFont"></param>
        /// <param name="textSize"></param>
        /// <param name="textColor"></param>
        /// <param name="waterPhoto"></param>
        /// <returns></returns>
        public FileInfo SaveAs(int waterType, int waterPossition, string text, string textFont, int textSize, string textColor, string waterPhoto, string filePath, string fileExtension, string fileName)
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            FileInfo info = null;
            try
            {       this.saveFileFolderPath =ServerHelper.MapPath(filePath);
                    this.fileExtension = fileExtension;
                   
                    //this.saveFileName = FileHelper.CreateFileName(this.fileNameType, this.localFileName, this.fileExtension);
                    this.saveFileName = fileName + fileExtension;

                    this.saveFileFullPath = this.saveFileFolderPath + this.saveFileName;                 
                    //this.postedFile.SaveAs(this.saveFileFullPath);
                //如果原始图片存在
                    if (System.IO.File.Exists(saveFileFullPath))
                    {
                        var imgTypes = ".jpg|.gif|.bmp|.png|.jpeg";
                        if (imgTypes.IndexOf(this.fileExtension.ToLower()) != -1)//必须是图片类型才能加压缩
                        {
                            if (allImageIsNail == 1)//如果网站设置开启压缩图片
                            {
                                Image image = Image.FromFile(this.saveFileFullPath);
                                if (image.Width > maxWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                                {
                                    //string filePath = this.saveFileFullPath;
                                    string nailPath = this.saveFileFolderPath + "temp_nail" + this.fileExtension;
                                    ImageHelper.MakeThumbnailImage(this.saveFileFullPath, nailPath, maxWidth, maxWidth, ThumbnailType.WidthFix);
                                    image.Dispose();
                                    System.IO.File.Delete(this.saveFileFullPath);//删除原图
                                    System.IO.File.Move(nailPath, this.saveFileFullPath);//重命名压缩后的文件                            
                                }
                                image.Dispose();
                            }

                            //添加水印
                            string sFileName = System.IO.Path.GetFileNameWithoutExtension(saveFileName) + "_wm" + this.fileExtension;
                            string newPath = System.IO.Path.Combine(this.saveFileFolderPath, sFileName);
                            if (waterType == 2)
                            {
                                ImageHelper.AddTextWater(saveFileFullPath, newPath, waterPossition, text, textFont, textColor, textSize);
                            }
                            else
                            {
                                ImageHelper.AddImageWater(saveFileFullPath, newPath, waterPossition, waterPhoto);
                            }
                            //删除没上水印的老图
                            //if (System.IO.File.Exists(saveFileFullPath))
                            //{
                            //    System.IO.File.Delete(saveFileFullPath);
                            //}

                            info = new FileInfo(newPath);
                        }
                    }
               
            }
            catch
            {
                throw;
            }
            return info;
        }

        public SkyCES.EntLib.FileNameType FileNameType
        {
            get
            {
                return this.fileNameType;
            }
            set
            {
                this.fileNameType = value;
            }
        }

        public string FileType
        {
            get
            {
                return this.fileType;
            }
            set
            {
                this.fileType = value;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public int Sizes
        {
            get
            {
                return this.sizes;
            }
            set
            {
                this.sizes = value;
            }
        }
        /// <summary>
        /// 最大图片宽度
        /// </summary>
        public int MaxWidth
        {
            get
            {
                return this.maxWidth;
            }
            set
            {
                this.maxWidth = value;
            }
        }
        /// <summary>
        /// 是否自动压缩图片
        /// </summary>
        public int AllImageIsNail
        {
            get
            {
                return this.allImageIsNail;
            }
            set
            {
                this.allImageIsNail = value;
            }
        }
    }
}

