using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using InTheHand.Net.Bluetooth.AttributeIds;
using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using Windows.Media.Protection.PlayReady;
using System.Runtime.InteropServices.ComTypes;


namespace XDTK32Feet
{
    public static class XDTK32Feet
    {
        public static BluetoothClient cli;
        public static BluetoothDeviceInfo mDevice = null;
        public static IReadOnlyCollection<BluetoothDeviceInfo> peers;
        public static Stream stream = null;
        public static ArrayList peersNames = new ArrayList();

        public static bool GenerateConnectionToDevice(string androidDeviceName = "XDTKAndroid3")
        {
            cli = new BluetoothClient();
            peers = cli.DiscoverDevices();

            foreach (var deviceInfo in peers)
            {
                if (deviceInfo.DeviceName == androidDeviceName)
                {
                    mDevice = deviceInfo;
                }
            }

            if (mDevice == null)
                // Did not find intended device
                return false;

            BluetoothEndPoint endPoint = new BluetoothEndPoint(mDevice.DeviceAddress, BluetoothService.SerialPort);

            cli.Connect(mDevice.DeviceAddress, Guid.Parse("59a8bede-af7b-49de-b454-e9e469e740ab"));

            stream = cli.GetStream();

            return true;
        }

        public static async void GenerateConnectionUsingPicker()
        {
            cli = new BluetoothClient();

            var picker = new BluetoothDevicePicker();
            mDevice = await picker.PickSingleDeviceAsync();
            // Translation to 32Feet
            // Service Class is Telephony + Object Transfer + Capturing + Network
            // Device Class is DeviceClass.SmartPhone 

            BluetoothEndPoint endPoint = new BluetoothEndPoint(mDevice.DeviceAddress, BluetoothService.SerialPort);

            cli.Connect(mDevice.DeviceAddress, Guid.Parse("59a8bede-af7b-49de-b454-e9e469e740ab"));

            stream = cli.GetStream();
        }

        public static string read()
        {
            byte[] bytes = new byte[1024];
            int bytesRead = stream.Read(bytes, 0, bytes.Length);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytesRead);
        }

        public static ArrayList TestDetection()
        {
            peersNames.Clear();

            cli = new BluetoothClient();
            peers = cli.DiscoverDevices();

            foreach (var deviceInfo in peers)
            {
                peersNames.Add("DN: " + deviceInfo.DeviceName);
            }

            return peersNames;
        }

        public static void Close()
        {
            stream.Close();
            cli.Close();
        }
    }
}
