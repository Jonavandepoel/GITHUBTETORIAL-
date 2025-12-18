using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class Program
{
    [STAThreadAttribute]
    public static void Main()
    {
        Application.Run(new n());
    }
}
internal class n : Form
{
    int cellSize = 60;
    int bordgroote;
    Steen steen = new Steen();

    public n()
    {
        this.Text = "n";
        this.BackColor = Color.White;
        this.ClientSize = new Size(360, 360);

        steen.start();
        this.MouseClick += muisklik;
        this.Paint += steen.teken;
        this.Invalidate();
    }
    void muisklik(object sender, MouseEventArgs mea)
    {
        int kolom = mea.Y / cellSize;
        int rij = mea.X / cellSize;
        steen.doezet(kolom, rij);
        steen.aanzet();
        this.Invalidate();
    }

    class Steen
    {
        int cellSize = 60;
        int waarde;
        public int[,] array = new int[6, 6];//grootte bord, groote bord
        bool roodzet = false;
        bool blauwzet = true;
        public void start()
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    array[i, j] = 0;


            array[2, 2] = 2;
            array[2, 3] = 1;
            array[3, 2] = 1;
            array[3, 3] = 2;
        }
        public void aanzet()
        { 
             waarde = 0;
           
            if (blauwzet == true) { roodzet = true; blauwzet = false; waarde = 1;   }
            if (roodzet == true) { waarde = 2; blauwzet = true; roodzet = false; }
        }
        public void doezet(int kolom, int rij)
        {    
            array[kolom, rij] = waarde;
            
        }
        public void teken(object sender, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    int x = col * cellSize;
                    int y = row * cellSize;

                    g.DrawRectangle(Pens.Black, x, y, cellSize, cellSize);

                    if (array[row, col] != 0)
                    {
                        int margin = 6;// groote cel gedeeld 10

                        Rectangle rect = new Rectangle(
                            x + margin,
                            y + margin,
                            cellSize - 2 * margin,
                            cellSize - 2 * margin
                        );

                        if (array[row, col] == 1)
                            g.FillEllipse(Brushes.Red, rect);

                        if (array[row, col] == 2)
                            g.FillEllipse(Brushes.Blue, rect);
                    }
                }
            }
        }

    }
}