using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ExcelDataReader;

namespace VSC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Open_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName); // Получаем имя файла без расширения
                Info_TextBox.Text = fileNameWithoutExtension;

                // Чтение данных из файла Excel и отображение в DataGridView
                using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Используем AsDataSet для получения всех данных из Excel в DataSet
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                        });

                        // Берем первый лист и привязываем его к DataGridView
                        if (result.Tables.Count > 0)
                        {
                            DataTable dataTable = result.Tables[0];
                            Table_DataGridView.DataSource = dataTable;

                        }
                    }
                }
            }
        }
    }
}
