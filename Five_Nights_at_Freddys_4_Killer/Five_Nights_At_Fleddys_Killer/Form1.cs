using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;


namespace Five_Nights_At_Fleddys_Killer
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate();
        public Form1()
        {
            InitializeComponent();
        }

        private void activateWindow()
        {
            this.Activate();
        }

        private void volumeMute()
        {
            MMDevice device;
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            device.AudioEndpointVolume.MasterVolumeLevelScalar = 0.0f;
        }

        private void maxVolume()
        {
            MMDevice device;
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            device.AudioEndpointVolume.MasterVolumeLevelScalar = 1.0f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            maxVolume();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var capture = new WasapiLoopbackCapture();

            capture.DataAvailable += (s, a) =>
            {
                var maxVol = 0.0f;
                for (int i = 0; i < a.BytesRecorded; i = i + 4)
                {
                    float f = BitConverter.ToSingle(a.Buffer, 0);

                    f = Math.Abs(f);

                    if (maxVol < f)
                    {
                        maxVol = f;
                    }
                }
                if (maxVol > 0.3f)
                {
                    var d = new SafeCallDelegate(activateWindow);
                    this.Invoke(d);
                    volumeMute();
                    Console.WriteLine(maxVol);
                }


            };

            capture.StartRecording();
        }
    }
}
