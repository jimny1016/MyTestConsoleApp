using AutoIt;
using System.Runtime.InteropServices;

namespace MyTestConsoleApp.MyTestClass
{
    internal class AutoItX3Test
    {
        public AutoItX3Test()
        {
            Y70UpdateFW();
        }

        private void Y70UpdateFW()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Y70UpdateFiles", "RTDCustomerTool_V1.1_2555", "RCDTool.exe");
            AutoItX.Run(filePath, "", AutoItX.SW_NORMAL);
            AutoItX.WinWait("Realtek - Monitor Customer Tool V1.1 Beta3", "", 3);
            AutoItX.ControlClick("Realtek - Monitor Customer Tool V1.1 Beta3", "", "[CLASS:SysTreeView32; INSTANCE:1]", numClicks:5, x:34, y:27);

            AutoItX.WinMove("Realtek - Monitor Customer Tool V1.1 Beta3", "", 0, 0, 1200, 900);
            AutoItX.MouseClick("left", 150, 90, 1, 0);
            AutoItX.MouseClick("left", 750, 90, 3, 0);

            AutoItX.WinActivate("開啟");
            var title = AutoItX.WinGetTitle("[REGEXPTITLE:(開啟|Open)]");
            AutoItX.ControlSetText("", "", "[CLASS:ToolbarWindow32; INSTANCE:3]", "C:\\path\to\\your\\file");

            AutoItX.Sleep(200);
            var fwFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Y70UpdateFiles", "WH069_3840_1100_xxxx_A2FC_0513_V16.bin");
            AutoItX.ControlSetText($"{title}", "", "[CLASS:Edit; INSTANCE:1]", fwFilePath);
            AutoItX.Send("{ENTER}");
            AutoItX.Sleep(100);
            AutoItX.ControlClick("Realtek - Monitor Customer Tool V1.1 Beta3", "", "[CLASS:Button; INSTANCE:73]",numClicks: 1);
            //AutoItX.WinSetState("Realtek - Monitor Customer Tool V1.1 Beta3", "", 0);
            //AutoItX.WinWait("Realtek - Monitor Customer Tool V1.1 Beta3", "", 30);
            //AutoItX.WinClose("Realtek - Monitor Customer Tool V1.1 Beta3");
        }

        private void KeebUpdateFW()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Firmwares", "K2862_50343_64LQFP_HV133_20240308_2439H.exe");

            AutoItX.Run(filePath, "", AutoItX.SW_HIDE);
            AutoItX.WinWait("Holtek TinyProgrammer V1.0.0", "", 1);
            AutoItX.WinSetState("Holtek TinyPorgrammer V1.0.0", "", 0);
            AutoItX.ControlClick("Holtek TinyPorgrammer V1.0.0", "", "[CLASS:Button; TEXT:Connect]");
            AutoItX.WinWait("Holtek TinyProgrammer V1.0.0", "", 10);
            AutoItX.WinClose("Holtek TinyPorgrammer V1.0.0");
        }
    }
}
