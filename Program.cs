using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class Program
{
    [STAThreadAttribute]
    public static void Main()
    {
        Application.Run(new N());
    }
}
internal class N : Form
{
    int cellSize = 60;
    
    Steen steen = new Steen();

    public N()
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
        steen.aanzet();
        if (steen.legalezet(kolom, rij)== true) {
            steen.doezet(kolom, rij);
        } 
            this.Invalidate();
    }

    class Steen
    {
        int cellSize = 60;
        int waarde;
        int bordgrootte = 6;
        int midden;
        public int[,] array = new int[6, 6];//grootte bord, groote bord
        bool roodzet = false;
        bool blauwzet = true;
        int[] verschuifx = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
        int [] verschuify = new int[] {-1, 0, 1, -1, 1, -1, 0, 1 };

        public void start()
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    array[i, j] = 0;
            midden = bordgrootte / 2;
            int a = midden - 1;
            int b = midden;

            array[a, a] = 2;
            array[a, b] = 1;
            array[b, a] = 1;
            array[b, b] = 2;
        }
        public void aanzet()
        {
            waarde = 0;
             
            if (blauwzet == true) { waarde = 1; roodzet = true; blauwzet = false;  }
            else 
            if (roodzet == true) { waarde = 2; blauwzet = true; roodzet = false; }
        }
        public bool legalezet(int kolom, int rij)
        {
            int verschoofkolom = kolom;
            int verschoofrij = rij;

            if (array[kolom, rij] == 0)
            {
                // onderstaande code is een begin voor het checken of een zet legaal is of niet. 
                // Hier moet dus vooral gekeken worden naar of de zet op een bepaald veld iets insluit ja of nee. 
                //for (int i = 0; i < 8; i++)
                //{ verschoofkolom = kolom + verschuifx[i];
                //    verschoofrij = kolom + verschuifx[i];
                //    if (array[verschoofkolom, verschoofrij] != 0)



                 return true;   
             }
         
            else 
                return false;
         }
        public bool inbord(int r, int k)
        { // hier moet een formule komen te staan die aangeeft of het aangeklikte punt op het bord staat en of ingevuld kan worden in de matrix  
        return true;  }
        public void doezet(int kolom, int rij)
        {
            array[kolom, rij] = waarde;

        }
        public void teken(object sender, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            for (int row = 0; row < bordgrootte; row++)
            {
                for (int col = 0; col < bordgrootte; col++)
                {
                    int x = col * cellSize;
                    int y = row * cellSize;

                    g.DrawRectangle(Pens.Black, x, y, cellSize, cellSize);

                    if (array[row, col] != 0)
                    {
                        int margin = bordgrootte;// groote cel gedeeld 10

                        Rectangle rect = new Rectangle(
                            x + margin,
                            y + margin,
                            cellSize - 2 * margin,
                            cellSize - 2 * margin
                        );

                        if (array[row, col] == 2)
                            g.FillEllipse(Brushes.Red, rect);

                        if (array[row, col] == 1)
                            g.FillEllipse(Brushes.Blue, rect);
                    }
                }
            }
        }

    }
}
