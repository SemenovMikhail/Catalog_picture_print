using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Pic_print.Lib
{
    class Procedures
    {
        static public string Pic_id(string name)
        {
            string result = "";
            bool check = false;
            int count = 0;
            for (int i = 0; i < name.Length && count < 2; i++)
            {
                if (name[i].ToString() == "_")
                {
                    check = !check;
                    count++;
                    continue;
                }
                if (check)
                    result += name[i].ToString();
            }
            return result;
        }

        static private string RemoveSpaces(string inputString)
        {
            inputString = inputString.Replace("  ", string.Empty);
            inputString = inputString.Trim().Replace(" ", string.Empty);

            return inputString;
        }

        static public int WalkDirectoryTree(DirectoryInfo root, List<Classes.Title_folder> list)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            int progress_count = 0;

            try
            {
                files = root.GetFiles("*.*");
            }

            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                throw new Exception(e.Message);
            }

            if (files != null)
            {
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    Classes.Title_folder new_title_folder = new Classes.Title_folder(dirInfo.Name, dirInfo, new List<Classes.Header_folder>(), new List<string>(), new List<string>(), new List<string>());
                    files = dirInfo.GetFiles("*.*");
                    foreach (FileInfo fi in files)
                    {
                        try
                        {
                            Image img = Image.FromFile(fi.FullName);
                            if (img.Height >= img.Width && !(Procedures.Pic_id(fi.Name) == ""))
                            {
                                new_title_folder.h_pic.Add(fi.Name);
                                new_title_folder.all_pic.Add(fi.Name);
                                progress_count++;
                            }
                            else
                                if (!(Procedures.Pic_id(fi.Name) == ""))
                                {
                                    new_title_folder.w_pic.Add(fi.Name);
                                    new_title_folder.all_pic.Add(fi.Name);
                                    progress_count++;
                                }
                            img.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    if (new_title_folder.all_pic.Count > 0)
                    {
                        new_title_folder.empty = false;
                        new_title_folder.all_pic.Sort(delegate(string str1, string str2) { return (RemoveSpaces(str1).CompareTo(RemoveSpaces(str2))); });
                    }
                    list.Add(new_title_folder);
                }

                list.Sort(delegate(Classes.Title_folder tf1, Classes.Title_folder tf2) { return (RemoveSpaces(tf1.name).CompareTo(RemoveSpaces(tf2.name))); });

                foreach (Classes.Title_folder tf in list)
                {
                    subDirs = tf.folder.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        Classes.Header_folder new_header_folder = new Classes.Header_folder(dirInfo.Name, dirInfo, new List<Classes.Subtitle_folder>(), new List<string>(), new List<string>(), new List<string>());
                        files = tf.folder.GetFiles("*.*");
                        foreach (FileInfo fi in files)
                        {
                            try
                            {
                                Image img = Image.FromFile(fi.FullName);
                                if (img.Height >= img.Width && !(Procedures.Pic_id(fi.Name) == ""))
                                {
                                    new_header_folder.h_pic.Add(fi.Name);
                                    new_header_folder.all_pic.Add(fi.Name);
                                    progress_count++;
                                }
                                else
                                    if (!(Procedures.Pic_id(fi.Name) == ""))
                                    {
                                        new_header_folder.w_pic.Add(fi.Name);
                                        new_header_folder.all_pic.Add(fi.Name);
                                        progress_count++;
                                    }
                                img.Dispose();
                            }
                            catch
                            {
                            }
                        }
                        if (new_header_folder.all_pic.Count() > 0)
                        {
                            new_header_folder.empty = false;
                            tf.empty = false;
                            new_header_folder.all_pic.Sort(delegate(string str1, string str2) { return (RemoveSpaces(str1).CompareTo(RemoveSpaces(str2))); });
                        }
                        tf.header_list.Add(new_header_folder);
                    }

                    tf.header_list.Sort(delegate(Classes.Header_folder hf1, Classes.Header_folder hf2) { return (RemoveSpaces(hf1.name).CompareTo(RemoveSpaces(hf2.name))); });

                    foreach (Classes.Header_folder hf in tf.header_list)
                    {
                        subDirs = hf.folder.GetDirectories();
                        foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                        {
                            Classes.Subtitle_folder new_subfolder = new Classes.Subtitle_folder(dirInfo.Name, dirInfo, new List<string>(), new List<string>(), new List<string>());
                            files = dirInfo.GetFiles("*.*");
                            foreach (FileInfo fi in files)
                            {
                                try
                                {
                                    Image img = Image.FromFile(fi.FullName);
                                    if (img.Height >= img.Width && !(Procedures.Pic_id(fi.Name) == ""))
                                    {
                                        new_subfolder.h_pic.Add(fi.Name);
                                        new_subfolder.all_pic.Add(fi.Name);
                                        progress_count++;
                                    }
                                    else
                                        if (!(Procedures.Pic_id(fi.Name) == ""))
                                        {
                                            new_subfolder.w_pic.Add(fi.Name);
                                            new_subfolder.all_pic.Add(fi.Name);
                                            progress_count++;
                                        }
                                    img.Dispose();
                                }
                                catch
                                {
                                }
                            }
                            new_subfolder.all_pic.Sort(delegate(string str1, string str2) { return (RemoveSpaces(str1).CompareTo(RemoveSpaces(str2))); });
                            if (new_subfolder.all_pic.Count > 0)
                            {
                                new_subfolder.empty = false;
                                tf.empty = false;
                                hf.empty = false;
                                new_subfolder.all_pic.Sort(delegate(string str1, string str2) { return (RemoveSpaces(str1).CompareTo(RemoveSpaces(str2))); });
                            }
                            hf.sub_list.Add(new_subfolder);
                        }
                        hf.sub_list.Sort(delegate(Classes.Subtitle_folder sf1, Classes.Subtitle_folder sf2) { return (RemoveSpaces(sf1.name).CompareTo(RemoveSpaces(sf2.name))); });
                    }
                }
            }
            return progress_count;
        }

        static public string Save_name(string name)
        {
            string new_name = "";
            for (int i = 0; i < 20 && i < name.Length; i++)
                new_name += name[i];
            return new_name;
        }

        static public  void DrawLetter(Graphics g, float width, float height, string letter, int Y, Font f)
        {
            SizeF s = new SizeF(width, height);

            Color name_color = Color.FromArgb(0, 0, 205);
            Brush brush = new SolidBrush(name_color);

            SizeF size = g.MeasureString(letter.ToString(), f);
            g.DrawString(letter, f, brush, (width - size.Width) / 2, Y);
            brush.Dispose();

        }

    }
}
