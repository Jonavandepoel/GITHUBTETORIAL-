using System;

using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows.Forms;



Form scherm = new Form();
scherm.Text = "MandelBrot Figuur";
scherm.BackColor = Color.Gray;
scherm.ClientSize = new Size(440, 620);
Bitmap plaatje = new Bitmap(400, 400);
Label xmiddenL = new Label();
TextBox xmiddenT = new TextBox();
Label schalingL = new Label();
TextBox schalingT = new TextBox();
TextBox ymiddenT = new TextBox();
Label ymiddenL = new Label();
TextBox aantalT = new TextBox();
Label aantalL = new Label();
Label afbeelding = new Label();

Button knop = new Button();
Button voorbeeld1 = new Button();



scherm.Controls.Add(xmiddenL);
scherm.Controls.Add(xmiddenT);
scherm.Controls.Add(schalingL);
scherm.Controls.Add(schalingT);
scherm.Controls.Add(ymiddenL);
scherm.Controls.Add(ymiddenT);
scherm.Controls.Add(aantalT);
scherm.Controls.Add(aantalL);
scherm.Controls.Add(afbeelding);

scherm.Controls.Add(knop);
scherm.Controls.Add(voorbeeld1);


afbeelding.Location = new Point(20, 180);
xmiddenL.Location = new Point(10, 20);
xmiddenT.Location = new Point(80, 20);
ymiddenL.Location = new Point(10, 50);
ymiddenT.Location = new Point(80, 50);
aantalT.Location = new Point(80, 110);
aantalL.Location = new Point(10, 110);
schalingL.Location = new Point(10, 85);
schalingT.Location = new Point(80, 85);

knop.Location = new Point(200, 150);
voorbeeld1.Location = new Point(350, 20);

afbeelding.Size = new Size(400, 400);
xmiddenL.Size = new Size(60, 20);
xmiddenT.Size = new Size(250, 20);
ymiddenL.Size = new Size(60, 20);
ymiddenT.Size = new Size(250, 20);
schalingL.Size = new Size(60, 20);
schalingT.Size = new Size(250, 20);
aantalL.Size = new Size(60, 20);
aantalT.Size = new Size(60, 20);

knop.Size = new Size(50, 20);
voorbeeld1.Size =new Size(80, 20);

xmiddenL.Text = "midden x:";
ymiddenL.Text = "midden y";
schalingL.Text = "schaal:";
aantalL.Text = "aantal";

knop.Text = "Go!";
voorbeeld1.Text = "Voorbeeld1";



afbeelding.BackColor = Color.Black;
afbeelding.Image = plaatje;

//parameters
// definieert parameters voor de afbeedling

// parameters voor basisplaatje
double vx = 0; // parameter voor midden x
double vy = 0; // parameter voor midden y
double schaal = 0.01; // parameter voor schaal
int n = 100; // parameter voor max aantal

// parameters voor kleuren
int rv = 0;
int gv = 100;
int bv = 175;



//parameters invullen in invoer (moet nog afgemaakt)
string schaalinv = schaal.ToString();
schalingT.Text = schaalinv;
string xmiddenTinv = vx.ToString();
xmiddenT.Text = xmiddenTinv;
string ymiddenTinv = vy.ToString();
ymiddenT.Text = ymiddenTinv;
string aantalTinv = n.ToString();
aantalT.Text = aantalTinv;

Point hier = new Point(0, 0);


int mandelbrotgetal(double x, double y)
//functie die mandelbrotgetal berekent van een punt (x,y) 
{
    double a = 0;
    double b = 0;
    double pita = 0;
    int i = 0;
    for (pita = 0; pita < 2 && i < n;)
    {
        double alfa = (a * a) - (b * b) + x;
        double beta = (2 * a * b) + y;
        a = alfa;
        b = beta;
        i = i + 1;
        pita = Math.Sqrt((Math.Pow(a, 2) + Math.Pow(b, 2)));


    }
    return i;
}





void Bitmapmaken()
{

    for (int x = -200; x < 200; x++)
    {
        for (int y = -200; y < 200; y++)
        {
            double xmandel = x * schaal + vx;// houdt rekening met verschuiving middden en de schaal


            double ymandel = -y * schaal + vy;

            int i = mandelbrotgetal(xmandel, ymandel);

          

                int kl = i % 256;

            int r = (kl + rv) % 256;
            int g = (kl + gv) % 256;
            int b = (kl + bv) % 256;



            Color pixel = Color.FromArgb(r, g, b);
            plaatje.SetPixel(x + 200, y + 200, pixel);
        }
    }
    
}



void knopklik(object sender2, EventArgs ea)
{

    schaal = double.Parse(schalingT.Text);
    vx = double.Parse(xmiddenT.Text);
    vy = double.Parse(ymiddenT.Text);

    n = int.Parse(aantalT.Text);



    Bitmapmaken();
    afbeelding.Invalidate();


}
void vb1(object a, EventArgs mea)
{
    vx = -0.108625; // parameter voor midden x
    vy = 0.9014428; // parameter voor midden y
    schaal = 3.8147E-8;  // parameter voor schaal
    n = 400; // parameter voor max aantal
    string schaalinv = schaal.ToString();
    schalingT.Text = schaalinv;
    string xmiddenTinv = vx.ToString();
    xmiddenT.Text = xmiddenTinv;
    string ymiddenTinv = vy.ToString();
    ymiddenT.Text = ymiddenTinv;
    string aantalTinv = n.ToString();
    aantalT.Text = aantalTinv;
    Bitmapmaken();
afbeelding.Invalidate();


}
void teken(object sender,PaintEventArgs pea)
{ pea.Graphics.DrawImage(plaatje, 0, 0);

}


void muisklik(object a, MouseEventArgs mea)
{

    hier = mea.Location;
    vy = vy + -1 * (hier.Y - 200) * schaal;
    string ymiddenTinv = vy.ToString();
    ymiddenT.Text = ymiddenTinv;
    vx = vx + (hier.X - 200) * schaal;
    string xmiddenTinv = vx.ToString();
    xmiddenT.Text = xmiddenTinv;

    if (mea.Button == MouseButtons.Left) // inzoomen bij klik linkermuisknop
    {
        schaal = schaal * 0.9;
        string schaalinv = schaal.ToString();
        schalingT.Text = schaalinv;
    }

    if (mea.Button == MouseButtons.Right) // uitzoomen bij klik rechtermuisknop
    {
        schaal = schaal * 1.1;
        string schaalinv = schaal.ToString();
        schalingT.Text = schaalinv;
    }
}


afbeelding.Paint += teken;
    Bitmapmaken();



voorbeeld1.Click += vb1;

knop.Click += knopklik;

afbeelding.MouseClick += muisklik;




    Application.Run(scherm);  