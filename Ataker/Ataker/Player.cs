using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class Player : GameObject
    {
        public Player(int x, int y) : base(x, y)
        {

        }

        public bool Move(int deltaX, int deltaY, GameObject[,] grid)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
            {
                if (grid[newX, newY] == null) // ตรวจสอบว่าตำแหน่งใหม่ไม่ใช่กำแพง
                {
                    grid[X, Y] = null; // ลบตำแหน่งเดิมออกจาก grid
                    X = newX;
                    Y = newY;
                    grid[X, Y] = this;
                    return true; // ย้ายได้
                }
            }
            return false; // ย้ายไม่ได้
        }

        public override void Draw(Graphics g, int tileSize)
        {
            g.FillRectangle(Brushes.Red, X * tileSize, Y * tileSize, tileSize, tileSize); // Draw player as a red rectangle
        }
    }
}
