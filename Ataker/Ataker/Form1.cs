using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Ataker
{
    public partial class Form1 : Form
    {
        private const int Layer = 2;
        private LevelManager levelManager = new LevelManager();
        private int currentLevel = 1;

        private GameObject[,,] grid;
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

            LoadLevel();
        }

        private void LoadLevel()
        {
            levelManager.LoadSize(currentLevel);
            Console.WriteLine(currentLevel.ToString());

            grid = new GameObject[levelManager.GridWidth, levelManager.GridHeight, Layer];

            levelManager.ClearGrid(grid);
            levelManager.LoadLevel(currentLevel, grid);

            player = levelManager.player;
            player.OnLevelUp = () =>
            {
                Console.WriteLine("Next level");
                currentLevel++;
                LoadLevel();
            };

            Console.WriteLine("{0} {1}", levelManager.GridWidth, levelManager.GridHeight);
            Invalidate();
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
                case Keys.R: LoadLevel(); break;
                case Keys.X: Application.Exit(); break;
                default: return;
            }

            if (levelManager.player.Move(deltaX, deltaY, 1, grid))
            {
                Invalidate(); // Redraw screen after move
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int y = 0; y < levelManager.GridHeight; y++)
            {
                for (int x = 0; x < levelManager.GridWidth; x++)
                {
                    for (int l = 0; l < Layer; l++)
                    {
                        Rectangle tile = new Rectangle(x * levelManager.TileSize, y * levelManager.TileSize,
                                                           levelManager.TileSize, levelManager.TileSize);
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
