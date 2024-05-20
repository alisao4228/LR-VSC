using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ExcelDataReader;

namespace VSC
{
    public partial class Form1 : Form
    {
        private ExcelHandler excelHandler;
        private ChartHandler chartHandler;
        

        public Form1()
        {
            InitializeComponent();
            excelHandler = new ExcelHandler();
            chartHandler = new ChartHandler(Chart);

        }

        private void Open_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName); // Получаем имя файла без расширения
                Info_TextBox.Text = fileNameWithoutExtension;

                DataTable dataTable = excelHandler.ReadExcelFile(openFileDialog.FileName);
                if (dataTable != null)
                {
                    Table_DataGridView.DataSource = dataTable;
                    chartHandler.CreateChart(dataTable);
                   
                }
            }
        }
}
}
