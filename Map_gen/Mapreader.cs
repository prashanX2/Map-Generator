using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator
{
 
    public partial class Mapreader : Form
    {
        int robow;
        Bitmap map;
        string path;
        int[,] cords;
        List<string> admtrx=new List<string>();
        List<string> adjpath = new List<string>();
        List<string> param = new List<string>();
      

        public Mapreader()
        {

            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        Bitmap myBitmap;









        delegate void SetTextCallback(string text);

        private void SetText1(string text)
        {

            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.AppendText(text);
            }
        }



        private void SetText3(string text)
        {

            if (this.textBox3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText3);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox3.AppendText(text);
            }
        }








        private void Form_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            try
            {
                


                

                pictureBox1.Image = myBitmap;
                this.panel1.AutoScroll = true;
                this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            }
            catch (IOException tt)
            {
                MessageBox.Show(tt.ToString());

            }

        }
      
     

      
   

        

      
        
        
      
       
      
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            robow = Convert.ToInt32(textBox2.Text);
            param.Add(robow.ToString());
            int cx = map.Width / robow;
            int cy = map.Height / robow;
            cords = new int[cx + 1, cy + 1];
            int adjcordx = e.Location.X;
            int adjcordy = e.Location.Y;
           
           
            if (e.Button == MouseButtons.Left)
            {
               // MessageBox.Show(map.GetPixel(e.X,e.Y).ToArgb().ToString());
                label1.Text = e.Location.X.ToString();
                label2.Text = e.Location.Y.ToString();
                label5.Text = (adjcordx / robow).ToString();
                label6.Text = (adjcordy / robow).ToString();
                label9.Text = ((adjcordy / robow) + (((adjcordx / robow) - 1) * cords.GetUpperBound(0))).ToString();

            }//MessageBox.Show(mousecordx.ToString()+","+mousecordy.ToString());
            else
            {
                label3.Text = e.Location.X.ToString();
                label4.Text = e.Location.Y.ToString();
                label7.Text = (adjcordx / robow).ToString();
                label8.Text = (adjcordy / robow).ToString();
                label10.Text = ((adjcordy / robow) + (((adjcordx / robow) - 1) * cords.GetUpperBound(0))).ToString();
            }
        }
  

        private void pictureBox1_OnHover(object sender, MouseEventArgs e)
        {
           
        }

       
       

      

       

        private void Simulator_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Thread draw = new Thread(button1cick);
           draw.Start();
            
        }




        public void button1cick() {
        
            map = new Bitmap(path);
            robow = Convert.ToInt32(textBox2.Text.ToString());
            param.Add(robow.ToString());
            int cx = map.Width / robow;
            int cy = map.Height / robow;
            cords = new int[cx + 1, cy + 1];

            for (int z = 0; z < map.Width; z = z + robow)
            {
                for (int w = 0; w < map.Height; w = w + robow)
                {
                    if (z > robow / 2 && w > robow / 2 && z < map.Height - (robow / 2) && w < map.Width - (robow / 2))
                    {

                        int r = robow / 2;
                        int ox = z, oy = w;
                        List<int> colr = new List<int>();


                        for (int x = -r; x < r; x++)
                        {
                            int height = (int)Math.Sqrt(r * r - x * x);

                            for (int y = -height; y < height; y++)
                            {
                                //map.SetPixel(x + ox, y + oy, Color.Red);
                                if (map.GetPixel(x + ox, y + oy).ToArgb() == -16777216 || map.GetPixel(x + ox, y + oy).ToArgb() == -16776961)
                                {
                                    // textBox4.AppendText(map.GetPixel(x + ox, y + oy).ToArgb().ToString()+"\n");
                                    colr.Add(map.GetPixel(x + ox, y + oy).ToArgb());
                                }
                            }
                        }






                        if (colr.Count < 1)
                        {
                            SetText1(z + "," + w + "\n");
                            cords[z / robow, w / robow] = 1;
                            for (int x = -r; x < r; x++)
                            {
                                int height = (int)Math.Sqrt(r * r - x * x);

                                for (int y = -height; y < height; y++)
                                {
                                    map.SetPixel(x + ox, y + oy, Color.Green);
                                   // pictureBox1.Image = map;
                                }
                            }


                        }


                       

                    }    //textBox1.AppendText(i+","+j+"\n");
                   
                }
            }
            pictureBox1.Image = map;
        }
        

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
             DialogResult file = openFileDialog1.ShowDialog();
            if(DialogResult.OK==file)
            {
                 path = openFileDialog1.FileName;
                map = new Bitmap(path);
                pictureBox1.Image = map;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread gen = new Thread(generate);
            gen.Start();
        }


        public void generate() {

            add();
            printlist();
            writefile();
        }


        private void add()
        {
            MessageBox.Show(cords.GetUpperBound(0).ToString() + "," + cords.GetUpperBound(1).ToString());
            param.Add(cords.GetUpperBound(0).ToString());
            param.Add(cords.GetUpperBound(1).ToString());

            param.Add((cords.GetUpperBound(0) * cords.GetUpperBound(1)).ToString());
            for (int i = 0; i <= cords.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= cords.GetUpperBound(1); j++)
                {
                    if (cords[i, j] == 1 && i > 0 && j > 0 && i < cords.GetUpperBound(0) && j < cords.GetUpperBound(1))
                    {

                        //if (cords[i - 1, j - 1] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}", i, j, i - 1, j - 1)); }
                        if (cords[i, j - 1] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3} // {4}-->{5}", i, j, i, j - 1, j + ((i - 1) * cords.GetUpperBound(0)), j - 1 + ((i - 1) * cords.GetUpperBound(0)))); adjpath.Add((j + ((i - 1) * cords.GetUpperBound(0))).ToString() + " " + (j - 1 + ((i - 1) * cords.GetUpperBound(0))).ToString()); }
                        // if (cords[i + 1, j - 1] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}", i, j, i + 1, j - 1)); }
                        if (cords[i - 1, j] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}// {4}-->{5}", i, j, i - 1, j, j + ((i - 1) * cords.GetUpperBound(0)), j + ((i - 1 - 1) * cords.GetUpperBound(0)))); adjpath.Add((j + ((i - 1) * cords.GetUpperBound(0))).ToString() + " " + (j + ((i - 1 - 1) * cords.GetUpperBound(0))).ToString()); }
                        if (cords[i + 1, j] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}// {4}-->{5}", i, j, i + 1, j, j + ((i - 1) * cords.GetUpperBound(0)), j + ((i + 1 - 1) * cords.GetUpperBound(0)))); adjpath.Add((j + ((i - 1) * cords.GetUpperBound(0))).ToString() + " " + (j + ((i + 1 - 1) * cords.GetUpperBound(0))).ToString()); }
                        //if (cords[i - 1, j + 1] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}", i, j, i - 1, j + 1)); }
                        if (cords[i, j + 1] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}// {4}-->{5}", i, j, i, j + 1, j + ((i - 1) * cords.GetUpperBound(0)), j + 1 + ((i - 1) * cords.GetUpperBound(0)))); adjpath.Add((j + ((i - 1) * cords.GetUpperBound(0))).ToString() + " " + (j + 1 + ((i - 1) * cords.GetUpperBound(0))).ToString()); }
                        //if (cords[i + 1, j + 1] == 1) { admtrx.Add(string.Format("{0},{1}-------->{2},{3}", i, j, i + 1, j + 1)); }

                        //textBox3.AppendText(cords[i,j].ToString());
                    }
                }
            }

        }


        private void printlist()
        {
            foreach (string x in admtrx)
            {
                SetText3(x + "\n");
            }
        }

        private void writefile()
        {
            try
            {
                System.IO.File.WriteAllLines(@"adj_matrix.txt", adjpath);
                System.IO.File.WriteAllLines(@"param.txt", param);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread matrix = new Thread(makematrixstart);
            matrix.Start();
        }

        public void makematrixstart() {
            map = new Bitmap(path);

            makebitmatrix();
            MessageBox.Show("created");
        }


        public void makebitmatrix()
        {




            try
            {
                FileStream sb = new FileStream("bmatrix.txt", FileMode.OpenOrCreate);

                StreamWriter sw = new StreamWriter(sb);

                for (int x = 0; x < map.Width; x++)
                {
                    for (int y = 0; y < map.Height; y++)
                    {
                        if (map.GetPixel(x, y).ToArgb() != -1 && map.GetPixel(x, y).ToArgb() != -1107935)
                        {

                            sw.Write("1 ", x, y);
                        }
                        else
                        {

                            sw.Write("0 ", x, y);
                        }

                    }
                    sw.WriteLine();
                }

                sw.Close();



            }
            catch (Exception yu)
            {
                MessageBox.Show(yu.ToString());
            }



        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }


        }

        

    

     

      



       
    }
  
   

    
   

