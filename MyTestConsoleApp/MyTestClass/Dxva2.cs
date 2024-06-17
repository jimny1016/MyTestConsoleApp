using System.Runtime.InteropServices;
using System.Text;

namespace MyTestConsoleApp.MyTestClass
{
    public partial class Win32
    {
        public struct IDirect3DDevice9
        {
        }

        public static class Dxva2
        {
            public static class PhysicalMonitorEnumerationApi
            {
                #region Physical Monitor Constants

                /// <summary>
                /// A physical monitor description is always an array of 128 characters.  Some
                /// of the characters may not be used.
                /// </summary>
                public const int PHYSICAL_MONITOR_DESCRIPTION_SIZE = 128;

                #endregion

                #region Physical Monitor Structures

                [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
                public struct PHYSICAL_MONITOR
                {
                    public IntPtr hPhysicalMonitor;

                    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PHYSICAL_MONITOR_DESCRIPTION_SIZE)]
                    public string szPhysicalMonitorDescription;
                }

                #endregion

                #region Physical Monitor Enumeration Functions

                [DllImport("Dxva2.dll", ExactSpelling = true, SetLastError = true, PreserveSig = false)]
                public static extern void GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, [Out] out UInt32 pdwNumberOfPhysicalMonitors);

                [DllImport("Dxva2.dll", ExactSpelling = true, SetLastError = true, PreserveSig = false)]
                public static extern void GetPhysicalMonitorsFromHMONITOR([In] IntPtr hMonitor, [In] UInt32 dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

                [DllImport("Dxva2.dll", ExactSpelling = true, SetLastError = true, PreserveSig = false)]
                public static extern bool DestroyPhysicalMonitor([In] IntPtr hMonitor);

                [DllImport("Dxva2.dll", ExactSpelling = true, SetLastError = true, PreserveSig = false)]
                public static extern bool DestroyPhysicalMonitors([In] UInt32 dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

                [DllImport("dxva2.dll", SetLastError = true)]
                public static extern bool GetCapabilitiesStringLength(IntPtr hMonitor, out uint pdwCapabilitiesStringLengthInCharacters);

                [DllImport("dxva2.dll", SetLastError = true, CharSet = CharSet.Ansi)]
                public static extern bool CapabilitiesRequestAndCapabilitiesReply(IntPtr hMonitor, StringBuilder pszASCIICapabilitiesString, uint dwCapabilitiesStringLengthInCharacters);

                [DllImport("dxva2.dll", SetLastError = true)]
                public static extern bool SetVCPFeature(IntPtr hMonitor, byte bVCPCode, uint dwNewValue);

                [DllImport("Dxva2.dll", SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool GetVCPFeatureAndVCPFeatureReply(
                    IntPtr hMonitor,
                    byte bVCPCode,
                    out LPMC_VCP_CODE_TYPE pvct,
                    out uint pdwCurrentValue,
                    out uint pdwMaximumValue);

                public enum LPMC_VCP_CODE_TYPE
                {
                    MC_MOMENTARY,
                    MC_SET_PARAMETER
                }
                #endregion
            }
        }
    }
}