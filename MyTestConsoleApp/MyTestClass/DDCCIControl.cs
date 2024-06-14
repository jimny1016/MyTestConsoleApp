using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;
using static MyTestConsoleApp.MyTestClass.Win32.Dxva2;
using static MyTestConsoleApp.MyTestClass.Win32.Dxva2.PhysicalMonitorEnumerationApi;
using static Vanara.PInvoke.User32;

public class DDCCIControl
{
    public DDCCIControl()
    {
        EnumDisplayMonitors(IntPtr.Zero, null, MonitorEnumProc1, IntPtr.Zero);
    }

    private bool MonitorEnumProc1(IntPtr Arg1, IntPtr Arg2, PRECT Arg3, IntPtr Arg4)
    {
        var monitorInfo = new MONITORINFOEX { cbSize = (uint)Marshal.SizeOf<MONITORINFOEX>() };
        GetMonitorInfo(Arg1, ref monitorInfo);
        uint a = 0;
        PhysicalMonitorEnumerationApi.GetNumberOfPhysicalMonitorsFromHMONITOR(Arg1, out a);
        var physicalMonitors = new PHYSICAL_MONITOR[a];
        PhysicalMonitorEnumerationApi.GetPhysicalMonitorsFromHMONITOR(Arg1, a, physicalMonitors);

        foreach (var physicalMonitor in physicalMonitors)
        {
            //if (physicalMonitor.hPhysicalMonitor != 0x0000000000000003)
            //    return true;
            if (GetCapabilitiesStringLength(physicalMonitor.hPhysicalMonitor, out uint length))
            {
                // Allocate a buffer and get the capabilities string
                StringBuilder sb = new StringBuilder((int)length);
                if (CapabilitiesRequestAndCapabilitiesReply(physicalMonitor.hPhysicalMonitor, sb, length))
                {
                    Console.WriteLine($"Capabilities: {sb.ToString()}");

                    // Set the monitor RGB values
                    SetMonitorRGB(physicalMonitor.hPhysicalMonitor, 80, 80, 80);
                }
            }
            DestroyPhysicalMonitors(a, physicalMonitors);
        }
        return true;
    }
    private void SetMonitorRGB(IntPtr hPhysicalMonitor, byte red, byte green, byte blue)
    {
        const byte VCP_CODE_RED_GAIN = 0x10;
        const byte VCP_CODE_GREEN_GAIN = 0x12;
        const byte VCP_CODE_BLUE_GAIN = 0x14;

        if (!SetVCPFeature(hPhysicalMonitor, VCP_CODE_RED_GAIN, red))
        {
            Console.WriteLine("Failed to set red gain.");
        }

        if (!SetVCPFeature(hPhysicalMonitor, VCP_CODE_GREEN_GAIN, green))
        {
            Console.WriteLine("Failed to set green gain.");
        }

        if (!SetVCPFeature(hPhysicalMonitor, VCP_CODE_BLUE_GAIN, blue))
        {
            Console.WriteLine("Failed to set blue gain.");
        }
    }
}

