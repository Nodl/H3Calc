﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using H3Calc.Engine;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace H3Calc
{
    public partial class UnitPicker : Form
    {
        public List<Unit> Units { get; set; }        

        public event EventHandler<UnitEventArgs> UnitPicked;

        public UnitPicker(List<Unit> units)
        {
            InitializeComponent();
            this.ClientSize = new Size(950, 695);

            Units = units;
        }        

        private Unit UnitFromPicker(int x, int y)
        {
            const int kPortraitWidth = 58;
            const int kPortraitHeight = 64;
            const int kPadding = 5;

            if ((x % (kPortraitWidth + kPadding) < kPadding) || (y % (kPortraitHeight + kPadding) < kPadding))
            {
                return null;
            }

            int row = y / (kPortraitHeight + kPadding);
            int column = x / (kPortraitWidth + kPadding);

            if (row != 9 && column == 14)
            {
                return null;
            }

            int unitId = row * 14 + column;
            return Units.FirstOrDefault(u => u.Id == unitId);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Unit unit = UnitFromPicker(e.X, e.Y);
            pictureBox1.Cursor = (unit != null) ? Cursors.Hand : Cursors.Default;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Unit unit = UnitFromPicker(e.X, e.Y);

            if (unit != null && UnitPicked != null)
            {
                UnitPicked(this, new UnitEventArgs(unit));
            }            
        }
    }

    public class UnitEventArgs : EventArgs
    {
        public UnitEventArgs(Unit unit)
        {
            Unit = unit;
        }

        public Unit Unit { get; set; }
    }
}
