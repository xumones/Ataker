using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class Player : GameObject, Moveable
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
                if (grid[newX, newY] != null && grid[newX, newY] is DocumentPile doc)
                {
                    doc.Move(deltaX, deltaY, grid);
                }

                if (grid[newX, newY] != null && grid[newX, newY] is Monster mon)
                {
                    mon.Move(deltaX, deltaY, grid);
                    mon.TakeDamage(grid);

                    if (mon.health <= 0) // ถ้ามอนสเตอร์ตาย
                    {
                        bool monsterBlocked =
                            (newX + deltaX < 0 || newX + deltaX >= grid.GetLength(0)) ||
                            (newY + deltaY < 0 || newY + deltaY >= grid.GetLength(1)) ||
                            (grid[newX + deltaX, newY + deltaY] != null); // something block behind

                        if (monsterBlocked)
                        {
                            return true; //Player stand still
                        }
                    }
                }

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
