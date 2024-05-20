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
        private RichTextBox Result_richTextBox;

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
        }

        private void FindPopulationChanges(DataTable dataTable)
        {
            if (dataTable.Columns.Count < 2)
            {
                MessageBox.Show("Недостаточно данных для расчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var changes = new Dictionary<string, double>();

            for (int colIndex = 1; colIndex < dataTable.Columns.Count; colIndex++)
            {
                string region = dataTable.Columns[colIndex].ColumnName;

                if (double.TryParse(dataTable.Rows[0][colIndex].ToString(), out double initialPopulation) &&
                    double.TryParse(dataTable.Rows[dataTable.Rows.Count - 1][colIndex].ToString(), out double finalPopulation))
                {
                    double change = initialPopulation - finalPopulation;
                    if (change > 0)
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
        }
    }
}
