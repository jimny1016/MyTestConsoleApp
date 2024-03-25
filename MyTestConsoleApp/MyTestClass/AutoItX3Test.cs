using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoIt;

namespace MyTestConsoleApp.MyTestClass
{
    internal class AutoItX3Test
    {
        // 導入AutoItX3.dll的功能
        [DllImport("plugin/AutoItX3_x64.dll", SetLastError = true)]
        public static extern void AU3_Init();

        [DllImport("plugin/AutoItX3_x64.dll", SetLastError = true)]
        public static extern int AU3_WinWait([MarshalAs(UnmanagedType.LPWStr)] string title,
            [MarshalAs(UnmanagedType.LPWStr)] string text, int timeout);

        [DllImport("plugin/AutoItX3_x64.dll", SetLastError = true)]
        public static extern int AU3_WinSetState([MarshalAs(UnmanagedType.LPWStr)] string title,
            [MarshalAs(UnmanagedType.LPWStr)] string text, int flags);

        [DllImport("plugin/AutoItX3_x64.dll", SetLastError = true)]
        public static extern int AU3_ControlClick([MarshalAs(UnmanagedType.LPWStr)] string title,
            [MarshalAs(UnmanagedType.LPWStr)] string text, [MarshalAs(UnmanagedType.LPWStr)] string control,
            [MarshalAs(UnmanagedType.LPWStr)] string button, int numClicks, int x, int y);

        public AutoItX3Test()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Firmwares", "K2862_50343_64LQFP_HV133_20240308_2439H.exe");

            //AutoItX.Run(filePath, "", AutoItX.SW_SHOW);
            AutoItX.Run(filePath, "", AutoItX.SW_HIDE);
            AutoItX.WinWait("Holtek TinyProgrammer V1.0.0", "", 1);
            AutoItX.WinSetState("Holtek TinyPorgrammer V1.0.0", "", 0);
            AutoItX.ControlClick("Holtek TinyPorgrammer V1.0.0", "", "[CLASS:Button; TEXT:Connect]");
            AutoItX.WinWait("Holtek TinyProgrammer V1.0.0", "", 10);
            AutoItX.WinClose("Holtek TinyPorgrammer V1.0.0");
            //AutoItX.WinClose();            
            //IntPtr winHandle = AutoItX.WinGetHandle("Holtek TinyProgrammer V1.0.0");
            //AutoItX.WinKill(winHandle);

            //var title = AutoItX.WinGetTitle();
            //// 初始化AutoIt
            //AU3_Init();

            //// 啟動EXE檔案（請將路徑替換為您的實際應用程式路徑）
            //System.Diagnostics.Process.Start(filePath);

            //// 等待視窗出現
            //var ddd = AU3_WinWait("Holtek TinyProgrammer V1.0.0", "", 10);

            //// 隱藏視窗
            //var dddd = AU3_WinSetState("Holtek TinyProgrammer V1.0.0", "", 2); // 2代表SW_HIDE

            //// 等待一小段時間
            //Thread.Sleep(1000);

            //// 點擊'Connect'按鈕
            //AU3_ControlClick("Holtek TinyProgrammer V1.0.0", "", "[CLASS:Button; TEXT:Connect]", "left", 1, -1, -1);

            //// 可能需要一點時間去處理
            //Thread.Sleep(5000);

            //// 操作完成
            //Console.WriteLine("Operation completed");
         }
    }
}
