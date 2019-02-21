using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tersine_Cikarma2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int dugumsayisi = 0;
        int[,] grafmatris;
        List<Dugum> dugumlistesi = new List<Dugum>();
        List<Dugum> silinenler = new List<Dugum>();
        List<Dugum> kalanlar = new List<Dugum>();


        private void btnonayla_Click(object sender, EventArgs e)
        {
            dugumsayisi = Convert.ToInt32(txtdugumsayisi.Text);
            grafmatris = new int[dugumsayisi, dugumsayisi];
            for (int i = 0; i < Math.Sqrt(grafmatris.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(grafmatris.Length); j++)
                {
                    grafmatris[i, j] = 0;
                }
            }
            txtdugumsayisi.Visible = false;
            lbldugumsayisi.Visible = false;
            btnonayla.Visible = false;
        }

        private void btnagirlikgir_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text = "";
            if (txtdugumsayisi.Visible == true)
            {
                MessageBox.Show("düğüm sayısı giriniz");
                return;

            }
            int dugum1 = Convert.ToInt32(txtdugum1.Text) - 1;
            int dugum2 = Convert.ToInt32(txtdugum2.Text) - 1;
            int agirlik = Convert.ToInt32(txtagirlik.Text);

            listBox1.Items.Add(txtdugum1.Text);
            listBox2.Items.Add(txtdugum2.Text);
            listBox3.Items.Add(txtagirlik.Text);

            grafmatris[dugum1, dugum2] = 1;
            grafmatris[dugum2, dugum1] = 1;
            Dugum new_dugum = new Dugum();
            new_dugum.dugum1 = dugum1;
            new_dugum.dugum2 = dugum2;
            new_dugum.kenar_agırlık = agirlik;
            int donen_index = index_bulucu(agirlik);
            if (donen_index == -1) dugumlistesi.Add(new_dugum);
            else dugumlistesi.Insert(donen_index, new_dugum);
            //matrisi_yazdir();
        }

        int index_bulucu(int deger)
        {
            int donecek = 0;
            int i;
            for (i = 0; i < dugumlistesi.Count; i++)
            {
                if (dugumlistesi[i].kenar_agırlık >= deger)
                {
                    donecek = i;
                    break;
                }
            }

            if (i == dugumlistesi.Count)
            {
                donecek = -1;
            }
            return donecek;
        }

        void KoprulerinCikarilmasi()
        {

            for (int i = dugumlistesi.Count - 1; i >= 0; i--)
            {
                if (stun_toplam(dugumlistesi[i].dugum1) == 1 || stun_toplam(dugumlistesi[i].dugum2) == 1)
                {
                    grafmatris[dugumlistesi[i].dugum1, dugumlistesi[i].dugum2] = 0;
                    grafmatris[dugumlistesi[i].dugum2, dugumlistesi[i].dugum1] = 0;

                    kalanlar.Add(dugumlistesi[i]);
                    dugumlistesi.RemoveAt(i);
                    KoprulerinCikarilmasi();
                    return;
                }
            }

        }

        int stun_toplam(int index)
        {
            int toplam = 0;
            for (int i = 0; i < Math.Sqrt(grafmatris.Length); i++)
            {
                toplam += grafmatris[i, index];
            }

            return toplam;
        }
        
        void TersineCikarma()
        {
            for (int i = dugumlistesi.Count - 1; i >= 0; i--)
            {
                //if (biragirlikkontrol() != 0) i = dugumlistesi.Count - 1;
                int gexixisayi = dugumlistesi.Count - 1;
                KoprulerinCikarilmasi();
                if (dugumlistesi.Count - 1 != gexixisayi) i = dugumlistesi.Count - 1;
                if (dugumsayisi - 1 == kalanlar.Count + dugumlistesi.Count)
                {
                    //kalanlariyaz();
                    return;
                }
                if (stun_toplam(dugumlistesi[i].dugum1) > 1 && stun_toplam(dugumlistesi[i].dugum2) > 1 || stun_toplam(dugumlistesi[i].dugum1) > 1 && stun_toplam(dugumlistesi[i].dugum2) > 1)
                {
                    if (!KoprumuDegilMi(dugumlistesi[i].dugum1, dugumlistesi[i].dugum1, dugumlistesi[i].dugum2, null))
                    {
                        continue;
                    }   
                    grafmatris[dugumlistesi[i].dugum1, dugumlistesi[i].dugum2] = 0;
                    grafmatris[dugumlistesi[i].dugum2, dugumlistesi[i].dugum1] = 0;
                    silinenler.Add(dugumlistesi[i]);
                    listBox4.Items.Add((1+dugumlistesi[i].dugum1).ToString() + " - " + (1+dugumlistesi[i].dugum2).ToString() + "  / " + dugumlistesi[i].kenar_agırlık);
                    dugumlistesi.RemoveAt(i);
                }
            }
            //if (dugumsayisi - 1 != kalanlar.Count + dugumlistesi.Count) TersineCikarma();
        }

        int biragirlikkontrol()
        {
            int islemsayisi = 0;
            for (int i = dugumlistesi.Count - 1; i >= 0; i--)
            {
                if (stun_toplam(dugumlistesi[i].dugum1) == 1 || stun_toplam(dugumlistesi[i].dugum2) == 1)
                {
                    grafmatris[dugumlistesi[i].dugum1, dugumlistesi[i].dugum2] = 0;
                    grafmatris[dugumlistesi[i].dugum2, dugumlistesi[i].dugum1] = 0;

                    kalanlar.Add(dugumlistesi[i]);
                    dugumlistesi.RemoveAt(i);
                    islemsayisi++;
                }
            }
            return islemsayisi;
        }

        

        Boolean KoprumuDegilMi(int dugum1,int ilkdugum1, int dugum2, ArrayList oncekidugumler)
        {
            bool donus_Degeri = false;
            if (oncekidugumler == null) oncekidugumler = new ArrayList();

            ArrayList kullanılacakarray = new ArrayList();
            kullanılacakarray.AddRange(oncekidugumler);
            kullanılacakarray.Add(dugum1);

            for (int i = 0; i < Math.Sqrt(grafmatris.Length); i++)
            {
                if (grafmatris[dugum1, i] == 1 && kullanılacakarray.IndexOf(i) == -1 && i != dugum2)
                {
                    //kullanılacakarray.Add(i);
                    donus_Degeri = KoprumuDegilMi(i,ilkdugum1 ,dugum2, kullanılacakarray);
                    if (donus_Degeri == true) return donus_Degeri;
                    
                } 
                if (grafmatris[dugum1, i] == 1 && i == dugum2 && dugum1!=ilkdugum1)
                {
                    donus_Degeri = true;
                    break;
                }
            }
            return donus_Degeri;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            TersineCikarma();
            for (int i = 0; i < dugumlistesi.Count; i++)
            {
                kalanlar.Add(dugumlistesi[i]);
            }
            cizim();
            if (kalanlar.Count == dugumsayisi - 1)
            {
                MessageBox.Show("kalanlarla bitti");
            }
            else
            {
                MessageBox.Show(" bitti");
            }
        }

        List<Point> konumlar = new List<Point>();
        void cizim()
        {
            int cemberboyut = 40;
            Graphics gr = panel1.CreateGraphics();
            Pen renk = new Pen(Color.DarkRed, 5);
            Point olusturulacak_nokta = new Point();
            Brush color = Brushes.Black;
            int x=20, y = 20;

            for (int i = 0; i < dugumsayisi; i++)
            {
                if(i%2==0)
                {
                    gr.DrawEllipse(renk, x, y, cemberboyut, cemberboyut);
                    
                    olusturulacak_nokta.X = x;
                    olusturulacak_nokta.Y = y;
                    gr.DrawString((i + 1).ToString(), DefaultFont, color, olusturulacak_nokta.X + 20, olusturulacak_nokta.Y + 20);
                    konumlar.Add(olusturulacak_nokta);
                }
                else
                {
                    x += 100;


                    gr.DrawEllipse(renk, x, y, cemberboyut, cemberboyut);
                    olusturulacak_nokta.X = x;
                    olusturulacak_nokta.Y = y;
                    gr.DrawString((i + 1).ToString(), DefaultFont, color, olusturulacak_nokta.X+20,olusturulacak_nokta.Y+20);
                    konumlar.Add(olusturulacak_nokta);

                    x -= 100;
                    y += 100;
                }
                
            }
            for (int i = 0; i < kalanlar.Count; i++)
            {
                gr.DrawLine(renk, konumlar[kalanlar[i].dugum1].X+20, konumlar[kalanlar[i].dugum1].Y + 20, konumlar[kalanlar[i].dugum2].X + 20, konumlar[kalanlar[i].dugum2].Y + 20);
            }

            renk = new Pen(Color.Yellow, 5);
            for (int i = 0; i < silinenler.Count; i++)
            {
                gr.DrawLine(renk, konumlar[silinenler[i].dugum1].X + 20, konumlar[silinenler[i].dugum1].Y + 20, konumlar[silinenler[i].dugum2].X + 20, konumlar[silinenler[i].dugum2].Y + 20);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    class Dugum
    {
        public int dugum1 = 0;
        public int dugum2 = 0;
        public int kenar_agırlık = 0;
    }

}