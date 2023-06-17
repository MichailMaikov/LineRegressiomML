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



        public List<string> TableColomnListX { get; set; } = new List<string>();
        public List<string> TableColomnListY { get; set; } = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            comboBox3.Items.Add("Линейная регрессия");
            comboBox3.Items.Add("Дерево решений");
            


        }

        private void OxyPlotPost(object sender, EventArgs e)
        {
            OxyPlot.WindowsForms.PlotView pv = new PlotView();
            pv.Location = new Point(0, 0);
            pv.Size = new Size(500, 250);
            this.Controls.Add(pv);

            pv.Model = new PlotModel { Title = "example" };

            FunctionSeries fs = new FunctionSeries();
            fs.Points.Add(new DataPoint(0, 0));
            fs.Points.Add(new DataPoint(10, 10));
            pv.Model.Series.Add(fs);


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
                MessageBox.Show("Вы не выбрали файл для открытия", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            var dataArr = (object[,])Rng.Value; //чтение данных из ячеек в массив            
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
            int a = comboBox1.SelectedIndex;


            int b = comboBox2.SelectedIndex;

            int name = 0;
            switch (name)
            {
                case 1:
                {

                    //LinearRegression();
                    break;
                }

                case 2:
                {
                   // DecisionTree();
                    break;
                }

                default:
                    Console.WriteLine("Неизвестное имя");
                    break;
            }
        }
    }
}
