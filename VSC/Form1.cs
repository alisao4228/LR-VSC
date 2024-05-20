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
                            // Построение графиков на основе данных из DataGridView
                            CreateChart(dataTable);
                            // Вычисление изменения преступности
                            CalculateCrimeTrends(dataTable);
                            // Определение типа данных и выполнение соответствующей обработки
                            DetermineDataTypeAndProcess(dataTable);
                        }

                    }
                }
            }
        }
        private void DetermineDataTypeAndProcess(DataTable dataTable)
        {
            if (dataTable.Columns.Contains("Вид преступности"))
            {
                CalculateCrimeTrends(dataTable);
            }
        }
        private void CalculateCrimeTrends(DataTable dataTable)
        {
            var xColumn = dataTable.Columns[0];

            // Поиск числовых колонок для анализа, исключая первый столбец
            var numericColumns = dataTable.Columns.Cast<DataColumn>()
                                .Where(col => col != xColumn && (col.DataType == typeof(double) || col.DataType == typeof(int)))
                                .ToList();

            if (numericColumns.Count == 0)
            {
                MessageBox.Show("Не найдены числовые столбцы для анализа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maxDecreaseCrimeType = "";
            string minDecreaseCrimeType = "";
            double maxDecreaseValue = double.MinValue;
            double minDecreaseValue = double.MaxValue;
            // Анализ каждого числового столбца
            foreach (var numericColumn in numericColumns)
            {
                var values = dataTable.AsEnumerable()
                                      .Where(row => row[numericColumn] != DBNull.Value)
                                      .Select(row => Convert.ToDouble(row[numericColumn]))
                                      .ToList();

                if (values.Count < 2) continue; // Пропустить анализ, если недостаточно данных

                double initial = values.First();
                double final = values.Last();
                double decrease = initial - final;

                if (decrease > maxDecreaseValue)
                {
                    maxDecreaseValue = decrease;
                    maxDecreaseCrimeType = numericColumn.ColumnName;
                }

                if (decrease < minDecreaseValue)
                {
                    minDecreaseValue = decrease;
                    minDecreaseCrimeType = numericColumn.ColumnName;
                }
            }

            Result_richTextBox.Clear();
            Result_richTextBox.AppendText($"Вид преступности, который снизился больше всего: {maxDecreaseCrimeType} ({maxDecreaseValue})\n");
            Result_richTextBox.AppendText($"Вид преступности, который снизился меньше всего: {minDecreaseCrimeType} ({minDecreaseValue})");
        }
        private void CreateChart(DataTable dataTable)
        {
            // Очистка предыдущих настроек
            Chart.Series.Clear();
            Chart.ChartAreas.Clear();
            Chart.ChartAreas.Add(new ChartArea("MainArea"));

            // Используем первый столбец в качестве оси X
            var xColumn = dataTable.Columns[0];

            // Поиск числовых колонок для построения графиков, исключая первый столбец
            var numericColumns = dataTable.Columns.Cast<DataColumn>()
                                .Where(col => col != xColumn && (col.DataType == typeof(double) || col.DataType == typeof(int)))
                                .ToList();

            if (numericColumns.Count == 0)
            {
                MessageBox.Show("Не найдены числовые столбцы для построения графиков.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Добавление данных в график
            foreach (var numericColumn in numericColumns)
            {
                var series = new Series(numericColumn.ColumnName) { ChartType = SeriesChartType.Line };

                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[xColumn] != DBNull.Value && row[numericColumn] != DBNull.Value)
                    {
                        // Попытка преобразовать значения оси X и Y
                        if (double.TryParse(row[xColumn].ToString(), out double xValue) && double.TryParse(row[numericColumn].ToString(), out double yValue))
                        {
                            series.Points.AddXY(xValue, yValue);
                        }
                    }
                }

                Chart.Series.Add(series);
            }
        }

        private void Extrapolate_Button_Click(object sender, EventArgs e)
        {
            if (Table_DataGridView.DataSource is DataTable dataTable)
            {
                if (int.TryParse(Extrapolate_TextBox.Text, out int years) && years > 0)
                {
                    ExtrapolateData(dataTable, years);
                }
                else
                {
                    MessageBox.Show("Введите корректное положительное число лет для экстраполяции.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void ExtrapolateData(DataTable dataTable, int years)
        {
            // Очистка предыдущих данных
            Result_richTextBox.Clear();
            Chart.Series.Clear();

            var xColumn = dataTable.Columns[0];
            var numericColumns = dataTable.Columns.Cast<DataColumn>()
                                .Where(col => col != xColumn && (col.DataType == typeof(double) || col.DataType == typeof(int)))
                                .ToList();

            int n = 5; // Период скользящей средней, можно сделать его настраиваемым


            if (!int.TryParse(dataTable.Rows[dataTable.Rows.Count - 1][xColumn].ToString(), out int lastYear))
            {
                MessageBox.Show("Невозможно определить последний год из данных таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Random random = new Random();
            foreach (var numericColumn in numericColumns)
            {
                // Проверка наличия данных для региона
                var originalData = dataTable.AsEnumerable()
                                            .Where(row => row[numericColumn] != DBNull.Value)
                                            .Select(row => Convert.ToDouble(row[numericColumn]))
                                            .ToList();

                if (originalData.Count > 0)
                {
                    var series = new Series(numericColumn.ColumnName + " Прогноз")
                    {
                        ChartType = SeriesChartType.Line,
                        Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)),
                        BorderDashStyle = ChartDashStyle.Dash
                    };

                    List<double> forecastData = new List<double>(originalData);

                    Result_richTextBox.AppendText($"Прогноз для {numericColumn.ColumnName}\n");

                    for (int i = 0; i < years; i++)
                    {
                        double movingAverage = forecastData.Skip(Math.Max(0, forecastData.Count - n)).Take(n).Average();
                        forecastData.Add(movingAverage);

                        int forecastYear = lastYear + i + 1;
                        Result_richTextBox.AppendText($"Год {forecastYear}: Скользящая средняя последних {n} значений = {movingAverage}\n");
                    }

                    for (int i = 0; i < forecastData.Count; i++)
                    {
                        double xValue;
                        if (i < dataTable.Rows.Count)
                        {
                            xValue = Convert.ToDouble(dataTable.Rows[i][xColumn]);
                        }
                        else
                        {
                            xValue = lastYear + (i - dataTable.Rows.Count + 1);
                        }
                        series.Points.AddXY(xValue, forecastData[i]);
                    }

                    Chart.Series.Add(series);
                }
                else
                {
                    Result_richTextBox.AppendText($"Нет данных для {numericColumn.ColumnName}\n");
                }
            }
        }
    }
}
