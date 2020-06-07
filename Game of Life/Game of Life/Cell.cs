using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Game_of_Life
{
    public partial class Cell : UserControl, IComparer
    {
        #region Private members
        private bool alive;
        private bool dying;
        private bool resing;
        private int xPos;
        private int yPos;
        #endregion

        #region Constructors
        public Cell(int xPos, int yPos)
        {
            XPos = xPos;
            YPos = yPos;

            InitializeComponent();
        }
        #endregion

        #region Public properties
        public bool Alive { get => alive; set {
                alive = value;
                if (!alive) BackColor = Color.Black;
                else BackColor = Color.Green;
            } }
        public int XPos { get => xPos; set => xPos = value; }
        public int YPos { get => yPos; set => yPos = value; }
        public bool Dying { get => dying; set => dying = value; }
        public bool Resing { get => resing; set => resing = value; }
        #endregion

        #region Methods
        private void Cell_Click(object sender, EventArgs e)
        {
            Toggle();
        }

        public void Toggle()
        {
            if (Alive)
            {
                Alive = false;
            }
            else
            {
                Alive = true;
            }
        }

        public void Change()
        {
            if (Dying)
                Alive = false;
            if (Resing)
                Alive = true;
            Dying = false;
            Resing = false;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }
        //nested class to do sorting on name attribute
        private class SortByNameHelper : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                try
                {

                    Cell c1 = x as Cell;
                    Cell c2 = y as Cell;
                    Int32.TryParse(c1.Name.Substring(4), out int c1Int);
                    Int32.TryParse(c2.Name.Substring(4), out int c2Int);
                    if (c1Int > c2Int)
                        return 1;
                    else if (c2Int > c1Int)
                        return -1;
                    else
                        return 0;
                }
                catch (Exception)
                {
                    if (x == null && y == null)
                        return 0;
                    else if (y == null)
                        return 1;
                    else
                        return -1;
                }
            }
        }

        public static IComparer SortByName()
        {
            return (IComparer)new SortByNameHelper();
        }

        #endregion

        #region Constants

        #endregion

        #region Overrides
        public override string ToString()
        {
            return Name;
        }
        #endregion


    }
}
