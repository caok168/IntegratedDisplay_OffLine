using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Reflection;

namespace InGraph.Classes
{
    class GraphicsClass
    {
       public static void DrawingPoints(MainForm mf, PaintEventArgs e)
       {
           Bitmap bmp = new Bitmap(mf.MFPictureBox.ClientSize.Width, mf.MFPictureBox.ClientSize.Height);
           Graphics g1 = Graphics.FromImage(bmp);
           //g1.DrawLines(new Pen(Color.Red), CommonClass.PCCommon.Channel_1);
           PointF[] p = new PointF[800];
           //typeof(CommonClass.PCCommon).GetField

          // MessageBox.Show(pp[2].Y.ToString());
           //MessageBox.Show(pi.GetValue(CommonClass.PCCommon));
           if (CommonClass.PCCommon != null)
           {
               FieldInfo pi;
               for (int j = 0; j < 16; j++)
               {
                   if (mf.MFRightPictureBox.Controls["Channel_" + (j + 1)].Visible==false)
                   continue;
                   pi = (CommonClass.PCCommon.GetType()).GetField("Channel_"+(j+1) );
                   PointF[] pp = (PointF[])pi.GetValue(CommonClass.PCCommon);
                   for (int i = 0; i < 800; i++)
                   {
                       p[i].Y = mf.MFRightPictureBox.Controls["Channel_" + (j + 1)].Top + 2 + mf.MFRightPictureBox.Controls["Channel_" + (j + 1)].Height + pp[i].Y;

                       p[i].X = pp[i].X;
                   }
                   mf.MFRightPictureBox.Controls["Channel_" + (j + 1) + "_Data"].Text = pp[799].Y.ToString();
                   g1.DrawLines(new Pen(Color.FromArgb(CommonClass.DCConfig[j].Color)), p);
               }


               e.Graphics.DrawImage(bmp, new Point(0, 0));
           }
       }
    
    }
}
