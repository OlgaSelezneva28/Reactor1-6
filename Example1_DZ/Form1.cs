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

namespace Example1_DZ
{
    public partial class Form1 : Form
    {

        double dt; // Шаг по времени 
        double P;  // Выход по итерациям


        double nu = 1;
        double r = 1;

        double a, b, E; 

        List<double> t;
        List<double> x;
        List<double> y;

        List<MyPoint> ListPointNY;

        public Form1()
        {
            InitializeComponent();

            ListInitialize();
        }

        double X(double x, double y)
        {
            return y; 
        }

        double YL(double x, double y)
        {
            return 7 * x - 3 * y;
        }

        double YNL(double x, double y)
        {
            return 7 * x - 3 * y - 12 * Math.Abs(x) * Math.Sign(4 * x + y);
        }

        double YU(double x, double y)
        {
            if (Math.Abs( a * x + b * y) > E)
                return 7 * x - 3 * y - 12 * Math.Abs(x) * Math.Sign(4 * x + y);
            else
                return 7 * x - 3 * y;
 
        }

        double YG(double x, double y)
        {
            
            if (7 * x + y  <= 0) //s' <= 0 
            {
                if (a * x + b * y > -E) // s > -E
                    return 7 * x - 3 * y - 12 * Math.Abs(x);
                if (a * x + b * y <= -E) // s < -E
                    return 7 * x - 3 * y + 12 * Math.Abs(x);
            }
            else //s' > 0
            {
                if (a * x + b * y <= E) // s <= E
                    return 7 * x - 3 * y + 12 * Math.Abs(x);
                if (a * x + b * y > E) // s > E
                    return 7 * x - 3 * y - 12 * Math.Abs(x);
            }

            return 0; 
        }

        double Y(double x, double y)
        {
            if ((radioButton1.Checked == true) && (radioButton2.Checked == false) && (radioButton3.Checked == false) && (radioButton4.Checked == false))
            {
                return YL(x, y);
            }
            if ((radioButton1.Checked == false) && (radioButton2.Checked == true) && (radioButton3.Checked == false) && (radioButton4.Checked == false))
            {
                return YNL(x, y);
            }
            if ((radioButton1.Checked == false) && (radioButton2.Checked == false) && (radioButton3.Checked == true) && (radioButton4.Checked == false))
            {
                return YU(x, y);
            }
            if ((radioButton1.Checked == false) && (radioButton2.Checked == false) && (radioButton3.Checked == false) && (radioButton4.Checked == true))
            {
                return YG(x, y);
            }

            return 0;
        }

        double S(double x, double y)
        {
            return 4 * x + y; 
        }

        double XS(double x, double y)
        {
            return (4 * x + y) * x;
        }

        //Метод Рунге-Кутты 4-го порядка 
        public void RK_4(double t0, double x0, double y0)
        {

            // 
            t = new List<double>();
            x = new List<double>();
            y = new List<double>();


            t.Add(t0);
            x.Add(x0);
            y.Add(y0);

            int p = 0; // счет итераций 

            while (p < P)
            {

                double[] K = new double[8];

                //K1
                K[0] = dt * X(x0, y0);
                K[1] = dt * Y(x0, y0);

                //K2
                K[2] = dt * X(x0 + K[0] / 2.0, y0 + K[1] / 2.0);
                K[3] = dt * Y(x0 + K[0] / 2.0, y0 + K[1] / 2.0);

                //K3
                K[4] = dt * X(x0 + K[2] / 2.0, y0 + K[3] / 2.0);
                K[5] = dt * Y(x0 + K[2] / 2.0, y0 + K[3] / 2.0);

                //K4
                K[6] = dt * X(x0 + K[4], y0 + K[5]);
                K[7] = dt * Y(x0 + K[4], y0 + K[5]);

                //Новые точки 
                double t1 = t0 + dt;
                double x1 = x0 + (K[0] + 2.0 * K[2] + 2.0 * K[4] + K[6]) / 6.0;
                double y1 = y0 + (K[1] + 2.0 * K[3] + 2.0 * K[5] + K[7]) / 6.0;

                t.Add(t1);
                x.Add(x1);
                y.Add(y1);

                t0 = t1;
                x0 = x1;
                y0 = y1;

                p++;
            }

        }

        //Запуск обычного примера
        private void button1_Click(object sender, EventArgs e)
        {
            // Шаг по времени
            dt = double.Parse(textBox1.Text);
            P = int.Parse(textBox2.Text);

            radioButton3.Checked = false;
            radioButton4.Checked = false;

             t = new List<double>();
             x = new List<double>();
             y = new List<double>();

            //Проверка введенных параметров
            if (r <= 0)
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0");
                //return;
            }

            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();

            //Н У 
            double t0 = -2;


            List<PointPairList> Yx_list = new List<PointPairList>();

            for (int k = 0; k < ListPointNY.Count; k++)
            {

                RK_4(t0, ListPointNY[k].x, ListPointNY[k].y);

                //Graphics
                PointPairList list = new PointPairList();
                for (int i = 0; i < t.Count; i++)
                {
                    list.Add(x[i], y[i]);
                }
                Yx_list.Add(list);

               
                //Line
                LineItem Curve1 = panel.AddCurve("", Yx_list[k], Color.Red, SymbolType.None);

            }

            DrawS(ListPointNY[0].x, panel);
            //DrawSep(ListPointNY[0].x , panel);
            
            panel.XAxis.Scale.Min = -5; 
            panel.XAxis.Scale.Max = 5;
            panel.YAxis.Scale.Min = -10;
            panel.YAxis.Scale.Max = 10;
            
            zedGraphControl1.AxisChange();
            // Обновляем график
            zedGraphControl1.Invalidate();
        }
        //Запуск примера с мертвой зоной
        private void button2_Click(object sender, EventArgs e)
        {
            dt = double.Parse(textBox3.Text);
            P = int.Parse(textBox4.Text);

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = true;
            radioButton4.Checked = false;

            a = double.Parse(textBox5.Text);
            b = double.Parse(textBox6.Text);
            E = double.Parse(textBox7.Text);

            t = new List<double>();
            x = new List<double>();
            y = new List<double>();

            //Проверка введенных параметров
            if (r <= 0)
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0");
                //return;
            }

            GraphPane panel = zedGraphControl2.GraphPane;
            panel.CurveList.Clear();
            //
            GraphPane panelOsc = zedGraphControl3.GraphPane;
            panelOsc.CurveList.Clear();

            //Н У 
            double t0 = -2;


            List<PointPairList> Yx_list = new List<PointPairList>();

            for (int k = 0; k < ListPointNY.Count; k++)
            {

                RK_4(t0, ListPointNY[k].x, ListPointNY[k].y);

                //Graphics
                PointPairList list = new PointPairList();
                PointPairList listX = new PointPairList();
                PointPairList listY = new PointPairList();
                PointPairList listS = new PointPairList();

                for (int i = 0; i < t.Count; i++)
                {
                    list.Add(x[i], y[i]);
                    listX.Add(t[i], x[i]);
                    listY.Add(t[i], y[i]);
                    listS.Add(t[i], 4 * x[i] + y[i]);
                }
                Yx_list.Add(list);

                
                //Line
                LineItem Curve1 = panel.AddCurve("", Yx_list[k], Color.Red, SymbolType.None);
                //Osc
                LineItem CurveO1 = panelOsc.AddCurve("", listX, Color.Red, SymbolType.None);
                LineItem CurveO2 = panelOsc.AddCurve("", listY, Color.Blue, SymbolType.None);
                LineItem CurveOS = panelOsc.AddCurve("", listS, Color.Green, SymbolType.None);
            }
            LineItem CurveO11 = panelOsc.AddCurve("x(t)", new PointPairList(), Color.Red, SymbolType.None);
            LineItem CurveO21 = panelOsc.AddCurve("y(t)", new PointPairList(), Color.Blue, SymbolType.None);
            LineItem CurveOSS = panelOsc.AddCurve("s(t)", new PointPairList(), Color.Green, SymbolType.None);

            DrawS(-5, panel);
            DrawS_plusE(-5, panel);
            DrawS_minusE(-5, panel);
            
            panel.XAxis.Scale.Min = -5;
            panel.XAxis.Scale.Max = 5;
            panel.YAxis.Scale.Min = -10;
            panel.YAxis.Scale.Max = 10;
            
            zedGraphControl2.AxisChange();
            // Обновляем график
            zedGraphControl2.Invalidate();

            zedGraphControl3.AxisChange();
            // Обновляем график
            zedGraphControl3.Invalidate();
        }
        //Очистить график 1 примера 
        private void button4_Click(object sender, EventArgs e)
        {
            // Обновляем график
            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();
            zedGraphControl1.Invalidate();
        }
        //Очистить график примера с мертвой зоной
        private void button3_Click(object sender, EventArgs e)
        {
            // Обновляем график
            GraphPane panel = zedGraphControl2.GraphPane;
            panel.CurveList.Clear();
            zedGraphControl2.Invalidate();

            panel = zedGraphControl3.GraphPane;
            panel.CurveList.Clear();
            zedGraphControl3.Invalidate();
        }

        //Построить s = 0
        public void DrawS(double x0, GraphPane panel)
        {
            PointPairList listS = new PointPairList();
            int p = 0;

            double t = x0; 

            while (p < P)
            {
                listS.Add(t, -1 * a * t / b);

                p++;
                t += dt;
            }

                LineItem Curve = panel.AddCurve("s = 0", listS, Color.Blue, SymbolType.Diamond);

        }

        //Построить s = +E
        public void DrawS_plusE(double x0, GraphPane panel)
        {
            PointPairList listS = new PointPairList();
            int p = 0;

            double t = x0;

            while (p < P)
            {
                listS.Add(t, (E - a * t )/ b);

                p++;
                t += dt;
            }

            LineItem Curve = panel.AddCurve("s = +E", listS, Color.Yellow, SymbolType.Diamond);

        }

        //Построить s = -E
        public void DrawS_minusE(double x0, GraphPane panel)
        {
            PointPairList listS = new PointPairList();
            int p = 0;

            double t = x0;

            while (p < P)
            {
                listS.Add(t, (-E - a * t) / b);

                p++;
                t += dt;
            }

            LineItem Curve = panel.AddCurve("s = -E", listS, Color.Yellow, SymbolType.Diamond);
            
        }

        //Построить сепаратрисы
        public void DrawSep(double x0, GraphPane pan)
        {
            PointPairList listS1 = new PointPairList();
            PointPairList listS2 = new PointPairList();
            int p = 0;

            double t = x0;

            while (p < P)
            {
                //listS1.Add(t, (38 / (3 + Math.Sqrt(85))) * t);
                //listS2.Add(t, (38 / (3 - Math.Sqrt(85))) * t);
                listS1.Add(t, 0);
                listS2.Add(t, (19 / 3) * t);

                p++;
                t += dt;
            }

            LineItem Curve1 = pan.AddCurve("Sep1", listS1, Color.Green, SymbolType.None);
            LineItem Curve2 = pan.AddCurve("Sep2", listS2, Color.Green, SymbolType.None);
        }


        //Добавление начальных точек и вывод их в список 
        public void ListInitialize()
        {
            ListPointNY = new List<MyPoint>();

            ListPointNY.Add(new MyPoint (-3, 14));
            ListPointNY.Add(new MyPoint(3, -14));
            ListPointNY.Add(new MyPoint(-3, -14));
            ListPointNY.Add(new MyPoint(3, 14));
            
            ListPointNY.Add(new MyPoint(-5, 5));
            ListPointNY.Add(new MyPoint(-5, -5));
            ListPointNY.Add(new MyPoint(5, 5));
            ListPointNY.Add(new MyPoint(5, -5));
            
            List<MyPoint> RP = new List<MyPoint>();

            for (int i = 0; i < ListPointNY.Count; i++)
            {
                double x = ListPointNY[i].x;
                for (int j = 0; j < 10; j++)
                {
                    if (ListPointNY[i].x <= ListPointNY[i].y)
                    {
                        x += 0.5;
                        RP.Add(new MyPoint(x, ListPointNY[i].y));

                    }
                    else 
                    {
                        x -= 0.5;
                        RP.Add(new MyPoint(x, ListPointNY[i].y));

                    }


                }
            }

            for (int i = 0; i < RP.Count; i++)
            {
                ListPointNY.Add(RP[i]);
            }
                /*
                ListPointNY.Add(new MyPoint(-0.5, -0.5));
                ListPointNY.Add(new MyPoint(0.5, 0.5));
                ListPointNY.Add(new MyPoint(-0.5, 0.5));
                ListPointNY.Add(new MyPoint(0.5, -0.5));

                ListPointNY.Add(new MyPoint(-1, -1));
                ListPointNY.Add(new MyPoint(1, 1));
                ListPointNY.Add(new MyPoint(1, -1));
                ListPointNY.Add(new MyPoint(-1, 1));

                ListPointNY.Add(new MyPoint(-1, -3));
                ListPointNY.Add(new MyPoint(1, 3));
                ListPointNY.Add(new MyPoint(-1, 3));
                ListPointNY.Add(new MyPoint(1, -3));
                */

                // добавляем элемент в ListView
                for (int i = 0; i < ListPointNY.Count(); i++)
                {
                    //Строка
                    ListViewItem item = new ListViewItem(new string[] { ListPointNY[i].X(), ListPointNY[i].Y() });

                    listView1.Items.Add(item);
                }
        }

        //Очистить таблицу НУ 
        private void button6_Click(object sender, EventArgs e)
        {
            if (ListPointNY != null)
                ListPointNY.Clear();
            listView1.Items.Clear();
        }

        //Обновление отображения списка
        public void UpdateList()
        {
            listView1.Items.Clear();

            // добавляем элемент в ListView
            for (int i = 0; i < ListPointNY.Count(); i++)
            {
                //Строка
                ListViewItem item = new ListViewItem(new string[] { ListPointNY[i].X(), ListPointNY[i].Y() });

                listView1.Items.Add(item);
            }


        }

        //Проверка на пустоту полей 
        public bool EmptyFields()
        {

            if ((textBox9.Text.Length == 0) || (textBox8.Text.Length == 0))
            {
                string message = "Поле ввода данных не заполнено";
                string caption = "Ошибка ввода";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);

                return true;
            }

            return false;
        }

        // Добавить новое НУ 
        private void button5_Click(object sender, EventArgs e)
        {
            if (ListPointNY == null)
                ListPointNY = new List<MyPoint>();

            if (!EmptyFields())
            {
                
                try
                {
                    ListPointNY.Add(new MyPoint(textBox8.Text, textBox9.Text));
                }
                catch
                {
                    string message = "Данные введены неверно";
                    string caption = "Ошибка ввода";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, buttons);
                }
            }

            UpdateList();
        }

        // Удалить выбранные элементы 
        private void button7_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Int32 index = item.Index;
                listView1.Items.Remove(item);
                ListPointNY.RemoveAt(index);
            }
        }

        //Запуск примера с гистерезисом
        private void button9_Click(object sender, EventArgs e)
        {
            dt = double.Parse(textBox14.Text);
            P = int.Parse(textBox13.Text);

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = true;

            a = double.Parse(textBox12.Text);
            b = double.Parse(textBox11.Text);
            E = double.Parse(textBox10.Text);

            t = new List<double>();
            x = new List<double>();
            y = new List<double>();

            //Проверка введенных параметров
            if (r <= 0)
            {
                MessageBox.Show("Проверьте корректность введенных данных: r > 0");
                //return;
            }

            GraphPane panel = zedGraphControl4.GraphPane;
            panel.CurveList.Clear();
            //
            GraphPane panelOsc = zedGraphControl5.GraphPane;
            panelOsc.CurveList.Clear();

            //Н У 
            double t0 = -2;


            List<PointPairList> Yx_list = new List<PointPairList>();

            for (int k = 0; k < ListPointNY.Count; k++)
            {

                RK_4(t0, ListPointNY[k].x, ListPointNY[k].y);

                //Graphics
                PointPairList list = new PointPairList();
                PointPairList listX = new PointPairList();
                PointPairList listY = new PointPairList();
                PointPairList listS = new PointPairList();

                for (int i = 0; i < t.Count; i++)
                {
                    list.Add(x[i], y[i]);
                    listX.Add(t[i], x[i]);
                    listY.Add(t[i], y[i]);
                    listS.Add(t[i], 4 * x[i] + y[i]);
                }
                Yx_list.Add(list);
                

                //Line
                LineItem Curve1 = panel.AddCurve("", Yx_list[k], Color.Red, SymbolType.None);
                //Osc
                LineItem CurveO1 = panelOsc.AddCurve("", listX, Color.Red, SymbolType.None);
                LineItem CurveO2 = panelOsc.AddCurve("", listY, Color.Blue, SymbolType.None);
                LineItem CurveOS = panelOsc.AddCurve("", listS, Color.Green, SymbolType.None);

            }
            LineItem CurveO11 = panelOsc.AddCurve("x(t)", new PointPairList(), Color.Red, SymbolType.None);
            LineItem CurveO21 = panelOsc.AddCurve("y(t)", new PointPairList(), Color.Blue, SymbolType.None);
            LineItem CurveOSS = panelOsc.AddCurve("s(t)", new PointPairList(), Color.Green, SymbolType.None);

            DrawS(-5, panel);
            DrawS_plusE(-5, panel);
            DrawS_minusE(-5, panel);
            

            panel.XAxis.Scale.Min = -5;
            panel.XAxis.Scale.Max = 5;
            panel.YAxis.Scale.Min = -10;
            panel.YAxis.Scale.Max = 10;

            zedGraphControl4.AxisChange();
            // Обновляем график
            zedGraphControl4.Invalidate();

            zedGraphControl5.AxisChange();
            // Обновляем график
            zedGraphControl5.Invalidate();
        }
        //Очистить графики  примера с гистерезисом
        private void button8_Click(object sender, EventArgs e)
        {
            // Обновляем график
            GraphPane panel = zedGraphControl4.GraphPane;
            panel.CurveList.Clear();
            zedGraphControl4.Invalidate();

            panel = zedGraphControl5.GraphPane;
            panel.CurveList.Clear();
            zedGraphControl5.Invalidate();
        }

    }
}
