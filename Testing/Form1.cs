using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSProject;
using DualSenseSupport;
using Microsoft.SqlServer.Server;

namespace Testing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // AllocConsole();
            
            DualSenseSupport.Devices.Init();

            
            ComboIndex.DisplayMember = "Text";
            ComboIndex.ValueMember = "Value";
            var list= new List<object>();
            for (int i = 0; i < DualSenseSupport.Devices.GetDeviceCount(); i++)
            {
                list.Add(new {Text=$"Input #{i}", Value= i});
            }

            ComboIndex.DataSource = list;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            
            var listModes= new List<object>();
            listModes.Add(new {Text="Off",Value=DsTrigger.Modes.Off});
            listModes.Add(new {Text="Rigid",Value=DsTrigger.Modes.Rigid});
            listModes.Add(new {Text="Pulse",Value=DsTrigger.Modes.Pulse});
            listModes.Add(new {Text="Rigid + Extra1",Value=DsTrigger.Modes.Rigid_A});
            listModes.Add(new {Text="Rigid + Extra2",Value=DsTrigger.Modes.Rigid_B});
            listModes.Add(new {Text="Rigid + Extra1 + Extra 2",Value=DsTrigger.Modes.Rigid_AB});
            listModes.Add(new {Text="Pulse + Extra1 ",Value=DsTrigger.Modes.Pulse_A});
            listModes.Add(new {Text="Pulse + Extra2 ",Value=DsTrigger.Modes.Pulse_B});
            listModes.Add(new {Text="Pulse + Extra1 + Extra 2 ",Value=DsTrigger.Modes.Pulse_AB});
            listModes.Add(new {Text="GameCube ",Value=DsTrigger.Modes.GameCube});
            listModes.Add(new {Text="Calibration ",Value=DsTrigger.Modes.Calibration});
            var listModes2= new List<object>();
            listModes2.Add(new {Text="Off",Value=DsTrigger.Modes.Off});
            listModes2.Add(new {Text="Rigid",Value=DsTrigger.Modes.Rigid});
            listModes2.Add(new {Text="Pulse",Value=DsTrigger.Modes.Pulse});
            listModes2.Add(new {Text="Rigid + Extra1",Value=DsTrigger.Modes.Rigid_A});
            listModes2.Add(new {Text="Rigid + Extra2",Value=DsTrigger.Modes.Rigid_B});
            listModes2.Add(new {Text="Rigid + Extra1 + Extra 2",Value=DsTrigger.Modes.Rigid_AB});
            listModes2.Add(new {Text="Pulse + Extra1 ",Value=DsTrigger.Modes.Pulse_A});
            listModes2.Add(new {Text="Pulse + Extra2 ",Value=DsTrigger.Modes.Pulse_B});
            listModes2.Add(new {Text="Pulse + Extra1 + Extra 2 ",Value=DsTrigger.Modes.Pulse_AB});
            listModes2.Add(new {Text="GameCube ",Value=DsTrigger.Modes.GameCube});
            listModes2.Add(new {Text="Calibration ",Value=DsTrigger.Modes.Calibration});
            comboLeftMode.DisplayMember = "Text";
            comboLeftMode.ValueMember = "Value";
            comboLeftMode.DataSource = listModes;
            comboRightMode.DisplayMember = "Text";
            comboRightMode.ValueMember = "Value";
            comboRightMode.DataSource = listModes2;
            

        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void button1_Click(object sender, EventArgs e)
        {
            var result=colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetColor(colorDialog1.Color);
                
            }
        }

        private int? SelectedDevice()
        {
            return (int?) ComboIndex.SelectedValue;
        }

        private void trackR_Scroll(object sender, EventArgs e)
        {
            var color = Color.FromArgb(trackR.Value,0,0);
            
            trackR.BackColor=color;
            ChangeImage();
        }

        private void ChangeImage()
        {
            pictureBox1.BackColor = Color.FromArgb(trackR.Value, trackG.Value, trackB.Value);
            if(SelectedDevice().HasValue)
                Devices.GetDevice(SelectedDevice().Value)
                    .SetColor(pictureBox1.BackColor);
        }

        private void trackG_Scroll(object sender, EventArgs e)
        {
            var color = Color.FromArgb(0,trackG.Value,0);
            
            trackG.BackColor=color;
            ChangeImage();
        }

        private void trackB_Scroll(object sender, EventArgs e)
        {
            var color = Color.FromArgb(0,0,trackB.Value);
            
            trackB.BackColor=color;
            ChangeImage();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            labelPlayerNumber.Text = trackBar1.Value.ToString();
            if(SelectedDevice().HasValue)
                Devices.GetDevice(SelectedDevice().Value)
                    .SetPlayerNumber(trackBar1.Value);
            
        
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if(SelectedDevice().HasValue)
                Devices.GetDevice(SelectedDevice().Value)
                    .SetLightBrightness(trackBar2.Value);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetLedMode(DSLight.LedOptions.None);
            }
            if (comboBox1.SelectedIndex == 1)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetLedMode(DSLight.LedOptions.PlayerLedBrightnes);
            }
            if (comboBox1.SelectedIndex == 2)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetLedMode(DSLight.LedOptions.UninterrumpableLed);
            }
            if (comboBox1.SelectedIndex == 3)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetLedMode(DSLight.LedOptions.Both);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetPulseMode(DSLight.PulseOptions.None);
            }
            if (comboBox1.SelectedIndex == 1)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetPulseMode(DSLight.PulseOptions.FadeBlue);
            }
            if (comboBox1.SelectedIndex == 2)
            {
                if(SelectedDevice().HasValue)
                    Devices.GetDevice(SelectedDevice().Value)
                        .SetPulseMode(DSLight.PulseOptions.FadeOut);
            }
        }

        private void comboLeftMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void l_v_1_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void UpdateLeftTrigger()
        {
            l_l_1.Text = l_v_1.Value.ToString();
            l_l_2.Text = l_v_2.Value.ToString();
            l_l_3.Text = l_v_3.Value.ToString();
            l_l_4.Text = l_v_4.Value.ToString();
            l_l_5.Text = l_v_5.Value.ToString();
            l_l_6.Text = l_v_6.Value.ToString();
            l_l_7.Text = l_v_7.Value.ToString();
            
            if(SelectedDevice().HasValue)
                Devices.GetDevice(SelectedDevice().Value)
                    .SetTriggerLeft(
                        new DsTrigger(
                            (DsTrigger.Modes) comboLeftMode.SelectedValue,
                                l_v_1.Value,
                                l_v_2.Value,
                                l_v_3.Value,
                                l_v_4.Value,
                                l_v_5.Value,
                                l_v_6.Value,
                                l_v_7.Value
                            ));
        }

        private void l_v_2_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }



        private void l_v_4_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void l_v_5_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void l_v_7_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void l_v_6_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void l_v_3_Scroll(object sender, EventArgs e)
        {
            UpdateLeftTrigger();
        }

        private void comboRightMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }
        public void UpdateRightTrigger()
        {
            r_l_1.Text = r_v_1.Value.ToString();
            r_l_2.Text = r_v_2.Value.ToString();
            r_l_3.Text = r_v_3.Value.ToString();
            r_l_4.Text = r_v_4.Value.ToString();
            r_l_5.Text = r_v_5.Value.ToString();
            r_l_6.Text = r_v_6.Value.ToString();
            r_l_7.Text = r_v_7.Value.ToString();
            
            if(SelectedDevice().HasValue)
                Devices.GetDevice(SelectedDevice().Value)
                    .SetTriggerRight(
                        new DsTrigger(
                            (DsTrigger.Modes) comboRightMode.SelectedValue,
                            r_v_1.Value,
                            r_v_2.Value,
                            r_v_3.Value,
                            r_v_4.Value,
                            r_v_5.Value,
                            r_v_6.Value,
                            r_v_7.Value
                        ));
        }

        private void r_v_1_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }

        private void r_v_2_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }

        private void r_v_3_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }
        
        private void r_v_4_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }


        private void r_v_5_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }

        private void r_v_6_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }

        private void r_v_7_Scroll(object sender, EventArgs e)
        {
            UpdateRightTrigger();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
           

        }

        private void trackBar3_Scroll_1(object sender, EventArgs e)
        {
             if (SelectedDevice().HasValue)
                            Devices.GetDevice(SelectedDevice().Value)
                                .SetOveralMotors(trackBar3.Value);
        }
    }
        
}
   