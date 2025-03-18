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
        private Image sprite;
        public ProfLittle(int x, int y, int z) : base(x, y, z)
        {
            sprite = Image.FromFile(@".\Assets\BossSprite.png");
        }
        public override void Draw(Graphics g, int tileSize)
        {
            g.DrawImage(sprite, X * tileSize, Y * tileSize, tileSize, tileSize);
        }
    }
}
