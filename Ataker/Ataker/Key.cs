using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class Key : GameObject
    {
        public Key(int x, int y, int z) : base(x, y, z) { }

        public void KeyPick(GameObject[,,] grid)
        {
            grid[X, Y, 1] = null;
        }

        public override void Draw(Graphics g, int tileSize)
        {
            g.FillRectangle(Brushes.LightGreen, X * tileSize, Y * tileSize, tileSize, tileSize); // Trap เขียวอ่อน
        }

    }
}
