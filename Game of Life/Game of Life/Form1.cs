using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using System.Windows.Forms;
using System.Collections;

namespace Game_of_Life
{
    public partial class Form1 : Form
    {
        int gridWidth = 10;
        int gridHeight = 10;
        Dictionary<string, Cell> cells = new Dictionary<string, Cell>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGrid(10, 10);
        }

        void CreateGrid(int width, int height)
        {
            //creating grid of cells
            //max 58
            //cells = new Cell[width*height];
            cells.Clear();
            gridWidth = width;
            gridHeight = height;
            for (int y = 0; y < height; y++)
            {
                //max 35
                for (int x = 0; x < width; x++)
                {
                    Cell newC = new Cell(x, y)
                    {
                        Location = new Point(x * 30, y * 30),
                        Name = "Cell" + (x + (y * gridWidth)).ToString(),
                        Alive = false
                    };
                    cells.Add(newC.Name, newC);
                    panel1.Controls.Add(newC);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            //creating height and width of grid, forcing values to within maximum range
            Int32.TryParse(XBox.Text, out int width);
            if (width <= 0)
                width = 1;
            else if (width > 58)
                width = 58;
            Int32.TryParse(YBox.Text, out int height);
            if (height <= 0)
                height = 1;
            else if (height > 35)
                height = 35;
            int widthReq = width * 30;
            if (widthReq > 600)
            {
                panel1.Width = widthReq;
                this.Width = widthReq + 183;
            }
            int heightReq = height * 30;
            if (heightReq > 450)
            {
                panel1.Height = heightReq;
                //needs extra room to include top bar of app
                this.Height = heightReq + 41;
            }
            else
            {
                panel1.Height = 450;
                this.Height = 491;
            }
            CreateGrid(width, height);
        }

        private bool Tick()
        {
            //visual indicator for when tested w/o headphones
            if (button1.BackColor != Color.Red)
                button1.BackColor = Color.Red;
            else
                button1.BackColor = Color.Gray;
            //simulating the tick
            foreach (Cell c in panel1.Controls)
            {
                Cell[] neighbors = FindNeighbours(c);
                int num = CountNeighbours(neighbors);
                if (num == 3)
                    c.Resing = true;
                else if (num != 2)
                    c.Dying = true;
            }
            //cells change state
            foreach (Cell c in panel1.Controls)
                c.Change();
            //counting living cells
            int living = 0;
            foreach (Cell c in panel1.Controls)
                if (c.Alive)
                    living++;
            if (living == 0)
                return false;
            else
                return true;
        }

        Cell[] FindNeighbours(Cell cell)
        {
            Int32.TryParse(cell.Name.Substring(4), out int num);
            Cell[] neighbours = new Cell[8];

            //top row neighbours
            neighbours[0] = ((num > gridWidth-1 && num % gridWidth != 0) ? cells["Cell" + (num - gridWidth - 1)] : null);
            neighbours[1] = ((num > gridWidth-1) ? cells["Cell" + (num - gridWidth)] : null);
            neighbours[2] = ((num > gridWidth-1 && num % gridWidth != gridWidth - 1) ? cells["Cell" + (num - gridWidth + 1)] : null);

            //adjacent neighbors
            neighbours[3] = ((num % gridWidth != 0) ? cells["Cell" + (num - 1)] : null);
            neighbours[4] = ((num % gridWidth != gridWidth - 1) ? cells["Cell" + (num + 1)] : null);

            //bottom row neighbours
            neighbours[5] = (((num < (gridHeight - 1) * gridWidth) && num % gridWidth != 0) ? cells["Cell" + (num + gridWidth - 1)] : null);
            neighbours[6] = ((num < (gridHeight - 1) * gridWidth) ? cells["Cell" + (num + gridWidth)] : null);
            neighbours[7] = (((num < (gridHeight - 1) * gridWidth) && num % gridWidth != gridWidth - 1) ? cells["Cell" + (num + gridWidth + 1)] : null);

            return neighbours;
        }

        int CountNeighbours(Cell[] neighbours)
        {
            int count = 0;
            foreach (Cell c in neighbours)
            {
                if (c == null)
                    continue;
                if (c.Alive) {
                    count++;
                }
            }
            return count;
        }

        int CountNeighbours(Cell cell)
        {
            int count = 0;
            Cell dead = new Cell(99, 99)
            {
                Alive = false
            };
            Int32.TryParse(cell.Name, out int num);
            //finding each neighboring cell and checking if it is alive, in order to prevent this value requiring storage
            if (((num > gridWidth && num % gridWidth != 0) ? (Cell)panel1.Controls.Find("Cell" + (num - gridWidth - 1), false)[0] : new Cell(99, 99)).Alive)
                    count++;
            if (((num > gridWidth) ? (Cell)panel1.Controls.Find("Cell" + (num - gridWidth), false)[0] : new Cell(99, 99)).Alive)
                count++;
            if (((num > gridWidth && num % gridWidth != gridWidth - 1) ? (Cell)panel1.Controls.Find("Cell" + (num - gridWidth + 1), false)[0] : new Cell(99, 99)).Alive)
                count++;
            if (((num % gridWidth != 0) ? (Cell)panel1.Controls.Find("Cell" + (num - 1), false)[0] : new Cell(99, 99)).Alive)
                count++;
            if (((num % gridWidth != gridWidth - 1) ? (Cell)panel1.Controls.Find("Cell" + (num + 1), false)[0] : new Cell(99, 99)).Alive)
                count++;
            if ((((num < (gridHeight - 1) * gridWidth) && num % gridWidth != 0) ? (Cell)panel1.Controls.Find("Cell" + (num + gridWidth - 1), false)[0] : new Cell(99, 99)).Alive)
                count++;
            if (((num < (gridHeight - 1) * gridWidth) ? (Cell)panel1.Controls.Find("Cell" + (num + gridWidth), false)[0] : new Cell(99, 99)).Alive)
                count++;
            if ((((num < (gridHeight - 1) * gridWidth) && num % gridWidth != gridWidth - 1) ? (Cell)panel1.Controls.Find("Cell" + (num + gridWidth + 1), false)[0] : new Cell(99, 99)).Alive)
                count++;

            return count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                timer1.Enabled = false;
            else
                timer1.Enabled = true;
        }

        WindowsMediaPlayer player = new WindowsMediaPlayer
        {
            URL = @"C:\Users\Harvey\Downloads\Tick.mp3"
        };

        private void timer1_Tick(object sender, EventArgs e)
        {
            player.controls.play();
            if (!Tick())
                timer1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tick();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(maskedTextBox1.Text, out int time) && time > 0)
                timer1.Interval = time;
        }
    }
}
