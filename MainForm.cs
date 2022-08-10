using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBImgConverter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Color[] GbPalette = new Color[] { Color.FromArgb(255, 169, 169, 169), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 90, 90, 90), Color.Black };
        //Color[] GbPalette = new Color[] { Color.White, Color.LightGray, Color.DarkGray, Color.Black };

        double DistanceToColour(Color c1, Color c2)
        {
            return Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R) + (c1.G - c2.G) * (c1.G - c2.G) + (c1.B - c2.B) * (c1.B - c2.B));
        }

        public void Convert()
        {
            double[] distances = new double[GbPalette.Length];

            var origImg = pbOriginal.Image; 

            var GbImage = new Bitmap(origImg.Width, origImg.Height);

            for (int x = 0; x < origImg.Width; x++)
            {
                for (int y = 0; y < origImg.Height; y++)
                {
                    Color oc = ((Bitmap)origImg).GetPixel(x, y);
                    double md = double.MaxValue;
                    var mi = 0;
                    for(int i = 0; i < GbPalette.Length; i++)
                    {
                        var d = DistanceToColour(oc, GbPalette[i]);
                        if(d < md)
                        {
                            md = d;
                            mi = i;
                        }
                    }

                    GbImage.SetPixel(x, y, GbPalette[mi]);
                }
            }

            pbGb.Image = GbImage;
        }

        private void Form_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            // get all files droppeds  
            if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Any())
            {
                pbOriginal.Image = Image.FromFile(files.First());
                Convert();
            }
        }
    }
}
