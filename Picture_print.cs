using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Xml;
using Pic_print.Lib;

namespace Pic_print
{
    public partial class Picture_print : Form
    {
        BackgroundWorker bgw;
        DirectoryInfo root_folder;
        string path, save_path;
        int width = 0, height = 0, indent_logo_h = 0, logo_h = 0, indent_header_h = 0, header_h = 0, indent_subtitle_h = 0, subtitle_h = 0, indent_pic_h = 0,
            horizontal_pic_h = 0, indent_name_h = 0, name_h = 0, vertical_pic_h = 0,

        info_h = 0, info_indent_h = 0, info_line_h = 0,

        indent_page_w = 0, horizontal_pic_w = 0, vertical_pic_w = 0, horizontal_indent_w = 0, vertical_indent_w = 0, indent_logo_w = 0,

        title_font = 0, header_font = 0, subtitle_font = 0, name_font = 0,

        free_space_h = 0, progress_count = 0, progress_maximum = 0;

        bool work_files_process_complete = false;

        public Picture_print()
        {
            InitializeComponent();
            bgw = new BackgroundWorker();
        }

        void XmlLoad(string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            List<string> error_list = new List<string> { "width", "height", "indent_logo_h", "logo_h", "indent_header_h", "header_h", "indent_subtitle_h",
            "subtitle_h", "indent_pic_h", "horizontal_pic_h", "indent_name_h", "name_h", "vertical_pic_h", "info_line_h", "indent_page_w", "horizontal_pic_w",
            "vertical_pic_w", "indent_logo_w", "horizontal_indent_w", "header_font", "subtitle_font", "name_font", "title_font"};
            foreach (XmlElement element in xml.GetElementsByTagName("Parametrs"))
            {
                foreach (XmlElement el in element)
                    switch (el.Name)
                    {
                        case "width":
                            width = Convert.ToInt32(el.InnerText);
                            error_list.Remove("width");
                            break;
                        case "height":
                            height = Convert.ToInt32(el.InnerText);
                            error_list.Remove("height");
                            break;
                        case "indent_logo_h":
                            indent_logo_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_logo_h");
                            break;
                        case "logo_h":
                            logo_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("logo_h");
                            break;
                        case "indent_header_h":
                            indent_header_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_header_h");
                            break;
                        case "header_h":
                            header_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("header_h");
                            break;
                        case "indent_subtitle_h":
                            indent_subtitle_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_subtitle_h");
                            break;
                        case "subtitle_h":
                            subtitle_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("subtitle_h");
                            break;
                        case "indent_pic_h":
                            indent_pic_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_pic_h");
                            break;
                        case "horizontal_pic_h":
                            horizontal_pic_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("horizontal_pic_h");
                            break;
                        case "indent_name_h":
                            indent_name_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_name_h");
                            break;
                        case "name_h":
                            name_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("name_h");
                            break;
                        case "vertical_pic_h":
                            vertical_pic_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("vertical_pic_h");
                            break;
                        case "info_h":
                            info_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("info_h");
                            break;
                        case "info_line_h":
                            info_line_h = Convert.ToInt32(el.InnerText);
                            error_list.Remove("info_line_h");
                            break;
                        case "indent_page_w":
                            indent_page_w = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_page_w");
                            break;
                        case "horizontal_pic_w":
                            horizontal_pic_w = Convert.ToInt32(el.InnerText);
                            error_list.Remove("horizontal_pic_w");
                            break;
                        case "vertical_pic_w":
                            vertical_pic_w = Convert.ToInt32(el.InnerText);
                            error_list.Remove("vertical_pic_w");
                            break;
                        case "horizontal_indent_w":
                            horizontal_indent_w = Convert.ToInt32(el.InnerText);
                            error_list.Remove("horizontal_indent_w");
                            break;
                        case "vertical_indent_w":
                            vertical_indent_w = Convert.ToInt32(el.InnerText);
                            error_list.Remove("vertical_indent_w");
                            break;
                        case "indent_logo_w":
                            indent_logo_w = Convert.ToInt32(el.InnerText);
                            error_list.Remove("indent_logo_w");
                            break;
                        case "header_font":
                            header_font = Convert.ToInt32(el.InnerText);
                            error_list.Remove("header_font");
                            break;
                        case "subtitle_font":
                            subtitle_font = Convert.ToInt32(el.InnerText);
                            error_list.Remove("subtitle_font");
                            break;
                        case "name_font":
                            name_font = Convert.ToInt32(el.InnerText);
                            error_list.Remove("name_font");
                            break;
                        case "title_font":
                            title_font = Convert.ToInt32(el.InnerText);
                            error_list.Remove("title_font");
                            break;
                    }

            }
            if (error_list.Count != 0)
            {
                string error = "";
                foreach (string s in error_list)
                    error = error + s + ";";
                throw new Exception("В parametrs.xml отсутствуют следующие параметры: " + error);
            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            Progress_change_label.Text = String.Format("Выполнено: {0} %", e.ProgressPercentage);
        }

        void bgw_RunWorkerfiles_process_completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (work_files_process_complete)
            {
                Progress_change_label.Parent.Invoke(new MethodInvoker(delegate { Progress_change_label.Text = "Готово!"; }));
                Export_button.Enabled = true;
                Path_button.Enabled = true;
                Save_path_button.Enabled = true;
                Jpeg_checkbox.Enabled = true;
                Pdf_checkbox.Enabled = true;
            }
        }

        private void Export_button_Click(object sender, EventArgs e)
        {
            if (path != null)
                root_folder = new DirectoryInfo(path);
            else
            {
                MessageBox.Show("Не выбрана папка для загрузки", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (save_path == null)
            {
                MessageBox.Show("Не выбрана папка для сохранения", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Jpeg_checkbox.Checked && !Pdf_checkbox.Checked)
            {
                MessageBox.Show("Не выбран выходной формат", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!work_files_process_complete)
            {
                bgw.DoWork += new DoWorkEventHandler(Export_procedure);
                bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerfiles_process_completed);
            }
            bgw.WorkerReportsProgress = true;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            Export_button.Enabled = false;
            Path_button.Enabled = false;
            Save_path_button.Enabled = false;
            Jpeg_checkbox.Enabled = false;
            Pdf_checkbox.Enabled = false;
            bgw.RunWorkerAsync();
        }

        void Export_procedure(object sender, EventArgs e)
        {
            work_files_process_complete = false;
            Bitmap current_bmp;
            List<Classes.Title_folder> title_folders = new List<Classes.Title_folder>();
            PdfDocument doc = new PdfDocument();

            FileInfo[] files;
            List<FileInfo> files_list;
            Bitmap logo = new Bitmap(1, 1), info = new Bitmap(1, 1);
            try
            {
                XmlLoad(Environment.CurrentDirectory + "\\parametrs.xml");
                logo = new Bitmap(Image.FromFile(Environment.CurrentDirectory + "\\samples\\logo.bmp"));
                info = new Bitmap(Image.FromFile(Environment.CurrentDirectory + "\\samples\\info.bmp"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проблема с исходными файлами", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Не найдено: " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Export_button.Enabled = true;
                Path_button.Enabled = true;
                Save_path_button.Enabled = true;
                Jpeg_checkbox.Enabled = true;
                Pdf_checkbox.Enabled = true;
            }

            try
            {
                Progress_change_label.Parent.Invoke(new MethodInvoker(delegate { Progress_change_label.Text = "Подождите, идет предварительный анализ файлов..."; }));
                progress_maximum = Procedures.WalkDirectoryTree(root_folder, title_folders);
            }
            catch
            {
                MessageBox.Show("Ошибка размещения файлов в выбранной папке", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Export_button.Enabled = true;
                Path_button.Enabled = true;
                Save_path_button.Enabled = true;
                Jpeg_checkbox.Enabled = true;
                Pdf_checkbox.Enabled = true;
                return;
            }
            int pic_count = 1;

            current_bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(current_bmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(Color.White);

            RectangleF rectf = new RectangleF(0, 0, logo.Width, logo.Height);
            RectangleF dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
            g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
            rectf = new RectangleF(0, 0, info.Width, info.Height);
            dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
            g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
            free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;

            Point start_p = new Point(indent_page_w, indent_logo_h + logo_h), current_p;
            int files_count = 0, h_pic_count = 0, w_pic_count = 0, percents = 0;
            bool header_folder_process_files_process_complete, files_process_complete, title_process_complete;
            float delta;
            FileInfo fi;
            Bitmap current_image;
            Rectangle sourceRectangle, destRectangle;
            XGraphics xgr;
            XImage img;
            progress_count = 0;

            foreach (Classes.Title_folder tf in title_folders)
            {
                if (tf.empty)
                    continue;

                current_bmp.Dispose();
                g.Dispose();
                free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                current_bmp = new Bitmap(width, height);
                g = Graphics.FromImage(current_bmp);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.White);
                rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                rectf = new RectangleF(0, 0, info.Width, info.Height);
                dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);

                Procedures.DrawLetter(g, width, 24, tf.name.ToUpper(), height/2, new Font("Tahoma", title_font, FontStyle.Bold));

                if (Jpeg_checkbox.Checked)
                    current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                if (Pdf_checkbox.Checked)
                {
                    PdfPage pPage = new PdfPage();
                    pPage.Size = PdfSharp.PageSize.A4;
                    doc.Pages.Add(pPage);
                    xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                    img = XImage.FromGdiPlusImage(current_bmp);
                    xgr.DrawImage(img, 0, 0, 595, 842);
                    xgr.Dispose();
                    img.Dispose();
                }

                pic_count++;

                current_bmp.Dispose();
                g.Dispose();
                free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                current_bmp = new Bitmap(width, height);
                g = Graphics.FromImage(current_bmp);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.White);
                rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                rectf = new RectangleF(0, 0, info.Width, info.Height);
                dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                
                title_process_complete = false;
                while (!title_process_complete)
                    if (free_space_h > indent_pic_h + vertical_pic_h + indent_name_h + name_h)
                    {
                        files = tf.folder.GetFiles("*.*");
                        files_count = tf.w_pic.Count + tf.h_pic.Count;
                        h_pic_count = tf.h_pic.Count;
                        w_pic_count = tf.w_pic.Count;
                        files_list = files.Cast<FileInfo>().ToList();
                        files_process_complete = false;
                        while (!files_process_complete)
                            if (free_space_h > vertical_pic_h + indent_name_h + name_h + indent_pic_h)
                            {
                                while (files_count > 0)
                                {
                                    if (free_space_h > vertical_pic_h + indent_name_h + name_h + indent_pic_h)
                                    {
                                        if (h_pic_count > 0)
                                        {
                                            start_p.Y += indent_pic_h;
                                            current_p = start_p;
                                            if (h_pic_count < 4)
                                            {
                                                delta = 4 - h_pic_count;
                                                delta = delta / 2;
                                                delta = (delta * vertical_pic_w) + (delta * vertical_indent_w);
                                                current_p.X += (int)delta;
                                            }
                                            for (int i = 0; i < 4 && h_pic_count > 0; i++)
                                            {
                                                fi = files_list.Find(f => f.Name == tf.h_pic[0]);
                                                current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                int compressed_w = (int)(Convert.ToDouble(current_image.Width) * vertical_pic_h / current_image.Height);
                                                if (compressed_w > vertical_pic_w)
                                                {
                                                    int compressed_h = vertical_pic_h * vertical_pic_w / compressed_w;
                                                    destRectangle = new Rectangle(current_p.X, current_p.Y + ((vertical_pic_h - compressed_h) / 2), vertical_pic_w, compressed_h);
                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                    current_p.Y += vertical_pic_h + indent_name_h;
                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                }
                                                else
                                                {
                                                    destRectangle = new Rectangle(current_p.X + ((vertical_pic_w - compressed_w) / 2), current_p.Y, compressed_w, vertical_pic_h);
                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                    current_p.Y += vertical_pic_h + indent_name_h;
                                                    current_p.X += ((vertical_pic_w - compressed_w) / 2);
                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                    current_p.X -= ((vertical_pic_w - compressed_w) / 2);
                                                }

                                                current_p.Y -= (vertical_pic_h + indent_name_h);
                                                current_p.X += vertical_pic_w + vertical_indent_w;
                                                tf.h_pic.RemoveAt(0);
                                                h_pic_count--;
                                                files_count--;
                                                current_image.Dispose();
                                                progress_count++;
                                                System.Threading.Thread.Sleep(50);
                                                percents = (progress_count * 100) / progress_maximum;
                                                bgw.ReportProgress(percents, progress_count);
                                            }


                                            free_space_h -= (vertical_pic_h + indent_name_h + name_h + indent_pic_h);
                                            start_p.Y += vertical_pic_h + indent_name_h + name_h;

                                        }
                                        else if (w_pic_count > 0)
                                        {
                                            start_p.Y += indent_pic_h;
                                            current_p = start_p;
                                            if (w_pic_count < 3)
                                            {
                                                delta = 3 - w_pic_count;
                                                delta = delta / 2;
                                                delta = (delta * horizontal_pic_w) + (delta * horizontal_indent_w);
                                                current_p.X += (int)delta;
                                            }
                                            for (int i = 0; i < 3 && w_pic_count > 0; i++)
                                            {
                                                fi = files_list.Find(f => f.Name == tf.w_pic[0]);
                                                current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                int compressed_h = (int)(Convert.ToDouble(current_image.Height) * horizontal_pic_w / current_image.Width);
                                                if (compressed_h > horizontal_pic_h)
                                                {
                                                    int compressed_w = horizontal_pic_w * horizontal_pic_h / compressed_h;
                                                    destRectangle = new Rectangle(current_p.X + ((horizontal_pic_w - compressed_w) / 2), current_p.Y, compressed_w, horizontal_pic_h);
                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                    current_p.X += ((horizontal_pic_w - compressed_w) / 2);
                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                    current_p.X -= ((horizontal_pic_w - compressed_w) / 2);
                                                }
                                                else
                                                {
                                                    destRectangle = new Rectangle(current_p.X, current_p.Y + ((horizontal_pic_h - compressed_h) / 2), horizontal_pic_w, compressed_h);
                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                }
                                                current_p.Y -= (horizontal_pic_h + indent_name_h);
                                                current_p.X += horizontal_pic_w + horizontal_indent_w;
                                                tf.w_pic.RemoveAt(0);
                                                w_pic_count--;
                                                files_count--;
                                                current_image.Dispose();
                                                progress_count++;
                                                System.Threading.Thread.Sleep(50);
                                                percents = (progress_count * 100) / progress_maximum;
                                                bgw.ReportProgress(percents, progress_count);
                                            }

                                            free_space_h -= (horizontal_pic_h + indent_name_h + name_h + indent_pic_h);
                                            start_p.Y += horizontal_pic_h + indent_name_h + name_h;
                                        }
                                    }
                                    else
                                    {
                                        if (free_space_h > horizontal_pic_h + indent_name_h + name_h + indent_pic_h)
                                        {
                                            start_p.Y += indent_pic_h;
                                            current_p = start_p;
                                            if (w_pic_count < 3)
                                            {
                                                delta = 3 - w_pic_count;
                                                delta = delta / 2;
                                                delta = (delta * horizontal_pic_w) + (delta * horizontal_indent_w);
                                                current_p.X += (int)delta;
                                            }
                                            for (int i = 0; i < 3 && w_pic_count > 0; i++)
                                            {
                                                fi = files_list.Find(f => f.Name == tf.w_pic[0]);
                                                current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                int compressed_h = (int)(Convert.ToDouble(current_image.Height) * horizontal_pic_w / current_image.Width);
                                                if (compressed_h > horizontal_pic_h)
                                                {
                                                    int compressed_w = horizontal_pic_w * horizontal_pic_h / compressed_h;
                                                    destRectangle = new Rectangle(current_p.X + ((horizontal_pic_w - compressed_w) / 2), current_p.Y, compressed_w, horizontal_pic_h);
                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                    current_p.X += ((horizontal_pic_w - compressed_w) / 2);
                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                    current_p.X -= ((horizontal_pic_w - compressed_w) / 2);
                                                }
                                                else
                                                {
                                                    destRectangle = new Rectangle(current_p.X, current_p.Y + ((horizontal_pic_h - compressed_h) / 2), horizontal_pic_w, compressed_h);
                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                }
                                                current_p.Y -= (horizontal_pic_h + indent_name_h);
                                                current_p.X += horizontal_pic_w + horizontal_indent_w;
                                                tf.w_pic.RemoveAt(0);
                                                w_pic_count--;
                                                files_count--;
                                                current_image.Dispose();
                                                progress_count++;
                                                System.Threading.Thread.Sleep(50);
                                                percents = (progress_count * 100) / progress_maximum;
                                                bgw.ReportProgress(percents, progress_count);
                                            }


                                            free_space_h -= (horizontal_pic_h + indent_name_h + name_h + indent_pic_h);
                                            start_p.Y += horizontal_pic_h + indent_name_h + name_h;


                                        }
                                        else // новый лист для картинок
                                        {
                                            if (Jpeg_checkbox.Checked)
                                                current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                                            if (Pdf_checkbox.Checked)
                                            {
                                                PdfPage pPage = new PdfPage();
                                                pPage.Size = PdfSharp.PageSize.A4;
                                                doc.Pages.Add(pPage);
                                                xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                                                img = XImage.FromGdiPlusImage(current_bmp);
                                                xgr.DrawImage(img, 0, 0, 595, 842);
                                                xgr.Dispose();
                                                img.Dispose();
                                            }

                                            current_bmp.Dispose();
                                            pic_count++;
                                            g.Dispose();
                                            free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                                            start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                                            current_bmp = new Bitmap(width, height);
                                            g = Graphics.FromImage(current_bmp);
                                            g.CompositingQuality = CompositingQuality.HighQuality;
                                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                            g.Clear(Color.White);
                                            rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                                            dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                                            g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                                            rectf = new RectangleF(0, 0, info.Width, info.Height);
                                            dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                                            g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                                        }
                                    }
                                }
                                files_process_complete = true;
                            }
                            else // новый лист для картинок
                            {
                                if (Jpeg_checkbox.Checked)
                                    current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                                if (Pdf_checkbox.Checked)
                                {
                                    PdfPage pPage = new PdfPage();
                                    pPage.Size = PdfSharp.PageSize.A4;
                                    doc.Pages.Add(pPage);
                                    xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                                    img = XImage.FromGdiPlusImage(current_bmp);
                                    xgr.DrawImage(img, 0, 0, 595, 842);
                                    xgr.Dispose();
                                    img.Dispose();
                                }

                                current_bmp.Dispose();
                                pic_count++;
                                g.Dispose();
                                free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                                start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                                current_bmp = new Bitmap(width, height);
                                g = Graphics.FromImage(current_bmp);
                                g.CompositingQuality = CompositingQuality.HighQuality;
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.Clear(Color.White);
                                rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                                dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                                g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                                rectf = new RectangleF(0, 0, info.Width, info.Height);
                                dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                                g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                            }

                        foreach (Classes.Header_folder hf in tf.header_list)
                        {
                            if (hf.empty)
                                continue;
                            header_folder_process_files_process_complete = false;
                            while (!header_folder_process_files_process_complete)
                                if (free_space_h > indent_header_h + header_h + indent_pic_h + vertical_pic_h + indent_name_h + name_h) // костыль с subtitle
                                {
                                    start_p.Y += indent_header_h;
                                    Procedures.DrawLetter(g, width, header_h, hf.name.ToUpper(), start_p.Y, new Font("Tahoma", header_font, FontStyle.Bold));
                                    start_p.Y += header_h;
                                    free_space_h -= (indent_header_h + header_h);
                                    files = hf.folder.GetFiles("*.*");
                                    files_count = hf.w_pic.Count + hf.h_pic.Count;
                                    h_pic_count = hf.h_pic.Count;
                                    w_pic_count = hf.w_pic.Count;
                                    files_list = files.Cast<FileInfo>().ToList();
                                    files_process_complete = false;
                                    while (!files_process_complete)
                                        if (free_space_h > vertical_pic_h + indent_name_h + name_h + indent_pic_h)
                                        {
                                            while (files_count > 0)
                                            {
                                                if (free_space_h > vertical_pic_h + indent_name_h + name_h + indent_pic_h)
                                                {
                                                    if (h_pic_count > 0)
                                                    {
                                                        start_p.Y += indent_pic_h;
                                                        current_p = start_p;
                                                        if (h_pic_count < 4)
                                                        {
                                                            delta = 4 - h_pic_count;
                                                            delta = delta / 2;
                                                            delta = (delta * vertical_pic_w) + (delta * vertical_indent_w);
                                                            current_p.X += (int)delta;
                                                        }
                                                        for (int i = 0; i < 4 && h_pic_count > 0; i++)
                                                        {
                                                            fi = files_list.Find(f => f.Name == hf.h_pic[0]);
                                                            current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                            sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                            int compressed_w = (int)(Convert.ToDouble(current_image.Width) * vertical_pic_h / current_image.Height);
                                                            if (compressed_w > vertical_pic_w)
                                                            {
                                                                int compressed_h = vertical_pic_h * vertical_pic_w / compressed_w;
                                                                destRectangle = new Rectangle(current_p.X, current_p.Y + ((vertical_pic_h - compressed_h) / 2), vertical_pic_w, compressed_h);
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += vertical_pic_h + indent_name_h;
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                            }
                                                            else
                                                            {
                                                                destRectangle = new Rectangle(current_p.X + ((vertical_pic_w - compressed_w) / 2), current_p.Y, compressed_w, vertical_pic_h);
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += vertical_pic_h + indent_name_h;
                                                                current_p.X += ((vertical_pic_w - compressed_w) / 2);
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                current_p.X -= ((vertical_pic_w - compressed_w) / 2);
                                                            }

                                                            current_p.Y -= (vertical_pic_h + indent_name_h);
                                                            current_p.X += vertical_pic_w + vertical_indent_w;
                                                            hf.h_pic.RemoveAt(0);
                                                            h_pic_count--;
                                                            files_count--;
                                                            current_image.Dispose();
                                                            progress_count++;
                                                            System.Threading.Thread.Sleep(50);
                                                            percents = (progress_count * 100) / progress_maximum;
                                                            bgw.ReportProgress(percents, progress_count);
                                                        }


                                                        free_space_h -= (vertical_pic_h + indent_name_h + name_h + indent_pic_h);
                                                        start_p.Y += vertical_pic_h + indent_name_h + name_h;

                                                    }
                                                    else if (w_pic_count > 0)
                                                    {
                                                        start_p.Y += indent_pic_h;
                                                        current_p = start_p;
                                                        if (w_pic_count < 3)
                                                        {
                                                            delta = 3 - w_pic_count;
                                                            delta = delta / 2;
                                                            delta = (delta * horizontal_pic_w) + (delta * horizontal_indent_w);
                                                            current_p.X += (int)delta;
                                                        }
                                                        for (int i = 0; i < 3 && w_pic_count > 0; i++)
                                                        {
                                                            fi = files_list.Find(f => f.Name == hf.w_pic[0]);
                                                            current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                            sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                            int compressed_h = (int)(Convert.ToDouble(current_image.Height) * horizontal_pic_w / current_image.Width);
                                                            if (compressed_h > horizontal_pic_h)
                                                            {
                                                                int compressed_w = horizontal_pic_w * horizontal_pic_h / compressed_h;
                                                                destRectangle = new Rectangle(current_p.X + ((horizontal_pic_w - compressed_w) / 2), current_p.Y, compressed_w, horizontal_pic_h);
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += horizontal_pic_h + indent_name_h;
                                                                current_p.X += ((horizontal_pic_w - compressed_w) / 2);
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                current_p.X -= ((horizontal_pic_w - compressed_w) / 2);
                                                            }
                                                            else
                                                            {
                                                                destRectangle = new Rectangle(current_p.X, current_p.Y + ((horizontal_pic_h - compressed_h) / 2), horizontal_pic_w, compressed_h);
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += horizontal_pic_h + indent_name_h;
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                            }
                                                            current_p.Y -= (horizontal_pic_h + indent_name_h);
                                                            current_p.X += horizontal_pic_w + horizontal_indent_w;
                                                            hf.w_pic.RemoveAt(0);
                                                            w_pic_count--;
                                                            files_count--;
                                                            current_image.Dispose();
                                                            progress_count++;
                                                            System.Threading.Thread.Sleep(50);
                                                            percents = (progress_count * 100) / progress_maximum;
                                                            bgw.ReportProgress(percents, progress_count);
                                                        }

                                                        free_space_h -= (horizontal_pic_h + indent_name_h + name_h + indent_pic_h);
                                                        start_p.Y += horizontal_pic_h + indent_name_h + name_h;
                                                    }
                                                }
                                                else
                                                {
                                                    if (free_space_h > horizontal_pic_h + indent_name_h + name_h + indent_pic_h)
                                                    {
                                                        start_p.Y += indent_pic_h;
                                                        current_p = start_p;
                                                        if (w_pic_count < 3)
                                                        {
                                                            delta = 3 - w_pic_count;
                                                            delta = delta / 2;
                                                            delta = (delta * horizontal_pic_w) + (delta * horizontal_indent_w);
                                                            current_p.X += (int)delta;
                                                        }
                                                        for (int i = 0; i < 3 && w_pic_count > 0; i++)
                                                        {
                                                            fi = files_list.Find(f => f.Name == hf.w_pic[0]);
                                                            current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                            sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                            int compressed_h = (int)(Convert.ToDouble(current_image.Height) * horizontal_pic_w / current_image.Width);
                                                            if (compressed_h > horizontal_pic_h)
                                                            {
                                                                int compressed_w = horizontal_pic_w * horizontal_pic_h / compressed_h;
                                                                destRectangle = new Rectangle(current_p.X + ((horizontal_pic_w - compressed_w) / 2), current_p.Y, compressed_w, horizontal_pic_h);
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += horizontal_pic_h + indent_name_h;
                                                                current_p.X += ((horizontal_pic_w - compressed_w) / 2);
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                current_p.X -= ((horizontal_pic_w - compressed_w) / 2);
                                                            }
                                                            else
                                                            {
                                                                destRectangle = new Rectangle(current_p.X, current_p.Y + ((horizontal_pic_h - compressed_h) / 2), horizontal_pic_w, compressed_h);
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += horizontal_pic_h + indent_name_h;
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                            }
                                                            current_p.Y -= (horizontal_pic_h + indent_name_h);
                                                            current_p.X += horizontal_pic_w + horizontal_indent_w;
                                                            hf.w_pic.RemoveAt(0);
                                                            w_pic_count--;
                                                            files_count--;
                                                            current_image.Dispose();
                                                            progress_count++;
                                                            System.Threading.Thread.Sleep(50);
                                                            percents = (progress_count * 100) / progress_maximum;
                                                            bgw.ReportProgress(percents, progress_count);
                                                        }


                                                        free_space_h -= (horizontal_pic_h + indent_name_h + name_h + indent_pic_h);
                                                        start_p.Y += horizontal_pic_h + indent_name_h + name_h;


                                                    }
                                                    else // новый лист для картинок
                                                    {
                                                        if (Jpeg_checkbox.Checked)
                                                            current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                                                        if (Pdf_checkbox.Checked)
                                                        {
                                                            PdfPage pPage = new PdfPage();
                                                            pPage.Size = PdfSharp.PageSize.A4;
                                                            doc.Pages.Add(pPage);
                                                            xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                                                            img = XImage.FromGdiPlusImage(current_bmp);
                                                            xgr.DrawImage(img, 0, 0, 595, 842);
                                                            xgr.Dispose();
                                                            img.Dispose();
                                                        }

                                                        current_bmp.Dispose();
                                                        pic_count++;
                                                        g.Dispose();
                                                        free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                                                        start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                                                        current_bmp = new Bitmap(width, height);
                                                        g = Graphics.FromImage(current_bmp);
                                                        g.CompositingQuality = CompositingQuality.HighQuality;
                                                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                        g.Clear(Color.White);
                                                        rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                                                        dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                                                        g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                                                        rectf = new RectangleF(0, 0, info.Width, info.Height);
                                                        dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                                                        g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                                                    }
                                                }
                                            }
                                            files_process_complete = true;
                                        }

                                    foreach (Classes.Subtitle_folder sf in hf.sub_list)
                                    {
                                        if (sf.empty)
                                            continue;
                                        files = sf.folder.GetFiles("*.*");
                                        files_list = new List<FileInfo>();
                                        files_count = sf.w_pic.Count + sf.h_pic.Count; h_pic_count = sf.h_pic.Count; w_pic_count = sf.w_pic.Count;
                                        files_list = files.Cast<FileInfo>().ToList();
                                        files_process_complete = false;
                                        while (!files_process_complete)
                                            if (free_space_h > indent_subtitle_h + subtitle_h + indent_pic_h + vertical_pic_h + indent_name_h + name_h + indent_pic_h)
                                            {
                                                start_p.Y += indent_subtitle_h;
                                                free_space_h -= indent_subtitle_h;
                                                Procedures.DrawLetter(g, width, subtitle_h, sf.name, start_p.Y, new Font("Tahoma", subtitle_font, FontStyle.Bold));
                                                free_space_h -= subtitle_h;
                                                start_p.Y += subtitle_h;
                                                while (files_count > 0)
                                                {
                                                    if (free_space_h > vertical_pic_h + indent_name_h + name_h + indent_pic_h)
                                                    {
                                                        if (h_pic_count > 0)
                                                        {
                                                            start_p.Y += indent_pic_h;
                                                            current_p = start_p;
                                                            if (h_pic_count < 4)
                                                            {
                                                                delta = 4 - h_pic_count;
                                                                delta = delta / 2;
                                                                delta = (delta * vertical_pic_w) + (delta * vertical_indent_w);
                                                                current_p.X += (int)delta;
                                                            }
                                                            for (int i = 0; i < 4 && h_pic_count > 0; i++)
                                                            {
                                                                fi = files_list.Find(f => f.Name == sf.h_pic[0]);
                                                                current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                                sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                                int compressed_w = (int)(Convert.ToDouble(current_image.Width) * vertical_pic_h / current_image.Height);
                                                                if (compressed_w > vertical_pic_w)
                                                                {
                                                                    int compressed_h = vertical_pic_h * vertical_pic_w / compressed_w;
                                                                    destRectangle = new Rectangle(current_p.X, current_p.Y + ((vertical_pic_h - compressed_h) / 2), vertical_pic_w, compressed_h);
                                                                }
                                                                else
                                                                    destRectangle = new Rectangle(current_p.X + ((vertical_pic_w - compressed_w) / 2), current_p.Y, compressed_w, vertical_pic_h); // разобраться с пропорциями.
                                                                g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                current_p.Y += vertical_pic_h + indent_name_h;
                                                                g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                current_p.Y -= (vertical_pic_h + indent_name_h);
                                                                current_p.X += vertical_pic_w + vertical_indent_w;
                                                                sf.h_pic.RemoveAt(0);
                                                                h_pic_count--;
                                                                files_count--;
                                                                current_image.Dispose();
                                                                progress_count++;
                                                                System.Threading.Thread.Sleep(50);
                                                                percents = (progress_count * 100) / progress_maximum;
                                                                bgw.ReportProgress(percents, progress_count);
                                                            }


                                                            free_space_h -= (vertical_pic_h + indent_name_h + name_h + indent_pic_h);
                                                            start_p.Y += vertical_pic_h + indent_name_h + name_h;

                                                        }
                                                        else if (w_pic_count > 0)
                                                        {
                                                            start_p.Y += indent_pic_h;
                                                            current_p = start_p;
                                                            if (w_pic_count < 3)
                                                            {
                                                                delta = 3 - w_pic_count;
                                                                delta = delta / 2;
                                                                delta = (delta * horizontal_pic_w) + (delta * horizontal_indent_w);
                                                                current_p.X += (int)delta;
                                                            }
                                                            for (int i = 0; i < 3 && w_pic_count > 0; i++)
                                                            {
                                                                fi = files_list.Find(f => f.Name == sf.w_pic[0]);
                                                                current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                                sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                                int compressed_h = (int)(Convert.ToDouble(current_image.Height) * horizontal_pic_w / current_image.Width);
                                                                if (compressed_h > horizontal_pic_h)
                                                                {
                                                                    int compressed_w = horizontal_pic_w * horizontal_pic_h / compressed_h;
                                                                    destRectangle = new Rectangle(current_p.X + ((horizontal_pic_w - compressed_w) / 2), current_p.Y, compressed_w, horizontal_pic_h);
                                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                                    current_p.X += ((horizontal_pic_w - compressed_w) / 2);
                                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                    current_p.X -= ((horizontal_pic_w - compressed_w) / 2);
                                                                }
                                                                else
                                                                {
                                                                    destRectangle = new Rectangle(current_p.X, current_p.Y + ((horizontal_pic_h - compressed_h) / 2), horizontal_pic_w, compressed_h);
                                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                }
                                                                current_p.Y -= (horizontal_pic_h + indent_name_h);
                                                                current_p.X += horizontal_pic_w + horizontal_indent_w;
                                                                sf.w_pic.RemoveAt(0);
                                                                w_pic_count--;
                                                                files_count--;
                                                                current_image.Dispose();
                                                                progress_count++;
                                                                System.Threading.Thread.Sleep(50);
                                                                percents = (progress_count * 100) / progress_maximum;
                                                                bgw.ReportProgress(percents, progress_count);
                                                            }


                                                            free_space_h -= (horizontal_pic_h + indent_name_h + name_h + indent_pic_h);
                                                            start_p.Y += horizontal_pic_h + indent_name_h + name_h;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (free_space_h > horizontal_pic_h + indent_name_h + name_h + indent_pic_h)
                                                        {
                                                            start_p.Y += indent_pic_h;
                                                            current_p = start_p;
                                                            if (w_pic_count < 3)
                                                            {
                                                                delta = 3 - w_pic_count;
                                                                delta = delta / 2;
                                                                delta = (delta * horizontal_pic_w) + (delta * horizontal_indent_w);
                                                                current_p.X += (int)delta;
                                                            }
                                                            for (int i = 0; i < 3 && w_pic_count > 0; i++)
                                                            {
                                                                fi = files_list.Find(f => f.Name == sf.w_pic[0]);
                                                                current_image = (Bitmap)(Image.FromFile(fi.FullName));
                                                                sourceRectangle = new Rectangle(0, 0, current_image.Width, current_image.Height);
                                                                int compressed_h = (int)(Convert.ToDouble(current_image.Height) * horizontal_pic_w / current_image.Width);
                                                                if (compressed_h > horizontal_pic_h)
                                                                {
                                                                    int compressed_w = horizontal_pic_w * horizontal_pic_h / compressed_h;
                                                                    destRectangle = new Rectangle(current_p.X + ((horizontal_pic_w - compressed_w) / 2), current_p.Y, compressed_w, horizontal_pic_h);
                                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                                    current_p.X += ((horizontal_pic_w - compressed_w) / 2);
                                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                    current_p.X -= ((horizontal_pic_w - compressed_w) / 2);
                                                                }
                                                                else
                                                                {
                                                                    destRectangle = new Rectangle(current_p.X, current_p.Y + ((horizontal_pic_h - compressed_h) / 2), horizontal_pic_w, compressed_h);
                                                                    g.DrawImage(current_image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                                                                    current_p.Y += horizontal_pic_h + indent_name_h;
                                                                    g.DrawString(Procedures.Pic_id(fi.Name), new Font("Tahoma", name_font, FontStyle.Bold), Brushes.Black, current_p);
                                                                }
                                                                current_p.Y -= (horizontal_pic_h + indent_name_h);
                                                                current_p.X += horizontal_pic_w + horizontal_indent_w;
                                                                sf.w_pic.RemoveAt(0);
                                                                w_pic_count--;
                                                                files_count--;
                                                                current_image.Dispose();
                                                                progress_count++;
                                                                System.Threading.Thread.Sleep(50);
                                                                percents = (progress_count * 100) / progress_maximum;
                                                                bgw.ReportProgress(percents, progress_count);
                                                            }


                                                            free_space_h -= (horizontal_pic_h + indent_name_h + name_h + indent_pic_h);
                                                            start_p.Y += horizontal_pic_h + indent_name_h + name_h;
                                                        }
                                                        else // новый лист для картинок
                                                        {
                                                            if (Jpeg_checkbox.Checked)
                                                                current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                                                            if (Pdf_checkbox.Checked)
                                                            {
                                                                PdfPage pPage = new PdfPage();
                                                                pPage.Size = PdfSharp.PageSize.A4;
                                                                doc.Pages.Add(pPage);
                                                                xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                                                                img = XImage.FromGdiPlusImage(current_bmp);
                                                                xgr.DrawImage(img, 0, 0, 595, 842);
                                                                xgr.Dispose();
                                                                img.Dispose();
                                                            }

                                                            current_bmp.Dispose();
                                                            pic_count++;
                                                            g.Dispose();
                                                            free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                                                            start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                                                            current_bmp = new Bitmap(width, height);
                                                            g = Graphics.FromImage(current_bmp);
                                                            g.CompositingQuality = CompositingQuality.HighQuality;
                                                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                            g.Clear(Color.White);
                                                            rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                                                            dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                                                            g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                                                            rectf = new RectangleF(0, 0, info.Width, info.Height);
                                                            dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                                                            g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                                                        }
                                                    }
                                                }
                                                files_process_complete = true;
                                            }
                                            else // новый лист для подзаголовка
                                            {
                                                if (Jpeg_checkbox.Checked)
                                                    current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                                                if (Pdf_checkbox.Checked)
                                                {
                                                    PdfPage pPage = new PdfPage();
                                                    pPage.Size = PdfSharp.PageSize.A4;
                                                    doc.Pages.Add(pPage);
                                                    xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                                                    img = XImage.FromGdiPlusImage(current_bmp);
                                                    xgr.DrawImage(img, 0, 0, 595, 842);
                                                    xgr.Dispose();
                                                    img.Dispose();
                                                }

                                                current_bmp.Dispose();
                                                pic_count++;
                                                g.Dispose();
                                                current_bmp = new Bitmap(width, height);
                                                free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                                                start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                                                g = Graphics.FromImage(current_bmp);
                                                g.CompositingQuality = CompositingQuality.HighQuality;
                                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                g.Clear(Color.White);
                                                rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                                                dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                                                g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                                                rectf = new RectangleF(0, 0, info.Width, info.Height);
                                                dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                                                g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                                            }
                                    }
                                    header_folder_process_files_process_complete = true;
                                }
                                else // новый лист для заголовка
                                {
                                    if (Jpeg_checkbox.Checked)
                                        current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                                    if (Pdf_checkbox.Checked)
                                    {
                                        PdfPage pPage = new PdfPage();
                                        pPage.Size = PdfSharp.PageSize.A4;
                                        doc.Pages.Add(pPage);
                                        xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                                        img = XImage.FromGdiPlusImage(current_bmp);
                                        xgr.DrawImage(img, 0, 0, 595, 842);
                                        xgr.Dispose();
                                        img.Dispose();
                                    }

                                    current_bmp.Dispose();
                                    pic_count++;
                                    g.Dispose();
                                    current_bmp = new Bitmap(width, height);
                                    free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                                    start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                                    g = Graphics.FromImage(current_bmp);
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.Clear(Color.White);
                                    rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                                    dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                                    g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                                    rectf = new RectangleF(0, 0, info.Width, info.Height);
                                    dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                                    g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                                }

                        }
                        title_process_complete = true;
                    }
                    else // новый лист для титульного заголовка
                    {
                        if (Jpeg_checkbox.Checked)
                            current_bmp.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + pic_count.ToString("000000") + ".jpg");

                        if (Pdf_checkbox.Checked)
                        {
                            PdfPage pPage = new PdfPage();
                            pPage.Size = PdfSharp.PageSize.A4;
                            doc.Pages.Add(pPage);
                            xgr = XGraphics.FromPdfPage(doc.Pages[pic_count - 1]);
                            img = XImage.FromGdiPlusImage(current_bmp);
                            xgr.DrawImage(img, 0, 0, 595, 842);
                            xgr.Dispose();
                            img.Dispose();
                        }

                        current_bmp.Dispose();
                        pic_count++;
                        g.Dispose();
                        current_bmp = new Bitmap(width, height);
                        free_space_h = height - indent_logo_h - logo_h - info_h - info_indent_h;
                        start_p = new Point(indent_page_w, indent_logo_h + logo_h);
                        g = Graphics.FromImage(current_bmp);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.Clear(Color.White);
                        rectf = new RectangleF(0, 0, logo.Width, logo.Height);
                        dest_rect = new RectangleF(indent_logo_w, indent_logo_h, width - (2 * indent_logo_w), logo_h);
                        g.DrawImage(logo, dest_rect, rectf, GraphicsUnit.Pixel);
                        rectf = new RectangleF(0, 0, info.Width, info.Height);
                        dest_rect = new RectangleF(indent_logo_w, height - info_h, width - (2 * indent_logo_w), info_line_h);
                        g.DrawImage(info, dest_rect, rectf, GraphicsUnit.Pixel);
                    }

                if (Pdf_checkbox.Checked)
                {
                    doc.Save(save_path + "\\" + Procedures.Save_name(root_folder.Name) + ".pdf");
                    doc.Close();
                }
                work_files_process_complete = true;
                doc.Dispose();
                current_bmp.Dispose();
                g.Dispose();
            }
        }

        private void Path_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.path_Textbox.Text = folderBrowserDialog1.SelectedPath;
                path = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Save_path_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.save_Textbox.Text = folderBrowserDialog1.SelectedPath;
                save_path = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Exit_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Завершить работу программы?", "Выход", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                this.Close();
        }
    }
}
