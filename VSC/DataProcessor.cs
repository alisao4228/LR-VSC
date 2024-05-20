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
            
            if (dataTable.Columns.Contains("Вид преступности"))
            {
                CalculateCrimeTrends(dataTable);
            }
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
