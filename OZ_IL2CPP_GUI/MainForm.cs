using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OZ_IL2CPP_GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InputCustomPwd.Text = JsonConvert.DeserializeObject<ConfigJson>(System.IO.File.ReadAllText("Config.json")).key.ToString();
            SelectApk.Click += SelectApk_Click;
            SelectExe.Click += SelectExe_Click;
            Encrypt.Click += Encrypt_Click;
            Install.Click += Install_Click;
            UnInstall.Click += UnInstall_Click;
            BtnApplyOption.Click += BtnApplyOption_Click;
            LabelSelectedFileShow.Text = "未选择文件";
        }

        private void BtnApplyOption_Click(object sender, EventArgs e)
        {
            ConfigJson cfg = JsonConvert.DeserializeObject<ConfigJson>(System.IO.File.ReadAllText("Config.json"));
            try
            {
                cfg.key = int.Parse(InputCustomPwd.Text);
                System.IO.File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg));
            }
            catch
            {
                Utilitys.ShowError("密码必须是整数数字");
            }
        }

        private void SelectApk_Click(object sender, System.EventArgs e)
        {
            string p = Utilitys.UserSelectApkFile();
            if (!string.IsNullOrEmpty(p))
            {
                Il2cppSecurity.ProcessApk(p);
                LabelSelectedFileShow.Text = "选择的文件:" + p;
                Encrypt.Enabled = true;
            }
        }

        private void SelectExe_Click(object sender, System.EventArgs e)
        {
            string p = Utilitys.UserSelectExeFile();
            if (!string.IsNullOrEmpty(p))
            {
                Il2cppSecurity.ProcessExe(p);
                LabelSelectedFileShow.Text = "选择的文件:" + p;
                Encrypt.Enabled = true;
            }
        }

        private void Encrypt_Click(object sender, System.EventArgs e)
        {
            Il2cppSecurity.Process();
            LabelSelectedFileShow.Text = "未选择文件";
            Encrypt.Enabled = false;
        }

        private void UnInstall_Click(object sender, System.EventArgs e)
        {
            Il2cppInstaller.UnInstall(Utilitys.UserSelectUnityExe());
        }

        private void Install_Click(object sender, System.EventArgs e)
        {
            Il2cppInstaller.Install(Utilitys.UserSelectUnityExe());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void LabelSelectedFileShow_Click(object sender, EventArgs e)
        {

        }
    }
}
