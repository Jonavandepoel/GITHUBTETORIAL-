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
Button knop = new Button();
TextBox ymiddenT = new TextBox();
Label ymiddenL = new Label();
TextBox aantalT = new TextBox();
Label aantalL = new Label();


scherm.Controls.Add(xmiddenL);
scherm.Controls.Add(xmiddenT);
scherm.Controls.Add(schalingL);
scherm.Controls.Add(schalingT);
scherm.Controls.Add(knop);
scherm.Controls.Add(ymiddenL);
scherm.Controls.Add(ymiddenT);
scherm.Controls.Add(aantalT);
scherm.Controls.Add(aantalL);

xmiddenL.Location = new Point(10, 20);
xmiddenT.Location = new Point(80, 20);
ymiddenL.Location = new Point(10, 50);
ymiddenT.Location = new Point(80, 50);
aantalT.Location = new Point(80, 110);
aantalL.Location = new Point(10, 110);
schalingL.Location = new Point(10, 85);
schalingT.Location = new Point(80, 85);
knop.Location = new Point(200, 150);

xmiddenL.Size = new Size(60, 20);
xmiddenT.Size = new Size(250, 20);
ymiddenL.Size = new Size(60, 20);
ymiddenT.Size = new Size(250, 20);
schalingL.Size = new Size(60, 20);
schalingT.Size = new Size(250, 20);
knop.Size = new Size(50, 20);
aantalL.Size = new Size(60, 20);
aantalT.Size = new Size(60, 20);

xmiddenL.Text = "midden x:";
ymiddenL.Text = "midden y";
schalingL.Text = "schaal:";
aantalL.Text = "aantal";
knop.Text = "Go!";


//parameters
// definieert paratemeters voor de afbeedling


double vx = -0.108625; // parameter voor midden x
double vy = 0.9014428; // parameter voor midden y
double schaal = 3.8147E-8; // parameter voor schaal
int n = 400; // parameter voor max aantal
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
Label afbeelding = new Label();

afbeelding.Location = new Point(20, 180);
afbeelding.Size = new Size(400, 400);
afbeelding.BackColor = Color.Black;
afbeelding.Image = plaatje;
Point hier = new Point(0, 0);
scherm.Controls.Add(afbeelding);

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



// functie die een kleur toekent aan ieder punt in de weer te geven figuur op basis van mandelbrotgetal en rekening houdend met de parameters voor midden x, midden y, schaal en kleur (nu zwart, kleur nog doen)
  

    // even mandelbroodgettaalen worden nu zwart (of grijstint als kl < 255)
    // dit moet aangepast nog; rood, groen en blauw component moeten alle drie van mandelbrotgetal afhangen 



void teken(object sender, PaintEventArgs pea)
// functie die de weer te geven bitmap (opnieuw) tekent
{
    

    for (int x = -200; x < 200; x++)
    {
        for (int y = -200; y < 200; y++)
        {
            

            int i = mandelbrotgetal(x * schaal + vx, -y * schaal + vy); // houdt rekening met verschuiving middden en de schaal
            int kl = i % 256;

            int r = (kl + rv) % 256;    
            int g = (kl + gv) % 256;
            int b = (kl + bv) % 256;



            Color pixel = Color.FromArgb(r,g, b);
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
    //teken(null,null);
    afbeelding.Invalidate();

    // Dit is poging schaal op te halen uit tekstinvoer maar lijkt niet goed te werken
}
void muisklik(object a, MouseEventArgs ea)
{
    
    hier = ea.Location;
    vy = hier.Y*schaal;
    afbeelding.Invalidate();
}





knop.Click += knopklik;

afbeelding.MouseClick += muisklik;

//scherm.MouseClick += muisklik;
afbeelding.Paint += teken;

Application.Run(scherm);