using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pic_print.Lib
{
    class Classes
    {
        public class Subtitle_folder
        {
            public string name;
            public DirectoryInfo folder;
            public List<string> w_pic;
            public List<string> h_pic;
            public Subtitle_folder(string name, DirectoryInfo folder, List<string> w_list, List<string> h_list, List<string> all_list)
            {
                this.name = name;
                this.folder = folder;
                this.w_pic = w_list;
                this.h_pic = h_list;
                this.all_pic = all_list;
            }
            public bool empty = true;
            public List<string> all_pic;
        }

        public class Header_folder
        {
            public string name;
            public DirectoryInfo folder;
            public List<Subtitle_folder> sub_list;
            public List<string> w_pic;
            public List<string> h_pic;
            public bool empty = true;
            public List<string> all_pic;
            public Header_folder(string name, DirectoryInfo folder, List<Subtitle_folder> list, List<string> w_list, List<string> h_list, List<string> all_list)
            {
                this.name = name;
                this.folder = folder;
                this.sub_list = list;
                this.w_pic = w_list;
                this.h_pic = h_list;
                this.all_pic = all_list;
            }
        }

        public class Title_folder
        {
            public string name;
            public DirectoryInfo folder;
            public List<Header_folder> header_list;
            public List<string> w_pic;
            public List<string> h_pic;
            public bool empty = true;
            public List<string> all_pic;
            public Title_folder(string name, DirectoryInfo folder, List<Header_folder> list, List<string> w_list, List<string> h_list, List<string> all_list)
            {
                this.name = name;
                this.folder = folder;
                this.header_list = list;
                this.w_pic = w_list;
                this.h_pic = h_list;
                this.all_pic = all_list;
            }
        }
    }
}
