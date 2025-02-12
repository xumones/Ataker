using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class DocumentPile : GameObject , Moveable
    {
        public DocumentPile(int x, int y) : base(x, y) { }

        public override void Draw(Graphics g, int tileSize)
        {
            g.FillRectangle(Brushes.Blue, X * tileSize, Y * tileSize, tileSize, tileSize); // Doc สีฟ้า
        }

        public bool Move(int deltaX, int deltaY, GameObject[,] grid)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
            {
                if (grid[newX, newY] == null)
                {
                    grid[X, Y] = null;
                    X = newX;
                    Y = newY;
                    grid[X, Y] = this;
                    return true;
                }
            }
            return false;
        }
    }
}
