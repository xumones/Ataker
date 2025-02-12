using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class Monster : GameObject , Moveable
    {
        public int health { get; protected set; }
        public Monster(int x, int y, int health) : base(x, y) 
        {
            this.health = health;
        }

        public override void Draw(Graphics g, int tileSize)
        {
            g.FillRectangle(Brushes.LightBlue, X * tileSize, Y * tileSize, tileSize, tileSize); // Doc สีฟ้าอ่อน
        }

        public bool Move(int deltaX, int deltaY, GameObject[,] grid)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (!CheckCollision(newX, newY, grid))
            {
                grid[X, Y] = null;
                X = newX;
                Y = newY;
                grid[X, Y] = this;
                return true;
            }
            return false;
        }

        public void TakeDamage(GameObject[,] grid)
        {
            health--;

            if (health <= 0)
            {
                grid[X,Y] = null;
            }

            Console.WriteLine(health.ToString()); //for debug
        }

        public bool CheckCollision(int newX, int newY, GameObject[,] grid)
        {
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1)) //Check if it out of grid edge
            {
                return true;
            }

            bool isNotNull = (grid[newX, newY] != null); //Check if that place have object or not
            return isNotNull;
        }
    }
}
