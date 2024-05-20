using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSC
{
    public class DataProcessor
    {
        public RichTextBox Result_richTextBox;

        public DataProcessor(RichTextBox resultTextBox)
        {
            this.Result_richTextBox = resultTextBox;
        }

        public void DetermineDataTypeAndProcess(DataTable dataTable)
        {
            if (dataTable.Columns.Contains("Регион России"))
            {
                FindPopulationChanges(dataTable);
            }
            if (dataTable.Columns.Contains("Вид преступности"))
            {
                CalculateCrimeTrends(dataTable);
            }
        }

        private void FindPopulationChanges(DataTable dataTable)
        {
            // Проверка наличия столбцов
            if (dataTable.Columns.Count < 2)
            {
                MessageBox.Show("Недостаточно данных для расчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var changes = new Dictionary<string, double>();

            // Перебор столбцов, начиная со второго (первый - год)
            for (int colIndex = 1; colIndex < dataTable.Columns.Count; colIndex++)
            {
                string region = dataTable.Columns[colIndex].ColumnName;

                // Проверка на наличие данных в ячейках и их типов
                if (double.TryParse(dataTable.Rows[0][colIndex].ToString(), out double initialPopulation) &&
                    double.TryParse(dataTable.Rows[dataTable.Rows.Count - 1][colIndex].ToString(), out double finalPopulation))
                {
                    double change = initialPopulation - finalPopulation;
                    if (change > 0) // Учитываем только случаи снижения численности
                    {
                        changes[region] = change;
                    }
                }
                else
                {
                    MessageBox.Show($"Не удалось преобразовать данные для региона {region}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (changes.Count == 0)
            {
                MessageBox.Show("Не удалось найти данные для расчета изменений численности.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var maxDecreaseRegion = changes.OrderByDescending(kv => kv.Value).First();
            var minDecreaseRegion = changes.OrderBy(kv => kv.Value).First();

            Result_richTextBox.Text = $"Наибольшее снижение численности: {maxDecreaseRegion.Key} ({maxDecreaseRegion.Value})\n" +
                                      $"Наименьшее снижение численности: {minDecreaseRegion.Key} ({minDecreaseRegion.Value})";

        }
        private void CalculateCrimeTrends(DataTable dataTable)
        {
            var xColumn = dataTable.Columns[0];

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

            foreach (var numericColumn in numericColumns)
            {
                var values = dataTable.AsEnumerable()
                                      .Where(row => row[numericColumn] != DBNull.Value)
                                      .Select(row => Convert.ToDouble(row[numericColumn]))
                                      .ToList();

                if (values.Count < 2) continue;

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
    
    }
}
