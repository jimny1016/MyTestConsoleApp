using MyTestConsoleApp.MyTestClass;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using WindowsDisplayAPI;

namespace MyTestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                Console.WriteLine("restarting as admin");
                StartAsAdmin(Assembly.GetExecutingAssembly().Location);
                return;
            }

            _ = new AutoItX3Test();
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void StartAsAdmin(string fileName)
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = fileName,
                    UseShellExecute = true,
                    Verb = "runas"
                }
            };

            proc.Start();
        }
    }
}