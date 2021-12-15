using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using static HIDTester.HID;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace HIDTester
{

    public delegate void ShowMsg(string str);
    public partial class FormMain : Form
    {
        private HID M_Hid;
        private Thread mythread;
        public FormMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            OutputListBox.Clear();
            OpenDevice.Enabled = true;
            DevListCombox.Enabled = true;
            GetDeviceInfoBtn.Enabled = false;
            SendDataBtn.Enabled = false;
            CloseDeviceBtn.Enabled = false;
            GetPowrZValue.Enabled = false;
            DataTypeCombox.SelectedIndex = 1;

            txtVendorID.Clear();
            txtProductID.Clear();
            M_Hid = new HID();
            M_Hid.DataReceived += DataReceived; //订阅DataRec事件
            M_Hid.DeviceRemoved += DeviceRemoved;
            DeviceArrived();
            if (DevListCombox.Items.Count > 0)
            {
                DevListCombox.SelectedIndex = 0;
            }
        }
        private void OpenDevice_Click(object sender, EventArgs e)
        {
            if (M_Hid.DeviceOpened == false)
            {
                if (M_Hid.OpenDevice(DevListCombox.SelectedItem.ToString()) != M_Hid.INVALID_HANDLE_VALUE)
                {
                    OpenDevice.Enabled = false;
                    DevListCombox.Enabled = false;
                    GetDeviceInfoBtn.Enabled = true;
                    SendDataBtn.Enabled = true;
                    CloseDeviceBtn.Enabled = true;
                    if (M_Hid.Device.DevicePath.Contains(@"vid_0483&pid_5750"))
                    {
                        GetPowrZValue.Enabled = true;
                    }

                    HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
                    HidD_GetAttributes(M_Hid.Device.HidDevice, out attributes);
                    txtVendorID.Text = attributes.VendorID.ToString("x4");
                    txtProductID.Text = attributes.ProductID.ToString("x4");

                    StateLabel.Text = "设备打开";
                    StateLabel.BackColor = this.StateLabel.BackColor = Color.Green;
                }
                else
                {
                    OpenDevice.Enabled = true;
                    DevListCombox.Enabled = true;
                    GetDeviceInfoBtn.Enabled = false;
                    SendDataBtn.Enabled = false;
                    CloseDeviceBtn.Enabled = false;
                    GetPowrZValue.Enabled = false;

                    txtVendorID.Clear();
                    txtProductID.Clear();
                    MessageBox.Show("open drive Failed");
                    StateLabel.BackColor = this.StateLabel.BackColor = Color.Red;
                }
            }
            else
            {
                OpenDevice.Enabled = false;
                DevListCombox.Enabled = false;
                GetDeviceInfoBtn.Enabled = true;
                SendDataBtn.Enabled = true;
                CloseDeviceBtn.Enabled = true;
                GetPowrZValue.Enabled = true;

                StateLabel.Text = "设备打开";
                StateLabel.BackColor = this.StateLabel.BackColor = Color.Green;
            }
        }
        protected void DataReceived(object sender, report e)//数据到达事件
        {
            Thread.Sleep(200);
            string receiveData = Getsting(e.reportBuff);
            if (M_Hid.Device.DevicePath.Contains(@"vid_0483&pid_5750"))
            {
                int lengthTemp = 6;
                byte[] byteTemp = new byte[4];
                Array.Copy(e.reportBuff, lengthTemp, byteTemp, 0, byteTemp.Length);
                int valtemp = BitConverter.ToInt32(byteTemp, 0);
                OutputListBox.AppendText(string.Format($"电压: {valtemp}uV\r\n"));

                lengthTemp += byteTemp.Length;
                Array.Copy(e.reportBuff, lengthTemp, byteTemp, 0, byteTemp.Length);
                int curtemp = BitConverter.ToInt32(byteTemp, 0);
                OutputListBox.AppendText(string.Format($"电流: {Math.Abs(curtemp)}uA\r\n"));
            }
            else
            {
                OutputListBox.AppendText(receiveData);
            }
        }
        private string Getsting(byte[] by)
        {
            string str = "";
            for (int i = 0; i < by.Length; i++)
            {
                if (by[i] < 16)
                {
                    str += "0x0" + Convert.ToString(by[i], 16) + ",";
                }
                else
                {
                    str += "0x" + Convert.ToString(by[i], 16) + ",";
                }
            }
            return str;
        }
        protected void DeviceRemoved(object sender, EventArgs e)//设备移除事件
        {
            OpenDevice.Enabled = true;
            DevListCombox.Enabled = true;
            GetDeviceInfoBtn.Enabled = false;
            SendDataBtn.Enabled = false;
            CloseDeviceBtn.Enabled = false;
            GetPowrZValue.Enabled = false;
            txtVendorID.Clear();
            txtProductID.Clear();
        }
        private void ClearRecData_Click(object sender, EventArgs e)
        {
            OutputListBox.Clear();
        }
        private void CloseDevice_Click(object sender, EventArgs e)
        {
            M_Hid.CloseDevice();
            StateLabel.Text = "设备关闭";
            StateLabel.BackColor = SystemColors.ActiveCaption;
            OpenDevice.Enabled = true;
            DevListCombox.Enabled = true;
            GetDeviceInfoBtn.Enabled = false;
            SendDataBtn.Enabled = false;
            CloseDeviceBtn.Enabled = false;
            txtVendorID.Clear();
            txtProductID.Clear();
        }
        private void SendData_Click(object sender, EventArgs e)
        {
            //55 05 22 05 0b 01 8d 1c   //PoweZ读取电压电流指令
            //02 0a 06 01 0d 0a         //联想音响指令
            if (SendDataSousse.Text != "")
            {
                List<byte> dataBytes = StringToBytes(DataTypeCombox.SelectedItem.ToString(), SendDataSousse.Text);
                if (dataBytes.Count < 64)
                {
                    dataBytes.AddRange((new byte[64 - dataBytes.Count]));
                }
                M_Hid.Write(dataBytes.ToArray());
                //HidD_SetOutputReport(M_Hid.Device.HidDevice, dataBytes.ToArray(), dataBytes.Count);
                //var aa = HidD_SetFeature(M_Hid.Device.HidDevice, dataBytes.ToArray(), dataBytes.Count);
                //var bb = GetLastError();
                //byte[] data = new byte[126];
                //HidD_GetFeature(M_Hid.Device.HidDevice, data, dataBytes.Count);   
            }
            else
            {
                MessageBox.Show("请输入需要发送的数据");
            }
        }
        public List<byte> StringToBytes(string type, string Str)//Hex数据转为byte
        {
            List<byte> arryByte = new List<byte>();
            switch (type)
            {
                case "Byte":
                    {
                        var arrstr = Str.Split(new[] { "0x", " ", "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arrstr.Length; i++)
                        {
                            byte intval = 0;
                            intval = byte.Parse(arrstr[i], NumberStyles.HexNumber);
                            arryByte.Add(intval);
                        }
                    }
                    break;
                case "String":
                    {
                        //hexString = Regex.Escape(hexString);//将转义字符转为转义符串
                        Str = Regex.Unescape(Str);//将转义字符串转为转义符
                        arryByte = Encoding.ASCII.GetBytes(Str).ToList();
                    }
                    break;
            }
            return arryByte;
        }
        private void GetDeviceInfo_Click(object sender, EventArgs e)
        {
            M_Hid.GetDeviceInfo(DevListCombox.SelectedIndex, ShowRunInfo);
        }
        public void ShowRunInfo(string str)
        {
            try
            {
                OutputListBox.AppendText(str + "\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void DeviceArrived()
        {
            DevListCombox.Items.Clear();
            M_Hid.GetDeviceList(AddDeviceItem);
            if (M_Hid.Device.HidDevice == M_Hid.INVALID_HANDLE_VALUE)
            {
                if (DevListCombox.Items.Count > 0)
                {
                    DevListCombox.SelectedIndex = 0;
                }
            }
            else
            {
                if (M_Hid.Device.DevicePath != "")
                {
                    DevListCombox.SelectedItem = M_Hid.Device.DevicePath;
                }
                else
                {
                    DevListCombox.SelectedIndex = 0;
                }
            }
        }
        private void DeviceRemoved()
        {
            DevListCombox.Items.Clear();
            M_Hid.GetDeviceList(AddDeviceItem);
            if (!DevListCombox.Items.Contains(M_Hid.Device.DevicePath))
            {
                M_Hid.CloseDevice();
                if (DevListCombox.Items.Count > 0)
                {
                    DevListCombox.SelectedIndex = 0;
                }
            }
            else
            {
                if (M_Hid.Device.HidDevice == M_Hid.INVALID_HANDLE_VALUE)
                {
                    if (DevListCombox.Items.Count > 0)
                    {
                        DevListCombox.SelectedIndex = 0;
                    }
                }
                else
                {
                    DevListCombox.SelectedItem = M_Hid.Device.DevicePath;
                }
            }
        }
        public void AddDeviceItem(string str)
        {
            DevListCombox.Items.Add(str);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Guid gHid = new Guid();
            HidD_GetHidGuid(ref gHid);

            DeviceBroadcastInterface oInterfaceIn = new DeviceBroadcastInterface();
            oInterfaceIn.Size = Marshal.SizeOf(oInterfaceIn);
            oInterfaceIn.ClassGuid = gHid;
            oInterfaceIn.DeviceType = HID.DEVTYP_DEVICEINTERFACE;
            oInterfaceIn.Reserved = 0;
            RegisterDeviceNotification(Handle, oInterfaceIn, HID.DEVICE_NOTIFY_WINDOW_HANDLE);
        }
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)  // we got a device change message! A USB device was inserted or removed
                {
                    switch (m.WParam.ToInt32()) // Check the W parameter to see if a device was inserted or removed
                    {
                        case DEVICE_ARRIVAL:   // inserted
                            DeviceArrived();
                            break;
                        case DEVICE_REMOVECOMPLETE:    // removed
                            DeviceRemoved();
                            break;
                    }
                }
                base.WndProc(ref m); // pass message on to base form
            }
            catch (Exception e)
            {

            }
        }
        private void ExitSoftWare_Click(object sender, EventArgs e)
        {

        }
        private void GetPowrZValue_Click(object sender, EventArgs e)
        {
            List<byte> dataBytes = new List<byte>(new byte[] { 0x55, 0x05, 0x22, 0x05, 0x0b, 0x01, 0x8d, 0x1c });
            if (dataBytes.Count < 64)
            {
                dataBytes.AddRange((new byte[64 - dataBytes.Count]));
            }
            //55 05 22 05 0b 01 8d 1c   //PoweZ读取电压电流指令
            //02 0a 06 01 0d 0a         //联想音响指令
            M_Hid.Write(dataBytes.ToArray());
            //HidD_SetOutputReport(M_Hid.Device.HidDevice, dataBytes.ToArray(), dataBytes.Count);

            //HidD_SetFeature(M_Hid.Device.HidDevice, dataBytes.ToArray(), dataBytes.Count);
        }
    }
}
