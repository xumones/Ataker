using Ataker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Wall : GameObject
{
    public Wall(int x, int y) : base(x, y) { }

    public override void Draw(Graphics g, int tileSize)
    {
        g.FillRectangle(Brushes.DarkGray, X * tileSize, Y * tileSize, tileSize, tileSize); // Wall สีเทา
    }
}
