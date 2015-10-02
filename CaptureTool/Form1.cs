using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace CaptureTool {
    public partial class Form1 : Form {

        private System.Timers.Timer _timer;

        private int _n;

        private System.Media.SoundPlayer player = null;

        private System.Reflection.Assembly _myAssembly;
        private System.IO.Stream _sound1;

        private DateTime _dt0;

        public Form1() {
            InitializeComponent();
            _myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            _sound1 = _myAssembly.GetManifestResourceStream("CaptureTool.shot.wav");
        }

        private void OnClickStartButton(object sender, EventArgs e) {
            this.label.Text = "Start";

            _dt0 = DateTime.Now.AddMinutes(-10);

            if (_timer == null) {
                _timer = new System.Timers.Timer(100);
                _timer.Elapsed += (object source, System.Timers.ElapsedEventArgs ee) => {

                    DateTime d = DateTime.Now;

                    // 1分に1枚スクリーンショットを撮影する。
                    if (d.Minute != _dt0.Minute) {
                        _dt0 = DateTime.Now;
                        CaptureScreenshot();
                    }

                    Invoke((MethodInvoker)(() => {
                        if (_n % 2 == 0) {
                            this.label.Text = d.ToString();
                        } else {
                            this.label.Text = d.ToString() + " .";
                        }
                    }));
                    _n++;
                };
                _timer.Enabled = true;
                _timer.Start();
            }
        }

        private void OnClickStopButton(object sender, EventArgs e) {
            this.label.Text = "Stop";

            if (_timer != null) {
                _timer.Stop();
                _timer = null;
            }

        }

        /// <summary>
        /// スクリーンショットを撮影する。
        /// </summary>
        private void CaptureScreenshot() {
            PlaySound();

            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //Graphicsの作成
            Graphics g = Graphics.FromImage(bmp);

            //画面全体をコピーする
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bmp.Size);
            //解放
            g.Dispose();

            string path = _dt0.ToString("yyyyMMdd-HHmm");

            CaptureUtils.SaveAsJpeg(bmp, path + ".jpg", 80);
            bmp.Dispose();
        }

        /// <summary>
        /// WAVEファイルを再生する。
        /// </summary>
        private void PlaySound() {
            //再生されている場合は止める
            if (player != null)
                StopSound();

            _sound1.Position = 0;

            //読み込む
            player = new System.Media.SoundPlayer(_sound1);
            //非同期再生する
            player.Play();
        }

        /// <summary>
        /// 再生されている音を止める。
        /// </summary>
        private void StopSound() {
            if (player != null) {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }

    }
}
