using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Handler;
using Win_CBZ.Helper;
using Win_CBZ.Img;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Win_CBZ.Models
{
    [SupportedOSPlatform("windows")]
    internal class ImageTask
    {
        public const string TASK_SPLIT = "SplitImage";
        
        public const string TASK_RESIZE = "ResizeImage";

        public const string TASK_CONVERT = "ConvertImage";

        public const string TASK_ROTATE = "RotateImage";

        public String PageId { get; set; }

        public bool UseLocalTask { get; set; }

        public ImageAdjustments ImageAdjustments { get; set; }

        public PageImageFormat[] ImageFormat { get; set; }

        public PageImageFormat SourceFormat { get; set; }

        public Image[] ResultImage { get; set; }

        public Stream[] ResultStream { get; set; }

        public Page[] ResultPage { get; set; }

        public LocalFile[] ResultFileName { get; set; }

        public LocalFile SourceFileName { get; set; }

        public List<String> Tasks { get; set; }

        public ImageTaskOrder TaskOrder { get; set; }

        public bool Success { get; set;  } = false ;

        public Page SourcePage { get; set; }

        protected List<LocalFile> FilesToCleanup { get; set; }


        public ImageTask(String pageId)
        {
            Tasks = new List<String>();
            ImageAdjustments = new ImageAdjustments();
            ResultImage = new Image[2];
            ResultFileName = new LocalFile[2];
            FilesToCleanup = new List<LocalFile>();
            ImageFormat = new PageImageFormat[2];
            ResultPage = new Page[2];
            PageId = pageId;
            TaskOrder = new ImageTaskOrder();
        }

        public ImageTask SetupTasks(ref Page source)
        {
            
            ResultFileName[0] = new LocalFile(source.TemporaryFile.FilePath + RandomId.GetInstance().Make() + ".0.res");
            ResultFileName[1] = new LocalFile(source.TemporaryFile.FilePath + RandomId.GetInstance().Make() + ".1.res");
            //File.Copy(source.TemporaryFile.FullPath, ResultFileName[0].FullPath, true);
            //File.Copy(source.TemporaryFile.FullPath, ResultFileName[1].FullPath, true);

            //Tasks = commandsTodo;

            ImageFormat[0] = source?.Format;
            SourceFormat = source?.Format;
            SourcePage = source;
            string[] orderedTasks = new string[4];
            int index = 0;  
            int autoIndex = 0;
            foreach (string task in Tasks) 
            {
                switch (task) 
                {
                    case TASK_ROTATE:
                        index = TaskOrder.Rotate.HasFlag(ImageTaskOrderValue.Auto) ? autoIndex : (int)TaskOrder.Rotate - 1;
                        orderedTasks[index] = task;
                        break;
                    case TASK_RESIZE:
                        index = TaskOrder.Resize.HasFlag(ImageTaskOrderValue.Auto) ? autoIndex : (int)TaskOrder.Resize - 1;
                        orderedTasks[index] = task;
                        break;
                    case TASK_CONVERT:
                        index = TaskOrder.Convert.HasFlag(ImageTaskOrderValue.Auto) ? autoIndex : (int)TaskOrder.Convert - 1;
                        orderedTasks[index] = task;
                        break;
                    case TASK_SPLIT:
                        index = TaskOrder.Split.HasFlag(ImageTaskOrderValue.Auto) ? autoIndex : (int)TaskOrder.Split - 1;
                        orderedTasks[index] = task;
                        break;
                }

                autoIndex++;
                
            }

            Tasks = orderedTasks.ToList();  
            
            return this;
        }

        public ImageTask SetTaskResize() 
        {
            if (Tasks.IndexOf(TASK_RESIZE) == -1)
            {
                Tasks.Add(TASK_RESIZE);
            }

            return this;
        }

        public ImageTask SetTaskSplit()
        {
            if (Tasks.IndexOf(TASK_SPLIT) == -1)
            {
                Tasks.Add(TASK_SPLIT);
            }

            return this;
        }

        public ImageTask SetTaskRotate()
        {
            if (Tasks.IndexOf(TASK_ROTATE) == -1)
            {
                Tasks.Add(TASK_ROTATE);
            }

            return this;
        }

        public ImageTask SetTaskConvert()
        {
            if (Tasks.IndexOf(TASK_CONVERT) == -1)
            {
                Tasks.Add(TASK_CONVERT);
            }

            return this;
        }

        public ImageTask Apply()
        {
            PageImageFormat targetFormat = new PageImageFormat(SourcePage.Format);
            Stream outputStream = null;
            Stream inputStream = SourcePage.GetImageStream();
            Image imageInfo = null;

            int tempFileCounter = 0;

            LocalFile inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.GetInstance().Make() + "." + tempFileCounter.ToString() + ".tmp");
            

            foreach (String task in Tasks)
            {
                
                if (task == null || task == TASK_SPLIT)
                {
                    continue;
                }

                try
                {
                    if (outputStream == null)
                    {
                        outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    } else
                    {
                        outputStream.CopyTo(inputStream);
                        outputStream.Close();

                        inputStream.Position = 0;

                        inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.GetInstance().Make() + "." + tempFileCounter.ToString() + ".tmp");

                        outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    }

                    inProgressFile.Refresh();

                    FilesToCleanup.Add(new LocalFile(inProgressFile.FullPath));

                    switch (task)
                    {
                        case TASK_ROTATE:
                            int rotate = 0;

                            switch (ImageAdjustments.RotateMode)
                            {
                                case 1:
                                    rotate = 90;
                                    break;
                                case 2:
                                    rotate = 180;
                                    break;
                                case 3:
                                    rotate = 270;
                                    break;
                            }

                            ImageOperations.RotateImage(ref inputStream, ref outputStream, targetFormat, rotate);
                            break;

                        case TASK_RESIZE:                           
                            targetFormat.W = ImageAdjustments.ResizeTo.X;
                            targetFormat.H = ImageAdjustments.ResizeTo.Y;

                            if (ImageAdjustments.ResizeMode == 1)  // Resize to page
                            {
                                if (targetFormat.W == 0 || targetFormat.H == 0)
                                {
                                    throw new PageException(SourcePage, "Error resizing page! Width and/or Height must not be <= 0!");
                                }

                                if (ImageAdjustments.IgnoreDoublePagesResizingToPage)
                                {
                                    if (SourcePage.DoublePage || SourcePage.Format.W > SourcePage.Format.H)
                                    {
                                        continue; // Skip resizing for double pages
                                    }
                                }
                            }

                            if (ImageAdjustments.ResizeMode == 2)  // Resize to fixed size
                            {
                                if (ImageAdjustments.KeepAspectRatio)
                                {
                                    float ratio = 1.0f;

                                    //Math.Min(targetFormat.W / SourceFormat.W, targetFormat.H / SourceFormat.H);                               

                                    if (targetFormat.H == 0)
                                    {
                                        ratio = (float)targetFormat.W / SourceFormat.W;
                                    }
                                    else if (targetFormat.W == 0)
                                    {
                                        ratio = (float)targetFormat.H / SourceFormat.H;
                                    }
                                    else if (targetFormat.W > 0 && targetFormat.H > 0)
                                    {
                                        //targetFormat.W = (int)(targetFormat.H * (ImageAdjustments.ResizeTo.X / ImageAdjustments.ResizeTo.Y));
                                        //targetFormat.H = (int)(targetFormat.W * (ImageAdjustments.ResizeTo.Y / ImageAdjustments.ResizeTo.X));
                                    }

                                    targetFormat.W = (int)(SourceFormat.W * ratio);
                                    targetFormat.H = (int)(SourceFormat.H * ratio);
                                }
                            }

                            if (ImageAdjustments.ResizeMode == 3)  // Resize percentage
                            {
                                targetFormat.W = (int)(SourceFormat.W * (ImageAdjustments.ResizeToPercentage / 100));
                                targetFormat.H = (int)(SourceFormat.H * (ImageAdjustments.ResizeToPercentage / 100));
                            }

                            if (ImageAdjustments.DontStretch)
                            {
                               if (targetFormat.W > SourceFormat.W)
                                {
                                    targetFormat.W = SourceFormat.W;
                                }

                                if (targetFormat.H > SourceFormat.H)
                                {
                                    targetFormat.H = SourceFormat.H;
                                }
                            }

                            ImageOperations.ResizeImage(ref inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

                            break;
                        case TASK_CONVERT:                           
                            targetFormat.Format = ImageAdjustments.ConvertFormat.Format;

                            ImageOperations.ConvertImage(inputStream, outputStream, targetFormat);

                            if (SourcePage.FileExtension.TrimStart('.').ToLower() != IndexToDataMappings.GetInstance().GetImageFormatNameFromIndex(SourcePage.ImageTask.ImageAdjustments.ConvertType))
                            {
                                SourcePage.UpdateImageExtension(IndexToDataMappings.GetInstance().GetImageFormatNameFromIndex(SourcePage.ImageTask.ImageAdjustments.ConvertType));

                                AppEventHandler.OnPageChanged(null, new PageChangedEvent(SourcePage, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                            }
                            
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message);

                    Success = false;
                }
                finally
                {
                    //outputStream?.Close();
                    //inputStream?.Close();

                    //outputStream?.Dispose();
                    //inputStream?.Dispose();
                }

                tempFileCounter++;
            }

            outputStream?.Close();
            outputStream?.Dispose();

            try
            {
                inProgressFile.Refresh();

                if (inProgressFile.FileSize == 0)
                {
                    if (inputStream.Length == 0)
                    {
                        throw new PageException(SourcePage, "Error processing image! Image is empty!");
                    }

                    if (inputStream.CanRead)
                    {
                        inputStream.Position = 0;
                    }
                    else
                    {
                        throw new PageException(SourcePage, "Error processing image! Image stream is not readable!");
                    }

                    outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    inputStream.CopyTo(outputStream);

                    outputStream.Close();
                }

                inputStream.Close();
                inputStream.Dispose();

                SourcePage.FreeImage();

                inProgressFile.Refresh();

                inProgressFile.LocalFileInfo.MoveTo(ResultFileName[0].FullPath);

                ResultFileName[0].Refresh();

                if (ImageAdjustments.SplitOnlyDoublePages)
                {
                    imageInfo = Image.FromFile(ResultFileName[0].FullPath);
                    if (imageInfo.Width < imageInfo.Height && !SourcePage.DoublePage)
                    {
                        // Not a double page, so we remove the split task again
                        Tasks.Remove(TASK_SPLIT);
                    }
                    imageInfo.Dispose();
                    imageInfo = null;
                }

                // If the task is split, create a second file
                if (Tasks.Contains(TASK_SPLIT))
                {
                    LocalFile splitSource = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.GetInstance().Make());

                    ResultFileName[1] = new LocalFile(ResultFileName[0].FullPath.Replace(".0.res", ".1.res"));
                    
                    File.Copy(ResultFileName[0].FullPath, ResultFileName[1].FullPath, true);
                    File.Copy(ResultFileName[0].FullPath, splitSource.FullPath);

                    ResultFileName[1].Refresh();

                    inputStream = File.Open(splitSource.FullPath, FileMode.Open, FileAccess.Read);

                    inProgressFile = new LocalFile(splitSource.FilePath + RandomId.GetInstance().Make() + "." + tempFileCounter.ToString() + ".tmp");
                    outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    tempFileCounter++;

                    imageInfo = Image.FromStream(stream: inputStream,
                                                     useEmbeddedColorManagement: false,
                                                     validateImageData: false);

                    switch (ImageAdjustments.SplitType)
                    {
                        case 0:
                            targetFormat.X = 0;
                            targetFormat.Y = 0;
                            targetFormat.W = (int)(imageInfo.Width * ImageAdjustments.SplitPageAt / 100);
                            targetFormat.H = imageInfo.Height;

                            ImageOperations.CutImage(ref inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

                            outputStream.Close();
                            outputStream.Dispose();

                            File.Copy(inProgressFile.FullPath, ResultFileName[0].FullPath, true);

                            inputStream.Position = 0;

                            inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.GetInstance().Make() + "." + tempFileCounter.ToString() + ".tmp");
                            outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                            //outputStream = File.Open(ResultFileName[1].FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                            targetFormat.X = (int)(imageInfo.Width * ImageAdjustments.SplitPageAt / 100);
                            targetFormat.Y = 0;
                            targetFormat.W = imageInfo.Width - targetFormat.X;
                            targetFormat.H = imageInfo.Height;

                            ImageOperations.CutImage(ref inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

                            outputStream.Close();
                            outputStream.Dispose();

                            inputStream.Close();
                            inputStream.Dispose();

                            File.Copy(inProgressFile.FullPath, ResultFileName[1].FullPath, true);


                            break;
                        case 1:
                            targetFormat.X = 0;
                            targetFormat.Y = 0;
                            targetFormat.W = ImageAdjustments.SplitPageAt;
                            targetFormat.H = imageInfo.Height;

                            ImageOperations.CutImage(ref inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

                            outputStream.Close();
                            outputStream.Dispose();

                            File.Copy(inProgressFile.FullPath, ResultFileName[0].FullPath, true);

                            inputStream.Position = 0;

                            inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.GetInstance().Make() + "." + tempFileCounter.ToString() + ".tmp");
                            outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                            //outputStream = File.Open(ResultFileName[1].FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                            targetFormat.X = ImageAdjustments.SplitPageAt + 1;
                            targetFormat.Y = 0;
                            targetFormat.W = imageInfo.Width - targetFormat.X;
                            targetFormat.H = imageInfo.Height;

                            ImageOperations.CutImage(ref inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

                            outputStream.Close();
                            outputStream.Dispose();

                            inputStream.Close();
                            inputStream.Dispose();

                            File.Copy(inProgressFile.FullPath, ResultFileName[1].FullPath, true);


                            break;
                            
                    }

                    //ImageOperations.CutImage(ref inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

                }

                Success = true; 
            } catch (Exception e) 
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message));
                Success = false; 
            } finally
            {
                imageInfo?.Dispose();

                outputStream?.Close();
                inputStream?.Close();

                outputStream?.Dispose();
                inputStream?.Dispose();
            }
                
            return this;
        }

        public int TaskCount()
        { 
            return Tasks.Count;
        }

        public Stream[] GetResultStream()
        {
            ResultStream = new Stream[2];

            ResultStream[0] = File.OpenRead(ResultFileName[0].FullPath);

            return ResultStream;    
        }

        public Page[] GetResultPage()
        {
            ResultPage = new Page[2];

            ResultPage[0] = new Page(new LocalFile(ResultFileName[0].FullPath), SourcePage.WorkingDir, FileAccess.ReadWrite);
            ResultPage[0].UpdatePageAttributes(SourcePage);
            ResultPage[0].Compressed = false;
            //ResultPage[0].UpdateTemporaryFile(new LocalFile(ResultFileName[0].FullPath));
            //ResultPage[0].CreateLocalWorkingCopy();
            //ResultPage[0].IsMemoryCopy = false;

            if (ResultFileName[1] != null && ResultFileName[1].Exists())
            {
                ResultPage[1] = new Page(new LocalFile(ResultFileName[1].FullPath), SourcePage.WorkingDir, FileAccess.ReadWrite);
                ResultPage[1].UpdatePageAttributes(SourcePage);
                ResultPage[1].Id = Guid.NewGuid().ToString(); // Important! Need to create a new Id for the second page
                ResultPage[1].Name = SourcePage.NameWithoutExtension() + "_split" + SourcePage.FileExtension;
                //ResultPage[1].

                //AppEventHandler.OnPageChanged(null, new PageChangedEvent(ResultPage[1], null, PageChangedEvent.IMAGE_STATUS_NEW, true));

            }

            return ResultPage;
        }

        public ImageTask CleanUp()
        {

            foreach (LocalFile lf in FilesToCleanup)
            {
                lf.LocalFileInfo.Delete();
            }

            return this;
        }

        public ImageTask FreeResults()
        {
            foreach (Page result in ResultPage)
            {
                result?.FreeImage();
                result?.FreeStreams();
            }

            if (ResultStream != null)
            {
                foreach (Stream resultStrm in ResultStream)
                {
                    resultStrm?.Close();
                    resultStrm?.Dispose();
                }
            }

            //SourcePage?.Close();

            return this;
        }
    }
}
