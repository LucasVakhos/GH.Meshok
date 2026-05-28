using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace GH.UserForms
{
    public class DefSplashScreen : SplashScreen
    {
        private DevExpress.XtraEditors.ProgressBarControl progressBar;
        public DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.LabelControl labelRunBegin;
        private DevExpress.XtraEditors.LabelControl labelCopyrith;
        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgress;
        static DefSplashScreen instance = null;
        public DefSplashScreen()
        {
            instance = this;
            InitializeComponent();
        }

        public static DefSplashScreen Instance => instance;

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);

            SplashScreenCommand command = (SplashScreenCommand)cmd;
            SplashCommandArgs args = arg as SplashCommandArgs;
            switch (command)
            {
                case SplashScreenCommand.SetProgressMax:
                    progressBar.Properties.Maximum = args.Max;
                    break;
                case SplashScreenCommand.SetProgress:
                    if (marqueeProgress.Visible)
                    {
                        progressBar.Visible = true;
                        marqueeProgress.Visible = false;
                    }

                    if (progressBar.Properties.Maximum != args.Max)
                        progressBar.Properties.Maximum = args.Max;

                    progressBar.Position = args.Value;
                    break;
                case SplashScreenCommand.EndProgress:
                    progressBar.Visible = false;
                    marqueeProgress.Visible = true;
                    break;
                case SplashScreenCommand.Activate:
                    if (args.Value == 0)
                        WindowState = FormWindowState.Minimized;
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        Activate();
                    }
                    break;
                default:
                    break;
            }

        }

        public static string AssemblyTitle
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (customAttributes.Length > 0)
                {
                    AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
                    if (!string.IsNullOrEmpty(assemblyTitleAttribute.Title))
                        return assemblyTitleAttribute.Title;
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (customAttributes.Length == 0)
                    return "";
                return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
            }
        }

        #endregion

        public enum SplashScreenCommand
        {
            SetProgressMax,
            SetProgress,
            EndProgress,
            Activate
        }

        public class SplashCommandArgs
        {
            int _max = 0;
            int _value = 0;

            public SplashCommandArgs(int max, int value)
            {
                _max = max;
                _value = value;
            }

            public int Value { get => _value; }
            public int Max { get => _max; }
        }

        private void DefSplashScreen_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            progressBar.Hide();
            progressBar.Bounds = marqueeProgress.Bounds;

            labelCopyrith.Text = AssemblyCopyright;
            labelRunBegin.Text = "Запуск " + AssemblyTitle;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefSplashScreen));
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.labelRunBegin = new DevExpress.XtraEditors.LabelControl();
            this.labelCopyrith = new DevExpress.XtraEditors.LabelControl();
            this.marqueeProgress = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgress.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(123, 238);
            this.progressBar.Name = "progressBar";
            this.progressBar.Properties.ShowTitle = true;
            this.progressBar.Size = new System.Drawing.Size(315, 18);
            this.progressBar.TabIndex = 11;
            this.progressBar.Visible = false;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.AllowFocused = false;
            this.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.ShowMenu = false;
            this.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit2.Size = new System.Drawing.Size(426, 220);
            this.pictureEdit2.TabIndex = 12;
            // 
            // labelRunBegin
            // 
            this.labelRunBegin.Location = new System.Drawing.Point(13, 238);
            this.labelRunBegin.Name = "labelRunBegin";
            this.labelRunBegin.Size = new System.Drawing.Size(105, 13);
            this.labelRunBegin.TabIndex = 13;
            this.labelRunBegin.Text = "Запуск программы...";
            // 
            // labelCopyrith
            // 
            this.labelCopyrith.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelCopyrith.Location = new System.Drawing.Point(13, 284);
            this.labelCopyrith.Name = "labelCopyrith";
            this.labelCopyrith.Size = new System.Drawing.Size(87, 13);
            this.labelCopyrith.TabIndex = 15;
            this.labelCopyrith.Text = "Copyright © 2018";
            // 
            // marqueeProgress
            // 
            this.marqueeProgress.EditValue = 0;
            this.marqueeProgress.Location = new System.Drawing.Point(12, 261);
            this.marqueeProgress.Name = "marqueeProgress";
            this.marqueeProgress.Size = new System.Drawing.Size(426, 17);
            this.marqueeProgress.TabIndex = 14;
            // 
            // DefSplashScreen
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(450, 310);
            this.Controls.Add(this.labelCopyrith);
            this.Controls.Add(this.marqueeProgress);
            this.Controls.Add(this.labelRunBegin);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.progressBar);
            this.MaximumSize = new System.Drawing.Size(450, 310);
            this.MinimumSize = new System.Drawing.Size(450, 310);
            this.Name = "DefSplashScreen";
            this.Padding = new System.Windows.Forms.Padding(10);
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgress.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}