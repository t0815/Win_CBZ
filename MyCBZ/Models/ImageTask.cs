using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public ImageAdjustments ImageAdjustments { get; set; }

        public PageImageFormat[] ImageFormat { get; set; }

        public PageImageFormat SourceFormat { get; set; }

        public Image[] ResultImage { get; set; }

        public Image[] ResultThumbnail { get; set; }

        public Stream[] ResultStream { get; set; }

        public LocalFile[] ResultFileName { get; set; }

        public LocalFile SourceFileName { get; set; }

        public LocalFile[] PreviewFile { get; set; }

        public List<String> Tasks { get; set; }

        public bool Success = false;

        public Page[] ResultPages { get; set; }

        public Page SourcePage { get; set; }

        protected List<LocalFile> FilesToCleanup { get; set; }


        public ImageTask()
        {
            Tasks = new List<String>();
            ImageAdjustments = new ImageAdjustments();
            ResultImage = new Image[2];
            PreviewFile = new LocalFile[2];
            ResultThumbnail = new Image[2];
            ResultFileName = new LocalFile[2];
            ImageFormat = new PageImageFormat[2];
            ResultPages = new Page[2];
            FilesToCleanup = new List<LocalFile>();

        }

        public ImageTask SetupTasks(Page source)
        {
            try
            {
                PreviewFile[0] = new LocalFile(source.TemporaryFile.FilePath + RandomId.getInstance().make() + ".0.prev");
                PreviewFile[1] = new LocalFile(source.TemporaryFile.FilePath + RandomId.getInstance().make() + ".1.prev");
               // File.Copy(source.TemporaryFile.FullPath, PreviewFile[0].FullPath, true);
               // File.Copy(source.TemporaryFile.FullPath, PreviewFile[1].FullPath, true);

                ResultFileName[0] = new LocalFile(source.TemporaryFile.FilePath + RandomId.getInstance().make() + ".0.res");
                ResultFileName[1] = new LocalFile(source.TemporaryFile.FilePath + RandomId.getInstance().make() + ".1.res");
                //File.Copy(source.TemporaryFile.FullPath, ResultFileName[0].FullPath, true);
                //File.Copy(source.TemporaryFile.FullPath, ResultFileName[1].FullPath, true);

                //Tasks = commandsTodo;
                ImageFormat[0] = source.Format;
                SourceFormat = source.Format;
                SourcePage = source;
            }
            catch (Exception)
            {

            }    
            
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
        
        public ImageTask Apply()
        {
            PageImageFormat targetFormat = null;
            Stream outputStream = null;
            Stream inputStream = SourcePage.GetImageStream();

            int tempFileCounter = 0;

            LocalFile inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.getInstance().make() + "." + tempFileCounter.ToString() + ".tmp");

            foreach (String task in Tasks)
            {
                targetFormat = new PageImageFormat(SourcePage.Format);
                targetFormat.W = ImageAdjustments.ResizeTo.X;
                targetFormat.H = ImageAdjustments.ResizeTo.Y;

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

                        inProgressFile = new LocalFile(SourcePage.TemporaryFile.FilePath + RandomId.getInstance().make() + "." + tempFileCounter.ToString() + ".tmp");

                        outputStream = File.Open(inProgressFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    }

                    inProgressFile.Refresh();

                    FilesToCleanup.Add(new LocalFile(inProgressFile.FullPath));

                    switch (task)
                    {
                        case TASK_RESIZE:
                            ImageOperations.ResizeImage(SourcePage.GetImageStream(), ref outputStream, targetFormat, ImageAdjustments.Interpolation);

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
