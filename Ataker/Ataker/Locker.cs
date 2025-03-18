using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public class Locker : GameObject
    {
        private Image spriteSheet;
        private int currentFrame = 0;
        private int frameWidth = 300;  // ปรับให้ตรงกับขนาดของเฟรม
        private int frameHeight = 300;
        private int totalFrames = 5;  // จำนวนเฟรมทั้งหมด
        public Locker(int x, int y, int z) : base(x, y, z) 
        {
            spriteSheet = Image.FromFile(@".\Assets\LockerSpriteSheet.png");
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
