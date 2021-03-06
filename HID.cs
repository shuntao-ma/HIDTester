/* 
 *Author:	 Zh.Kaihua@gmail.com
 *Date:	 2011.06.07
 *Licensees are granted free, non-transferable use of the information. 
* NO WARRENTY of ANY KIND is provided. This heading must NOT be removed from  the file.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Windows;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HIDTester
{
    public class report : EventArgs
    {
        public readonly byte reportID;
        public readonly byte[] reportBuff;

        public report(byte id, byte[] arrayBuff)
        {
            reportID = id;
            reportBuff = arrayBuff;
        }
    }

    public class HID : Object
    {

        #region DataStruct

        public const int DIGCF_DEFAULT = 0x00000001;
        public const int DIGCF_ALLCLASSES = 0x00000004;
        public const int DIGCF_PROFILE = 0x00000008;

        public const uint GENERIC_EXECUTE = 0x20000000;
        public const uint GENERIC_ALL = 0x10000000;

        public const uint CREATE_NEW = 1;
        public const uint CREATE_ALWAYS = 2;

        public const uint TRUNCATE_EXISTING = 5;
        public const int HIDP_STATUS_SUCCESS = 1114112;
        public const int DEVICE_PATH = 260;
        public IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);


        public const uint FILE_SHARE_DELETE = 0x00000004;

        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const uint FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        public const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
        public const uint FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public const uint FILE_FLAG_POSIX_SEMANTICS = 0x01000000;
        public const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        public const uint FILE_FLAG_OPEN_NO_RECALL = 0x00100000;
        public const uint FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000;

        /// <summary>Windows message sent when a device is inserted or removed</summary>
        public const int WM_DEVICECHANGE = 0x0219;
        /// <summary>WParam for above : A device was inserted</summary>
        public const int DEVICE_ARRIVAL = 0x8000;
        /// <summary>WParam for above : A device was removed</summary>
        public const int DEVICE_REMOVECOMPLETE = 0x8004;
        /// <summary>Used in SetupDiClassDevs to get devices present in the system</summary>
        public const int DIGCF_PRESENT = 0x02;
        /// <summary>Used in SetupDiClassDevs to get device interface details</summary>
        public const int DIGCF_DEVICEINTERFACE = 0x10;
        /// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
        public const int DEVTYP_DEVICEINTERFACE = 0x05;
        /// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        /// <summary>Purges Win32 transmit buffer by aborting the current transmission.</summary>
        public const uint PURGE_TXABORT = 0x01;
        /// <summary>Purges Win32 receive buffer by aborting the current receive.</summary>
        public const uint PURGE_RXABORT = 0x02;
        /// <summary>Purges Win32 transmit buffer by clearing it.</summary>
        public const uint PURGE_TXCLEAR = 0x04;
        /// <summary>Purges Win32 receive buffer by clearing it.</summary>
        public const uint PURGE_RXCLEAR = 0x08;
        /// <summary>CreateFile : Open file for read</summary>
        public const uint GENERIC_READ = 0x80000000;
        /// <summary>CreateFile : Open file for write</summary>
        public const uint GENERIC_WRITE = 0x40000000;
        /// <summary>CreateFile : file share for write</summary>
        public const uint FILE_SHARE_WRITE = 0x2;
        /// <summary>CreateFile : file share for read</summary>
        public const uint FILE_SHARE_READ = 0x1;
        /// <summary>CreateFile : Open handle for overlapped operations</summary>
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        /// <summary>CreateFile : Resource to be "created" must exist</summary>
        public const uint OPEN_EXISTING = 3;
        /// <summary>CreateFile : Resource will be "created" or existing will be used</summary>
        public const uint OPEN_ALWAYS = 4;
        /// <summary>ReadFile/WriteFile : Overlapped operation is incomplete.</summary>
        public const uint ERROR_IO_PENDING = 997;
        /// <summary>Infinite timeout</summary>
        public const uint INFINITE = 0xFFFFFFFF;
        /// <summary>Simple representation of a null handle : a closed stream will get this handle. Note it is public for comparison by higher level classes.</summary>
        public static IntPtr NullHandle = IntPtr.Zero;
        /// <summary>Simple representation of the handle returned when CreateFile fails.</summary>

        #region DeviceBroadcastInterface
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public class DeviceBroadcastInterface
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public Guid ClassGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }
        #endregion

        #region HID_RETURN
        public enum HID_RETURN
        {
            SUCCESS = 0,
            NO_DEVICE_CONECTED,
            DEVICE_NOT_FIND,
            DEVICE_OPENED,
            WRITE_FAILD,
            READ_FAILD
        }

        #endregion

        #region DevBroadcastDeviceInterfaceBuffer
        [StructLayout(LayoutKind.Explicit)]
        class DevBroadcastDeviceInterfaceBuffer
        {
            [FieldOffset(0)]
            public Int32 dbch_size;
            [FieldOffset(4)]
            public Int32 dbch_devicetype;
            [FieldOffset(8)]
            public Int32 dbch_reserved;
            public DevBroadcastDeviceInterfaceBuffer(Int32 deviceType)
            {
                dbch_size = Marshal.SizeOf(typeof(DevBroadcastDeviceInterfaceBuffer));
                dbch_devicetype = deviceType;
                dbch_reserved = 0;
            }
        }

        #endregion

        #region DIGCF 控制由 SetupDiGetClassDevs 构建的设备信息集中包含的内容的标志
        public enum DIGCF
        {
            DIGCF_DEFAULT = 0x00000001, // only valid with DIGCF_DEVICEINTERFACE                 
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010
        }

        #endregion

        #region DESIREDACCESS
        static class DESIREDACCESS// Type of access to the object. 
        {
            public const uint GENERIC_READ = 0x80000000;
            public const uint GENERIC_WRITE = 0x40000000;
            public const uint GENERIC_EXECUTE = 0x20000000;
            public const uint GENERIC_ALL = 0x10000000;
        }

        #endregion

        #region CREATIONDISPOSITION
        static class CREATIONDISPOSITION// Action to take on files that exist, and which action to take when files do not exist. 
        {
            public const uint CREATE_NEW = 1;
            public const uint CREATE_ALWAYS = 2;
            public const uint OPEN_EXISTING = 3;
            public const uint OPEN_ALWAYS = 4;
            public const uint TRUNCATE_EXISTING = 5;
        }

        #endregion

        #region DEV_BROADCAST_HDR
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
        }
        #endregion

        #region DEV_BROADCAST_DEVICEINTERFACE
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public string dbcc_name;
        }
        #endregion

        #region SHAREMODE
        static class SHAREMODE
        {
            public const uint FILE_SHARE_READ = 0x00000001;
            public const uint FILE_SHARE_WRITE = 0x00000002;
            public const uint FILE_SHARE_DELETE = 0x00000004;
        }
        #endregion

        #region FileMapProtection
        public enum FileMapProtection : uint
        {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }
        #endregion

        #region HIDP_REPORT_TYPE
        public enum HIDP_REPORT_TYPE : ushort
        {
            HidP_Input = 0x00,
            HidP_Output = 0x01,
            HidP_Feature = 0x02,
        }
        #endregion

        #region LIST_ENTRY
        [StructLayout(LayoutKind.Sequential)]
        struct LIST_ENTRY
        {
            public IntPtr Flink;
            public IntPtr Blink;
        }
        #endregion

        #region DEVICE_LIST_NODE
        [StructLayout(LayoutKind.Sequential)]
        struct DEVICE_LIST_NODE
        {
            public LIST_ENTRY Hdr;
            public IntPtr NotificationHandle;
            public HID_DEVICE HidDeviceInfo;
            public bool DeviceOpened;
        }
        #endregion

        #region SP_DEVICE_INTERFACE_DATA
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public Int32 cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private UIntPtr reserved;
        }
        #endregion

        #region SP_DEVICE_INTERFACE_DETAIL_DATA 结构包含设备接口的路径。
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DEVICE_PATH)]
            public string DevicePath;
        }
        #endregion

        #region SP_DEVINFO_DATA
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid classGuid;
            public UInt32 devInst;
            public IntPtr reserved;
        }
        #endregion

        #region HIDP_CAPS
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_CAPS
        {
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 Usage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 UsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 InputReportByteLength;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 OutputReportByteLength;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public UInt16[] Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberLinkCollectionNodes;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputDataIndices;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputDataIndices;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureDataIndices;
        };

        #endregion

        #region HIDD_ATTRIBUTES
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public Int16 VendorID;
            public Int16 ProductID;
            public Int16 VersionNumber;
        }
        #endregion

        #region ButtonData
        [StructLayout(LayoutKind.Sequential)]
        public struct ButtonData
        {
            public Int32 UsageMin;
            public Int32 UsageMax;
            public Int32 MaxUsageLength;
            public Int16 Usages;
        }
        #endregion

        #region ValueData
        [StructLayout(LayoutKind.Sequential)]
        public struct ValueData
        {
            public ushort Usage;
            public ushort Reserved;
            public ulong Value;
            public long ScaledValue;
        }
        #endregion

        #region HID_DATA
        [StructLayout(LayoutKind.Explicit)]
        public struct HID_DATA
        {
            [FieldOffset(0)]
            public bool IsButtonData;
            [FieldOffset(1)]
            public byte Reserved;
            [FieldOffset(2)]
            public ushort UsagePage;
            [FieldOffset(4)]
            public Int32 Status;
            [FieldOffset(8)]
            public Int32 ReportID;
            [FieldOffset(16)]
            public bool IsDataSet;

            [FieldOffset(17)]
            public ButtonData ButtonData;
            [FieldOffset(17)]
            public ValueData ValueData;
        }
        #endregion

        #region HIDP_Range
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_Range
        {
            public ushort UsageMin, UsageMax;
            public ushort StringMin, StringMax;
            public ushort DesignatorMin, DesignatorMax;
            public ushort DataIndexMin, DataIndexMax;
        }
        #endregion

        #region HIDP_NotRange
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_NotRange
        {
            public ushort Usage, Reserved1;
            public ushort StringIndex, Reserved2;
            public ushort DesignatorIndex, Reserved3;
            public ushort DataIndex, Reserved4;
        }
        #endregion

        #region HIDP_BUTTON_CAPS
        [StructLayout(LayoutKind.Explicit)]
        public struct HIDP_BUTTON_CAPS
        {
            [FieldOffset(0)]
            public ushort UsagePage;
            [FieldOffset(2)]
            public byte ReportID;
            [FieldOffset(3), MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;
            [FieldOffset(4)]
            public short BitField;
            [FieldOffset(6)]
            public short LinkCollection;
            [FieldOffset(8)]
            public short LinkUsage;
            [FieldOffset(10)]
            public short LinkUsagePage;
            [FieldOffset(12), MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [FieldOffset(13), MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [FieldOffset(14), MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [FieldOffset(15), MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [FieldOffset(16), MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] Reserved;

            [FieldOffset(56)]
            public HIDP_Range Range;
            [FieldOffset(56)]
            public HIDP_NotRange NotRange;
        }
        #endregion

        #region HIDP_VALUE_CAPS
        [StructLayout(LayoutKind.Explicit)]
        public struct HIDP_VALUE_CAPS
        {
            [FieldOffset(0)]
            public ushort UsagePage;
            [FieldOffset(2)]
            public byte ReportID;
            [FieldOffset(3), MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;
            [FieldOffset(4)]
            public ushort BitField;
            [FieldOffset(6)]
            public ushort LinkCollection;
            [FieldOffset(8)]
            public ushort LinkUsage;
            [FieldOffset(10)]
            public ushort LinkUsagePage;
            [FieldOffset(12), MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [FieldOffset(13), MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [FieldOffset(14), MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [FieldOffset(15), MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [FieldOffset(16), MarshalAs(UnmanagedType.U1)]
            public bool HasNull;
            [FieldOffset(17)]
            public byte Reserved;
            [FieldOffset(18)]
            public short BitSize;
            [FieldOffset(20)]
            public short ReportCount;
            [FieldOffset(22)]
            public ushort Reserved2a;
            [FieldOffset(24)]
            public ushort Reserved2b;
            [FieldOffset(26)]
            public ushort Reserved2c;
            [FieldOffset(28)]
            public ushort Reserved2d;
            [FieldOffset(30)]
            public ushort Reserved2e;
            [FieldOffset(32)]
            public int UnitsExp;
            [FieldOffset(36)]
            public int Units;
            [FieldOffset(40)]
            public int LogicalMin;
            [FieldOffset(44)]
            public int LogicalMax;
            [FieldOffset(48)]
            public int PhysicalMin;
            [FieldOffset(52)]
            public int PhysicalMax;
            [FieldOffset(56)]
            public HIDP_Range Range;
            [FieldOffset(56)]
            public HIDP_NotRange NotRange;
        }

        #endregion

        #region HID_DEVICE
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class HID_DEVICE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DEVICE_PATH)]
            public string DevicePath;
            public IntPtr HidDevice;
            public bool OpenedForRead;
            public bool OpenedForWrite;
            public bool OpenedOverlapped;
            public bool OpenedExclusive;

            public IntPtr Ppd;
            public HIDP_CAPS Caps;
            public HIDD_ATTRIBUTES Attributes;

            public List<IntPtr> InputReportBuffer;
            public List<HID_DATA> InputData;
            public Int32 InputDataLength;
            public List<HIDP_BUTTON_CAPS> InputButtonCaps;
            public List<HIDP_VALUE_CAPS> InputValueCaps;

            public List<IntPtr> OutputReportBuffer;
            public List<HID_DATA> OutputData;
            public Int32 OutputDataLength;
            public List<HIDP_BUTTON_CAPS> OutputButtonCaps;
            public List<HIDP_VALUE_CAPS> OutputValueCaps;

            public List<IntPtr> FeatureReportBuffer;
            public List<HID_DATA> FeatureData;
            public Int32 FeatureDataLength;
            public List<HIDP_BUTTON_CAPS> FeatureButtonCaps;
            public List<HIDP_VALUE_CAPS> FeatureValueCaps;

            public HID_DEVICE()
            {
                DevicePath = "";
                HidDevice = new IntPtr(-1);
                OpenedForRead = false;
                OpenedForWrite = false;
                OpenedOverlapped = false;
                OpenedExclusive = false;

                Ppd = new IntPtr();
                Caps = new HIDP_CAPS();
                Attributes = new HIDD_ATTRIBUTES();

                InputReportBuffer = new List<IntPtr>();
                InputData = new List<HID_DATA>();
                InputDataLength = -1;
                InputButtonCaps = new List<HIDP_BUTTON_CAPS>();
                InputValueCaps = new List<HIDP_VALUE_CAPS>();

                OutputReportBuffer = new List<IntPtr>();
                OutputData = new List<HID_DATA>();
                OutputDataLength = -1;
                OutputButtonCaps = new List<HIDP_BUTTON_CAPS>();
                OutputValueCaps = new List<HIDP_VALUE_CAPS>();

                FeatureReportBuffer = new List<IntPtr>();
                FeatureData = new List<HID_DATA>();
                FeatureDataLength = -1;
                FeatureButtonCaps = new List<HIDP_BUTTON_CAPS>();
                FeatureValueCaps = new List<HIDP_VALUE_CAPS>();
            }
        }

        #endregion

        #endregion

        #region HID WinAPI

        //功能：获取一个指定类别或全部类别的所有已安装设备的信息.
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);

        // SetupDiEnumDeviceInterfaces 枚举设备信息集中的全部接口。
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid,
            int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid,
            UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        //SetupDiGetDeviceInterfaceDetail 返回有关设备接口的详细信息
        [DllImport(@"setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

        [DllImport(@"setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);

        //功能：销毁一个设备信息集合，并且释放所有关联的内存。
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        //HidD_GetSerialNumberString 返回用于标识物理设备的序列号。
        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool HidD_GetSerialNumberString(IntPtr hidDeviceObject, IntPtr pointerToBuffer, uint bufferLength);

        [DllImport("hid.dll")]
        public static extern Boolean HidD_GetSerialNumberString(IntPtr hidDeviceObject, IntPtr buffer, int bufferLength);

        //HidD_GetHidGuid 返回HID设备的接口GUID。
        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(ref Guid Guid);

        //HidD_FreePreparsedData 释放为HID预解析数据而分配的资源。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

        //HidD_GetPreparsedData 返回HID设备的预解析数据。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetPreparsedData(IntPtr HidDeviceObject, out IntPtr PreparsedData);

        //HidD_GetAttributes 返回HID设备的属性
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetAttributes(IntPtr DeviceObject, out HIDD_ATTRIBUTES Attributes);

        //HidP_GetCaps 返回HID设备集合的HIDP_CAPS结构。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern uint HidP_GetCaps(IntPtr PreparsedData, out HIDP_CAPS Capabilities);

        //HidP_GetButtonCaps 返回一个按钮功能数组，该数组描述指定类型HID报告的顶级集合中的所有HID控制按钮。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_GetButtonCaps(HIDP_REPORT_TYPE ReportType, [In, Out] HIDP_BUTTON_CAPS[] ButtonCaps,
            ref ushort ButtonCapsLength, IntPtr PreparsedData);

        //HidP_GetValueCaps 返回一个值数组，该数组描述指定类型的HID设备中的所有控制值。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_GetValueCaps(HIDP_REPORT_TYPE ReportType, [In, Out] HIDP_VALUE_CAPS[] ValueCaps,
            ref ushort ValueCapsLength, IntPtr PreparsedData);

        //HidP_MaxUsageListLength 返回指定类型的HID设备的最大HID使用数。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_MaxUsageListLength(HIDP_REPORT_TYPE ReportType, ushort UsagePage, IntPtr PreparsedData);

        //HidP_SetUsages 将HID设备中的指定的HID控制按钮设置为 ON (1)。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_SetUsages(HIDP_REPORT_TYPE ReportType, ushort UsagePage, short LinkCollection, short Usages,
            ref int UsageLength, IntPtr PreparsedData, IntPtr Report, int ReportLength);

        //HidP_SetUsageValue 指定的HID设备中设置HID的控制值。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_SetUsageValue(HIDP_REPORT_TYPE ReportType, ushort UsagePage, short LinkCollection,
            ushort Usage, ulong UsageValue, IntPtr PreparsedData, IntPtr Report, int ReportLength);

        //HidD_SetOutputReport 将报告发送到指定句柄。
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool HidD_SetOutputReport(IntPtr DeviceObject, byte[] Buffer, int BufferLength);

        //HidD_GetInputReport 从指定句柄返回输入报告。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetInputReport(IntPtr DeviceObject, byte[] Buffer, int BufferLength);

        //HidD_SetFeature 将特征报告发送到指定句柄
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool HidD_SetFeature(IntPtr DeviceObject, byte[] Buffer, int BufferLength);

        //HidD_GetFeature 返回指定句柄的属性报告。
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetFeature(IntPtr DeviceObject, byte[] Buffer, int BufferLength);

        //HidD_GetManufacturerString 返回标识制造商的字符串。
        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool HidD_GetManufacturerString(IntPtr DeviceObject, IntPtr pointerToBuffer, int bufferLength);

        //HidD_GetProductString 返回标识制造商产品的字符串。
        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool HidD_GetProductString(IntPtr hidDeviceObject, IntPtr pointerToBuffer, int bufferLength);

        //HidD_GetIndexedString 从句柄返回指定的字符串。
        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool HidD_GetIndexedString(IntPtr hidDeviceObject, uint StringIdex, IntPtr pointerToBuffer, int bufferLength);

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string fileName, uint fileAccess, uint fileShare, FileMapProtection securityAttributes,
            uint creationDisposition, uint flags, IntPtr overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string fileName, uint desiredAccess, uint shareMode,
            uint securityAttributes, uint creationDisposition, uint flagsAndAttributes, uint templateFile);

        [DllImport("kernel32.dll")]
        public static extern bool WriteFile(IntPtr hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern bool ReadFile(IntPtr hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(object hMem);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hwnd, DeviceBroadcastInterface oInterface, uint nFlags);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]// Closes the specified device notification handle.
        public static extern bool UnregisterDeviceNotification(IntPtr handle);

        #endregion

        public HID_DEVICE Device { get; set; }          //HID设备
        public bool DeviceOpened { get; set; }          //指示设备打开与否
        public event EventHandler<report> DataReceived; // 事件:数据到达,处理此事件以接收输入数据
        public event EventHandler DeviceRemoved;        // 事件:设备断开
        private FileStream HidStream { get; set; }      //异步IO流
        private IAsyncResult ReadResult { get; set; }   //异步IO流控制
        private int OutputReportLength { get; set; }    //输出报告长度,包括一个字节的报告ID
        private int InputReportLength { get; set; }      //输入报告长度,包括一个字节的报告ID 
        public HID()
        {
            OutputReportLength = 0;
            InputReportLength = 0;
            DeviceOpened = false; //指示设备打开与否
            HidStream = null; //异步IO流
            ReadResult = null; //异步IO流控制
            Device = new HID_DEVICE();
        }
        public IntPtr OpenDevice(string DevicePath) // 打开指定信息的设备
        {
            Debug.WriteLine(DeviceOpened);
            if (DeviceOpened == false)
            {
                Debug.WriteLine(DevicePath);
                Device.HidDevice = CreateFile(DevicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
                    0, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
                if (Device.HidDevice != INVALID_HANDLE_VALUE)
                {
                    Device.DevicePath = DevicePath;
                    IntPtr PreparseData = new IntPtr(-1);
                    HIDP_CAPS caps = new HIDP_CAPS();

                    HidD_GetPreparsedData(Device.HidDevice, out PreparseData);
                    HidP_GetCaps(PreparseData, out caps);
                    HidD_FreePreparsedData(PreparseData);

                    OutputReportLength = caps.OutputReportByteLength;
                    InputReportLength = caps.InputReportByteLength;
                    HidStream = new FileStream(new SafeFileHandle(Device.HidDevice, false), FileAccess.ReadWrite, InputReportLength, true);
                    DeviceOpened = true;
                    BeginAsyncRead();//开始异步读
                    return Device.HidDevice;
                }
            }
            return INVALID_HANDLE_VALUE;
        }
        public void CloseDevice()// 关闭打开的设备
        {
            if (DeviceOpened == true)
            {
                CloseHandle(Device.HidDevice);
                EventArgs ex = new EventArgs();
                OnDeviceRemoved(ex);
            }
        }
        private void BeginAsyncRead()// 开始一次异步读
        {
            byte[] inputBuff = new byte[InputReportLength];
            if (HidStream.Handle != null) //BeginRead为异步读操作,异步回调函数,目测是完成操作后回调
            {
                ReadResult = HidStream.BeginRead(inputBuff, 0, InputReportLength, new AsyncCallback(ReadCompleted), inputBuff);
            }
        }
        private void ReadCompleted(IAsyncResult iResult)//异步读取结束,发出有数据到达事件
        {
            byte[] readBuff = (byte[])(iResult.AsyncState);
            try
            {
                if (DeviceOpened == true)
                {
                    HidStream.EndRead(iResult); //读取结束,如果读取错误就会产生一个异常
                    report e = new report(readBuff[0], readBuff);
                    OnDataReceived(e); //发出数据到达消息
                    BeginAsyncRead(); //启动下一次读操作
                }
            }
            catch (IOException e) //读写错误,设备可能已经被移除
            {
                MessageBox.Show("数据读写错误,设备已关闭");
                EventArgs ex = new EventArgs();
                OnDeviceRemoved(ex); //发出设备移除消息
            }
        }
        protected virtual void OnDataReceived(report e)
        {
            if (DataReceived != null)
            {
                DataReceived(this, e);
            }
        }
        protected virtual void OnDeviceRemoved(EventArgs e)
        {
            DeviceOpened = false;
            HidStream.Close();
            Device.DevicePath = "";
            if (DeviceRemoved != null)
            {
                DeviceRemoved(this, e);
            }
        }
        public HID_RETURN Write(report r) //写数据
        {
            try
            {
                if (DeviceOpened)
                {
                    byte[] buffer = new byte[OutputReportLength];
                    buffer[0] = r.reportID;
                    int Length = r.reportBuff.Length < OutputReportLength - 1 ? r.reportBuff.Length : OutputReportLength - 1;
                    for (int i = 1; i <= Length; i++)
                    {
                        buffer[i] = r.reportBuff[i - 1];
                    }
                    HidStream.Write(buffer, 0, buffer.Length);
                    return HID_RETURN.SUCCESS;
                }
                else
                {
                    return HID_RETURN.NO_DEVICE_CONECTED;
                }
            }
            catch
            {
                MessageBox.Show("数据发送异常,设备已关闭");
                EventArgs ex = new EventArgs();
                OnDeviceRemoved(ex); //发出设备移除消息                  
                return HID_RETURN.WRITE_FAILD;
            }

        }
        public HID_RETURN Write(byte[] data)
        {
            try
            {
                if (DeviceOpened)
                {
                    HidStream.Write(data, 0, data.Length);
                    return HID_RETURN.SUCCESS;
                }
                else
                {
                    return HID_RETURN.NO_DEVICE_CONECTED;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                EventArgs ex = new EventArgs();
                OnDeviceRemoved(ex); //发出设备移除消息                  
                return HID_RETURN.WRITE_FAILD;
            }
        }
        public void GetDeviceInfo(int Index, ShowMsg StrBuilder)
        {
            try
            {
                if (DeviceOpened)
                {
                    int bufferLength = 126;
                    var pointerToBuffer = Marshal.AllocHGlobal(126);
                    Marshal.FreeHGlobal(pointerToBuffer);
                    if (!HidD_GetManufacturerString(Device.HidDevice, pointerToBuffer, bufferLength))
                    {
                        StrBuilder($"USB hid get manufacturer fail:{GetLastError()}");
                    }
                    else
                    {
                        StrBuilder(Marshal.PtrToStringUni(pointerToBuffer).ToString());
                    }
                    if (!HidD_GetProductString(Device.HidDevice, pointerToBuffer, bufferLength))
                    {
                        StrBuilder($"USB hid get product fail:{GetLastError()}");
                    }
                    else
                    {
                        StrBuilder(Marshal.PtrToStringUni(pointerToBuffer));
                    }
                    if (!HidD_GetSerialNumberString(Device.HidDevice, pointerToBuffer, bufferLength))
                    {
                        Debug.Write($"USB hid get serial number fail:{GetLastError()}");
                    }
                    else
                    {
                        StrBuilder(Marshal.PtrToStringUni(pointerToBuffer));
                    }
                    uint index = 0;
                    for (int i = 0; i < 126; i++)
                    {
                        if (!HidD_GetIndexedString(Device.HidDevice, index, pointerToBuffer, bufferLength))
                        {
                            break;
                        }
                        else
                        {
                            StrBuilder($"Index{i}:{Marshal.PtrToStringUni(pointerToBuffer)}");
                        }
                        index++;
                    }
                }
                else
                {
                    Debug.Write("设备未开启");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        public void GetDeviceList(ShowMsg AddItem) // 获取所有连接的hid的设备路径
        {
            Guid hUSB = Guid.Empty;
            HidD_GetHidGuid(ref hUSB); // 取得hid设备全局id
            IntPtr HidInfoSet = SetupDiGetClassDevs(ref hUSB, 0, IntPtr.Zero,
                DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);//取得一个包含所有HID接口信息集合的句柄

            SP_DEVICE_INTERFACE_DATA InterfaceInfo = new SP_DEVICE_INTERFACE_DATA();
            InterfaceInfo.cbSize = Marshal.SizeOf(InterfaceInfo);

            int index = 0;
            while (SetupDiEnumDeviceInterfaces(HidInfoSet, IntPtr.Zero, ref hUSB, index, ref InterfaceInfo)) //得到第index个接口信息
            {
                SP_DEVICE_INTERFACE_DETAIL_DATA DeviceDetailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                if (IntPtr.Size == 8)
                {
                    DeviceDetailData.cbSize = 8;
                }
                else if (IntPtr.Size == 4)
                {
                    DeviceDetailData.cbSize = 5;
                }
                int buffsize = 0;
                // 取得接口详细信息:第一次读取错误,但可以取得信息缓冲区的大小
                SetupDiGetDeviceInterfaceDetail(HidInfoSet, ref InterfaceInfo, IntPtr.Zero,
                    0, ref buffsize, IntPtr.Zero);

                if (SetupDiGetDeviceInterfaceDetail(HidInfoSet, ref InterfaceInfo, ref DeviceDetailData,
                    buffsize, ref buffsize, IntPtr.Zero))
                {
                    AddItem(DeviceDetailData.DevicePath);
                }
                index++;
            }
            SetupDiDestroyDeviceInfoList(HidInfoSet);
        }

        //HIDD_ATTRIBUTES attributes;
        //IntPtr serialBuff = Marshal.AllocHGlobal(512);
        //if (!HidD_GetAttributes(Device.HidDevice, out attributes))
        //{
        //    CloseHandle(Device.HidDevice);
        //    return INVALID_HANDLE_VALUE;
        //}
        //Marshal.FreeHGlobal(serialBuff);
        //Debug.WriteLine(attributes.VendorID.ToString("x4"));
        //Debug.WriteLine(attributes.ProductID.ToString("x4"));

        //HID_DEVICE DevTemp = new HID_DEVICE();
        //DevTemp.DevicePath = DeviceDetailData.DevicePath;
        //CloseHandle(DevTemp.HidDevice);
        //DevTemp.HidDevice = CreateFile(DevTemp.DevicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
        //    0, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
        //DevTemp.Caps = new HIDP_CAPS();
        //DevTemp.Attributes = new HIDD_ATTRIBUTES();
        //DevTemp.Ppd = IntPtr.Zero;
        //HidD_GetPreparsedData(DevTemp.HidDevice, out DevTemp.Ppd);
        //HidD_GetAttributes(DevTemp.HidDevice, out DevTemp.Attributes);
        //HidP_GetCaps(DevTemp.Ppd, out DevTemp.Caps);
        //Device.Add(DevTemp);
    }
}