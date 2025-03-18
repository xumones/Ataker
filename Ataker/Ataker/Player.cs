using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Ataker
{
    public class Player : GameObject, Moveable
    {
        public int stamina;
        public bool isDie = false;
        public Action OnLevelUp;

        private bool keyIsPick;

        private bool isAttacking = false;

        private Dictionary<string, Image> animations = new Dictionary<string, Image>(); // เก็บหลายอนิเมชัน
        private string currentAnimation = "Idle"; // อนิเมชันที่ใช้อยู่ตอนนี้

        private int currentFrame = 0;
        private int frameWidth = 640, frameHeight = 640;
        private int totalFrames = 10;

        private bool isMoving = false;
        private float moveProgress = 0.0f;
        private int startX, startY, targetX, targetY;
        private Timer moveTimer;

        private GameObject[,,] gridRef;

        private Timer blinkTimer;
        private int blinkCount = 0;
        private bool isVisible = true;


        private bool isFaceRight = true;

        public Player(int x, int y, int z, int initialStamina) : base(x, y, z)
        {
            animations["Idle"] = Image.FromFile(@".\Assets\PlayerIdleSheet.png");
            animations["Walk"] = Image.FromFile(@".\Assets\PlayerWalkSheet.png");
            animations["Die"] = Image.FromFile(@".\Assets\PlayerDieSpriteSheet.png");
            animations["Attack"] = Image.FromFile(@".\Assets\PlayerAttackSpriteSheet.png");


            stamina = initialStamina;

            startX = x; startY = y; targetX = x; targetY = y;

            moveTimer = new Timer();
            moveTimer.Interval = 1; // ปรับค่าให้เหมาะสม (ค่าที่น้อยกว่าจะทำให้เดินเร็วขึ้น)
            moveTimer.Tick += MoveStep;

            blinkTimer = new Timer { Interval = 100 }; // กระพริบทุก 100ms
            blinkTimer.Tick += BlinkEffect;
        }

        private void BlinkEffect(object sender, EventArgs e)
        {
            isVisible = !isVisible; // สลับสถานะ On/Off
            blinkCount++;

            if (blinkCount >= 4) // กระพริบ 3 ครั้ง
            {
                blinkTimer.Stop();
                isVisible = true; // กลับมาเป็นปกติ
            }
        }

        public void SetAnimation(string animationName, int frameCount)
        {
            if (animations.ContainsKey(animationName) && currentAnimation != animationName)
            {
                currentAnimation = animationName;
                totalFrames = frameCount;
                currentFrame = 0; // รีเซ็ตเฟรมเมื่อเปลี่ยนอนิเมชัน
            }
        }

        private void NextFrame()
        {
            currentFrame = (currentFrame + 1) % totalFrames;

            if (currentAnimation == "Walk" && currentFrame == 0)
            {
                SetAnimation("Idle", 10);
            }
            if (currentAnimation == "Walk" && currentFrame == 0 && stamina == 0)
            {
                SetDieAnimation();
            }
            if (currentAnimation == "Attack" && currentFrame == totalFrames - 1)
            {
                isAttacking = false;
                SetAnimation("Idle", 10);
            }
        }

        public void SetDieAnimation()
        {
            isDie = true;
            frameWidth = 590; frameHeight = 720;
            SetAnimation("Die", 21);
        }

        public bool Move(int deltaX, int deltaY, int layer, GameObject[,,] grid)
        {
            if (isDie)
            {
                stamina = 0;
                return false;
            }

            if (isAttacking) return false;

            if (isMoving) return false;

            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (deltaX == 1)
            {
                isFaceRight = true;
            }
            else if(deltaX == -1)
            {
                isFaceRight = false;
            }

            // Check if something block or not
            if (CheckCollision(newX, newY, layer, grid))
            {
                Console.WriteLine("Collision detected, can't move.");
                return false;
            }

            if (grid[newX, newY, layer] is Monster mon)
            {
                if (isAttacking) return false;
                SetAnimation("Attack", 10);
                isAttacking = true;
                stamina--;
                mon.Move(deltaX, deltaY, layer, grid);
                mon.TakeDamage(layer, grid);
                return true;
            }

            if (grid[newX, newY, 0] is Trap trap && grid[newX, newY, layer] == null)
            {
                if (stamina == 1) stamina += 1;
                Console.WriteLine("Trap hit!");

                // เริ่มกระพริบ
                blinkCount = 0;
                blinkTimer.Start();
            }

            // Check if it DocumentPile or not
            if (grid[newX, newY, layer] is DocumentPile doc)
            {
                bool docMoved = doc.Move(deltaX, deltaY, layer, grid);
                if (!docMoved) return false; // if doc can't move then player can't move too
                SetAnimation("Attack", 10);
                stamina--;

                gridRef = grid;

                // เริ่มการเคลื่อนที่แบบลื่นไหล
                startX = X;
                startY = Y;
                targetX = newX;
                targetY = newY;
                moveProgress = 0.0f;
                isMoving = true;

                moveTimer.Start(); // เริ่มให้ Player เคลื่อนที่ทีละนิด
                return true;
            }

            // Check if it Monster or not

            if (grid[newX, newY, layer] is ProfLittle prof)
            {
                Console.WriteLine("load");
                RequestLevelUp();
                return true;
            }

            if (grid[newX, newY, layer] is Key key)
            {
                stamina--;
                Console.WriteLine("Key Picked!");
                keyIsPick = true;
                key.KeyPick(grid);
            }


            if (grid[newX, newY, layer] is Locker locker)
            {
                if (keyIsPick)
                {
                    grid[locker.X, locker.Y, layer] = null;
                    stamina--;
                }
                return true;
            }

            // Check if it empty space
            if (grid[newX, newY, layer] == null || grid[newX, newY, 0] is Trap)
            {
                SetAnimation("Walk", 10);
                stamina--;

                gridRef = grid;

                // เริ่มการเคลื่อนที่แบบลื่นไหล
                startX = X;
                startY = Y;
                targetX = newX;
                targetY = newY;
                moveProgress = 0.0f;
                isMoving = true;

                moveTimer.Start(); // เริ่มให้ Player เคลื่อนที่ทีละนิด

                return true;
            }

            return false;
        }

        public bool CheckCollision(int newX, int newY, int layer, GameObject[,,] grid)
        {
            // Check if it out of grid edge
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1))
            {
                Console.WriteLine("Out of bounds collision.");
                return true;
            }

            // Check if that place is a wall (NOT EMPTY SPACE , NOT DOCUMENTPILE, NOT MONSTER = wall) (Grid edge already check)
            if (grid[newX, newY, layer] != null && (grid[newX, newY, layer] is Wall))
            {
                Console.WriteLine("Wall detected.");
                return true;
            }

            return false; // Can move
        }

        private void MoveStep(object sender, EventArgs e)
        {
            moveProgress += 0.3f; // ค่าที่มากกว่าจะทำให้เดินเร็วขึ้น

            if (moveProgress >= 1.0f)
            {
                moveProgress = 1.0f;
                moveTimer.Stop();
                isMoving = false;

                gridRef[X, Y, 1] = null;
                X = targetX;
                Y = targetY;
                gridRef[X, Y, 1] = this;

                if (gridRef[X, Y, 0] is Trap trap)
                {
                    stamina--;
                    Console.WriteLine("Trap hit!");
                }
            }
        }

        public override void Draw(Graphics g, int tileSize)
        {
            if (!isVisible) return;

            NextFrame();

            // คำนวณตำแหน่งปัจจุบันระหว่าง Start กับ Target
            float interpolatedX = startX + (targetX - startX) * moveProgress;
            float interpolatedY = startY + (targetY - startY) * moveProgress;

            Image spriteSheet = animations[currentAnimation];
            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            Rectangle destRect = new Rectangle((int)(interpolatedX * tileSize), (int)(interpolatedY * tileSize), tileSize, tileSize);

            if (!isFaceRight) // ถ้าหันซ้าย ให้ Flip Sprite
            {
                g.TranslateTransform(destRect.X + destRect.Width, destRect.Y);
                g.ScaleTransform(-1, 1);
                g.DrawImage(spriteSheet, new Rectangle(0, 0, destRect.Width, destRect.Height), sourceRect, GraphicsUnit.Pixel);
                g.ResetTransform(); // รีเซ็ต Transform เพื่อไม่ให้มีผลกับภาพอื่น
            }
            else // ถ้าหันขวา วาดตามปกติ
            {
                g.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
            }
        }

        public void RequestLevelUp()
        {
            OnLevelUp?.Invoke();
        }
    }
}
