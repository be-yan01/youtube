using System;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;


namespace Five_Nights_At_Fleddys_Killer
{
    public partial class Form1 : Form
    {
        private delegate void ActivateFormDelegate();

        WasapiLoopbackCapture capture = new WasapiLoopbackCapture();

        float peakThreshold = 0.3f;
        float currentPeak   = 0.0f;

        public Form1()
        {
            InitializeComponent();

            capture.DataAvailable += (s, a) =>
            {
                var peak = 0.0f;
                for (int i = 0; i < a.BytesRecorded; i = i + 4)
                {
                    float f = BitConverter.ToSingle(a.Buffer, 0);

                    f = Math.Abs(f);

                    if (peak < f)
                    {
                        peak = f;
                    }
                }
                if (peakThreshold < 1.0f && peak > peakThreshold)
                {
                    setVolume(0.0f);
                    var d = new ActivateFormDelegate(this.Activate);
                    this.Invoke(d);
                }

                if (currentPeak < peak)
                {
                    currentPeak = peak;
                }
            };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(peakThreshold * 100.0f);
            label3.Text = Float2String(peakThreshold);

            capture.StartRecording();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            capture.StopRecording();
        }

        private void activateWindow()
        {
            this.Activate();
        }

        private void setVolume(float level)
        {
            MMDevice device;
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            device.AudioEndpointVolume.MasterVolumeLevelScalar = level;

            //label6.Text = ((int)(level * 100.0f)).ToString();
        }

        private void updateVolumeLavel()
        {
            MMDevice device;
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            float level = device.AudioEndpointVolume.MasterVolumeLevelScalar;

            label6.Text = ((int)(level * 100.0f)).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setVolume(0.0f);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            setVolume(1.0f);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            peakThreshold = (float)trackBar1.Value / 100.0f;
            label3.Text = peakThreshold < 1.0f ? Float2String(peakThreshold) : "なし";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = (int)(currentPeak * 100.0f + 0.5f);
            label4.Text = Float2String(currentPeak);
            currentPeak = 0.0f;

            updateVolumeLavel();
        }

        private static string Float2String(float val)
        {
            return val.ToString("F2");
        }

    }
}
