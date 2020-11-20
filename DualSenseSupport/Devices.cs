using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Device.Net;
using Hid.Net.Windows;
using Usb.Net.Windows;


namespace DualSenseSupport
{
    public class Devices
    {
        private static readonly DSProject.Logger _logger = new DSProject.Logger();
        private static readonly Tracer _tracer = new Tracer();
        
        
        private static List<DeviceInfo> devicesList= new List<DeviceInfo>();
        
        public static void Init()
        {
            WindowsHidDeviceFactory.Register(_logger, _tracer);
            WindowsUsbDeviceFactory.Register(_logger, _tracer);

            SearchDS5Controller();
        }

        public static int GetDeviceCount()
        {
            return devicesList.Count;
        }

        public static DeviceInfo GetDevice(int index)
        {
            return devicesList[index];
        } 
        

        public static IEnumerable<DeviceInfo> EnumerateDevices()
        {
            IEnumerable<ConnectedDeviceDefinition> devicesList = new List<ConnectedDeviceDefinition>();
            var thread = new Thread(o =>
            {
                devicesList = DeviceManager.Current.GetConnectedDeviceDefinitionsAsync(new FilterDeviceDefinition())
                    .Result;
            });
            thread.Start();
            thread.Join();
     
            // var task =  DeviceManager.Current.GetConnectedDeviceDefinitionsAsync(new FilterDeviceDefinition());
            
            // devicesList = task.Result;
            var devices = new List<DeviceInfo>();
            foreach (var deviceDefinition in devicesList)
            {
                if (deviceDefinition.VendorId == 1356)
                {
                    var device = new DeviceInfo
                    {
                        Connection = deviceDefinition,
                        Device = DeviceManager.Current.GetDevice(deviceDefinition),
                        Uid= Guid.NewGuid().ToString()
                    };
                    if (device.Connection.WriteBufferSize == 48)
                    {
                        device.ConnectionMethod = DeviceInfo.ConnectionType.Usb;
                    }
                    else if(device.Connection.WriteBufferSize == 547)
                    {
                        device.ConnectionMethod = DeviceInfo.ConnectionType.Bluetooth;
                    }
                    else
                    {
                        device.ConnectionMethod = DeviceInfo.ConnectionType.Unknow;
                    }
                    devices.Add(device);
                }
            }



            return devices;
        }

        public static void SearchDS5Controller()
        {
            var devices=EnumerateDevices();
            foreach (var deviceInfo in devicesList.Where(deviceInfo => deviceInfo.Device.IsInitialized))
            {
                deviceInfo.Close();
            }

            foreach (var deviceInfo in devices)
            {
                devicesList.Add(deviceInfo);
                deviceInfo.Init();
            }
            
        }


        public static void Close()
        {
            foreach (var deviceInfo in devicesList.Where(deviceInfo => deviceInfo.Device.IsInitialized))
            {
                deviceInfo.Close();
            }
        }
        
    }
    
   
}