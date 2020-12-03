using System.Threading;
using Device.Net;
using DSProject;

namespace DualSenseSupport
{
    public class DeviceInfoXbox
    {
        public enum ConnectionType
        {
            Usb,
            Bluetooth,
            Unknow
        }
        

        public ConnectionType ConnectionMethod;
        public string Uid;
        public IDevice Device;
        public ConnectedDeviceDefinition Connection;
        
        
        private Thread _timeoutCheckThread;
        private bool _isClosed = false;


        
        
        public void Read()
        {
            var readResult = Device.ReadAsync().Result;
        }

        public void Init()
        {
            var thread = new Thread(() =>
            {
                Device.InitializeAsync().Wait();
            });
            thread.Start();
            thread.Join();


            _timeoutCheckThread = new Thread(TimeoutTestThread);
            _timeoutCheckThread.Priority = ThreadPriority.BelowNormal;
            _timeoutCheckThread.Name = "Xbox Timeout thread: " + Device.DeviceId;
            _timeoutCheckThread.IsBackground = true;
            _timeoutCheckThread.Start();
        }

        public void Close()
        {
            _isClosed = true;
            Device.Close();
        }

        private void TimeoutTestThread()
        {
            while (!_isClosed)
            {
                Read();
                Thread.Sleep(1);
            }
        }
    }
}