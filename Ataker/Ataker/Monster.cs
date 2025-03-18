using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Ataker
{
    public class Monster : GameObject , Moveable
    {
        private Image spriteSheet;
        private int currentFrame = 0;
        private int frameWidth = 640;  // ปรับให้ตรงกับขนาดของเฟรม
        private int frameHeight = 640;
        private int totalFrames = 5;  // จำนวนเฟรมทั้งหมด
        public int health { get; protected set; }

        private Timer blinkTimer;
        private int blinkCount = 0;
        private bool isVisible = true;
        public Monster(int x, int y, int z,int health) : base(x, y, z) 
        {
            this.health = health;
            spriteSheet = Image.FromFile(@".\Assets\MonsterSpriteSheet.png");
            blinkTimer = new Timer { Interval = 100 }; // กระพริบทุก 100ms
            blinkTimer.Tick += BlinkEffect;
        }

        private void BlinkEffect(object sender, EventArgs e)
        {
            isVisible = !isVisible; // สลับสถานะ On/Off
            blinkCount++;

            if (blinkCount >= 2) // กระพริบ 3 ครั้ง
            {
                blinkTimer.Stop();
                isVisible = true; // กลับมาเป็นปกติ
            }
        }
        private void NextFrame()
        {
            currentFrame = (currentFrame + 1) % totalFrames;
        }

        public override void Draw(Graphics g, int tileSize)
        {
            if (!isVisible) return;
            NextFrame(); // 🔥 เปลี่ยนเฟรมทุกครั้งที่ถูกวาด (ใช้ Timer ใน Form1 แทน)

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            Rectangle destRect = new Rectangle(X * tileSize, Y * tileSize, tileSize, tileSize);

            g.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
        }

        public bool Move(int deltaX, int deltaY, int layer,GameObject[,,] grid)
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

        public void TakeDamage(int layer,GameObject[,,] grid)
        {
            blinkCount = 0;
            blinkTimer.Start();
            health--;

            if (health <= 0)
            {
                grid[X, Y, layer] = null;
            }

            Console.WriteLine(health.ToString()); //for debug
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
