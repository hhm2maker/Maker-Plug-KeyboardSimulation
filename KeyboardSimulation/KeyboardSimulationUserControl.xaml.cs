using MakerUI.Device;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace KeyboardSimulation
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardSimulationUserControl : UserControl
    {
        public KeyboardSimulationUserControl()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }

        bool isFirst = true;

        LaunchpadPro launchpadPro;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            double x = SystemParameters.WorkArea.Width;//得到屏幕工作区域宽度
            double y = SystemParameters.WorkArea.Height;//得到屏幕工作区域高度
            vbMain.Width = x / 2;
            vbMain.Height = vbMain.Width / 1338 * 860;

            if (isFirst)
            {
                launchpadPro = new LaunchpadPro();
                launchpadPro.Size = 500;
                launchpadPro.SetLaunchpadBackground(new SolidColorBrush(Colors.Transparent));
                launchpadPro.SetButtonBackground(new SolidColorBrush(Color.FromRgb(19, 40, 61)));
                launchpadPro.SetButtonBorderBackground(new SolidColorBrush(Color.FromRgb(73, 191, 231)));
                launchpadPro.SetButtonClickEvent(OnLaunchpadClick);
                spTop.Children.Insert(0, launchpadPro);

                InitKeyboard();
                LoadKeyboards();

                SelectPosition(11);

                isFirst = false;
            }
        }

        private void InitKeyboard()
        {
            AddButton(spKeyboard);

            foreach (var item in btns) {
                item.Click += Item_Click;
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            int position = int.Parse(tbPosition.Text);
            int keyPosition = btns.IndexOf(sender as Button);
            if (position > -1 && keyPosition!=-1)
            {
                bool isContain = false;
                foreach (var item in FileUtils.KeyboardModels) {
                    if (item.Position == position) {
                        item.KeyPosition = keyPosition;
                        isContain = true;
                        break;
                    }
                }
                if (!isContain) {
                    FileUtils.KeyboardModels.Add(new KeyboardModel(position,keyPosition));
                }
                RefreshView();
            }
        }

        private void RefreshView()
        {
            lbMain.Items.Clear();
            foreach (var item in FileUtils.KeyboardModels)
            {
                lbMain.Items.Add(item.Position + "       " + item.KeyPosition);
            }
        }

        private void AddButton(StackPanel sp) {
            foreach (var item in sp.Children) {
                if (item is StackPanel) {
                    AddButton(item as StackPanel);
                }
                if (item is Button) {
                    btns.Add(item as Button);
                }
            }
        }

        private void LoadKeyboards()
        {
            RefreshView();
        }

        private void lbMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbPosition.Text = lbMain.SelectedItem.ToString().Split(' ')[0];
        }

        private List<Button> btns = new List<Button>();

        private void OnLaunchpadClick(object sender, MouseButtonEventArgs e) {
            SelectPosition(launchpadPro.GetNumber((Shape)sender));
        }

        private void SelectPosition(int iPosition)
        {
            int position = int.Parse(tbPosition.Text);
            if (position == iPosition)
            {
                return;
            }
            if (position > -1)
            {
                launchpadPro.SetButtonBackground(position, new SolidColorBrush(Color.FromRgb(19, 40, 61)));
            }
            position = iPosition;
            launchpadPro.SetButtonBackground(position, new SolidColorBrush(Colors.Red));
            tbPosition.Text = position.ToString();
        }

        private void NewFile() {
            XDocument _doc = new XDocument();
            _doc.Add(new XElement("Root"));
            _doc.Save(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\keyboard.xml");
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            int position = int.Parse(tbPosition.Text);
            if (position > -1)
            {
                FileUtils.KeyboardModels.RemoveAll(x => x.Position == position);
                RefreshView();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\keyboard.xml"))
            {
                NewFile();
            }
            XDocument _doc = new XDocument();
            XElement _root = new XElement("Root");
            _doc.Add(_root);
            foreach (var item in FileUtils.KeyboardModels) {
                XElement keyElement = new XElement("Key");
                XAttribute xAttribute = new XAttribute("position", item.Position);
                keyElement.Add(xAttribute);
                XAttribute xAttributeKey = new XAttribute("keyPosition", item.KeyPosition);
                keyElement.Add(xAttributeKey);
                _root.Add(keyElement);
            }
            _doc.Save(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\keyboard.xml");
        }

       
        private void btnNormalModel_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnSendModel) {
                FileUtils.ChangeInputModel(FileUtils.InputModel.SendKeys);
            }
            else if (sender == btnDDkeyModel)
            {
                FileUtils.ChangeInputModel(FileUtils.InputModel.CDD);
            }
        }
    }
}
