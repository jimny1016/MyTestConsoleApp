using Emgu.CV;
using MediaInfoNET;
using System.Diagnostics;

namespace MyTestConsoleApp.MyTestClass
{
    internal class TranslateVideo
    {
        public TranslateVideo()
        {
            var filePath = "C:\\Users\\JimmyCHU\\Downloads\\OliysFailVideo1.mp4";
            var ddd = IsDashFormat(filePath);
            var filePathNew = "C:\\Users\\JimmyCHU\\Downloads\\OliysFailVideo1New.mp4";
            ConvertDashToMp4(filePath, filePathNew);
            Console.WriteLine("Hello, World!");
            //foreach (var value in Enum.GetValues(typeof(VideoCapture.API)))
            //{
            //}
            VideoCapture videoCapture = new VideoCapture(filePath);
            if (videoCapture.IsOpened)
            {
                Mat mat = new Mat();
                videoCapture.Read(mat);
                if (mat.IsEmpty)
                {
                    Console.WriteLine("Faile!");
                }
                else
                {
                    Console.WriteLine("Read Success!");
                }
            }
            else
            {
                Console.WriteLine("Open Fail!");
            }

            Console.ReadLine();
        }


        public static string GetVideoFormat(string filePath)
        {
            var mediaInfo = new MediaFile(filePath);

            return mediaInfo.General.Format;
        }

        public static bool IsDashFormat(string filePath)
        {
            string format = GetVideoFormat(filePath);
            return format.Equals("dash", StringComparison.OrdinalIgnoreCase);
        }

        public static void ConvertDashToMp4(string inputFilePath, string outputFilePath)
        {
            // 假设您的应用程序目录下有一个名为 "ffmpeg-bin" 的文件夹，其中包含 ffmpeg.exe
            string ffmpegExecutablePath = "C:\\Jimmy\\Tools\\ffmpeg-master-latest-win64-gpl-shared\\bin\\ffmpeg.exe";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegExecutablePath,
                    Arguments = $"-i \"{inputFilePath}\" -c:v libx264 -profile:v high -crf 20 -c:a aac -strict experimental \"{outputFilePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}
