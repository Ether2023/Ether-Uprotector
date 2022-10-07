

namespace OZ_IL2CPP_GUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Encrypt = new System.Windows.Forms.Button();
            this.Author = new System.Windows.Forms.Label();
            this.SelectExe = new System.Windows.Forms.Button();
            this.SelectApk = new System.Windows.Forms.Button();
            this.SelectIpa = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.InstallTip = new System.Windows.Forms.Label();
            this.LabelSelectFile = new System.Windows.Forms.Label();
            this.Install = new System.Windows.Forms.Button();
            this.UnInstall = new System.Windows.Forms.Button();
            this.LabelSelectedFileShow = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Encrypt
            // 
            this.Encrypt.Location = new System.Drawing.Point(694, 409);
            this.Encrypt.Name = "Encrypt";
            this.Encrypt.Size = new System.Drawing.Size(94, 29);
            this.Encrypt.TabIndex = 0;
            this.Encrypt.Text = "加密";
            this.Encrypt.UseVisualStyleBackColor = true;
            this.Encrypt.Click += Encrypt_Click;
            this.Encrypt.Enabled = false;
            // 
            // Author
            // 
            this.Author.AutoSize = true;
            this.Author.Location = new System.Drawing.Point(12, 421);
            this.Author.Name = "Author";
            this.Author.Size = new System.Drawing.Size(194, 20);
            this.Author.TabIndex = 1;
            this.Author.Text = "By OrangeSummer,Z1029";
            // 
            // SelectExe
            // 
            this.SelectExe.Location = new System.Drawing.Point(12, 123);
            this.SelectExe.Name = "SelectExe";
            this.SelectExe.Size = new System.Drawing.Size(117, 31);
            this.SelectExe.TabIndex = 2;
            this.SelectExe.Text = "选择exe文件";
            this.SelectExe.UseVisualStyleBackColor = true;
            this.SelectExe.Click += SelectExe_Click;
            // 
            // SelectApk
            // 
            this.SelectApk.Location = new System.Drawing.Point(12, 160);
            this.SelectApk.Name = "SelectApk";
            this.SelectApk.Size = new System.Drawing.Size(117, 31);
            this.SelectApk.TabIndex = 3;
            this.SelectApk.Text = "选择apk文件";
            this.SelectApk.UseVisualStyleBackColor = true;
            this.SelectApk.Click += SelectApk_Click;
            // 
            // SelectIpa
            // 
            this.SelectIpa.Location = new System.Drawing.Point(12, 197);
            this.SelectIpa.Name = "SelectIpa";
            this.SelectIpa.Size = new System.Drawing.Size(117, 31);
            this.SelectIpa.TabIndex = 4;
            this.SelectIpa.Text = "ipa(暂不支持)";
            this.SelectIpa.UseVisualStyleBackColor = true;
            this.SelectIpa.Enabled = false;
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(551, 409);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(125, 29);
            this.Progress.TabIndex = 5;
            // 
            // InstallTip
            // 
            this.InstallTip.AutoSize = true;
            this.InstallTip.Location = new System.Drawing.Point(13, 13);
            this.InstallTip.Name = "InstallTip";
            this.InstallTip.Size = new System.Drawing.Size(244, 20);
            this.InstallTip.TabIndex = 6;
            this.InstallTip.Text = "若您还没有安装OZIl2cpp,请先安装";
            // 
            // LabelSelectFile
            // 
            this.LabelSelectFile.AutoSize = true;
            this.LabelSelectFile.Location = new System.Drawing.Point(12, 100);
            this.LabelSelectFile.Name = "LabelSelectFile";
            this.LabelSelectFile.Size = new System.Drawing.Size(69, 20);
            this.LabelSelectFile.TabIndex = 7;
            this.LabelSelectFile.Text = "选择文件";
            // 
            // Install
            // 
            this.Install.Location = new System.Drawing.Point(13, 37);
            this.Install.Name = "Install";
            this.Install.Size = new System.Drawing.Size(94, 29);
            this.Install.TabIndex = 8;
            this.Install.Text = "安装";
            this.Install.UseVisualStyleBackColor = true;
            this.Install.Click += Install_Click;
            // 
            // UnInstall
            // 
            this.UnInstall.Location = new System.Drawing.Point(113, 37);
            this.UnInstall.Name = "UnInstall";
            this.UnInstall.Size = new System.Drawing.Size(94, 29);
            this.UnInstall.TabIndex = 9;
            this.UnInstall.Text = "卸载";
            this.UnInstall.UseVisualStyleBackColor = true;
            this.UnInstall.Click += UnInstall_Click;
            // 
            // LabelSelectedFileShow
            // 
            this.LabelSelectedFileShow.AutoSize = true;
            this.LabelSelectedFileShow.Location = new System.Drawing.Point(551, 386);
            this.LabelSelectedFileShow.Name = "LabelSelectedFileShow";
            this.LabelSelectedFileShow.Size = new System.Drawing.Size(88, 20);
            this.LabelSelectedFileShow.TabIndex = 10;
            this.LabelSelectedFileShow.Text = "选择的文件:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LabelSelectedFileShow);
            this.Controls.Add(this.UnInstall);
            this.Controls.Add(this.Install);
            this.Controls.Add(this.LabelSelectFile);
            this.Controls.Add(this.InstallTip);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.SelectIpa);
            this.Controls.Add(this.SelectApk);
            this.Controls.Add(this.SelectExe);
            this.Controls.Add(this.Author);
            this.Controls.Add(this.Encrypt);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "O&Z IL2CPP GUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void SelectApk_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void SelectExe_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Encrypt_Click(object sender, System.EventArgs e)
        {
            Il2cppSecurity.Process();
        }

        private void UnInstall_Click(object sender, System.EventArgs e)
        {
            Il2cppInstaller.UnInstall(Utilitys.UserSelectUnityExe());
        }

        private void Install_Click(object sender, System.EventArgs e)
        {
            Il2cppInstaller.Install(Utilitys.UserSelectUnityExe());
        }

        #endregion

        private System.Windows.Forms.Button Encrypt;
        private System.Windows.Forms.Label Author;
        private System.Windows.Forms.Button SelectExe;
        private System.Windows.Forms.Button SelectApk;
        private System.Windows.Forms.Button SelectIpa;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Label InstallTip;
        private System.Windows.Forms.Label LabelSelectFile;
        private System.Windows.Forms.Button Install;
        private System.Windows.Forms.Button UnInstall;
        private System.Windows.Forms.Label LabelSelectedFileShow;
    }
}

