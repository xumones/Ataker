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
        public Action OnLevelUp;
        private bool keyIsPick;

        //private static Image spriteSheet = Image.FromFile("./Assets/Playertest.png"); // โหลด Sprite Sheet
        //private int currentFrame = 0;
        //private int frameWidth = 32;  // กว้างของ 1 เฟรม
        //private int frameHeight = 32; // สูงของ 1 เฟรม
        //private int direction = 0; // 0 = ลง, 1 = ซ้าย, 2 = ขวา, 3 = ขึ้น
        //private Timer animationTimer;

        public Player(int x, int y, int z) : base(x, y, z)
        {
            //animationTimer = new Timer();
            //animationTimer.Interval = 100; // 100ms ต่อเฟรม
            //animationTimer.Tick += (s, e) => NextFrame();
        }

        public bool Move(int deltaX, int deltaY, int layer, GameObject[,,] grid)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            // Check if something block or not
            if (CheckCollision(newX, newY, layer, grid))
            {
                Console.WriteLine("Collision detected, can't move.");
                return false;
            }

            //Direction Control
            //if (deltaX == -1) direction = 1;  // ซ้าย
            //if (deltaX == 1) direction = 2;   // ขวา
            //if (deltaY == -1) direction = 3;  // ขึ้น
            //if (deltaY == 1) direction = 0;   // ลง

            // Check if it DocumentPile or not
            if (grid[newX, newY, layer] is DocumentPile doc)
            {
                bool docMoved = doc.Move(deltaX, deltaY, layer, grid);
                if (!docMoved) return false; // if doc can't move then player can't move too
            }

            // Check if it Monster or not
            if (grid[newX, newY, layer] is Monster mon)
            {
                mon.Move(deltaX, deltaY, layer, grid);
                mon.TakeDamage(layer,grid);
                return true;
            }

            if (grid[newX, newY, layer] is ProfLittle prof)
            {
                Console.WriteLine("load");
                RequestLevelUp();
                return true;
            }

            if (grid[newX, newY, layer] is Key key)
            {
                Console.WriteLine("Key Picked!");
                keyIsPick = true; 
                key.KeyPick(grid);
            }

            if(grid[newX, newY, layer] is Locker locker)
            {
                if(keyIsPick)
                {
                    grid[locker.X, locker.Y, layer] = null;
                }
                return true;
            }

            // Check if it empty space
            if (grid[newX, newY, layer] == null)
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

        public override void Draw(Graphics g, int tileSize)
        {
            g.FillRectangle(Brushes.Red, X * tileSize, Y * tileSize, tileSize, tileSize); // Draw player as a red rectangle
        }

        public void RequestLevelUp()
        {
            OnLevelUp?.Invoke();
        }
    }
}
