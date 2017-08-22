using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Blue_Screen_saver
{   

    //UID3244214440
    public class MainForm : Form 
    {
        
        //UID1213014214
        #region load-dll

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion
        
        //UID3422034244
        #region Components

        private System.ComponentModel.IContainer components = null;        
        private AxWMPLib.AxWindowsMediaPlayer videoPlayer;

        #endregion
            
        //UID3130322030
        #region Atributes
        
        private bool IsPreviewMode = false;
        private String VideosPath = @"";
        private Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);
        #endregion
        
        
        #region Constructors
        //UID4323403030
        public MainForm(Rectangle Bounds, int ScreenNo, String videoPath)
        {
            #region Windows Form Designer generated code
            
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.videoPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // videoPlayer
            // 
            this.videoPlayer.Enabled = true;
            this.videoPlayer.Location = new System.Drawing.Point(7, 55);
            this.videoPlayer.Name = "videoPlayer";
            this.videoPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("videoPlayer.OcxState")));
            this.videoPlayer.Size = new System.Drawing.Size(273, 151);
            this.videoPlayer.TabIndex = 0;
            this.videoPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.VideoPlayer_PlayStateChange);
            this.videoPlayer.MouseDownEvent += new AxWMPLib._WMPOCXEvents_MouseDownEventHandler(this.VideoPlayer_MouseDownEvent);
            this.videoPlayer.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.VideoPlayer_PreviewKeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.videoPlayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "MainForm";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayer)).EndInit();
            this.ResumeLayout(false);

            #endregion

            this.VideosPath = videoPath;
            this.Bounds = Bounds;
            ShowOnMonitor(ScreenNo);
            Cursor.Hide();
            
            InicializePlayer();
        }
        
        //UID4401414233
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region GUI

        //UID3231024101
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!IsPreviewMode)
            {
                this.Refresh();
            }

            this.BackColor = Color.FromArgb(0, 0, 130);

            LoadPlaylist();
            Play();
        }


        #endregion

        #region User Input
        //UID0413340022
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        //UID1231222014
        private void MainForm_Click(object sender, EventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        
        //UID1234024234
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        
        #endregion

        #region Player
        
        //UID3342304432
        private void InicializePlayer()
        {
            videoPlayer.Top = 0;
            videoPlayer.Left = 0;
            videoPlayer.Width = this.Width;
            videoPlayer.Height = this.Height;
            videoPlayer.stretchToFit = true;            
            videoPlayer.uiMode = "none";
            videoPlayer.settings.autoStart = false;
        }
        
        //UID3222140003
        private void Play()
        {
            videoPlayer.Ctlcontrols.play();
            videoPlayer.settings.mute = true;
        }
            
        //UID1201044034
        public void ShowOnMonitor(int ScreenNo)
        {
            //ScreenNo = 1;

            Screen[] sc;
            sc = Screen.AllScreens;
            this.Left = sc[ScreenNo].Bounds.Width;
            this.Top = sc[ScreenNo].Bounds.Height;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = sc[ScreenNo].Bounds.Location;
            Point p = new Point(sc[ScreenNo].Bounds.Location.X, sc[ScreenNo].Bounds.Location.Y);
            this.Location = p;
        }

        //UID0132100220
        private void LoadPlaylist() {
            Random rng = new Random();
            var extensions = new string[] { ".avi", ".wmv", ".mpeg", ".mpg", ".m1v", ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".mov" };
            var di = new DirectoryInfo(VideosPath);
            var rgFiles = di.GetFiles("*.*", SearchOption.AllDirectories).Where(f => extensions.Contains(f.Extension.ToLower())).OrderBy(f => rng.Next());

            if (rgFiles.Count() == 0)
            {
                return;
            }

            var myPlaylist = videoPlayer.playlistCollection.newPlaylist("myPlaylist");

            foreach (var fileName in rgFiles)
            {
                var media = videoPlayer.newMedia(fileName.FullName);
                myPlaylist.appendItem(media);
            }

            videoPlayer.currentPlaylist = myPlaylist;
        }

        //UID2023223444
        private void VideoPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)//Media finished
            {
                LoadPlaylist();
                Play();
            }
        }
        
        //UID4402231220
       private void VideoPlayer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Application.Exit();
        }

        //UID0440411321
        private void VideoPlayer_MouseDownEvent(object sender, AxWMPLib._WMPOCXEvents_MouseDownEvent e)
        {
            Application.Exit();
        }
        
        #endregion
    }
}
