using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardSimulation
{
    public class KeyboardModel
    {
        public int Position
        {
            get;
            set;
        }
        public int KeyPosition
        {
            get;
            set;
        }

        public KeyboardModel()
        {
        }

        public KeyboardModel(int position, int keyPosition)
        {
            Position = position;
            KeyPosition = keyPosition;
        }
    }
}
