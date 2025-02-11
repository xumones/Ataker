using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ataker
{
    public partial class Form1 : Form
    {
        private const int GridWidth = 10;
        private const int GridHeight = 5;
        private const int TileSize = 154;

        private GameObject[,] grid = new GameObject[GridWidth, GridHeight]; // ใช้ Object แทนตัวเลข
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
            // Clear grid
            for (int y = 0; y < GridHeight; y++)
                for (int x = 0; x < GridWidth; x++)
                    grid[x, y] = null;

            // Create player
            player = new Player(0, 0);
            grid[player.X, player.Y] = player;

            // Create walls
            grid[2, 2] = new Wall(2, 2);
            grid[3, 3] = new Wall(3, 3);

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
                default: return;
            }

            if (player.Move(deltaX, deltaY, grid))
            {
                Invalidate(); // Redraw screen after move
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
                    Rectangle tile = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

                    if (grid[x, y] is Wall)
                        g.FillRectangle(Brushes.DarkGray, tile); // Draw wall
                    else if (grid[x, y] is Player)
                        g.FillRectangle(Brushes.Red, tile); // Draw player
                    else
                        g.DrawRectangle(Pens.Black, tile); // Draw empty grid
                }
            }
        }
    }
}
