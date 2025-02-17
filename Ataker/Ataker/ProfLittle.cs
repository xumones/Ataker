using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class ProfLittle : GameObject
    {
        public ProfLittle(int x, int y, int z) : base(x, y, z)
        {

        }
        public override void Draw(Graphics g, int tileSize)
        {
            g.FillRectangle(Brushes.Yellow, X * tileSize, Y * tileSize, tileSize, tileSize); // Prof สีเหลือง
        }
    }
}
