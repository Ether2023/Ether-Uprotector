

namespace Ether_IL2CPP_GUI
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
            this.InstallTip = new System.Windows.Forms.Label();
            this.LabelSelectFile = new System.Windows.Forms.Label();
            this.Install = new System.Windows.Forms.Button();
            this.UnInstall = new System.Windows.Forms.Button();
            this.LabelSelectedFileShow = new System.Windows.Forms.Label();
            this.InputCustomPwd = new System.Windows.Forms.TextBox();
            this.TextCustomPwd = new System.Windows.Forms.Label();
            this.TextCustomPwdDesc = new System.Windows.Forms.Label();
            this.EncOption_Test = new System.Windows.Forms.CheckBox();
            this.TextEncOption = new System.Windows.Forms.Label();
            this.EncOption_AntiDump = new System.Windows.Forms.CheckBox();
            this.EncOption_ProtectAsset = new System.Windows.Forms.CheckBox();
            this.BtnApplyOption = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Encrypt
            // 
            this.Encrypt.Enabled = false;
            this.Encrypt.Location = new System.Drawing.Point(694, 409);
            this.Encrypt.Name = "Encrypt";
            this.Encrypt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Encrypt.Size = new System.Drawing.Size(94, 29);
            this.Encrypt.TabIndex = 0;
            this.Encrypt.Text = "加密";
            this.Encrypt.UseVisualStyleBackColor = true;
            // 
            // Author
            // 
            this.Author.AutoSize = true;
            this.Author.Location = new System.Drawing.Point(13, 413);
            this.Author.Name = "Author";
            this.Author.Size = new System.Drawing.Size(194, 20);
            this.Author.TabIndex = 1;
            this.Author.Text = "By OrangeSummer,Z1029";
            // 
            // SelectExe
            // 
            this.SelectExe.Location = new System.Drawing.Point(12, 149);
            this.SelectExe.Name = "SelectExe";
            this.SelectExe.Size = new System.Drawing.Size(117, 31);
            this.SelectExe.TabIndex = 2;
            this.SelectExe.Text = "选择exe文件";
            this.SelectExe.UseVisualStyleBackColor = true;
            // 
            // SelectApk
            // 
            this.SelectApk.Location = new System.Drawing.Point(12, 186);
            this.SelectApk.Name = "SelectApk";
            this.SelectApk.Size = new System.Drawing.Size(117, 31);
            this.SelectApk.TabIndex = 3;
            this.SelectApk.Text = "选择apk文件";
            this.SelectApk.UseVisualStyleBackColor = true;
            // 
            // InstallTip
            // 
            this.InstallTip.AutoSize = true;
            this.InstallTip.Location = new System.Drawing.Point(13, 13);
            this.InstallTip.Name = "InstallTip";
            this.InstallTip.Size = new System.Drawing.Size(423, 60);
            this.InstallTip.TabIndex = 6;
            this.InstallTip.Text = "若您还没有安装OZIl2cpp,请先安装,在打包IOS之前需要先卸载\r\n如果Unity路径位于C盘,需要以管理员身份运行本程序\r\n\r\n";
            // 
            // LabelSelectFile
            // 
            this.LabelSelectFile.AutoSize = true;
            this.LabelSelectFile.Location = new System.Drawing.Point(12, 126);
            this.LabelSelectFile.Name = "LabelSelectFile";
            this.LabelSelectFile.Size = new System.Drawing.Size(69, 20);
            this.LabelSelectFile.TabIndex = 7;
            this.LabelSelectFile.Text = "选择文件";
            // 
            // Install
            // 
            this.Install.Location = new System.Drawing.Point(12, 56);
            this.Install.Name = "Install";
            this.Install.Size = new System.Drawing.Size(94, 29);
            this.Install.TabIndex = 8;
            this.Install.Text = "安装";
            this.Install.UseVisualStyleBackColor = true;
            // 
            // UnInstall
            // 
            this.UnInstall.Location = new System.Drawing.Point(112, 56);
            this.UnInstall.Name = "UnInstall";
            this.UnInstall.Size = new System.Drawing.Size(94, 29);
            this.UnInstall.TabIndex = 9;
            this.UnInstall.Text = "卸载";
            this.UnInstall.UseVisualStyleBackColor = true;
            // 
            // LabelSelectedFileShow
            // 
            this.LabelSelectedFileShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelSelectedFileShow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LabelSelectedFileShow.Location = new System.Drawing.Point(212, 413);
            this.LabelSelectedFileShow.Name = "LabelSelectedFileShow";
            this.LabelSelectedFileShow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LabelSelectedFileShow.Size = new System.Drawing.Size(476, 20);
            this.LabelSelectedFileShow.TabIndex = 10;
            this.LabelSelectedFileShow.Text = "选择的文件:D://114514/1919810/Xianbei.apk\r\n";
            this.LabelSelectedFileShow.Click += new System.EventHandler(this.LabelSelectedFileShow_Click);
            // 
            // InputCustomPwd
            // 
            this.InputCustomPwd.Location = new System.Drawing.Point(336, 236);
            this.InputCustomPwd.Name = "InputCustomPwd";
            this.InputCustomPwd.Size = new System.Drawing.Size(125, 27);
            this.InputCustomPwd.TabIndex = 11;
            this.InputCustomPwd.Text = "HelloOZIl2cpp";
            // 
            // TextCustomPwd
            // 
            this.TextCustomPwd.AutoSize = true;
            this.TextCustomPwd.Location = new System.Drawing.Point(212, 240);
            this.TextCustomPwd.Name = "TextCustomPwd";
            this.TextCustomPwd.Size = new System.Drawing.Size(118, 20);
            this.TextCustomPwd.TabIndex = 12;
            this.TextCustomPwd.Text = "自定义加密密码:\r\n";
            // 
            // TextCustomPwdDesc
            // 
            this.TextCustomPwdDesc.AutoSize = true;
            this.TextCustomPwdDesc.Location = new System.Drawing.Point(467, 240);
            this.TextCustomPwdDesc.Name = "TextCustomPwdDesc";
            this.TextCustomPwdDesc.Size = new System.Drawing.Size(199, 20);
            this.TextCustomPwdDesc.TabIndex = 13;
            this.TextCustomPwdDesc.Text = "(请不要将密码泄露给其他人)";
            // 
            // EncOption_Test
            // 
            this.EncOption_Test.AutoCheck = false;
            this.EncOption_Test.AutoSize = true;
            this.EncOption_Test.Checked = true;
            this.EncOption_Test.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EncOption_Test.Location = new System.Drawing.Point(212, 149);
            this.EncOption_Test.Name = "EncOption_Test";
            this.EncOption_Test.Size = new System.Drawing.Size(249, 24);
            this.EncOption_Test.TabIndex = 14;
            this.EncOption_Test.Text = "Metadata全文加密,隐藏Header";
            this.EncOption_Test.UseVisualStyleBackColor = true;
            // 
            // TextEncOption
            // 
            this.TextEncOption.AutoSize = true;
            this.TextEncOption.Location = new System.Drawing.Point(212, 126);
            this.TextEncOption.Name = "TextEncOption";
            this.TextEncOption.Size = new System.Drawing.Size(188, 20);
            this.TextEncOption.TabIndex = 15;
            this.TextEncOption.Text = "加密选项(研发中,敬请期待)";
            // 
            // EncOption_AntiDump
            // 
            this.EncOption_AntiDump.AutoSize = true;
            this.EncOption_AntiDump.Checked = true;
            this.EncOption_AntiDump.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EncOption_AntiDump.Location = new System.Drawing.Point(212, 179);
            this.EncOption_AntiDump.Name = "EncOption_AntiDump";
            this.EncOption_AntiDump.Size = new System.Drawing.Size(194, 24);
            this.EncOption_AntiDump.TabIndex = 16;
            this.EncOption_AntiDump.Text = "保护代码防止dump内存";
            this.EncOption_AntiDump.UseVisualStyleBackColor = true;
            // 
            // EncOption_ProtectAsset
            // 
            this.EncOption_ProtectAsset.AutoSize = true;
            this.EncOption_ProtectAsset.Enabled = false;
            this.EncOption_ProtectAsset.Location = new System.Drawing.Point(212, 209);
            this.EncOption_ProtectAsset.Name = "EncOption_ProtectAsset";
            this.EncOption_ProtectAsset.Size = new System.Drawing.Size(285, 24);
            this.EncOption_ProtectAsset.TabIndex = 17;
            this.EncOption_ProtectAsset.Text = "游戏数据保护(测试性功能,可影响性能)";
            this.EncOption_ProtectAsset.UseVisualStyleBackColor = true;
            // 
            // BtnApplyOption
            // 
            this.BtnApplyOption.Location = new System.Drawing.Point(212, 270);
            this.BtnApplyOption.Name = "BtnApplyOption";
            this.BtnApplyOption.Size = new System.Drawing.Size(121, 30);
            this.BtnApplyOption.TabIndex = 18;
            this.BtnApplyOption.Text = "应用该设置";
            this.BtnApplyOption.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(800, 450);
            /*this.Controls.Add(this.BtnApplyOption);
            this.Controls.Add(this.EncOption_ProtectAsset);
            this.Controls.Add(this.EncOption_AntiDump);*/
            this.Controls.Add(this.TextEncOption);
            /*this.Controls.Add(this.EncOption_Test);
            this.Controls.Add(this.TextCustomPwdDesc);
            this.Controls.Add(this.TextCustomPwd);
            this.Controls.Add(this.InputCustomPwd);*/
            this.Controls.Add(this.LabelSelectedFileShow);
            this.Controls.Add(this.UnInstall);
            this.Controls.Add(this.Install);
            this.Controls.Add(this.LabelSelectFile);
            this.Controls.Add(this.InstallTip);
            this.Controls.Add(this.SelectApk);
            this.Controls.Add(this.SelectExe);
            this.Controls.Add(this.Author);
            this.Controls.Add(this.Encrypt);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "O&Z IL2CPP GUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Encrypt;
        private System.Windows.Forms.Label Author;
        private System.Windows.Forms.Button SelectExe;
        private System.Windows.Forms.Button SelectApk;
        private System.Windows.Forms.Label InstallTip;
        private System.Windows.Forms.Label LabelSelectFile;
        private System.Windows.Forms.Button Install;
        private System.Windows.Forms.Button UnInstall;
        private System.Windows.Forms.Label LabelSelectedFileShow;
        private System.Windows.Forms.TextBox InputCustomPwd;
        private System.Windows.Forms.Label TextCustomPwd;
        private System.Windows.Forms.Label TextCustomPwdDesc;
        private System.Windows.Forms.CheckBox EncOption_Test;
        private System.Windows.Forms.Label TextEncOption;
        private System.Windows.Forms.CheckBox EncOption_AntiDump;
        private System.Windows.Forms.CheckBox EncOption_ProtectAsset;
        private System.Windows.Forms.Button BtnApplyOption;
    }
}

