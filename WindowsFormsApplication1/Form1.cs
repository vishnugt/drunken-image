using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        String bits=null;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String location=null;
            OpenFileDialog fd = new OpenFileDialog();
           if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                location = fd.InitialDirectory + fd.FileName;
            }

           byte[] fileBytes = File.ReadAllBytes(location);
           StringBuilder sb = new StringBuilder();
           StringBuilder sb2 = new StringBuilder();


           foreach (byte b in fileBytes)
           {
               String temp_string = (Convert.ToString(b, 2).PadLeft(8, '0'));
               if (temp_string == "11111111") 
               {
                   temp_string = temp_string +"#"+ temp_string;
               }
               sb.Append(temp_string + "#");
           }
           sb.Append("11111111");
            File.WriteAllText("C:\\Projects\\photo_binary.txt", sb.ToString());
           foreach (byte b in fileBytes)
               sb2.Append((Convert.ToString(b, 2).PadLeft(8, '0')));
           File.WriteAllText("C:\\Projects\\photo_binary_withhash.txt", sb2.ToString());
    }


        private void button2_Click(object sender, EventArgs e)
        {
            String location = null;
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                location = fd.InitialDirectory + fd.FileName;
            }

            bits = File.ReadAllText(location);
            StringBuilder sb = new StringBuilder();

            Byte[] bytes = Enumerable.Range(0, bits.Length / 8).Select(pos => Convert.ToByte(bits.Substring(pos * 8, 8), 2)).ToArray();
            //File.WriteAllBytes("C:\\projects\\photo_from_binary.jpg", bytes);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(bytes.ToString());
                }
            }
            label1.Text = "" + bits.Length;
   
        }
        
        private void button3_Click(object sender, EventArgs ea)
        {
            String location = null;
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                location = fd.InitialDirectory + fd.FileName;
            }
            String read_from_file = File.ReadAllText(location);
            int size_of_binary = read_from_file.Length;
            int no_of_pixels = size_of_binary /24 +10;
            string[] zero_array ={"00000000","00000000","00000000","00000000","00000000","00000000"};
            string[] words2 = read_from_file.Split('#');

            int no_of_words = words2.Length;
            string[] words = new string[no_of_words + 10];

            Array.Copy(words2, words, no_of_words);
            Array.Copy(zero_array, 0, words, no_of_words, 6);


            Bitmap diffBM = new Bitmap(5 , 5, PixelFormat.Format24bppRgb);
            int i2 = 0;
            label1.Text = bits;
               for (int y = 0; y < 5 ; y++)
                {
                    for (int x = 0; x < 5 ; x++)
                    {
                        if (i2 < no_of_words + 4) 
                        {

                            int r = 0, g = 0, b = 0;
                            r = Convert.ToInt32(words[i2++], 2);
                            g = Convert.ToInt32(words[i2++], 2);
                            b = Convert.ToInt32(words[i2++], 2);

                            //Create new grayscale rgb colour   
                            Color newcol = Color.FromArgb(r, g, b);

                            diffBM.SetPixel(x, y, newcol);
      
                        }
                  }
                      

            }
            diffBM.Save("C:\\projects\\photo_converted.png", ImageFormat.Png);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            String temp=null;
            String location = null;
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                location = fd.InitialDirectory + fd.FileName;
            }
            Bitmap diffBM = new Bitmap(location);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Color col = diffBM.GetPixel(x, y);
                    String a = Convert.ToString(col.R,2).PadLeft(8,'0');
                    String b = Convert.ToString(col.G, 2).PadLeft(8, '0');
                    String c = Convert.ToString(col.B, 2).PadLeft(8, '0');
                    temp=temp+a+"#"+b+"#"+c+"#";

                }
            }
            String wholestring = null;
            String[] to_write = temp.Split('#');
            String[] to_write2 = new String[to_write.Length];
            int j = 0;
            for (int i = 0; i < to_write.Length; i++) 
            {
                if (to_write[i].ToString() == "11111111") 
                {
                    if (to_write[i + 1].ToString() == "11111111")
                    {
                        to_write2[j++] = to_write[i];
                        i++;
                    }
                    else
                    
                    {
                        for (int ii = 0; ii < to_write2.Length; ii++) 
                        {
                            wholestring = wholestring + to_write2[ii];
                        }
                        File.WriteAllText("C:\\projects\\final_binary_form.txt", wholestring);
                        Byte[] bytes2 = Enumerable.Range(0, wholestring.Length / 8).Select(pos => Convert.ToByte(wholestring.Substring(pos * 8, 8), 2)).ToArray();
                        File.WriteAllBytes("C:\\projects\\photo_from_binary_last.jpg", bytes2);
   

                        return;
                

                    }
                }
                else
                    to_write2[j++] = to_write[i];
            }
            for (int ii = 0; ii < to_write2.Length; ii++)
            {
                wholestring = wholestring + to_write2[ii];
            }
            File.WriteAllText("C:\\projects\\final_binary_form.txt", wholestring);


            StringBuilder sb = new StringBuilder();

            Byte[] bytes = Enumerable.Range(0, wholestring.Length / 8).Select(pos => Convert.ToByte(wholestring.Substring(pos * 8, 8), 2)).ToArray();
            File.WriteAllBytes("C:\\projects\\photo_from_binary_last.jpg", bytes);
   

        }

        private void button5_Click(object sender, EventArgs e)
        {
            byte[] fileBytes = File.ReadAllBytes("C:\\Projects\\raw_photo.jpg");
            StringBuilder sb = new StringBuilder();

            foreach (byte b in fileBytes)
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            File.WriteAllText("C:\\Projects\\vishnucomp10.txt", sb.ToString());
            String bits = sb.ToString();


            //String compressed = compression(sb);
            //store(compressed);
            //String decompressed = decompress(compressed);

            Byte[] bytes = Enumerable.Range(0, bits.Length / 8).Select(pos => Convert.ToByte(bits.Substring(pos * 8, 8), 2)).ToArray();
            File.WriteAllBytes("C:\\Projects\\vishnucomp9.jpg", bytes);
        }
    }
}
