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

namespace Reactor1
{
    public partial class Form1 : Form
    {

        //Параметры 
        double l;
        double a;//лямбда
        double b;//бетта
        double H;
        double r;
        double nu;//ню

        double dt; // Шаг по времени 

        List<double> t = new List<double>();
        List<double> n = new List<double>();
        List<double> c = new List<double>();
        List<double> y = new List<double>();
        List<double> z = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        double S(double n, double c, double y, double z)
        {
            double q = b / (l * (a + r)) + 1;

            return (nu / r) * ( -n / (r * q)  -  (a * c) / (l * r * q * (a + r))  - y  + z / (l * q * r * r));
        }

        double N(double t, double n, double c, double y, double z) // Для линейной системы
        {
            return (1 / l) * (-b * n + c * a - z);
        }
        double NNL(double t, double n, double c, double y, double z) // Для нелинейной системы
        {
            return (1 / l) * (-b * n + c * a - z * (1 + n));
        }
        double C(double t, double n, double c, double y, double z)
        {
            return (b * n - c * a );
        }
        double Y(double t, double n, double c, double y, double z)
        {
            return n;
        }
        double Z(double t, double n, double c, double y, double z)
        {
            return H * y * Math.Sign(y * S(n, c, y, z));
        }

        //Для линейной системы
        void Lin()
        {
            //Parametrs 
            a = double.Parse(textBox2.Text);
            b = double.Parse(textBox1.Text);
            l = double.Parse(textBox3.Text);
            H = double.Parse(textBox4.Text);

            nu = double.Parse(textBox6.Text);
            r = double.Parse(textBox7.Text);

            //Проверка введенных параметров
            if ((r <= 0) || (l <= 0) || (H >= 0))
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0, l > 0, H < 0");
                //return;
            }

            // Шаг по времени
            dt = double.Parse(textBox5.Text);
            double P = int.Parse(textBox8.Text); // Выход по итерациям 


            // 
             t = new List<double>();
             n = new List<double>();
             c = new List<double>();
             y = new List<double>();
             z = new List<double>();

            

            //Н У 
            double t0 = 0;
            double n0 = double.Parse(textBox10.Text);
            double c0 = double.Parse(textBox11.Text);
            double y0 = double.Parse(textBox12.Text);
            double z0 = double.Parse(textBox13.Text);

            t.Add(t0);
            n.Add(n0);
            c.Add(c0);
            y.Add(y0);
            z.Add(z0);



            //Метод Рунге-Кутты 4-го порядка 
            int p = 0; // счет итераций 

            while (p < P)
            {
                double[] K = new double[16];

                //K1
                K[0] = dt * N(t0, n0, c0, y0, z0);
                K[1] = dt * C(t0, n0, c0, y0, z0);
                K[2] = dt * Y(t0, n0, c0, y0, z0);
                K[3] = dt * Z(t0, n0, c0, y0, z0);

                //K2
                K[4] = dt * N(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);
                K[5] = dt * C(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);
                K[6] = dt * Y(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);
                K[7] = dt * Z(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);

                //K3
                K[8] = dt * N(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);
                K[9] = dt * C(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);
                K[10] = dt * Y(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);
                K[11] = dt * Z(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);

                //K4
                K[12] = dt * N(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);
                K[13] = dt * C(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);
                K[14] = dt * Y(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);
                K[15] = dt * Z(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);

                //Новые точки 
                double t1 = t0 + dt;
                double n1 = n0 + (K[0] + 2.0 * K[4] + 2.0 * K[8] + K[12]) / 6.0;
                double c1 = c0 + (K[1] + 2.0 * K[5] + 2.0 * K[9] + K[13]) / 6.0;
                double y1 = y0 + (K[2] + 2.0 * K[6] + 2.0 * K[10] + K[14]) / 6.0;
                double z1 = z0 + (K[3] + 2.0 * K[7] + 2.0 * K[11] + K[15]) / 6.0;

                t.Add(t1);
                n.Add(n1);
                c.Add(c1);
                y.Add(y1);
                z.Add(z1);

                t0 = t1;
                n0 = n1;
                c0 = c1;
                y0 = y1;
                z0 = z1;

                p++;
            }



            //Table 
            //Очистка строк и столбцов таблицы
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //Заполнение
            dataGridView1.RowCount = t.Count + 1;
            dataGridView1.ColumnCount = 6;
            dataGridView1.Rows[0].Cells[0].Value = "№";
            dataGridView1.Rows[0].Cells[1].Value = "t";
            dataGridView1.Rows[0].Cells[2].Value = "n(t)";
            dataGridView1.Rows[0].Cells[3].Value = "c(t)";
            dataGridView1.Rows[0].Cells[4].Value = "y(t)";
            dataGridView1.Rows[0].Cells[5].Value = "z(t)";
            for (int i = 0; i < t.Count; i++)
            {
                dataGridView1.Rows[i + 1].Cells[0].Value = i + 1;
                dataGridView1.Rows[i + 1].Cells[1].Value = t[i];
                dataGridView1.Rows[i + 1].Cells[2].Value = n[i];
                dataGridView1.Rows[i + 1].Cells[3].Value = c[i];
                dataGridView1.Rows[i + 1].Cells[4].Value = y[i];
                dataGridView1.Rows[i + 1].Cells[5].Value = z[i];

            }

            //Graphics
            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();

            PointPairList n_list = new PointPairList();
            PointPairList c_list = new PointPairList();
            PointPairList y_list = new PointPairList();
            PointPairList z_list = new PointPairList();

            // Устанавливаем интересующий нас интервал по оси X
            panel.XAxis.Scale.Min = t[0] - dt;
            panel.XAxis.Scale.Max = t[t.Count - 1] + dt;

            for (int i = 0; i < t.Count; i++)
            {
                n_list.Add(t[i], n[i]);
                c_list.Add(t[i], c[i]);
                y_list.Add(t[i], y[i]);
                z_list.Add(t[i], z[i]);

            }


            LineItem Curve1 = panel.AddCurve("n(t)", n_list, Color.Red, SymbolType.None);
            LineItem Curve2 = panel.AddCurve("c(t)", c_list, Color.DarkBlue, SymbolType.None);
            LineItem Curve3 = panel.AddCurve("y(t)", y_list, Color.DarkGreen, SymbolType.None);
            LineItem Curve4 = panel.AddCurve("z(t)", z_list, Color.DarkOrange, SymbolType.None);

            zedGraphControl1.AxisChange();
            // Обновляем график
            zedGraphControl1.Invalidate();
        }

        //Для нелинейной системы
        void NeLin()
        {
            //Parametrs 
            a = double.Parse(textBox2.Text);
            b = double.Parse(textBox1.Text);
            l = double.Parse(textBox3.Text);
            H = double.Parse(textBox4.Text);

            nu = double.Parse(textBox6.Text);
            r = double.Parse(textBox7.Text);

            //Проверка введенных параметров
            if ((r <= 0) || (l <= 0) || (H >= 0))
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0, l > 0, H < 0");
                //return;
            }

            // Шаг по времени
            dt = double.Parse(textBox5.Text);
            double P = int.Parse(textBox8.Text); // Выход по итерациям 

            
            // 
            t = new List<double>();
            n = new List<double>();
            c = new List<double>();
            y = new List<double>();
            z = new List<double>();

            //Н У 
            double t0 = 0;
            double n0 = double.Parse(textBox10.Text);
            double c0 = double.Parse(textBox11.Text);
            double y0 = double.Parse(textBox12.Text);
            double z0 = double.Parse(textBox13.Text);

            t.Add(t0);
            n.Add(n0);
            c.Add(c0);
            y.Add(y0);
            z.Add(z0);



            //Метод Рунге-Кутты 4-го порядка 
            int p = 0; // счет итераций 

            while (p < P)
            {
                double[] K = new double[16];

                //K1
                K[0] = dt * NNL(t0, n0, c0, y0, z0);
                K[1] = dt * C(t0, n0, c0, y0, z0);
                K[2] = dt * Y(t0, n0, c0, y0, z0);
                K[3] = dt * Z(t0, n0, c0, y0, z0);

                //K2
                K[4] = dt * NNL(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);
                K[5] = dt * C(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);
                K[6] = dt * Y(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);
                K[7] = dt * Z(t0 + dt / 2.0, n0 + K[0] / 2.0, c0 + K[1] / 2.0, y0 + K[2] / 2.0, z0 + K[3] / 2.0);

                //K3
                K[8] = dt * NNL(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);
                K[9] = dt * C(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);
                K[10] = dt * Y(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);
                K[11] = dt * Z(t0 + dt / 2.0, n0 + K[4] / 2.0, c0 + K[5] / 2.0, y0 + K[6] / 2.0, z0 + K[7] / 2.0);

                //K4
                K[12] = dt * NNL(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);
                K[13] = dt * C(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);
                K[14] = dt * Y(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);
                K[15] = dt * Z(t0 + dt, n0 + K[8], c0 + K[9], y0 + K[10], z0 + K[11]);

                //Новые точки 
                double t1 = t0 + dt;
                double n1 = n0 + (K[0] + 2.0 * K[4] + 2.0 * K[8] + K[12]) / 6.0;
                double c1 = c0 + (K[1] + 2.0 * K[5] + 2.0 * K[9] + K[13]) / 6.0;
                double y1 = y0 + (K[2] + 2.0 * K[6] + 2.0 * K[10] + K[14]) / 6.0;
                double z1 = z0 + (K[3] + 2.0 * K[7] + 2.0 * K[11] + K[15]) / 6.0;

                t.Add(t1);
                n.Add(n1);
                c.Add(c1);
                y.Add(y1);
                z.Add(z1);

                t0 = t1;
                n0 = n1;
                c0 = c1;
                y0 = y1;
                z0 = z1;

                p++;
            }



            //Table 
            //Очистка строк и столбцов таблицы
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //Заполнение
            dataGridView1.RowCount = t.Count + 1;
            dataGridView1.ColumnCount = 6;
            dataGridView1.Rows[0].Cells[0].Value = "№";
            dataGridView1.Rows[0].Cells[1].Value = "t";
            dataGridView1.Rows[0].Cells[2].Value = "n(t)";
            dataGridView1.Rows[0].Cells[3].Value = "c(t)";
            dataGridView1.Rows[0].Cells[4].Value = "y(t)";
            dataGridView1.Rows[0].Cells[5].Value = "z(t)";
            for (int i = 0; i < t.Count; i++)
            {
                dataGridView1.Rows[i + 1].Cells[0].Value = i + 1;
                dataGridView1.Rows[i + 1].Cells[1].Value = t[i];
                dataGridView1.Rows[i + 1].Cells[2].Value = n[i];
                dataGridView1.Rows[i + 1].Cells[3].Value = c[i];
                dataGridView1.Rows[i + 1].Cells[4].Value = y[i];
                dataGridView1.Rows[i + 1].Cells[5].Value = z[i];

            }

            //Graphics
            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();

            PointPairList n_list = new PointPairList();
            PointPairList c_list = new PointPairList();
            PointPairList y_list = new PointPairList();
            PointPairList z_list = new PointPairList();

            // Устанавливаем интересующий нас интервал по оси X
            panel.XAxis.Scale.Min = t[0] - dt;
            panel.XAxis.Scale.Max = t[t.Count - 1] + dt;

            for (int i = 0; i < t.Count; i++)
            {
                n_list.Add(t[i], n[i]);
                c_list.Add(t[i], c[i]);
                y_list.Add(t[i], y[i]);
                z_list.Add(t[i], z[i]);

            }


            LineItem Curve1 = panel.AddCurve("n(t)", n_list, Color.Red, SymbolType.None);
            LineItem Curve2 = panel.AddCurve("c(t)", c_list, Color.DarkBlue, SymbolType.None);
            LineItem Curve3 = panel.AddCurve("y(t)", y_list, Color.DarkGreen, SymbolType.None);
            LineItem Curve4 = panel.AddCurve("z(t)", z_list, Color.DarkOrange, SymbolType.None);

            zedGraphControl1.AxisChange();
            // Обновляем график
            zedGraphControl1.Invalidate();
        }

        //Построение S и подсчет di методом Гаусса 
        void Build_S()
        {
            // (B - rI)^T * d = nu *b - решаем эту СЛУ, где d - неизвестное

            //Матрица, отвечающая за правую часть
            double[] B = new double[4];
            B[0] = 0;
            B[1] = 0;
            B[2] = 1;
            B[3] = 0;

            //Правая часть nu*b
            for (int i = 0; i < 4; i++)
            {
                B[i] = B[i] * nu;
            }

            // сформируем матрицу (B - rI)^T
            double[,] BrI = new double[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    BrI[0, 0] = 0;
                }
            }

            BrI[0, 0] = (-1.0) * (1 / l) * b;
            BrI[0, 1] = a / l;
            BrI[0, 3] = -1 / l;

            BrI[1, 0] = b;
            BrI[2, 0] = 1;

            BrI[1, 1] = -a;


            for (int i = 0; i < 4; i++)
                BrI[i, i] = BrI[i, i] - r;


            //Транспонируем эту матрицу 
            for (int i = 0; i < 4; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    if (i == j)
                        continue;
                    double temp = BrI[i, j];
                    BrI[i, j] = BrI[j, i];
                    BrI[j, i] = temp;
                }
            }



            //Метод Гаусса 
            double[] Di = new double[4];
            for (int i = 0; i < 4; i++)
            {
                Di[i] = 0;
            }
            double Multi1, Multi2;

            for (int k = 0; k < 4; k++)
            {
                for (int j = k + 1; j < 4; j++)
                {
                    Multi1 = BrI[j, k] / BrI[k, k];
                    for (int i = k; i < 4; i++)
                    {
                        BrI[j, i] = BrI[j, i] - Multi1 * BrI[k, i];
                    }
                    B[j] = B[j] - Multi1 * B[k];
                }
            }


            for (int k = 3; k >= 0; k--)
            {
                Multi1 = 0;
                for (int j = k; j < 4; j++)
                {
                    Multi2 = BrI[k, j] * Di[j];
                    Multi1 += Multi2;
                }
                Di[k] = (B[k] - Multi1) / BrI[k, k];
            }


            //Вывод результатов в таблицу
            //Table 
            //Очистка строк и столбцов таблицы
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //
            dataGridView1.RowCount = 4;
            dataGridView1.ColumnCount = 2;
            dataGridView1.Rows[0].Cells[0].Value = "d1";
            dataGridView1.Rows[1].Cells[0].Value = "d2";
            dataGridView1.Rows[2].Cells[0].Value = "d3";
            dataGridView1.Rows[3].Cells[0].Value = "d4";

            for (int i = 0; i < 4; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = Di[i];
            }


            //Строим S по формуле и по di (S = d1 * n + d2 * c1 + d3 * y + d4 * z)
            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();

            PointPairList n_list = new PointPairList();
            //PointPairList S1_list = new PointPairList(); // формула 
            PointPairList S2_list = new PointPairList(); //  через di
            PointPairList z_list = new PointPairList();

            // Устанавливаем интересующий нас интервал по оси X
            panel.XAxis.Scale.Min = t[0] - dt;
            panel.XAxis.Scale.Max = t[t.Count - 1] + dt;

            for (int i = 0; i < t.Count; i++)
            {
                n_list.Add(t[i], n[i]);
                //S1_list.Add(t[i], S(n[i], c[i], y[i], z[i]));
                S2_list.Add(t[i], Di[0] * n[i] + Di[1] * c[i] + Di[2] * y[i] + Di[3] * z[i]);
                z_list.Add(t[i], z[i]);

            }


            LineItem Curve1 = panel.AddCurve("n(t)", n_list, Color.DarkGreen, SymbolType.None);
            LineItem Curve2 = panel.AddCurve("z(t)", z_list, Color.Orange, SymbolType.Star);
            LineItem Curve3 = panel.AddCurve("S(t)", S2_list, Color.Red, SymbolType.None);
            //LineItem Curve4 = panel.AddCurve("S1(t)", S1_list, Color.Pink, SymbolType.Star);

            zedGraphControl1.AxisChange();
            // Обновляем график
            zedGraphControl1.Invalidate();


            //Построим по методу Гаусса di и построим по ним S

            //Проверка введенных параметров
            if ((r <= 0) || (l <= 0) || (H >= 0))
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0, l > 0, H < 0");
                //return;
            }



            
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                Lin();
            }
            if (radioButton2.Checked == true)
            {
                NeLin();
            }
            if (checkBox1.Checked == true)
            {
                Build_S();
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
