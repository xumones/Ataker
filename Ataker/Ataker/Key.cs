using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Ataker
{
    public class Key : GameObject
    {
        private Image spriteSheet;
        private int currentFrame = 0;
        private int frameWidth = 360;  // ปรับให้ตรงกับขนาดของเฟรม
        private int frameHeight = 380;
        private int totalFrames = 5;  // จำนวนเฟรมทั้งหมด
        public Key(int x, int y, int z) : base(x, y, z) 
        {
            spriteSheet = Image.FromFile(@".\Assets\KeySpriteSheet.png");
        }

        public void KeyPick(GameObject[,,] grid)
        {
            grid[X, Y, 1] = null;
        }

        private void NextFrame()
        {
            currentFrame = (currentFrame + 1) % totalFrames;
        }

        public override void Draw(Graphics g, int tileSize)
        {
            NextFrame(); // 🔥 เปลี่ยนเฟรมทุกครั้งที่ถูกวาด (ใช้ Timer ใน Form1 แทน)

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            Rectangle destRect = new Rectangle(X * tileSize, Y * tileSize, tileSize, tileSize);

            g.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
        }

    }
}
