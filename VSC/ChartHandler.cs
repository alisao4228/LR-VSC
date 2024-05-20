using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace VSC
{
    public class ChartHandler
    {
        private Chart chart;

        public ChartHandler(Chart chart)
        {
            this.chart = chart;
        }

        public void CreateChart(DataTable dataTable)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(new ChartArea("MainArea"));

            var xColumn = dataTable.Columns[0];
            var numericColumns = dataTable.Columns.Cast<DataColumn>()
                                .Where(col => col != xColumn && (col.DataType == typeof(double) || col.DataType == typeof(int)))
                                .ToList();

            if (numericColumns.Count == 0)
            {
                MessageBox.Show("Не найдены числовые столбцы для построения графиков.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var numericColumn in numericColumns)
            {
                var series = new Series(numericColumn.ColumnName) { ChartType = SeriesChartType.Line };

                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[xColumn] != DBNull.Value && row[numericColumn] != DBNull.Value)
                    {
                        if (double.TryParse(row[xColumn].ToString(), out double xValue) && double.TryParse(row[numericColumn].ToString(), out double yValue))
                        {
                            series.Points.AddXY(xValue, yValue);
                        }
                    }
                }

                chart.Series.Add(series);
            }
        }
    }
    }
