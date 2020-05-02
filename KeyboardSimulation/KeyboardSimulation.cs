using PlugLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KeyboardSimulation
{
    public class KeyboardSimulation : BasePlug
    {
        public override ImageSource GetIcon()
        {
            // 获取当前程序集
            Assembly assembly = Assembly.GetAssembly(GetType());
            // 获取程序集中资源名称
            string resourceName = assembly.GetName().Name + ".g";
            // 资源管理器
            ResourceManager rm = new ResourceManager(resourceName, assembly);
            BitmapImage bitmap = new BitmapImage();
            using (ResourceSet set = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true))
            {
                UnmanagedMemoryStream s;
                s = (UnmanagedMemoryStream)set.GetObject("Image/keyboard_white.png", true);

                // img在XAML声明的空间      
                return BitmapFrame.Create(s, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        public override List<PermissionsClass.Permissions> GetPermissions()
        {
            return new List<PermissionsClass.Permissions>() { PermissionsClass.Permissions.InputAndOutput };
        }

        public override System.Windows.Controls.UserControl GetView()
        {
            return FileUtils.keyboardSimulationUserControl;
        }

        public override List<IControl> GetControl()
        {
            return FileUtils.iControls;
        }


        public override PlugInfo GetInfo()
        {
            PlugInfo plugInfo = new PlugInfo
            {
                Title = "模拟键盘",
                Author = "hhm",
                Version = "1.1.0",
                Describe = "将Launchpad按键转换为键盘的按键。"+Environment.NewLine+ "DD模式其核心内容调用的是他人编写的程序(与游戏脚本类似)，请谨慎使用。使用后造成的封号等后果,本插件概不负责。",
            };
            return plugInfo;
        }
    }
}
