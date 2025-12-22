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
    public int boardX = 0;
    public int boardY = 200;

    public N()
    {
       
        this.Text = "n";
        this.BackColor = Color.White;
        this.ClientSize = new Size(360, 560);

        steen.start();
        this.MouseClick += muisklik;
        this.Paint += steen.teken;
        this.Invalidate();
    }
    void muisklik(object sender, MouseEventArgs mea)
    {
        int kolom = (mea.Y-boardY) / cellSize;
        int rij = (mea.X-boardX) / cellSize;
       
        if (steen.legalezet(kolom, rij) == true)
        {
            steen.doezet(kolom, rij);
        }
        this.Invalidate();
    }

    class Steen
    {
        int cellSize = 60;
        int waarde = 2;
        int bordgrootte = 6;
        int midden;
        public int[,] bord;
        bool roodzet = false;
        bool blauwzet = true;
        int[] verschuifx = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] verschuify = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
        public int boardX = 0;
        public int boardY = 200;


        public void start()
        { bord = new int[bordgrootte, bordgrootte];
        //    for (int i = 0; i < bord.getlength(0); i++)
        //        for (int j = 0; j < bord.getlength(1); j++)
        //            bord[i, j] = 0;
           midden = bordgrootte / 2;
            int a = midden - 1;
            int b = midden;

            bord[a, a] = 2;
            bord[a, b] = 1;
            bord[b, a] = 1;
            bord[b, b] = 2;
        }
        public void aanzet()
        {
            

            if (blauwzet) { waarde = 1; roodzet = true; blauwzet = false; }
            else
            if (roodzet) { waarde = 2; blauwzet = true; roodzet = false; }
        }
        public int tegenspeler()
        { if (waarde == 1) return 2;
            else return 1;
                }
 
        public bool inbord(int r, int k)
        { 
                return bordgrootte > r && r >= 0 && bordgrootte > k && k >= 0;
            
        }
        public void doezet(int kolom, int rij)
        { if (!legalezet(kolom, rij)) return;
            bord[kolom, rij] = waarde;
                for ( int i=0; i < 8 ; i++ ) 
            {
                int verschoofkolom = kolom + verschuifx[i];
                int verschoofrij = rij + verschuify[i];
                if (!inbord(verschoofkolom, verschoofrij)) { continue; }

                if (bord[verschoofkolom, verschoofrij] == tegenspeler())
                {
                    for (int n = 2; n < bordgrootte; n++)
                    {
                        verschoofkolom += verschuifx[i];
                        verschoofrij += verschuify[i];
                        if (!inbord(verschoofkolom, verschoofrij)) { break; }

                        if (bord[verschoofkolom, verschoofrij] == waarde) { 
                        for(int j= n; j >2; j-- ) 
                                verschoofkolom -= verschuifx[i];
                            verschoofrij -= verschuify[i];
                            bord[verschoofkolom,verschoofrij] = waarde;
                        }
                    }

                }
                else continue;




            }

            aanzet();

        }
        public bool legalezet(int kolom, int rij)
        {
            int verschoofkolom = kolom;
            int verschoofrij = rij;
            if(!inbord(kolom,rij)) return false;
            if (bord[kolom, rij] != 0) return false;

            
                for (int i = 0; i < 8; i++)
                {
                    verschoofkolom = kolom + verschuifx[i];
                    verschoofrij = rij + verschuify[i];
                if (!inbord(verschoofkolom, verschoofrij)) { continue; }

                if (bord[verschoofkolom, verschoofrij] == tegenspeler())
                {
                    while(inbord(verschoofkolom, verschoofrij) && bord[verschoofkolom, verschoofrij] == tegenspeler())
                    {
                        verschoofkolom += verschuifx[i];
                        verschoofrij += verschuify[i];
                        if ( bord[verschoofkolom, verschoofrij] == waarde ) { return true; } 
                    }

                }
                else continue;



                    
                }
               return false;
         }
        
        public void teken(object sender, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            for (int row = 0; row < bordgrootte; row++)
            {
                for (int col = 0; col < bordgrootte; col++)
                {
                    int x = boardX + col * cellSize;
                    int y = boardY + row * cellSize;

                    g.DrawRectangle(Pens.Black, x, y, cellSize, cellSize);

                    if (bord[row, col] != 0)
                    {
                        int margin = bordgrootte;// groote cel gedeeld 10

                        Rectangle rect = new Rectangle(
                            x + margin,
                            y + margin,
                            cellSize - 2 * margin,
                            cellSize - 2 * margin
                        );

                        if (bord[row, col] == 2)
                            g.FillEllipse(Brushes.Red, rect);

                        if (bord[row, col] == 1)
                            g.FillEllipse(Brushes.Blue, rect);
                    }
                }
            }
        }

    }
}