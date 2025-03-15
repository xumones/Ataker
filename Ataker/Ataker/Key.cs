using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class Key : GameObject
    {
        private Image sprite;
        public Key(int x, int y, int z) : base(x, y, z) 
        {
            sprite = Image.FromFile(@".\Assets\KeySprite.png");
        }

        public void KeyPick(GameObject[,,] grid)
        {
            grid[X, Y, 1] = null;
        }

        public override void Draw(Graphics g, int tileSize)
        {
            g.DrawImage(sprite, X * tileSize + 17, Y * tileSize + 17, tileSize-35, tileSize-35);
        }

    }
}
