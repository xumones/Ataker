using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ataker
{
    public class LevelManager
    {
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int TileSize { get; set; }
        public Player player { get; set; }


        public void LoadSize(int levelNumber)
        {
            if (levelNumber == 1)
            {
                GridWidth = 10;
                GridHeight = 5;
                TileSize = 154;
            }
            else if (levelNumber == 2)
            {
                GridWidth = 14;
                GridHeight = 7;
                TileSize = 110;
            }
        }

        public void LoadLevel(int levelNumber,GameObject[,,] grid)
        {
            int[,] levelData = GetLevelData(levelNumber);
            SetLevelData(grid, levelData);
        }

        private int[,] GetLevelData(int levelNumber)
        {
            if (levelNumber == 1)
            {
                return new int[,] {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            {1, 6, 0, 0, 0, 0, 0, 0, 5, 1 },
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }};
            }
            else if (levelNumber == 2)
            {
                return new int[,] {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 0, 0, 3, 0, 1, 5, 0, 0, 0, 0, 1, 1},
            {1, 0, 4, 2, 0, 2, 0, 1, 2, 3, 0, 2, 0, 1},
            {1, 6, 7, 3, 0, 0, 2, 4, 0, 1, 2, 0, 1, 1},
            {1, 8, 1, 2, 2, 0, 0, 2, 0, 3, 0, 1, 1, 1},
            {1, 0, 0, 3, 0, 1, 0, 3, 2, 0, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}};
            }

            return new int[0, 0]; // Incase stage not found
        }

        private void SetLevelData(GameObject[,,] grid, int[,] levelData)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    switch (levelData[y, x])
                    {
                        case 1: grid[x, y, 1] = new Wall(x, y, 1); break;
                        case 2: grid[x, y, 1] = new DocumentPile(x, y, 1); break;
                        case 3: grid[x, y, 0] = new Trap(x, y, 0); break;
                        case 4: grid[x, y, 1] = new Monster(x, y, 1, 3); break;
                        case 5: grid[x, y, 1] = new ProfLittle(x, y, 1); break;
                        case 6: grid[x, y, 1] = player = new Player(x, y, 1); break;
                        case 7: grid[x, y, 1] = new Locker(x, y, 1); break;
                        case 8: grid[x, y, 1] = new Key(x, y, 1); break;
                    }
                }
            }
        }
        public void ClearGrid(GameObject[,,] grid)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        grid[x, y, l] = null;
                    }
                }
            }
        }
    }
}
