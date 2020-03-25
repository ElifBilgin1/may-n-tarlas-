using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace mayinTarlasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            radioButton2.Checked = false;//pogram açıldığında radiobutton1 in seçili olmasını sağladık.
            radioButton3.Checked = false;

            button4.TextAlign = ContentAlignment.MiddleLeft;
        }
        public static int mayinSayisi;//tanımlamaları yaptık
        public static int[] mayinlar;
        public static ArrayList gidilenYol = new ArrayList();
        public static PictureBox pct;
        public static PictureBox pct2;
        public static int coordinat = 389;
        public static int mayinlaraYakin=0;
        public static string isim;
        public static string dosyaYolu = @"skorlar.txt";
        public static string kazanmaDurumu=" Kaybetti ";

        void MayinDoldur(int mayin)
        {
            mayinlar = new int[mayin];
            Random rnd = new Random();//random fonksiyonu çağırdım.
            for (int i = 0; i < mayin; i++)
            {
                int secilen = rnd.Next(0, 400);//0-400 arası sayı seçildi.
                if (mayinlar.Contains(secilen) || secilen==389)//daha önce seçilmişse veya 389 ise;
                {
                    i--;//yeniden sayı üretilmesini sağladım.
                    continue; //aşağıdaki satırları okumadan diğer elemana geçer.
                }
                mayinlar[i] = secilen;  //sayıyı mayınlar dizisine atadım.
            }
            Array.Sort(mayinlar);

            for (int i = 0; i < 400; i++)
            {
                pct = new PictureBox();//pictureBox nesnesi oluşturuldu.
                pct.Height = 18;//yükseklik ve genişlik ayarlandı.
                pct.Width = 18;
                pct.BackColor = Color.Yellow;//arka planı sarı yaptık.
                pct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;//resmi ortaladım.
                pct.Margin =new Padding(0);//pictureBoxların arasında boşluk bırakmadan eklenmesi sağladım.

                pct.Tag = mayinlar.Contains(i);//mayın sayısı kontrol edildi.
                if ((bool)pct.Tag == true)//mayın varsa;
                {
                    pct.ImageLocation = "mayin.jpg";//mayın resmi eklendi.
                }
                flowLayoutPanel1.Controls.Add(pct);
                pct.Visible = false;
            }
        }
        
        void AvatarCiz(int cor)//koordinat alarak avatar çizdirme fonksiyonu
        {
            gidilenYol.Add(cor);//koordinatı gidilen yola ekledim,beyaz yolu çizmek için.
            int index = Array.IndexOf(mayinlar, cor);//koordinatta mayın var mı yok mu kontrol eder.Yoksa -1 döndürür.
            int win1 = Array.IndexOf(mayinlar, 0);//sol üst köşede mayın var mı yok mu kontrol eder.
            int win2 = Array.IndexOf(mayinlar, 19);//sağ üst köşede mayın var mı yok mu kontrol eder.

            if (index != -1)//kazanma kaybetme koşulları ayarlandı.
            {
                kazanmaDurumu = " Kaybetti ";
                oyunBitisi();
            }
            else if (win1 != -1 && win2 != -1)
            {
                if(cor>0 && cor < 19)
                {
                    timer1.Stop();
                    MessageBox.Show("KAZANDIN.");
                    kazanmaDurumu = " Kazandı ";
                    oyunBitisi();
                }
            }
            else if (cor == 0 || cor == 19)
            {
                timer1.Stop();
                MessageBox.Show("KAZANDIN.");
                kazanmaDurumu = " Kazandı ";
                oyunBitisi();
            }
            else
            {
                mayinlaraYakin = 0;//oyun başladıktan sonra mayına yakınlık uzaklığı bellirttik.
                flowLayoutPanel1.Controls.Clear();
                for (int i = 0; i < 400; i++)
                {
                    pct2 = new PictureBox();
                    pct2.Height = 18;
                    pct2.Width = 18;
                    pct2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                    pct2.Margin = new Padding(0);
                
                    bool kontrol = gidilenYol.Contains(i);//gidilen yol kontrol edilir.
                    if(kontrol == true)
                    {
                        pct2.BackColor = Color.White;//varsa beyaz yapılır.
                    }
                    if (i == cor)
                    {
                        pct2.ImageLocation = "penguen.jpg";
                    }
                    flowLayoutPanel1.Controls.Add(pct2);
                }
                int index1 = Array.IndexOf(mayinlar, cor + 1);
                int index2 = Array.IndexOf(mayinlar, cor - 1);
                int index3 = Array.IndexOf(mayinlar, cor + 20);
                int index4 = Array.IndexOf(mayinlar, cor - 20);

                if (index1 != -1)
                {
                    mayinlaraYakin++;
                }
                if (index2 != -1)
                {
                    mayinlaraYakin++;
                }
                if (index3 != -1)
                {
                    mayinlaraYakin++;
                }
                if (index4 != -1)
                {
                    mayinlaraYakin++;
                }
                textBox2.Text = mayinlaraYakin.ToString();
            }
        }

        void oyunBitisi()
        {
            timer1.Stop();
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < 400; i++)
            {
                pct = new PictureBox();
                pct.Height = 18;
                pct.Width = 18;
                pct.BackColor = Color.Yellow;
                pct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                pct.Margin = new Padding(0);

                pct.Tag = mayinlar.Contains(i);
                if ((bool)pct.Tag == true)
                {
                    pct.ImageLocation = "mayin.jpg";
                }

                flowLayoutPanel1.Controls.Add(pct);
            }
            int sure, saniye, dakika;
            string dosyayaYaz = isim + kazanmaDurumu;
            sure = Convert.ToInt32(textBox3.Text);
            dakika = sure / 60;
            saniye = sure % 60;
            if (dakika < 10)
            {
                dosyayaYaz = dosyayaYaz + " 0" + dakika.ToString()+":";
            }
            else
            {
                dosyayaYaz = dosyayaYaz + " " + dakika.ToString()+":";
            }
            if (saniye < 10)
            {
                dosyayaYaz = dosyayaYaz + "0" + saniye.ToString();
            }
            else
            {
                dosyayaYaz = dosyayaYaz +  saniye.ToString();
            }
            

            StreamWriter sw = new StreamWriter(dosyaYolu,true);
            sw.WriteLine(dosyayaYaz,true);
            sw.Flush();
            sw.Close();

            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show("Yeni Oyun", "Kaybettiniz", MessageBoxButtons.YesNo);
            if(dialog == DialogResult.Yes)
            {
                flowLayoutPanel1.Controls.Clear();
                coordinat = 389;
                mayinlaraYakin = 0;
                gidilenYol.Clear();
                textBox1.Text = isim;
                textBox3.Text = "0";

                MayinDoldur(mayinSayisi);
                AvatarCiz(coordinat);
            }
            else
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isim = textBox1.Text;
            if (radioButton1.Checked)
            {
                mayinSayisi = 2;
            }
            if (radioButton2.Checked)
            {
                mayinSayisi = 50;
            }
            if (radioButton3.Checked)
            {
                mayinSayisi = 80;
            }

            MessageBox.Show(textBox1.Text + " kaydedildi");

            MayinDoldur(mayinSayisi);
            AvatarCiz(coordinat);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("© Created by Elif Bilgin.\n\n" +
                "Seviye seçip isminizi girdikten sonra kişiyi kaydet dediğiniz anda " +
                "oyun oluşturulacaktır.\n" +
                "Yön tuşlarına bastığınız andan itibaren oyun ve süre başlayacaktır.\n" +
                "Amacınız sol veya sağ en üst köşeye ulaşmaktır.\n\n" +
                "Bol Şans ☺");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if(coordinat / 20.0 < 1.0)
            {
                MessageBox.Show("Yukarı tarafa çok gittin.");
            }
            else
            {
                coordinat -= 20;
                AvatarCiz(coordinat);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if (coordinat % 20 == 19)
            {
                MessageBox.Show("Sağ tarafa çok gittin.");
            }
            else
            {
                coordinat += 1;
                AvatarCiz(coordinat);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if (coordinat % 20 == 0)
            {
                MessageBox.Show("Sol tarafa çok gittin.");
            }
            else
            {
                coordinat -= 1;
                AvatarCiz(coordinat);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if (coordinat+20 > 399)
            {
                MessageBox.Show("Aşağı tarafa çok gittin.");
            }
            else
            {
                coordinat += 20;
                AvatarCiz(coordinat);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int saniye = Convert.ToInt32(textBox3.Text);
            saniye++;

            textBox3.Text = saniye.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] skorTut;
            string skorGoruntule="";
            int i = 0;
            
            FileStream fs = new FileStream(dosyaYolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string yazi = sw.ReadLine();
            int skorSayac=0;
            while (yazi != null)
            {
                yazi = sw.ReadLine();
                skorSayac++;
            }
            sw.Close();
            fs.Close();

            FileStream fs2 = new FileStream(dosyaYolu, FileMode.Open, FileAccess.Read);
            StreamReader sw2 = new StreamReader(fs2);
            skorTut = new string[skorSayac+1];

            string yazi2 = sw2.ReadLine();
            skorTut[i] = yazi2;
            while (yazi2 != null)
            {
                yazi2 = sw2.ReadLine();
                i++;
                skorTut[i] = yazi2;
            }

            for (i = 0; i < skorTut.Length; i++)
            {
                skorGoruntule = skorGoruntule + skorTut[i] + "\n";
            }

            MessageBox.Show(skorGoruntule,"Skorlar");

            sw2.Close();
            fs2.Close();
        }
    }
}
