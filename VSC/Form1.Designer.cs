namespace VSC
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Table_DataGridView = new System.Windows.Forms.DataGridView();
            this.Info_TextBox = new System.Windows.Forms.TextBox();
            this.Open_Button = new System.Windows.Forms.Button();
            this.Extrapolate_TextBox = new System.Windows.Forms.TextBox();
            this.Extrapolate_Button = new System.Windows.Forms.Button();
            this.Result_richTextBox = new System.Windows.Forms.RichTextBox();
            this.Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.Table_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            this.SuspendLayout();
            // 
            // Table_DataGridView
            // 
            this.Table_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Table_DataGridView.Location = new System.Drawing.Point(26, 137);
            this.Table_DataGridView.Name = "Table_DataGridView";
            this.Table_DataGridView.RowHeadersWidth = 51;
            this.Table_DataGridView.RowTemplate.Height = 24;
            this.Table_DataGridView.Size = new System.Drawing.Size(681, 497);
            this.Table_DataGridView.TabIndex = 5;
            // 
            // Info_TextBox
            // 
            this.Info_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.Info_TextBox.Location = new System.Drawing.Point(26, 31);
            this.Info_TextBox.Name = "Info_TextBox";
            this.Info_TextBox.Size = new System.Drawing.Size(681, 34);
            this.Info_TextBox.TabIndex = 4;
            // 
            // Open_Button
            // 
            this.Open_Button.Location = new System.Drawing.Point(26, 82);
            this.Open_Button.Name = "Open_Button";
            this.Open_Button.Size = new System.Drawing.Size(681, 34);
            this.Open_Button.TabIndex = 3;
            this.Open_Button.Text = "Открыть файл";
            this.Open_Button.UseVisualStyleBackColor = true;
            this.Open_Button.Click += new System.EventHandler(this.Open_Button_Click);
            // 
            // Extrapolate_TextBox
            // 
            this.Extrapolate_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.Extrapolate_TextBox.Location = new System.Drawing.Point(779, 31);
            this.Extrapolate_TextBox.Name = "Extrapolate_TextBox";
            this.Extrapolate_TextBox.Size = new System.Drawing.Size(1119, 34);
            this.Extrapolate_TextBox.TabIndex = 10;
            // 
            // Extrapolate_Button
            // 
            this.Extrapolate_Button.Location = new System.Drawing.Point(779, 82);
            this.Extrapolate_Button.Name = "Extrapolate_Button";
            this.Extrapolate_Button.Size = new System.Drawing.Size(1119, 34);
            this.Extrapolate_Button.TabIndex = 9;
            this.Extrapolate_Button.Text = "Реализовать статистическое прогнозирование";
            this.Extrapolate_Button.UseVisualStyleBackColor = true;
            // 
            // Result_richTextBox
            // 
            this.Result_richTextBox.Location = new System.Drawing.Point(779, 661);
            this.Result_richTextBox.Name = "Result_richTextBox";
            this.Result_richTextBox.Size = new System.Drawing.Size(1119, 180);
            this.Result_richTextBox.TabIndex = 8;
            this.Result_richTextBox.Text = "";
            // 
            // Chart
            // 
            chartArea1.Name = "ChartArea1";
            this.Chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.Chart.Legends.Add(legend1);
            this.Chart.Location = new System.Drawing.Point(779, 137);
            this.Chart.Name = "Chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Chart.Series.Add(series1);
            this.Chart.Size = new System.Drawing.Size(1119, 497);
            this.Chart.TabIndex = 11;
            this.Chart.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 863);
            this.Controls.Add(this.Chart);
            this.Controls.Add(this.Extrapolate_TextBox);
            this.Controls.Add(this.Extrapolate_Button);
            this.Controls.Add(this.Result_richTextBox);
            this.Controls.Add(this.Table_DataGridView);
            this.Controls.Add(this.Info_TextBox);
            this.Controls.Add(this.Open_Button);
            this.Name = "Form1";
            this.Text = "Статистика Саши и Алисы";
            ((System.ComponentModel.ISupportInitialize)(this.Table_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Table_DataGridView;
        private System.Windows.Forms.TextBox Info_TextBox;
        private System.Windows.Forms.Button Open_Button;
        private System.Windows.Forms.TextBox Extrapolate_TextBox;
        private System.Windows.Forms.Button Extrapolate_Button;
        private System.Windows.Forms.RichTextBox Result_richTextBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart Chart;
    }
}

