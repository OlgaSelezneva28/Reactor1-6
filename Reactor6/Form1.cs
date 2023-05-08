using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Reactor6
{
    public partial class Form1 : Form
    {
        //Параметры 
        double l;
        double H;
        double r;
        double nu;//ню

        //лямбды
        double a1 = 0.0124;
        double a2 = 0.0305;
        double a3 = 0.111;
        double a4 = 0.301;
        double a5 = 1.14;
        double a6 = 3.01;

        //бетты
        double b1 = 0.0002145;
        double b2 = 0.0014235;
        double b3 = 0.001274;
        double b4 = 0.0025675;
        double b5 = 0.0007475;
        double b6 = 0.000275;

        //
        double dt; // Шаг по времени 
        //
        List<double> t = new List<double>();
        List<double> n = new List<double>();
        List<double> c1 = new List<double>();
        List<double> c2 = new List<double>();
        List<double> c3 = new List<double>();
        List<double> c4 = new List<double>();
        List<double> c5 = new List<double>();
        List<double> c6 = new List<double>();
        List<double> y = new List<double>();
        List<double> z = new List<double>();


        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        double S(double n, double c1,double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            double q = b1 / (l * (a1 + r)) + b2 / (l * (a2 + r)) + b3 / (l * (a3 + r)) + b4 / (l * (a4 + r)) + b5 / (l * (a5 + r)) + b6 / (l * (a6 + r)) + 1;
            double sum = (a1 * c1) / (a1 + r) + (a2 * c2) / (a2 + r) + (a3 * c3) / (a3 + r) + (a4 * c4) / (a4 + r) + (a5 * c5) / (a5 + r) + (a6 * c6) / (a6 + r) ;

            return (nu / r) * ( (-1) * n / (r * q) - 1 / (l * r * q ) * (sum) - y + z / (l * q * r * r));
        }

        double N(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z) // Для линейной системы
        {
            double sum1 = n * (b1 + b2 + b3 + b4 + b5 + b6);
            double sum2 = c1 * a1 + c2 * a2 + c3 * a3 + c4 * a4 + c5 * a5 + c6 * a6;
            return (1 / l) * ( -sum1 + sum2 - z);
        }
        double NNL(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z) // Для линейной системы
        {
            double sum1 = n * (b1 + b2 + b3 + b4 + b5 + b6);
            double sum2 = c1 * a1 + c2 * a2 + c3 * a3 + c4 * a4 + c5 * a5 + c6 * a6;
            return (1 / l) * (-sum1 + sum2 - z * ( n + 1));
        }
        double C1(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return (b1 * n - c1 * a1);
        }
        double C2(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return (b2 * n - c2 * a2);
        }
        double C3(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return (b3 * n - c3 * a3);
        }
        double C4(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return (b4 * n - c4 * a4);
        }
        double C5(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return (b5 * n - c5 * a5);
        }
        double C6(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return (b6 * n - c6 * a6);
        }
        double Y(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return n;
        }
        double Z(double t, double n, double c1, double c2, double c3, double c4, double c5, double c6, double y, double z)
        {
            return H * y * Math.Sign(y * S(n, c1, c2, c3, c4, c5, c6, y, z));
        }

        
        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        
        //Запуск Линейной задачи
        private void button1_Click(object sender, EventArgs e)
        {

            l = double.Parse(textBox1.Text);
            H = double.Parse(textBox2.Text);

            nu = double.Parse(textBox3.Text);
            r = double.Parse(textBox4.Text);

            //Проверка введенных параметров
            if ((r <= 0) || (l <= 0) || (H >= 0))
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0, l > 0, H < 0");
                //return;
            }

            // Шаг по времени
            dt = double.Parse(textBox15.Text);
            double P = int.Parse(textBox14.Text); // Выход по итерациям 

            // 
            t = new List<double>();
            n = new List<double>();
            c1 = new List<double>();
            c2 = new List<double>();
            c3 = new List<double>();
            c4 = new List<double>();
            c5 = new List<double>();
            c6 = new List<double>();
            y = new List<double>();
            z = new List<double>();

            //Н У 
            double t0 = 0;
            double n0 = double.Parse(textBox5.Text);
            double c10 = double.Parse(textBox6.Text);
            double c20 = double.Parse(textBox7.Text);
            double c30 = double.Parse(textBox8.Text);
            double c40 = double.Parse(textBox9.Text);
            double c50 = double.Parse(textBox10.Text);
            double c60 = double.Parse(textBox11.Text);
            double y0 = double.Parse(textBox12.Text);
            double z0 = double.Parse(textBox13.Text);

            t.Add(t0);
            n.Add(n0);
            c1.Add(c10);
            c2.Add(c20);
            c3.Add(c30);
            c4.Add(c40);
            c5.Add(c50);
            c6.Add(c60);
            y.Add(y0);
            z.Add(z0);



            //Метод Рунге-Кутты 4-го порядка 
            int p = 0; // счет итераций 

            while (p < P)
            {
                double[] K = new double[36];

                //K1
                K[0] = dt * N(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[1] = dt * C1(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[2] = dt * C2(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[3] = dt * C3(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[4] = dt * C4(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[5] = dt * C5(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[6] = dt * C6(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[7] = dt * Y(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[8] = dt * Z(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);


                //K2
                K[9] = dt * N(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[10] = dt * C1(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[11] = dt * C2(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[12] = dt * C3(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[13] = dt * C4(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[14] = dt * C5(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[15] = dt * C6(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[16] = dt * Y(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[17] = dt * Z(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);


                //K3
                K[18] = dt * N(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[19] = dt * C1(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[20] = dt * C2(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[21] = dt * C3(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[22] = dt * C4(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[23] = dt * C5(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[24] = dt * C6(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[25] = dt * Y(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[26] = dt * Z(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);

                //K4
                K[27] = dt * N(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[28] = dt * C1(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[29] = dt * C2(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[30] = dt * C3(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[31] = dt * C4(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[32] = dt * C5(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[33] = dt * C6(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[34] = dt * Y(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[35] = dt * Z(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);

                //Новые точки 
                double t1 = t0 + dt;
                double n1 = n0 + (K[0] + 2.0 * K[9] + 2.0 * K[18] + K[27]) / 6.0;
                double c11 = c10 + (K[1] + 2.0 * K[10] + 2.0 * K[19] + K[28]) / 6.0;
                double c21 = c20 + (K[2] + 2.0 * K[11] + 2.0 * K[20] + K[29]) / 6.0;
                double c31 = c30 + (K[3] + 2.0 * K[12] + 2.0 * K[21] + K[30]) / 6.0;
                double c41 = c40 + (K[4] + 2.0 * K[13] + 2.0 * K[22] + K[31]) / 6.0;
                double c51 = c50 + (K[5] + 2.0 * K[14] + 2.0 * K[23] + K[32]) / 6.0;
                double c61 = c60 + (K[6] + 2.0 * K[15] + 2.0 * K[24] + K[33]) / 6.0;
                double y1 = y0 + (K[7] + 2.0 * K[16] + 2.0 * K[25] + K[34]) / 6.0;
                double z1 = z0 + (K[8] + 2.0 * K[17] + 2.0 * K[26] + K[35]) / 6.0;

                t.Add(t1);
                n.Add(n1);
                c1.Add(c11);
                c2.Add(c21);
                c3.Add(c31);
                c4.Add(c41);
                c5.Add(c51);
                c6.Add(c61);
                y.Add(y1);
                z.Add(z1);

                t0 = t1;
                n0 = n1;
                c10 = c11;
                c20 = c21;
                c30 = c31;
                c40 = c41;
                c50 = c51;
                c60 = c61;
                y0 = y1;
                z0 = z1;

                p++;
            }



            //Table 
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.RowCount = t.Count + 1;
            dataGridView1.ColumnCount = 11;
            dataGridView1.Rows[0].Cells[0].Value = "№";
            dataGridView1.Rows[0].Cells[1].Value = "t";
            dataGridView1.Rows[0].Cells[2].Value = "n(t)";
            dataGridView1.Rows[0].Cells[3].Value = "c1(t)";
            dataGridView1.Rows[0].Cells[4].Value = "c2(t)";
            dataGridView1.Rows[0].Cells[5].Value = "c3(t)";
            dataGridView1.Rows[0].Cells[6].Value = "c4(t)";
            dataGridView1.Rows[0].Cells[7].Value = "c5(t)";
            dataGridView1.Rows[0].Cells[8].Value = "c6(t)";
            dataGridView1.Rows[0].Cells[9].Value = "y(t)";
            dataGridView1.Rows[0].Cells[10].Value = "z(t)";
            for (int i = 0; i < t.Count; i++)
            {
                dataGridView1.Rows[i + 1].Cells[0].Value = i + 1;
                dataGridView1.Rows[i + 1].Cells[1].Value = t[i];
                dataGridView1.Rows[i + 1].Cells[2].Value = n[i];
                dataGridView1.Rows[i + 1].Cells[3].Value = c1[i];
                dataGridView1.Rows[i + 1].Cells[4].Value = c2[i];
                dataGridView1.Rows[i + 1].Cells[5].Value = c3[i];
                dataGridView1.Rows[i + 1].Cells[6].Value = c4[i];
                dataGridView1.Rows[i + 1].Cells[7].Value = c5[i];
                dataGridView1.Rows[i + 1].Cells[8].Value = c6[i];
                dataGridView1.Rows[i + 1].Cells[9].Value = y[i];
                dataGridView1.Rows[i + 1].Cells[10].Value = z[i];

            }

            //Graphics
            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();

            PointPairList n_list = new PointPairList();
            PointPairList c1_list = new PointPairList();
            PointPairList c2_list = new PointPairList();
            PointPairList c3_list = new PointPairList();
            PointPairList c4_list = new PointPairList();
            PointPairList c5_list = new PointPairList();
            PointPairList c6_list = new PointPairList();
            PointPairList y_list = new PointPairList();
            PointPairList z_list = new PointPairList();

            // Устанавливаем интересующий нас интервал по оси X
            panel.XAxis.Scale.Min = t[0] - dt;
            panel.XAxis.Scale.Max = t[t.Count - 1] + dt;

            for (int i = 0; i < t.Count; i++)
            {
                n_list.Add(t[i], n[i]);
                c1_list.Add(t[i], c1[i]);
                c2_list.Add(t[i], c2[i]);
                c3_list.Add(t[i], c3[i]);
                c4_list.Add(t[i], c4[i]);
                c5_list.Add(t[i], c5[i]);
                c6_list.Add(t[i], c6[i]);
                y_list.Add(t[i], y[i]);
                z_list.Add(t[i], z[i]);

            }


            LineItem Curve1 = panel.AddCurve("n(t)", n_list, Color.Red, SymbolType.None);
            LineItem Curve2 = panel.AddCurve("c1(t)", c1_list, Color.Silver, SymbolType.None);
            LineItem Curve3 = panel.AddCurve("c2(t)", c2_list, Color.Orange, SymbolType.None);
            LineItem Curve4 = panel.AddCurve("c3(t)", c3_list, Color.DarkViolet, SymbolType.None);
            LineItem Curve5 = panel.AddCurve("c4(t)", c4_list, Color.Chocolate, SymbolType.None);
            LineItem Curve6 = panel.AddCurve("c5(t)", c5_list, Color.DarkTurquoise, SymbolType.None);
            LineItem Curve7 = panel.AddCurve("c6(t)", c6_list, Color.DeepPink, SymbolType.None);
            LineItem Curve8 = panel.AddCurve("y(t)", y_list, Color.DarkGreen, SymbolType.None);
            LineItem Curve9 = panel.AddCurve("z(t)", z_list, Color.DarkBlue, SymbolType.None);

            zedGraphControl1.AxisChange();
            // Обновляем график
            zedGraphControl1.Invalidate();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Очистка строк и столбцов таблицы
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            // Обновляем график
            GraphPane panel2 = zedGraphControl2.GraphPane;
            panel2.CurveList.Clear();
            zedGraphControl2.Invalidate();
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            
        }

        //Запуск Нелинейной задачи
        private void button3_Click(object sender, EventArgs e)
        {
            l = double.Parse(textBox16.Text);
            H = double.Parse(textBox17.Text);

            nu = double.Parse(textBox18.Text);
            r = double.Parse(textBox19.Text);

            //Проверка введенных параметров
            if ((r <= 0) || (l <= 0) || (H >= 0))
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0, l > 0, H < 0");
                //return;
            }

            // Шаг по времени
            dt = double.Parse(textBox21.Text);
            double P = int.Parse(textBox20.Text); // Выход по итерациям 

            // 
            t = new List<double>();
            n = new List<double>();
            c1 = new List<double>();
            c2 = new List<double>();
            c3 = new List<double>();
            c4 = new List<double>();
            c5 = new List<double>();
            c6 = new List<double>();
            y = new List<double>();
            z = new List<double>();

            //Н У 
            double t0 = 0;
            double n0 = double.Parse(textBox22.Text);
            double c10 = double.Parse(textBox23.Text);
            double c20 = double.Parse(textBox24.Text);
            double c30 = double.Parse(textBox25.Text);
            double c40 = double.Parse(textBox26.Text);
            double c50 = double.Parse(textBox27.Text);
            double c60 = double.Parse(textBox28.Text);
            double y0 = double.Parse(textBox29.Text);
            double z0 = double.Parse(textBox30.Text);

            t.Add(t0);
            n.Add(n0);
            c1.Add(c10);
            c2.Add(c20);
            c3.Add(c30);
            c4.Add(c40);
            c5.Add(c50);
            c6.Add(c60);
            y.Add(y0);
            z.Add(z0);



            //Метод Рунге-Кутты 4-го порядка 
            int p = 0; // счет итераций 

            while (p < P)
            {
                double[] K = new double[36];

                //K1
                K[0] = dt * NNL(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[1] = dt * C1(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[2] = dt * C2(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[3] = dt * C3(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[4] = dt * C4(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[5] = dt * C5(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[6] = dt * C6(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[7] = dt * Y(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);
                K[8] = dt * Z(t0, n0, c10, c20, c30, c40, c50, c60, y0, z0);


                //K2
                K[9] = dt * NNL(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[10] = dt * C1(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[11] = dt * C2(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[12] = dt * C3(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[13] = dt * C4(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[14] = dt * C5(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[15] = dt * C6(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[16] = dt * Y(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);
                K[17] = dt * Z(t0 + dt / 2.0, n0 + K[0] / 2.0, c10 + K[1] / 2.0, c20 + K[2] / 2.0, c30 + K[3] / 2.0, c40 + K[4] / 2.0, c50 + K[5] / 2.0, c60 + K[6] / 2.0, y0 + K[7] / 2.0, z0 + K[8] / 2.0);


                //K3
                K[18] = dt * NNL(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[19] = dt * C1(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[20] = dt * C2(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[21] = dt * C3(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[22] = dt * C4(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[23] = dt * C5(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[24] = dt * C6(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[25] = dt * Y(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);
                K[26] = dt * Z(t0 + dt / 2.0, n0 + K[9] / 2.0, c10 + K[10] / 2.0, c20 + K[11] / 2.0, c30 + K[12] / 2.0, c40 + K[13] / 2.0, c50 + K[14] / 2.0, c60 + K[15] / 2.0, y0 + K[16] / 2.0, z0 + K[17] / 2.0);

                //K4
                K[27] = dt * NNL(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[28] = dt * C1(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[29] = dt * C2(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[30] = dt * C3(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[31] = dt * C4(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[32] = dt * C5(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[33] = dt * C6(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[34] = dt * Y(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);
                K[35] = dt * Z(t0 + dt, n0 + K[18], c10 + K[19], c20 + K[20], c30 + K[21], c40 + K[22], c50 + K[23], c60 + K[24], y0 + K[25], z0 + K[26]);

                //Новые точки 
                double t1 = t0 + dt;
                double n1 = n0 + (K[0] + 2.0 * K[9] + 2.0 * K[18] + K[27]) / 6.0;
                double c11 = c10 + (K[1] + 2.0 * K[10] + 2.0 * K[19] + K[28]) / 6.0;
                double c21 = c20 + (K[2] + 2.0 * K[11] + 2.0 * K[20] + K[29]) / 6.0;
                double c31 = c30 + (K[3] + 2.0 * K[12] + 2.0 * K[21] + K[30]) / 6.0;
                double c41 = c40 + (K[4] + 2.0 * K[13] + 2.0 * K[22] + K[31]) / 6.0;
                double c51 = c50 + (K[5] + 2.0 * K[14] + 2.0 * K[23] + K[32]) / 6.0;
                double c61 = c60 + (K[6] + 2.0 * K[15] + 2.0 * K[24] + K[33]) / 6.0;
                double y1 = y0 + (K[7] + 2.0 * K[16] + 2.0 * K[25] + K[34]) / 6.0;
                double z1 = z0 + (K[8] + 2.0 * K[17] + 2.0 * K[26] + K[35]) / 6.0;

                t.Add(t1);
                n.Add(n1);
                c1.Add(c11);
                c2.Add(c21);
                c3.Add(c31);
                c4.Add(c41);
                c5.Add(c51);
                c6.Add(c61);
                y.Add(y1);
                z.Add(z1);

                t0 = t1;
                n0 = n1;
                c10 = c11;
                c20 = c21;
                c30 = c31;
                c40 = c41;
                c50 = c51;
                c60 = c61;
                y0 = y1;
                z0 = z1;

                p++;
            }



            //Table 
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.RowCount = t.Count + 1;
            dataGridView2.ColumnCount = 11;
            dataGridView2.Rows[0].Cells[0].Value = "№";
            dataGridView2.Rows[0].Cells[1].Value = "t";
            dataGridView2.Rows[0].Cells[2].Value = "n(t)";
            dataGridView2.Rows[0].Cells[3].Value = "c1(t)";
            dataGridView2.Rows[0].Cells[4].Value = "c2(t)";
            dataGridView2.Rows[0].Cells[5].Value = "c3(t)";
            dataGridView2.Rows[0].Cells[6].Value = "c4(t)";
            dataGridView2.Rows[0].Cells[7].Value = "c5(t)";
            dataGridView2.Rows[0].Cells[8].Value = "c6(t)";
            dataGridView2.Rows[0].Cells[9].Value = "y(t)";
            dataGridView2.Rows[0].Cells[10].Value = "z(t)";
            for (int i = 0; i < t.Count; i++)
            {
                dataGridView2.Rows[i + 1].Cells[0].Value = i + 1;
                dataGridView2.Rows[i + 1].Cells[1].Value = t[i];
                dataGridView2.Rows[i + 1].Cells[2].Value = n[i];
                dataGridView2.Rows[i + 1].Cells[3].Value = c1[i];
                dataGridView2.Rows[i + 1].Cells[4].Value = c2[i];
                dataGridView2.Rows[i + 1].Cells[5].Value = c3[i];
                dataGridView2.Rows[i + 1].Cells[6].Value = c4[i];
                dataGridView2.Rows[i + 1].Cells[7].Value = c5[i];
                dataGridView2.Rows[i + 1].Cells[8].Value = c6[i];
                dataGridView2.Rows[i + 1].Cells[9].Value = y[i];
                dataGridView2.Rows[i + 1].Cells[10].Value = z[i];

            }

            //Graphics
            GraphPane panel2 = zedGraphControl2.GraphPane;
            panel2.CurveList.Clear();

            PointPairList n_list = new PointPairList();
            PointPairList c1_list = new PointPairList();
            PointPairList c2_list = new PointPairList();
            PointPairList c3_list = new PointPairList();
            PointPairList c4_list = new PointPairList();
            PointPairList c5_list = new PointPairList();
            PointPairList c6_list = new PointPairList();
            PointPairList y_list = new PointPairList();
            PointPairList z_list = new PointPairList();

            // Устанавливаем интересующий нас интервал по оси X
            panel2.XAxis.Scale.Min = t[0] - dt;
            panel2.XAxis.Scale.Max = t[t.Count - 1] + dt;

            for (int i = 0; i < t.Count; i++)
            {
                n_list.Add(t[i], n[i]);
                c1_list.Add(t[i], c1[i]);
                c2_list.Add(t[i], c2[i]);
                c3_list.Add(t[i], c3[i]);
                c4_list.Add(t[i], c4[i]);
                c5_list.Add(t[i], c5[i]);
                c6_list.Add(t[i], c6[i]);
                y_list.Add(t[i], y[i]);
                z_list.Add(t[i], z[i]);

            }


            LineItem Curve1 = panel2.AddCurve("n(t)", n_list, Color.Red, SymbolType.None);
            LineItem Curve2 = panel2.AddCurve("c1(t)", c1_list, Color.Silver, SymbolType.None);
            LineItem Curve3 = panel2.AddCurve("c2(t)", c2_list, Color.Orange, SymbolType.None);
            LineItem Curve4 = panel2.AddCurve("c3(t)", c3_list, Color.DarkViolet, SymbolType.None);
            LineItem Curve5 = panel2.AddCurve("c4(t)", c4_list, Color.Chocolate, SymbolType.None);
            LineItem Curve6 = panel2.AddCurve("c5(t)", c5_list, Color.DarkTurquoise, SymbolType.None);
            LineItem Curve7 = panel2.AddCurve("c6(t)", c6_list, Color.DeepPink, SymbolType.None);
            LineItem Curve8 = panel2.AddCurve("y(t)", y_list, Color.DarkGreen, SymbolType.None);
            LineItem Curve9 = panel2.AddCurve("z(t)", z_list, Color.DarkBlue, SymbolType.None);

            zedGraphControl2.AxisChange();
            // Обновляем график
            zedGraphControl2.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Очистка строк и столбцов таблицы
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Обновляем график
            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();
            zedGraphControl1.Invalidate();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        //Вычисление di и Построение S 
        //Start
        private void button5_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            
            l = double.Parse(textBox34.Text);
            H = double.Parse(textBox33.Text);

            nu = double.Parse(textBox31.Text);
            r = double.Parse(textBox32.Text);

            //Проверка введенных параметров
            if ((r <= 0) || (l <= 0) || (H >= 0))
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0, l > 0, H < 0");
                //return;
            }

            // (B - rI)^T * d = nu *b - решаем эту СЛУ, где d - неизвестное
            
            //Матрица, отвечающая за правую часть
            double[] b = new double[9];
            b[0] = double.Parse(textBox35.Text);
            b[1] = double.Parse(textBox36.Text);
            b[2] = double.Parse(textBox37.Text);
            b[3] = double.Parse(textBox38.Text);
            b[4] = double.Parse(textBox39.Text);
            b[5] = double.Parse(textBox40.Text);
            b[6] = double.Parse(textBox41.Text);
            b[7] = double.Parse(textBox42.Text);
            b[8] = double.Parse(textBox43.Text);

            //Правая часть nu*b
            for (int i = 0; i < 9; i++)
            {
                b[i] = b[i] * nu;
            }

            // сформируем матрицу (B - rI)^T
            double[,] BrI = new double[9, 9];
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    BrI[0, 0] = 0;
                }
            }

            BrI[0, 0] = (-1.0) * (1 / l) * (b1 + b2 + b3 + b4 + b5 + b6);
            BrI[0, 1] = a1 / l;
            BrI[0, 2] = a2 / l;
            BrI[0, 3] = a3 / l;
            BrI[0, 4] = a4 / l;
            BrI[0, 5] = a5 / l;
            BrI[0, 6] = a6 / l;
            BrI[0, 8] = -1 / l;

            BrI[1, 0] = b1;
            BrI[2, 0] = b2;
            BrI[3, 0] = b3;
            BrI[4, 0] = b4;
            BrI[5, 0] = b5;
            BrI[6, 0] = b6;
            BrI[7, 0] = 1;

            BrI[1, 1] = -a1;
            BrI[2, 2] = -a2;
            BrI[3, 3] = -a3;
            BrI[4, 4] = -a4;
            BrI[5, 5] = -a5;
            BrI[6, 6] = -a6;

            
            for (int i = 0; i < 9; i ++)
                BrI[i, i] = BrI[i, i] - r;


            //Транспонируем эту матрицу 
            for (int i = 0; i < 9; i++)
            {
                for (int j = i + 1; j < 9; j++)
                {
                    if (i == j)
                        continue;
                    double temp = BrI[i, j];
                    BrI[i, j] = BrI[j, i];
                    BrI[j, i] = temp;
                }
            }

            

            //Метод Гаусса 
            double[] Di = new double[9];
            for (int i = 0; i < 9; i++)
            {
                Di[i] = 0;
            }
            double Multi1, Multi2;

                    for (int k = 0; k < 9; k++)
                    {
                        for (int j = k + 1; j < 9; j++)
                        {
                            Multi1 = BrI[j, k] / BrI[k, k];
                            for (int i = k; i < 9; i++)
                            {
                                BrI[j, i] = BrI[j, i] - Multi1 * BrI[k, i];
                            }
                            b[j] = b[j] - Multi1 * b[k];
                        }
                    }

                    
                    for (int k = 8; k >= 0; k--)
                    {
                        Multi1 = 0;
                        for (int j = k; j < 9; j++)
                        {
                            Multi2 = BrI[k, j] * Di[j];
                            Multi1 += Multi2;
                        }
                        Di[k] = (b[k] - Multi1) / BrI[k, k];
                    }
            

            //Вывод результатов в таблицу
            //Table 
            dataGridView3.RowCount = 9;
            dataGridView3.ColumnCount = 2;
            dataGridView3.Rows[0].Cells[0].Value = "d1";
            dataGridView3.Rows[1].Cells[0].Value = "d2";
            dataGridView3.Rows[2].Cells[0].Value = "d3";
            dataGridView3.Rows[3].Cells[0].Value = "d4";
            dataGridView3.Rows[4].Cells[0].Value = "d5";
            dataGridView3.Rows[5].Cells[0].Value = "d6";
            dataGridView3.Rows[6].Cells[0].Value = "d7";
            dataGridView3.Rows[7].Cells[0].Value = "d8";
            dataGridView3.Rows[8].Cells[0].Value = "d9";

            for (int i = 0; i < 9; i++)
            {
                dataGridView3.Rows[i].Cells[1].Value = Di[i];
            }

            dataGridView4.RowCount = 9;
            dataGridView4.ColumnCount = 9;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView4.Rows[i].Cells[j].Value = BrI[i, j];
                }
            }


            //Graphics
            GraphPane panel3 = zedGraphControl3.GraphPane;
            panel3.CurveList.Clear();

            PointPairList n_list = new PointPairList();
            //PointPairList S1_list = new PointPairList(); // По формуле
            PointPairList S2_list = new PointPairList(); // Через di
            PointPairList z_list = new PointPairList();

            // Устанавливаем интересующий нас интервал по оси X
            panel3.XAxis.Scale.Min = t[0] - dt;
            panel3.XAxis.Scale.Max = t[t.Count - 1] + dt;

            for (int i = 0; i < t.Count; i++)
            {
                n_list.Add(t[i], n[i]);
                //S1_list.Add(t[i], S(n[i], c1[i], c2[i], c3[i], c4[i], c5[i], c6[i], y[i], z[i]));
                S2_list.Add(t[i], Di[0] * n[i] + Di[1] * c1[i] + Di[2] * c2[i] + Di[3] * c3[i]
                    + Di[4] * c4[i] + Di[5] * c5[i] + Di[6] * c6[i] + Di[7] * y[i] + Di[8] * z[i]);
                z_list.Add(t[i], z[i]);

            }


            LineItem Curve1 = panel3.AddCurve("n(t)", n_list, Color.DarkGreen, SymbolType.None);
            LineItem Curve2 = panel3.AddCurve("z(t)", z_list, Color.Orange, SymbolType.Star);
            //LineItem Curve3 = panel3.AddCurve("S1(t)", S1_list, Color.Red, SymbolType.None);
            LineItem Curve4 = panel3.AddCurve("S(t)", S2_list, Color.Red, SymbolType.None);
            

            zedGraphControl3.AxisChange();
            // Обновляем график
            zedGraphControl3.Invalidate();

        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox41_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
