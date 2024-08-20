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

        public String PageId { get; set; }

        public ImageAdjustments ImageAdjustments { get; set; }

        public PageImageFormat[] ImageFormat { get; set; }

        public PageImageFormat SourceFormat { get; set; }

        public Image[] ResultImage { get; set; }

        public Stream[] ResultStream { get; set; }

        public Page[] ResultPage { get; set; }

        public LocalFile[] ResultFileName { get; set; }

        public LocalFile SourceFileName { get; set; }

        public List<String> Tasks { get; set; }

        public bool Success = false;

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

            int tempFileCounter = 0;

            LocalFile inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.GetInstance().Make() + "." + tempFileCounter.ToString() + ".tmp");

            foreach (String task in Tasks)
            {
                
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
                        case TASK_RESIZE:                           
                            targetFormat.W = ImageAdjustments.ResizeTo.X;
                            targetFormat.H = ImageAdjustments.ResizeTo.Y;

                            ImageOperations.ResizeImage(inputStream, ref outputStream, targetFormat, ImageAdjustments.Interpolation);

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
                    Success = false;
                }
                finally
                {
                    outputStream?.Close();
                    inputStream?.Close();

                    outputStream?.Dispose();
                    inputStream?.Dispose();
                }

                tempFileCounter++;
            }

            try
            {
                inProgressFile.LocalFileInfo.MoveTo(ResultFileName[0].FullPath);

                ResultFileName[0].Refresh();

                Success = true; 
            } catch (Exception e) { Success = false; }
                
            


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
            //ResultPage[0].UpdateTemporaryFile(new LocalFile(ResultFileName[0].FullPath));
            //ResultPage[0].CreateLocalWorkingCopy();
            //ResultPage[0].IsMemoryCopy = false;

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
    }
}
