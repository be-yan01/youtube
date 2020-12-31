using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Matsuosama_eyecatch
{
    public partial class Form1 : Form
    {

        private System.Media.SoundPlayer player = null;
        private Form2 form2 = new Form2();
        private Form3 form3 = new Form3();

        [DllImport("user32", SetLastError = true)]
        private static extern Int16 GetAsyncKeyState(int vKey);

        const int VK_M = 0x4D;        // Mキー
        const int VK_CONTROL = 0x11;  // L-Ctrl
        const int VK_MENU = 0x12;   // L-ALT
        private static Form1 target = null;

        private static void InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            if (e.KeyCode == VK_M)
                if ((GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0)
                    if ((GetAsyncKeyState(VK_MENU) & 0x8000) != 0)
                    {
                        target.showEyecatch();
                        Console.WriteLine("Keydown KeyCode {0}", e.KeyCode);
                    }


        }

        InterceptKeyboard interceptKeyboard = new InterceptKeyboard();


        public Form1()
        {
            InitializeComponent();
            string app_path = Application.ExecutablePath;
            string soundfile_directory = System.IO.Path.GetDirectoryName(app_path);
            string soundfile_location = soundfile_directory + "\\Sound.wav";
            player = new System.Media.SoundPlayer(soundfile_location);

            this.Visible = false;

            target = this;
            interceptKeyboard.KeyDownEvent += InterceptKeyboard_KeyDownEvent;
            interceptKeyboard.Hook();

        }

        public void showEyecatch()
        {
            this.Visible = true;
            this.Activate();
            this.Visible = false;
            form3.Show();
            form2.Show();
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showEyecatch();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            player.PlaySync();
            form3.Visible = false;
            form2.Visible = false;
        }

        

    }
}
