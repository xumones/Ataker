using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public abstract class GameObject
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public GameObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        public abstract void Draw(Graphics g, int tileSize);
    }
}
