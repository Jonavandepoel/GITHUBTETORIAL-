using System;
using System.Drawing;
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


    ComboBox cmbBord;

    Button btnNieuw;
    Button btnHelp;

    bool helpAan = false;

    int clientsizex = 360;
    int clientsizey = 560;
    int cellSize = 60;

    Steen steen = new Steen();
    public int boardX = 0;
    public int boardY = 200;
    Label lblBlauw;
    Label lblRood;
    Label lblBeurt;
    Label lblGrootte;



    public N()
    {

        this.Paint += FormPaint;

        this.Text = "n";
        this.BackColor = Color.White;
        this.ClientSize = new Size(clientsizex, clientsizey);

        lblBlauw = new Label();
        lblBlauw.Location = new Point(10, 10);
        lblBlauw.AutoSize = true;

        lblRood = new Label();
        lblRood.Location = new Point(10, 35);
        lblRood.AutoSize = true;

        lblBeurt = new Label();
        lblBeurt.Location = new Point(10, 60);
        lblBeurt.AutoSize = true;

        lblGrootte = new Label();
        lblGrootte.Text = "Bordgrootte:";
        lblGrootte.AutoSize = true;
        lblGrootte.Location = new Point(10, 90);

        btnNieuw = new Button();
        btnNieuw.Text = "nieuw spel";
        btnNieuw.Location = new Point(140, 10);
        btnNieuw.AutoSize = true;
        btnNieuw.Click += NieuwSpelKlik;


        btnHelp = new Button();
        btnHelp.Text = "help";
        btnHelp.Location = new Point(230, 10);
        btnHelp.AutoSize = true;
        btnHelp.Click += HelpKlik;


        this.Controls.Add(lblGrootte);
        this.Controls.Add(lblBeurt);
        this.Controls.Add(lblBlauw);
        this.Controls.Add(lblRood);
        this.Controls.Add(btnNieuw);
        this.Controls.Add(btnHelp);

        steen.start();
        updatekleurlabels();
        this.MouseClick += muisklik;
        this.Paint += steen.teken;
        this.Invalidate();

        cmbBord = new ComboBox();
        cmbBord.Location = new Point(100, 90);
        cmbBord.DropDownStyle = ComboBoxStyle.DropDownList;

        cmbBord.Items.Add("4 x 4");
        cmbBord.Items.Add("6 x 6");
        cmbBord.Items.Add("8 x 8");
        cmbBord.Items.Add("10 x 10");

        cmbBord.SelectedIndex = 1; // start op 6x6
        cmbBord.SelectedIndexChanged += BordGrootteVeranderd;

        this.Controls.Add(cmbBord);


    }
    void updatekleurlabels()
    {
        int blauw = steen.TelBlauw();
        int rood = steen.TelRood();

        lblBlauw.Text = $"Blauw: {blauw}";
        lblRood.Text = $"Rood: {rood}";

        int aanZet = steen.WieIsAanZet();

        if (aanZet == 1) lblBeurt.Text = "Aan zet: Blauw";
        else lblBeurt.Text = "Aan zet: Rood";
    }


    void BordGrootteVeranderd(object sender, EventArgs e)
    {
        string tekst = cmbBord.SelectedItem.ToString();
        int grootte = int.Parse(tekst.Split('x')[0].Trim());
        clientsizex = cellSize * grootte;
        clientsizey = 200 + cellSize * grootte;
        this.ClientSize = new Size(clientsizex, clientsizey);
        steen.SetBordGrootte(grootte);
        steen.start();
        updatekleurlabels();
        this.Invalidate();

    }

    void NieuwSpelKlik(object sender, EventArgs e)
    {
        helpAan = false;
        steen.start();
        updatekleurlabels();
        this.Invalidate();


    }




    void HelpKlik(object sender, EventArgs e)
    {
        helpAan = !helpAan;   // toggle (aan/uit)
        this.Invalidate();    // redraw zodat hulp-cirkels wel/niet verschijnen
    }

    void FormPaint(object sender, PaintEventArgs pea)
    {
        steen.teken(sender, pea);  //roept je teken functie op

        if (!helpAan) return;  // als help uitstaat return dan niks. Je wilt dat hij alleen cicles gaat tekenen wanneer help aan staat.

        Graphics g = pea.Graphics;
        int speler = steen.WieIsAanZet();

        int n = steen.bord.GetLength(0);  // controleert de bordgrootte

        for (int kolom = 0; kolom < n; kolom++)
        {
            for (int rij = 0; rij < n; rij++)
            {
                if (steen.bord[kolom, rij] != 0) continue; // als vakje niet leeg is sla je het over

                if (steen.Legalezet(kolom, rij))
                {
                    int x = boardX + rij * cellSize;
                    int y = boardY + kolom * cellSize;   // rekent uit waar op de scherm het vakje is

                    int m = cellSize / 3;  // je wilt dat de cirkel kleiner is dan het vakje zodat het er niet rommelig uitziet.
                    Rectangle hint = new Rectangle(x + m, y + m, cellSize - 2 * m, cellSize - 2 * m);


                    g.DrawEllipse(Pens.Gray, hint);
                }
            }
        }
    }




    void muisklik(object sender, MouseEventArgs mea)
    {
        int kolom = (mea.Y - boardY) / cellSize;
        int rij = (mea.X - boardX) / cellSize;


        steen.doezet(kolom, rij);

        updatekleurlabels();
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
        {
            bord = new int[bordgrootte, bordgrootte];
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

        public void SetBordGrootte(int g)
        {
            bordgrootte = g;
        }


        public void aanzet()
        {


            if (blauwzet) { waarde = 1; roodzet = true; blauwzet = false; }
            else
            if (roodzet) { waarde = 2; blauwzet = true; roodzet = false; }
        }
        public int tegenspeler()
        {
            if (waarde == 1) return 2;
            else return 1;
        }

        public bool inbord(int r, int k)
        {
            return bordgrootte > r && r >= 0 && bordgrootte > k && k >= 0;

        }
        public void doezet(int kolom, int rij)
        {
            if (!Legalezet(kolom, rij)) return;
            bord[kolom, rij] = waarde;
            int verschoofkolom = kolom;
            int verschoofrij = rij;




            for (int i = 0; i < 8; i++)
            {
                verschoofkolom = kolom + verschuifx[i];
                verschoofrij = rij + verschuify[i];
                if (!inbord(verschoofkolom, verschoofrij)) { continue; }
                if (bord[verschoofkolom, verschoofrij] != tegenspeler()) { continue; }

                verschoofkolom = verschoofkolom + verschuifx[i];
                verschoofrij = verschoofrij + verschuify[i];
                if (!inbord(verschoofkolom, verschoofrij)) { continue; }

                while (inbord(verschoofkolom, verschoofrij) && bord[verschoofkolom, verschoofrij] == tegenspeler())
                {
                    verschoofkolom += verschuifx[i];
                    verschoofrij += verschuify[i];

                }


                if (!inbord(verschoofkolom, verschoofrij)) { continue; }
                if (bord[verschoofkolom, verschoofrij] == waarde)
                {

                    while (!(verschoofkolom == kolom && verschoofrij == rij))
                    {
                        bord[verschoofkolom, verschoofrij] = waarde;
                        verschoofkolom -= verschuifx[i];
                        verschoofrij -= verschuify[i];


                    }
                }

            }

            aanzet();

        }
        public bool Legalezet(int kolom, int rij)
        {
            int verschoofkolom = kolom;
            int verschoofrij = rij;
            if (!inbord(kolom, rij)) return false;
            if (bord[kolom, rij] != 0) return false;


            for (int i = 0; i < 8; i++)
            {
                verschoofkolom = kolom + verschuifx[i];
                verschoofrij = rij + verschuify[i];
                if (!inbord(verschoofkolom, verschoofrij)) { continue; }
                if (bord[verschoofkolom, verschoofrij] != tegenspeler()) { continue; }
                verschoofkolom = verschoofkolom + verschuifx[i];
                verschoofrij = verschoofrij + verschuify[i];
                if (!inbord(verschoofkolom, verschoofrij)) { continue; }

                while (inbord(verschoofkolom, verschoofrij) && bord[verschoofkolom, verschoofrij] == tegenspeler())
                {
                    verschoofkolom += verschuifx[i];
                    verschoofrij += verschuify[i];

                }

                if (!inbord(verschoofkolom, verschoofrij)) { continue; }
                if (bord[verschoofkolom, verschoofrij] == waarde) return true;





            }
            return false;
        }

        public int TelBlauw()
        {
            int blauw = 0;

            for (int r = 0; r < bordgrootte; r++)
            {
                for (int c = 0; c < bordgrootte; c++)
                {
                    if (bord[r, c] == 1) blauw++;
                }
            }

            return blauw;
        }

        public bool LegalezetVoorSpeler(int kolom, int rij, int speler)
        {
            int oudeWaarde = waarde; // onthoudt wie er aan zet is
            waarde = speler; // verandert waarde naar voor wie je wilt controleren

            bool ok = Legalezet(kolom, rij); // controleert of de zet legaal is voor "speler"

            waarde = oudeWaarde; // verandert waarde weer terug naar de start positie zodat je niks op het board verandert (het is gewoon een controlle)
            return ok;
        }



        public int TelRood()
        {
            int rood = 0;

            for (int r = 0; r < bordgrootte; r++)
            {
                for (int c = 0; c < bordgrootte; c++)
                {
                    if (bord[r, c] == 2) rood++;
                }

            }
            return rood;
        }


        public int WieIsAanZet()
        {
            return waarde; // 1 = blauw, 2 = rood
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
