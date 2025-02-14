﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ataker
{
    public partial class Form1 : Form
    {
        private const int GridWidth = 10;
        private const int GridHeight = 5;
        private const int Layer = 2;
        private const int TileSize = 154;

        private GameObject[,,] grid = new GameObject[GridWidth, GridHeight,Layer];
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
                    for (int l = 0; l < Layer; l++)
                        grid[x, y, l] = null;

            // Create player
            player = new Player(0, 0, 1);
            grid[player.X, player.Y, 1] = player;

            // Create walls
            grid[2, 2, 1] = new Wall(2, 2, 1);
            grid[3, 3, 1] = new Wall(3, 3, 1);

            // Create Obstacle
            grid[1, 1, 1] = new DocumentPile(1, 1, 1);
            grid[4, 4, 1] = new DocumentPile(4, 4, 1);

            // Create Trap
            grid[7, 3, 0] = new Trap(7, 3, 0);

            // Create Monster
            grid[5, 1, 1] = new Monster(5, 1, 1, 3);
            grid[6, 3, 1] = new Monster(6, 3, 1, 3);

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

            if (player.Move(deltaX, deltaY, 1, grid))
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
                    for (int l = 0; l < Layer; l++)
                    {
                        Rectangle tile = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

                        if (grid[x, y, l] is Wall)
                            g.FillRectangle(Brushes.DarkGray, tile); // Draw wall
                        else if (grid[x, y, l] is Player)
                            g.FillRectangle(Brushes.Red, tile); // Draw player
                        else if (grid[x, y, l] is DocumentPile)
                            g.FillRectangle(Brushes.Blue, tile);
                        else if (grid[x, y, l] is Monster)
                            g.FillRectangle(Brushes.LightBlue, tile);
                        else if (grid[x, y, l] is Trap)
                            g.FillRectangle(Brushes.LightPink, tile);
                        else if (l == 0) // layer 0 วาด grid
                            g.DrawRectangle(Pens.Black, tile); // Draw empty grid
                    }
                }
            }
        }
    }
}
