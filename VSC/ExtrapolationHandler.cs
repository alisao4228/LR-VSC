using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace VSC
{
    public class ExtrapolationHandler
    {
        private Chart Chart;
        private RichTextBox Result_richTextBox;

        public ExtrapolationHandler(Chart chart, RichTextBox resultTextBox)
        {
            this.Chart = chart;
            this.Result_richTextBox = resultTextBox;
        }

        public void ExtrapolateData(DataTable dataTable, int years)
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
