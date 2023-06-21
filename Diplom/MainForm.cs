using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;


namespace Diplom
{
    public partial class MainForm : Form
    {
        public object[,] dataArr;
        public List<string> TableColomnListX { get; set; } = new List<string>();
        public List<string> TableColomnListY { get; set; } = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            comboBox3.Items.Add("Линейная регрессия");
            comboBox3.Items.Add("Дерево решений");
        }

        public void OxyPlotPostLine(object sender, EventArgs e)
        {
            int a = comboBox1.SelectedIndex;
            List<double> DataColonmX = new List<double>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataColonmX.Add((double)dataArr[i + 2, a + 1]);
            }

            int b = comboBox2.SelectedIndex;
            List<double> DataColonmY = new List<double>();
            List<double> Ly = new List<double>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataColonmY.Add((double)dataArr[i + 2, b + 1]);
            }

            double La = 0;
            double Lb = 0;
            double M = 0;
            double Mxy = 0;
            double Mx = 0;
            double My = 0;
            double Mxx = 0;
            double Mx2 = 0;

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                M = M + 1;
                Mx += (DataColonmX[i]);
                My += (DataColonmY[i]);
                Mxy += (DataColonmX[i]) * (DataColonmY[i]);
                Mxx += (DataColonmX[i]) * (DataColonmX[i]);
                Mx2 += (DataColonmX[i]);
            }

            Mx = Mx / M;
            My = My / M;
            Mxy = Mxy / M;
            Mxx = Mxx / M;
            Mx2 = Mx2 / M ;
            Mx2 = Mx2 * Mx2;
            double MSE = 0;

            La = (Mxy - Mx * My) / (Mxx - Mx2);
            Lb = ((Mxx * My) - (Mx * Mxy)) / (Mxx - Mx2);

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {  
                Ly.Add(0);
                Ly[i] = (La * DataColonmX[i] + Lb);
            }

            for (int i = 0; i < DataColonmX.Count; i++)
            {
                MSE += (DataColonmY[i] - Ly[i]) * (DataColonmY[i] - Ly[i]);           
            }
            MSE = MSE / M;
   
            var line1 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Series 1",
                Color = OxyPlot.OxyColors.Blue,
                StrokeThickness = 0,
                MarkerSize = 2,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            var line2 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Series 2",
                Color = OxyPlot.OxyColors.Red,
                StrokeThickness = 1,
                MarkerSize = 1,
                MarkerType = OxyPlot.MarkerType.Circle      
            };

            for (int i = 0; i < DataColonmX.Count; i++)
            {
                line1.Points.Add(new OxyPlot.DataPoint(DataColonmX[i], DataColonmY[i]));
                line2.Points.Add(new OxyPlot.DataPoint(DataColonmX[i], Ly[i]));
            }
      
            var myModel = new OxyPlot.PlotModel
            {
                Title = "Line Regression"
            };

            myModel.Series.Add(line1);
            myModel.Series.Add(line2);
            plotView1.Model = myModel;         
            textBox1.Text = MSE.ToString();
        }

        public double DecisionTree(double x , List<double> Xtrain, List<double> Ytrain)
        {
            double y = 0;

            if (x <= Xtrain[0])
            {
                y = Ytrain[0];
            }
            else if (x >= Xtrain[^1])
            {
                y = Ytrain[^1];
            }
            else
                for (int i = 0; i < Xtrain.Count - 1; i++)
                {
                    if (x >= Xtrain[i] && x < Xtrain[i+1])
                    {
                        y = Ytrain[i];
                        break;
                    }
                }
            return y;
        }

        public void OxyPlotPostTree(object sender, EventArgs e)
        {
            int a = comboBox1.SelectedIndex;
            List<double> DataColonmX = new List<double>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataColonmX.Add((double)dataArr[i + 2, a + 1]);
            }

            int b = comboBox2.SelectedIndex;
            List<double> DataColonmY = new List<double>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataColonmY.Add((double)dataArr[i + 2, b + 1]);
            }

            double MinN = DataColonmX.Min();
            double MaxN = DataColonmX.Max();
            double step = 0.1;
            double XCurrent = MinN;
            double MSE = 0;

            List<double> XNew = new List<double>();
            List<double> YNew = new List<double>();

            while (XCurrent < MaxN)
            {
                XNew.Add(XCurrent);
                XCurrent = XCurrent + step;
            }
            

            for (int i = 0; i < XNew.Count; i++)
            {
                YNew.Add(DecisionTree(XNew[i], DataColonmX, DataColonmY));
            }

            for (int i = 0; i < DataColonmX.Count; i++)
            {
                double predictedY = DecisionTree(DataColonmX[i], DataColonmX, DataColonmY);
                MSE += (DataColonmY[i] - predictedY) * (DataColonmY[i] - predictedY);
            }

            MSE = MSE / DataColonmX.Count;




            var line1 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Series 1",
                Color = OxyPlot.OxyColors.Blue,
                StrokeThickness = 0,
                MarkerSize = 2,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            var line2 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Series 2",
                Color = OxyPlot.OxyColors.Red,
                StrokeThickness = 1,
                MarkerSize = 1,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            for (int i = 0; i < DataColonmX.Count; i++)
            {
                line1.Points.Add(new OxyPlot.DataPoint(DataColonmX[i], DataColonmY[i]));      
            }

            for (int i = 0; i < XNew.Count; i++)
            {
                line2.Points.Add(new OxyPlot.DataPoint(XNew[i], YNew[i]));
            }
             
            var myMode2 = new OxyPlot.PlotModel
            {
                Title = "Regression Tree"
            };

            myMode2.Series.Add(line1);
            myMode2.Series.Add(line2);
            plotView1.Model = myMode2;
            textBox1.Text = MSE.ToString();
        }

            private void button1_Click(object sender, EventArgs e)
        {
            //поиск файла Excel
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.DefaultExt = "*.xls;*.xlsx";
            ofd.Filter = "Microsoft Excel (*.xls*)|*.xls*";
            ofd.Title = "Выберите документ Excel";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Вы не выбрали файл для открытия", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string xlFileName = ofd.FileName; //имя нашего Excel файла

            //рабоата с Excel
            Excel.Range Rng;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSht;
            int iLastRow, iLastCol;

            Excel.Application xlApp = new Excel.Application(); //создаём приложение Excel
            xlWB = xlApp.Workbooks.Open(xlFileName); //открываем наш файл           

            xlSht = xlWB.ActiveSheet;//активный лист

            iLastRow = xlSht.Cells[xlSht.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row; //последняя заполненная строка в столбце А
            iLastCol = xlSht.Cells[1, xlSht.Columns.Count].End[Excel.XlDirection.xlToLeft].Column; //последний заполненный столбец в 1-й строке

            Rng = (Excel.Range)xlSht.Range["A1", xlSht.Cells[iLastRow, iLastCol]]; //пример записи диапазона ячеек в переменную Rng
                                                                                   //Rng = xlSht.get_Range("A1", "B10"); //пример записи диапазона ячеек в переменную Rng
                                                                                   //Rng = xlSht.get_Range("A1:B10"); //пример записи диапазона ячеек в переменную Rng
                                                                                   //Rng = xlSht.UsedRange; //пример записи диапазона ячеек в переменную Rng

            dataArr = (object[,])Rng.Value; //чтение данных из ячеек в массив            
                                            //xlSht.get_Range("K1").get_Resize(dataArr.GetUpperBound(0), dataArr.GetUpperBound(1)).Value = dataArr; //выгрузка массива на лист

            //закрытие Excel
            xlWB.Close(true); //сохраняем и закрываем файл
            xlApp.Quit();
            releaseObject(xlSht);
            releaseObject(xlWB);
            releaseObject(xlApp);

            //заполняем DataTable для последующего заполнения dataGridView
            DataTable dt = new DataTable();
            DataRow dtRow;
            //добавляем столбцы в DataTable
            for (int i = 1; i <= dataArr.GetUpperBound(1); i++)
                dt.Columns.Add((string)dataArr[1, i]);

            //цикл по строкам массива
            for (int i = 2; i <= dataArr.GetUpperBound(0); i++)
            {
                dtRow = dt.NewRow();
                //цикл по столбцам массива
                for (int n = 1; n <= dataArr.GetUpperBound(1); n++)
                {
                    dtRow[n - 1] = dataArr[i, n];
                }
                dt.Rows.Add(dtRow);
            }

            this.dataGridView1.DataSource = dt; //заполняем dataGridView
            comboBox1.Items.Clear();

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].HeaderCell.Value.ToString());
                TableColomnListX.Add(new string(""));
                TableColomnListX[i] = dataGridView1.Columns[i].HeaderCell.Value.ToString();
            }

            comboBox2.Items.Clear();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                comboBox2.Items.Add(dataGridView1.Columns[i].HeaderCell.Value.ToString());
                TableColomnListY.Add(new string(""));
                TableColomnListY[i] = dataGridView1.Columns[i].HeaderCell.Value.ToString();
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void plotView1_Click(object sender, EventArgs e)
        {

        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void button5_Click(object sender, EventArgs e)
        {
                string word = comboBox3.Text;
                switch (word)
                {
                    case "Линейная регрессия":
                        {
                            OxyPlotPostLine(sender,e);
                            break;
                        }

                    case "Дерево решений":
                        {
                            OxyPlotPostTree(sender, e);
                            break;
                        }

                    default:                       
                        break;
                }    
        }
    }
}
