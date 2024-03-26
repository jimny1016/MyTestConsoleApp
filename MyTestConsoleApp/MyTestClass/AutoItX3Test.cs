using AutoIt;

namespace MyTestConsoleApp.MyTestClass
{
    internal class AutoItX3Test
    {
        public AutoItX3Test()
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
