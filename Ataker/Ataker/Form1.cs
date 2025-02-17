using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Ataker
{
    public partial class Form1 : Form
    {
        private const int GridWidth = 14;
        private const int GridHeight = 7;
        private const int Layer = 2;
        private const int TileSize = 110;

        private GameObject[,,] grid = new GameObject[GridWidth, GridHeight, Layer];
        private Player player;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Can't delete this
        }

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true; // Prevent flickering
            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            // Create player
            //player = new Player(1, 3, 1);
            //grid[player.X, player.Y, 1] = player;

            ////Create ProfLittle
            //grid[7, 1, 1] = new ProfLittle(7, 1, 1);


            LoadLevel(1);

            Invalidate(); // Redraw the form
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            int deltaX = 0, deltaY = 0;

            switch (e.KeyCode)
            {
                case Keys.W: deltaY = -1; break;
                case Keys.S: deltaY = 1; break;
                case Keys.A: deltaX = -1; break;
                case Keys.D: deltaX = 1; break;
                case Keys.R: ClearGrid();
                             LoadLevel(1);
                             Invalidate();
                             break;
                default: return;
            }

            if (player.Move(deltaX, deltaY, 1, grid))
            {
                Invalidate(); // Redraw screen after move
            }
        }

        private void ClearGrid()
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
        private void LoadLevel(int levelNum)
        {
            int[,] levelData = {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 0, 0, 3, 0, 1, 5, 0, 0, 0, 0, 1, 1},
            {1, 0, 4, 2, 0, 2, 0, 1, 2, 3, 0, 2, 0, 1},
            {1, 6, 0, 3, 0, 0, 2, 4, 0, 1, 2, 0, 1, 1},
            {1, 0, 1, 2, 2, 0, 0, 2, 0, 3, 0, 1, 1, 1},
            {1, 0, 0, 3, 0, 1, 0, 3, 2, 0, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}};

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
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    for (int l = 0; l < Layer; l++)
                    {
                        Rectangle tile = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

                        if (grid[x, y, l] is Wall)
                            g.FillRectangle(Brushes.DarkGray, tile);
                        else if (grid[x, y, l] is Player)
                            g.FillRectangle(Brushes.Red, tile);
                        else if (grid[x, y, l] is DocumentPile)
                            g.FillRectangle(Brushes.Blue, tile);
                        else if (grid[x, y, l] is Monster)
                            g.FillRectangle(Brushes.LightBlue, tile);
                        else if (grid[x, y, l] is Trap)
                            g.FillRectangle(Brushes.LightPink, tile);
                        else if (grid[x, y, l] is ProfLittle)
                            g.FillRectangle(Brushes.Yellow, tile);
                        else if (l == 0) // layer 0 draw grid
                            g.DrawRectangle(Pens.Black, tile); // Draw empty grid
                    }
                }
            }
        }
    }
}
