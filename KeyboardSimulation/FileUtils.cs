using Operation;
using PlugLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KeyboardSimulation
{
    public class FileUtils
    {
        public static List<KeyboardModel> KeyboardModels
        {
            get
            {
                if (keyboardModels == null)
                {
                    keyboardModels = GetData();
                }
                return keyboardModels;
            }
            set
            {
                keyboardModels = value;
            }
        }

        private static List<KeyboardModel> keyboardModels;

        private static InputModel nowInputModel;

        public static InputModel NowInputModel
        {
            get
            {
                if (nowInputModel == InputModel.None)
                {
                    nowInputModel = GetInputModel();
                }
                return nowInputModel;
            }
            set
            {
                nowInputModel = value;
            }
        }

        public enum InputModel : int //1浅度 2深度(驱动)
        {
            None,
            SendKeys,
            CDD,
        }

        public static void ChangeInputModel(InputModel model)
        {
            if (nowInputModel == model)
            {
                return;
            }

            nowInputModel = model;

            SaveInputModel();
        }

        private static void SaveInputModel()
        {
            XDocument _doc = new XDocument();
            XElement _root = new XElement("Root");
            _doc.Add(_root);
            XElement inputModel = new XElement("InputModel");
            inputModel.Value = ((int)NowInputModel - 1).ToString();
            _root.Add(inputModel);
            _doc.Save(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\config.xml");
        }

        public static InputModel GetInputModel()
        {
            XDocument _doc = XDocument.Load(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\config.xml");
            XElement _root = _doc.Element("Root");
            XElement inputModel = _root.Element("InputModel");
            return (InputModel)(int.Parse(inputModel.Value)+1);
        }

        public static List<KeyboardModel> GetData()
        {
            List<KeyboardModel> keyboardModels = new List<KeyboardModel>();
            if (!System.IO.File.Exists(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\keyboard.xml"))
            {
                NewFile();
            }
            XDocument _doc = XDocument.Load(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\keyboard.xml");
            XElement _root = _doc.Element("Root");

            foreach (XElement keyElement in _root.Elements("Key"))
            {
                KeyboardModel keyboardModel = new KeyboardModel();
                String strPosition = keyElement.Attribute("position").Value;
                if (strPosition.Equals(String.Empty))
                {
                    keyboardModel.Position = -1;
                }
                else
                {
                    if (int.TryParse(strPosition, out int result))
                    {
                        keyboardModel.Position = result;
                    }
                    else
                    {
                        keyboardModel.Position = -1;
                    }
                }
                String strKeyPosition = keyElement.Attribute("keyPosition").Value;
                if (strKeyPosition.Equals(String.Empty))
                {
                    keyboardModel.KeyPosition = -1;
                }
                else
                {
                    if (int.TryParse(strKeyPosition, out int result))
                    {
                        keyboardModel.KeyPosition = result;
                    }
                    else
                    {
                        keyboardModel.KeyPosition = -1;
                    }
                }
                if (keyboardModel.Position != -1)
                    keyboardModels.Add(keyboardModel);
            }
            return keyboardModels;
        }

        private static void NewFile()
        {
            XDocument _doc = new XDocument();
            _doc.Add(new XElement("Root"));
            _doc.Save(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\keyboard.xml");
        }

        public static List<String> chars = new List<string>{
            "{ESC}", "{F1}", "{F2}", "{F3}", "{F4}", "{F5}", "{F6}", "{F7}", "{F8}", "{F9}", "{F10}", "{F11}", "{F12}","{PRTSC}","{SCROLLLOCK}","{BREAK}",
            "{`}", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "{-}", "{=}","{BACKSPACE}","{INSERT}","{HOME}","{PGUP}","{NUMLOCK}","{/}","{*}",
            "{TAB}", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "{[}", "{]}",@"{\}","{DELETE}","{END}","{PGDN}","7","8","9",
            "{CAPSLOCK}", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "{ENTER}","4","5","6",
            "+", "z", "x", "c", "v", "b", "n", "m", ",", ".", "{/}", "+", "{UP}","1","2","3",
            "^", "^{ESC}", "%", " ", "%", "^{ESC}", "", "^", "{LEFT}", "{DOWN}", "{LEFT}", "0", ".",
            "{-}","{+}","{ENTER}",
        };

        public static List<int> ddkey = new List<int>{
            100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 700, 701, 702,
            200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 214, 703, 704, 705, 810, 811, 812,
            300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 213, 706, 707, 708, 807, 808, 809,
            400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 313, 804, 805, 806,
            500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 709, 801, 802, 803,
            600, 601, 602, 603, 604, 605, 606, 607, 710, 711, 712, 800, 816,
            813, 814, 815,
        };

        public static KeyboardSimulationUserControl keyboardSimulationUserControl = new KeyboardSimulationUserControl();
        public static List<IControl> iControls = new List<IControl>() { new KeyboardInputAndOutputControl()};

        public static String version = "V1.0.0";
    }
}
