namespace Pic_print
{
    partial class Picture_print
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Path_button = new System.Windows.Forms.Button();
            this.Save_path_button = new System.Windows.Forms.Button();
            this.Format_label = new System.Windows.Forms.Label();
            this.Export_button = new System.Windows.Forms.Button();
            this.Jpeg_checkbox = new System.Windows.Forms.CheckBox();
            this.Pdf_checkbox = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Progress_label = new System.Windows.Forms.Label();
            this.Progress_change_label = new System.Windows.Forms.Label();
            this.Exit_button = new System.Windows.Forms.Button();
            this.path_Textbox = new System.Windows.Forms.TextBox();
            this.save_Textbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Path_button
            // 
            this.Path_button.Location = new System.Drawing.Point(12, 12);
            this.Path_button.Name = "Path_button";
            this.Path_button.Size = new System.Drawing.Size(115, 23);
            this.Path_button.TabIndex = 0;
            this.Path_button.Text = "Выбрать папку";
            this.Path_button.UseVisualStyleBackColor = true;
            this.Path_button.Click += new System.EventHandler(this.Path_button_Click);
            // 
            // Save_path_button
            // 
            this.Save_path_button.Location = new System.Drawing.Point(12, 41);
            this.Save_path_button.Name = "Save_path_button";
            this.Save_path_button.Size = new System.Drawing.Size(115, 23);
            this.Save_path_button.TabIndex = 2;
            this.Save_path_button.Text = "Сохранить в:";
            this.Save_path_button.UseVisualStyleBackColor = true;
            this.Save_path_button.Click += new System.EventHandler(this.Save_path_button_Click);
            // 
            // Format_label
            // 
            this.Format_label.AutoSize = true;
            this.Format_label.Location = new System.Drawing.Point(9, 78);
            this.Format_label.Name = "Format_label";
            this.Format_label.Size = new System.Drawing.Size(102, 13);
            this.Format_label.TabIndex = 5;
            this.Format_label.Text = "Выходной формат:";
            // 
            // Export_button
            // 
            this.Export_button.Location = new System.Drawing.Point(12, 137);
            this.Export_button.Name = "Export_button";
            this.Export_button.Size = new System.Drawing.Size(75, 23);
            this.Export_button.TabIndex = 7;
            this.Export_button.Text = "Экспорт";
            this.Export_button.UseVisualStyleBackColor = true;
            this.Export_button.Click += new System.EventHandler(this.Export_button_Click);
            // 
            // Jpeg_checkbox
            // 
            this.Jpeg_checkbox.AutoSize = true;
            this.Jpeg_checkbox.Location = new System.Drawing.Point(12, 104);
            this.Jpeg_checkbox.Name = "Jpeg_checkbox";
            this.Jpeg_checkbox.Size = new System.Drawing.Size(53, 17);
            this.Jpeg_checkbox.TabIndex = 8;
            this.Jpeg_checkbox.Text = "JPEG";
            this.Jpeg_checkbox.UseVisualStyleBackColor = true;
            // 
            // Pdf_checkbox
            // 
            this.Pdf_checkbox.AutoSize = true;
            this.Pdf_checkbox.Location = new System.Drawing.Point(80, 104);
            this.Pdf_checkbox.Name = "Pdf_checkbox";
            this.Pdf_checkbox.Size = new System.Drawing.Size(47, 17);
            this.Pdf_checkbox.TabIndex = 9;
            this.Pdf_checkbox.Text = "PDF";
            this.Pdf_checkbox.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(210, 137);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(286, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // Progress_label
            // 
            this.Progress_label.AutoSize = true;
            this.Progress_label.Location = new System.Drawing.Point(145, 142);
            this.Progress_label.Name = "Progress_label";
            this.Progress_label.Size = new System.Drawing.Size(59, 13);
            this.Progress_label.TabIndex = 11;
            this.Progress_label.Text = "Прогресс:";
            // 
            // Progress_change_label
            // 
            this.Progress_change_label.AutoSize = true;
            this.Progress_change_label.Location = new System.Drawing.Point(207, 173);
            this.Progress_change_label.Name = "Progress_change_label";
            this.Progress_change_label.Size = new System.Drawing.Size(0, 13);
            this.Progress_change_label.TabIndex = 12;
            // 
            // Exit_button
            // 
            this.Exit_button.Location = new System.Drawing.Point(12, 173);
            this.Exit_button.Name = "Exit_button";
            this.Exit_button.Size = new System.Drawing.Size(75, 23);
            this.Exit_button.TabIndex = 13;
            this.Exit_button.Text = "Выход";
            this.Exit_button.UseVisualStyleBackColor = true;
            this.Exit_button.Click += new System.EventHandler(this.Exit_button_Click);
            // 
            // path_Textbox
            // 
            this.path_Textbox.Location = new System.Drawing.Point(148, 15);
            this.path_Textbox.Name = "path_Textbox";
            this.path_Textbox.ReadOnly = true;
            this.path_Textbox.Size = new System.Drawing.Size(348, 20);
            this.path_Textbox.TabIndex = 14;
            // 
            // save_Textbox
            // 
            this.save_Textbox.Location = new System.Drawing.Point(148, 44);
            this.save_Textbox.Name = "save_Textbox";
            this.save_Textbox.ReadOnly = true;
            this.save_Textbox.Size = new System.Drawing.Size(348, 20);
            this.save_Textbox.TabIndex = 15;
            // 
            // Picture_print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 209);
            this.Controls.Add(this.save_Textbox);
            this.Controls.Add(this.path_Textbox);
            this.Controls.Add(this.Exit_button);
            this.Controls.Add(this.Progress_change_label);
            this.Controls.Add(this.Progress_label);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Pdf_checkbox);
            this.Controls.Add(this.Jpeg_checkbox);
            this.Controls.Add(this.Export_button);
            this.Controls.Add(this.Format_label);
            this.Controls.Add(this.Save_path_button);
            this.Controls.Add(this.Path_button);
            this.Name = "Picture_print";
            this.Text = "Экспорт";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Path_button;
        private System.Windows.Forms.Button Save_path_button;
        private System.Windows.Forms.Label Format_label;
        private System.Windows.Forms.Button Export_button;
        private System.Windows.Forms.CheckBox Jpeg_checkbox;
        private System.Windows.Forms.CheckBox Pdf_checkbox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label Progress_label;
        private System.Windows.Forms.Label Progress_change_label;
        private System.Windows.Forms.Button Exit_button;
        private System.Windows.Forms.TextBox path_Textbox;
        private System.Windows.Forms.TextBox save_Textbox;

    }
}

