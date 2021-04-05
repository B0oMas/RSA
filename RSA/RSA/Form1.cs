using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Diagnostics;

namespace RSA
{
    public partial class Form1 : Form
    {
        BigInteger p1, p2, mod, phi, publicExp, privateExp; // p = prime number

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private bool calcPrivKey()
        {
            try
            {
                p1 = BigInteger.Parse(richTextBox1.Text);
                p2 = BigInteger.Parse(richTextBox2.Text);
                publicExp = BigInteger.Parse(richTextBox5.Text);
            }
            catch
            {
                MessageBox.Show("Iveskite teisingus skaicius", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            if (p1 == p2)
                return false;

            if (p2 > p1) // apkeis vietomis
            {
                richTextBox1.Text = p2.ToString();
                richTextBox2.Text = p1.ToString();
                p1 = BigInteger.Parse(richTextBox1.Text);
                p2 = BigInteger.Parse(richTextBox2.Text);
            }

            UInt16 p1BitSize = p1.GetBitsize();
            UInt16 p2BitSize = p2.GetBitsize();

            if (p1BitSize < 126 || p2BitSize < 126)
            {
                MessageBox.Show("Iveskite skaicius, kad butu uzimti bent 126 bitai", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            UInt16 t;
            if (p1BitSize >= 1530 && p2BitSize >= 1530)
            {       //1536
                t = 3;
            }
            else if (p1BitSize >= 1020 && p2BitSize >= 1020)
            {       //1024
                t = 4;
            }
            else if (p1BitSize >= 508 && p2BitSize >= 508)
            {       //512
                t = 5;
            }
            else
            {       //default
                t = 20;
            }
            if (!p1.IsProbablePrime(t) || !p2.IsProbablePrime(t))
            {
                MessageBox.Show("Abu skaiciai turi buti pirminiai", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            mod = BigInteger.Multiply(p1, p2);
            richTextBox6.Text = mod.ToString();
            label9.Text = "Bitsize: " + mod.GetBitsize();

            phi = mod - p1 - p2 + 1;
            richTextBox7.Text = phi.ToString();
            BigInteger gcd = BigInteger.GreatestCommonDivisor(publicExp, phi);
            if (gcd != 1)
            {
                MessageBox.Show("gcd(e, phi) != 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            privateExp = publicExp.modInverse(ref phi);
            richTextBox8.Text = privateExp.ToString();
            BigInteger testD = BigInteger.Multiply(publicExp, privateExp) % phi;
            if (gcd != 1)
            {
                MessageBox.Show("(e*d)%phi != 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }

        
}
