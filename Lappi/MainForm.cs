﻿using System.Windows.Forms;

namespace Lappi {

    public class MainForm : Form {

        public MainForm () {
            AutoSize = true;
            Controls.Add(new PictureBox {
                AutoSize = true,
                Image = System.Drawing.Image.FromFile("Resources\\Lenna.png")
            });
        }

    }

}
