using Operation;
using PlugLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyboardSimulation
{
    public class KeyboardInputAndOutputControl : IInputAndOutputControl
    {
        private CDD dd;

        public void OnInput(int position, InputAndOutputControlEnum.KeyModel keyModel)
        {
            //模拟键盘输入
            KeyboardModel model = FileUtils.KeyboardModels.Find(delegate (KeyboardModel p) { return p.Position == position; });
            if (model == null)
            {
                return;
            }

            if (FileUtils.NowInputModel == FileUtils.InputModel.SendKeys) {
                if (keyModel == InputAndOutputControlEnum.KeyModel.KeyDown)
                {
                    System.Windows.Forms.SendKeys.SendWait(FileUtils.chars[model.KeyPosition]);
                }
            }
            else if (FileUtils.NowInputModel == FileUtils.InputModel.CDD)
            {
                if (dd == null)
                {
                    LoadDllFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DD94687.64.dll");
                }

                int ddcode = FileUtils.ddkey[model.KeyPosition];
                if (keyModel == InputAndOutputControlEnum.KeyModel.KeyDown)
                {
                    dd.key(ddcode, 1);
                }
                else if (keyModel == InputAndOutputControlEnum.KeyModel.KeyUp)
                {
                    dd.key(ddcode, 2);
                }
            }
        }

        private void LoadDllFile(string dllfile)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(dllfile);
            if (!fi.Exists)
            {
                MessageBox.Show("文件不存在");
                return;
            }
            dd = new CDD();

            int ret = dd.Load(dllfile);

            if (ret == -2) { MessageBox.Show("装载库时发生错误"); return; }
            if (ret == -1) { MessageBox.Show("取函数地址时发生错误"); return; }
            //if (ret == 0) { MessageBox.Show("非增强模块"); }
        }

        InputAndOutputControlEnum.SendLight sendLight;
        public void OutputLight(InputAndOutputControlEnum.SendLight sendLight)
        {
            this.sendLight = sendLight;
        }
    }
}
