using System.Management;
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
                    if (sb.ToString() != "(prot(monitor)type(LCD)model(RTK)cmds(01 02 03 07 0C E3 F3)vcp(02 04 05 06 08 0B 0C 10 12 14(01 02 04 05 06 08 0B) 16 18 1A 52 60(01 03 04 0F 10 11 12) 87 AC AE B2 B6 C6 C8 CA CC(01 02 03 04 06 0A 0D) D6(01 04 05) DF FD FF)mswhql(1)asset_eep(40)mccs_ver(2.2))")
                    {
                        continue;
                    }
                    var monitorFriendlyName = GetMonitorFriendlyName(monitorInfo.szDevice);
                    var monitorResolution = GetMonitorResolution(monitorInfo.szDevice);

                    Console.WriteLine($"Friendly Name: {monitorFriendlyName}");
                    Console.WriteLine($"Resolution: {monitorResolution.Width}x{monitorResolution.Height}");

                    // Set the monitor RGB values
                    SetMonitorRGB(physicalMonitor.hPhysicalMonitor, 0x0A, 0x0A, 0x0A);
                    CheckVCPFeatureSupport(physicalMonitor.hPhysicalMonitor, 0x16);
                    CheckVCPFeatureSupport(physicalMonitor.hPhysicalMonitor, 0x18);
                    CheckVCPFeatureSupport(physicalMonitor.hPhysicalMonitor, 0x1A);
                }
            }
            DestroyPhysicalMonitors(a, physicalMonitors);
        }
        return true;
    }
    private void SetMonitorRGB(IntPtr hPhysicalMonitor, byte red, byte green, byte blue)
    {
        const byte VCP_CODE_RED_GAIN = 0x16;
        const byte VCP_CODE_GREEN_GAIN = 0x18;
        const byte VCP_CODE_BLUE_GAIN = 0x1A;

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

    private string GetMonitorFriendlyName(string deviceId)
    {
        // 使用WMI查詢Win32_DesktopMonitor，這裡我們假設友好名稱是顯示設備的描述
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor"))
        {
            foreach (ManagementObject mo in searcher.Get())
            {
                if (mo["PNPDeviceID"] != null && mo["PNPDeviceID"].ToString().Contains(deviceId.Substring(4)))
                {
                    return mo["Name"]?.ToString();
                }
            }
        }
        return null;
    }

    private (int Width, int Height) GetMonitorResolution(string deviceId)
    {
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
        {
            foreach (ManagementObject mo in searcher.Get())
            {
                if (mo["PNPDeviceID"] != null && mo["PNPDeviceID"].ToString().Contains(deviceId.Substring(4)))
                {
                    int width = int.Parse(mo["CurrentHorizontalResolution"].ToString());
                    int height = int.Parse(mo["CurrentVerticalResolution"].ToString());
                    return (width, height);
                }
            }
        }
        return (0, 0);
    }

    private bool CheckVCPFeatureSupport(IntPtr hPhysicalMonitor, byte vcpCode)
    {
        LPMC_VCP_CODE_TYPE temop;
        uint currentValue, maximumValue;
        return GetVCPFeatureAndVCPFeatureReply(hPhysicalMonitor, vcpCode, out temop, out currentValue, out maximumValue);
    }
}