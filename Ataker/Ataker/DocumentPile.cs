using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class DocumentPile : GameObject , Moveable
    {
        private Image sprite;
        public DocumentPile(int x, int y, int z) : base(x, y, z) 
        {
            sprite = Image.FromFile(@".\Assets\TableSprite2.png");
        } 

        public override void Draw(Graphics g, int tileSize)
        {
            g.DrawImage(sprite, X * tileSize, Y * tileSize, tileSize, tileSize);
        }

        public bool Move(int deltaX, int deltaY, int layer, GameObject[,,] grid)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (!CheckCollision(newX, newY, layer, grid))
            {
                grid[X, Y, layer] = null;
                X = newX;
                Y = newY;
                grid[X, Y, layer] = this;
                return true;
            }
            return false;
        }

        public bool CheckCollision(int newX, int newY, int layer, GameObject[,,] grid)
        {
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1)) //Check if it out of grid edge
            {
                return true;
            }

            bool isNotNull = (grid[newX, newY, layer] != null); //Check if that place have object or not
            return isNotNull;
        }
    }
}
