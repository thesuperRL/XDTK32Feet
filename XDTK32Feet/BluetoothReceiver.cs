using System;
using System.Collections.Generic;
using System.Collections;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO;


namespace XDTK32Feet
{
    public static class BluetoothReceiver
    {
        public static BluetoothClient cli;
        public static BluetoothDeviceInfo mDevice = null;
        public static IReadOnlyCollection<BluetoothDeviceInfo> peers;
        public static Stream stream = null;
        public static ArrayList peersNames = new ArrayList();

        // Generates a connection to the device via a picker
        public static async void GenerateConnectionUsingPicker()
        {
            cli = new BluetoothClient();

            // A picker that borrows the default picker on the device running Unity to pick a bluetooth device
            // May have some problems with less common devices, but definitely works on Windows and is said to work on Mac too
            var picker = new BluetoothDevicePicker(); 
            // Filters can be made within 32Feet's code for specific devices. Be sure to check your device's Service Class before doing a filter
            // Note: reason the phone filters are not on by default is because they are currently not functioning (32Feet's creator said that the filter was reversed)
            mDevice = await picker.PickSingleDeviceAsync();

            BluetoothEndPoint endPoint = new BluetoothEndPoint(mDevice.DeviceAddress, BluetoothService.SerialPort);

            cli.Connect(mDevice.DeviceAddress, Guid.Parse("59a8bede-af7b-49de-b454-e9e469e740ab"));

            stream = cli.GetStream();
        }

        // Tests detection by asking 32Feet to list all discoverable devices in an ArrayList
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

        // Closes the entire system. Be sure to run this if you want to stop the system
        public static void Close()
        {
            stream.Close();
            cli.Close();
        }
    }
}
