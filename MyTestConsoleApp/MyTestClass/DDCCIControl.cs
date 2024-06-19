using Emgu.CV.Dnn;
using Emgu.CV.ML;
using Emgu.CV.XFeatures2D;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;
using WindowsDisplayAPI;
using static MyTestConsoleApp.MyTestClass.Win32.Dxva2;
using static MyTestConsoleApp.MyTestClass.Win32.Dxva2.PhysicalMonitorEnumerationApi;
using static Vanara.PInvoke.Gdi32;
using static Vanara.PInvoke.User32;

public class DDCCIControl
{
    byte[] allBytes = new byte[] { 0x02, 0x04, 0x05, 0x06, 0x08, 0x0B, 0x0C, 0x10, 0x12, 0x14, 0x16, 0x18, 0x1A, 0x52, 0x60, 0x87, 0xAC, 0xAE, 0xB2, 0xB6, 0xC6, 0xC8, 0xCA, 0xCC, 0xD6, 0xDF, 0xFD, 0xFF };
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
                    //Console.WriteLine($"Capabilities: {sb.ToString()}");
                    if (sb.ToString() != "(prot(monitor)type(LCD)model(RTK)cmds(01 02 03 07 0C E3 F3)vcp(02 04 05 06 08 0B 0C 10 12 14(01 02 04 05 06 08 0B) 16 18 1A 52 60(01 03 04 0F 10 11 12) 87 AC AE B2 B6 C6 C8 CA CC(01 02 03 04 06 0A 0D) D6(01 04 05) DF FD FF)mswhql(1)asset_eep(40)mccs_ver(2.2))")
                    {
                        continue;
                    }

                    PrintAllByteValue(physicalMonitor.hPhysicalMonitor);
                    // Set the monitor RGB values
                    //SetMonitorRGB(physicalMonitor.hPhysicalMonitor, 0x0A, 0x0A, 0x0A);
                    //CheckVCPFeatureSupport(physicalMonitor.hPhysicalMonitor, 0x16);
                    //CheckVCPFeatureSupport(physicalMonitor.hPhysicalMonitor, 0x18);
                    //CheckVCPFeatureSupport(physicalMonitor.hPhysicalMonitor, 0x1A);
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

    private void PrintAllByteValue(IntPtr hPhysicalMonitor)
    {
        foreach (byte target in allBytes) 
        {
            CheckVCPFeatureSupport(hPhysicalMonitor, target);
        }
    }

    private bool CheckVCPFeatureSupport(IntPtr hPhysicalMonitor, byte vcpCode)
    {
        LPMC_VCP_CODE_TYPE temop;
        uint currentValue, maximumValue;
        var result = GetVCPFeatureAndVCPFeatureReply(hPhysicalMonitor, vcpCode, out temop, out currentValue, out maximumValue);
        Console.WriteLine($"title:0x{vcpCode.ToString("X2")}, currentValue:{currentValue}, maximumValue:{maximumValue}");

        return result;
    }
    private void StartChangeRGB(PHYSICAL_MONITOR physicalMonitor)
    {
        int i = 0;
        bool isup = true;
        int scope = 3;
        while (true)
        {
            Console.WriteLine(i);
            SetMonitorRGB(physicalMonitor.hPhysicalMonitor, (byte)i, (byte)i, (byte)i);
            if (isup)
                i += scope;
            else
                i -= scope;

            if (i >= 100)
            {
                i = 100;
                isup = false;
            }

            if (i <= 0)
            {
                i = 0;
                isup = true;
            }

            if (i > 60)
                scope = 5;
            if (i < 60 && i < 40)
                scope = 3;
            if (i < 40)
                scope = 1;
            //Thread.Sleep(10);
        }
    }

}



//02: New Control Value
//04: Restore factory defaults
//05: Restore factory Luminance/Contrast defaults
//06: Restore factory color defaults
//08: Color Temperature Increment
//0B: Color Temperature Request
//0C: Luminance(Brightness)
//10: Contrast
//12: Select Color Preset
//14: Adjust Clock(Phase)
//Sub - settings:
//01: Normal
//02: Lower Sharpness
//04: Higher Sharpness
//05: Picture Offset
//06: Picture Size
//08: Horizontal Size
//0B: Vertical Size
//16: Video Gain(Red)
//18: Video Gain(Green)
//1A: Video Gain(Blue)
//52: Active Control
//60: Input Source
//Sub-settings:
//01: Analog Video(VGA)
//03: Digital Video(DVI)
//04: Composite Video
//0F: DisplayPort
//10: HDMI
//11: Component Video(YPbPr)
//12: SCART
//87: Audio Volume
//AC: Horizontal Frequency
//AE: Vertical Frequency
//B2: Display Technology Type
//B6: Horizontal Position
//C6: Video Black Level (Red)
//C8: Video Black Level(Green)
//CA: Video Black Level(Blue)
//CC: Screen Orientation
//Sub - settings:
//01: Default(Landscape)
//02: 90 degrees(Portrait)
//03: 180 degrees(Landscape Flipped)
//04: 270 degrees(Portrait Flipped)
//06: 90 degrees Counter-Clockwise
//0A: Landscape Right
//0D: Portrait Right
//D6: Display Usage Time
//Sub-settings:
//01: Current
//04: Total
//05: Reset
//DF: VCP Version
//FD: Manufacturer's Command Table Version
//FF: Manufacturer Specific Command
