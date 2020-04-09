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
    public class KeyboardSimulation : IBasePlug
    {
        public ImageSource GetIcon()
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

        public List<PermissionsClass.Permissions> GetPermissions()
        {
            return new List<PermissionsClass.Permissions>() { PermissionsClass.Permissions.InputAndOutput };
        }

        public System.Windows.Controls.UserControl GetView()
        {
            return FileUtils.keyboardSimulationUserControl;
        }

        public List<IControl> GetControl()
        {
            return FileUtils.iControls;
        }
    }
}
