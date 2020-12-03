using System.Threading;
using Device.Net;
using DSProject;

namespace DualSenseSupport
{
    public class DeviceInfo
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
        private readonly DSLight _dsLight= new DSLight();
        private readonly DsTrigger _dsTriggerLeft= new DsTrigger();
        private readonly DsTrigger _dsTriggerRight= new DsTrigger();
        private int OveralMotors = 0;
        
        private Thread _timeoutCheckThread;
        private bool _isClosed = false;


        public void SetLightBrightness(int value)
        {
            _dsLight.Brightness = value.InRange(0, 2);
        }

        public void SetPlayerNumber(int value)
        {
            _dsLight.PlayerNumber = value.InRange(0, 31);
        }

        public void SetColor(System.Drawing.Color color)
        {
            _dsLight.Colors.Blue = color.B;
            _dsLight.Colors.Green = color.G;
            _dsLight.Colors.Red = color.R;
        }

        public void SetTriggerLeft(DsTrigger trigger)
        {
            _dsTriggerLeft.Mode = trigger.Mode;
            _dsTriggerLeft.Force1 = trigger.Force1;
            _dsTriggerLeft.Force2 = trigger.Force2;
            _dsTriggerLeft.Force3 = trigger.Force3;
            _dsTriggerLeft.Force4 = trigger.Force4;
            _dsTriggerLeft.Force5 = trigger.Force5;
            _dsTriggerLeft.Force6 = trigger.Force6;
            _dsTriggerLeft.Force7 = trigger.Force7;
        }
        public void SetTriggerRight(DsTrigger trigger)
        {
            _dsTriggerRight.Mode = trigger.Mode;
            _dsTriggerRight.Force1 = trigger.Force1;
            _dsTriggerRight.Force2 = trigger.Force2;
            _dsTriggerRight.Force3 = trigger.Force3;
            _dsTriggerRight.Force4 = trigger.Force4;
            _dsTriggerRight.Force5 = trigger.Force5;
            _dsTriggerRight.Force6 = trigger.Force6;
            _dsTriggerRight.Force7 = trigger.Force7;
        }

        public void SetLedMode(DSLight.LedOptions ledOptions)
        {
            _dsLight.LedOption = (int) ledOptions;
        }
        public void SetPulseMode(DSLight.PulseOptions pulseOptions)
        {
            _dsLight.PulseOption = (int) pulseOptions;
        }

        public void SetOveralMotors(int value)
        {
            OveralMotors = value.InRange(0, 7);
        }
        
        public void Send()
        {
            if (ConnectionMethod == ConnectionType.Usb)
            {
                
                var outputReport = new byte[(int) Connection.WriteBufferSize];

                outputReport[0] = 0x02; //REPORT TYPE
                outputReport[1] = 0x4 | 0x8 ; //CONTROL FLAGS
                // outputReport[1] = 0x4 | 0x8 | 0x10 | 0x20 | 0x40; //CONTROL FLAGS
                // outputReport[1] = 0x00; //CONTROL FLAGS
                outputReport[2] = 0x1 | 0x4 | 0x10 | 0x40; //Control flags
                // outputReport[2] = 0x1 | 0x4 | 0x10 | 0x40; //Control flags
                
                
                //AUDIO TESTING
                // outputReport[5]  = 0x00; // audio volume of connected headphones (maxes out at about 0x7f)
                
                outputReport[39] = (byte) _dsLight.LedOption; //LED CONTROL

                outputReport[44] = (byte) _dsLight.PlayerNumber;
                outputReport[43] = (byte) _dsLight.Brightness; //Brillo Pulso
                outputReport[42] = (byte) _dsLight.PulseOption; //Pulse Option

                //COLORS
                outputReport[45] = (byte) _dsLight.Colors.Red; //R
                outputReport[46] = (byte) _dsLight.Colors.Green; //G
                outputReport[47] = (byte) _dsLight.Colors.Blue; //B


                //Triggers
                outputReport[11] = (byte) _dsTriggerRight.Mode; //Mode Motor Right
                outputReport[12] = (byte) _dsTriggerRight.Force1; //right trigger start of resistance section
                outputReport[13] =
                    (byte) _dsTriggerRight
                        .Force2; //right trigger (mode1) amount of force exerted (mode2) end of resistance section supplemental mode 4+20) flag(s?) 0x02 = do not pause effect when fully presse
                outputReport[14] = (byte) _dsTriggerRight.Force3; //right trigger force exerted in range (mode2)
                outputReport[15] =
                    (byte) _dsTriggerRight
                        .Force4; // strength of effect near release state (requires supplement modes 4 and 20)
                outputReport[16] =
                    (byte) _dsTriggerRight.Force5; // strength of effect near middle (requires supplement modes 4 and 20)
                outputReport[17] =
                    (byte) _dsTriggerRight
                        .Force6; // strength of effect at pressed state (requires supplement modes 4 and 20)
                outputReport[20] =
                    (byte) _dsTriggerRight
                        .Force7; // effect actuation frequency in Hz (requires supplement modes 4 and 20)

                if (_dsTriggerRight.Mode == DsTrigger.Modes.GameCube)
                {
                    outputReport[11] = (byte) DsTrigger.Modes.Pulse; //Mode Motor Right
                    outputReport[12] = 0x90; //right trigger start of resistance section
                    outputReport[13] = 0xA0; //right trigger (mode1) amount of force exerted (mode2) end of resistance section supplemental mode 4+20) flag(s?) 0x02 = do not pause effect when fully presse
                    outputReport[14] = 0xFF; //right trigger force exerted in range (mode2)
                    outputReport[15] = 0x0; // strength of effect near release state (requires supplement modes 4 and 20)
                    outputReport[16] = 0x0; // strength of effect near middle (requires supplement modes 4 and 20)
                    outputReport[17] = 0x0; // strength of effect at pressed state (requires supplement modes 4 and 20)
                    outputReport[20] = 0x0; // effect actuation frequency in Hz (requires supplement modes 4 and 20)
                }
                
                
                outputReport[22] = (byte) _dsTriggerLeft.Mode; //Mode Motor Left
                outputReport[23] = (byte) _dsTriggerLeft.Force1; //right Left start of resistance section
                outputReport[24] =
                    (byte) _dsTriggerLeft
                        .Force2; //right Left (mode1) amount of force exerted (mode2) end of resistance section supplemental mode 4+20) flag(s?) 0x02 = do not pause effect when fully presse
                outputReport[25] = (byte) _dsTriggerLeft.Force3; //right Left force exerted in range (mode2)
                outputReport[26] =
                    (byte) _dsTriggerLeft
                        .Force4; // strength of effect near release state (requires supplement modes 4 and 20)
                outputReport[27] =
                    (byte) _dsTriggerLeft.Force5; // strength of effect near middle (requires supplement modes 4 and 20)
                outputReport[28] =
                    (byte) _dsTriggerLeft
                        .Force6; // strength of effect at pressed state (requires supplement modes 4 and 20)
                outputReport[31] =
                    (byte) _dsTriggerLeft
                        .Force7; // effect actuation frequency in Hz (requires supplement modes 4 and 20)
                
                if (_dsTriggerLeft.Mode == DsTrigger.Modes.GameCube)
                {
                    outputReport[22] = (byte) DsTrigger.Modes.Pulse; //Mode Motor Right
                    outputReport[23] = 0x90; //right trigger start of resistance section
                    outputReport[24] = 0xA0; //right trigger (mode1) amount of force exerted (mode2) end of resistance section supplemental mode 4+20) flag(s?) 0x02 = do not pause effect when fully presse
                    outputReport[25] = 0xFF; //right trigger force exerted in range (mode2)
                    outputReport[26] = 0x0; // strength of effect near release state (requires supplement modes 4 and 20)
                    outputReport[27] = 0x0; // strength of effect near middle (requires supplement modes 4 and 20)
                    outputReport[28] = 0x0; // strength of effect at pressed state (requires supplement modes 4 and 20)
                    outputReport[31] = 0x0; // effect actuation frequency in Hz (requires supplement modes 4 and 20)
                }

                outputReport[37] = (byte) OveralMotors;

                Device.WriteAsync(outputReport).Wait();
            }

          
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
            _timeoutCheckThread.Name = "DualSense Timeout thread: " + Device.DeviceId;
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
                Send();
                Thread.Sleep(1);
            }
        }
    }
}