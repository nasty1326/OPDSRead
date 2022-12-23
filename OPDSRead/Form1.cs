using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Policy;

namespace OPDSRead
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Catalog
        {

            public string Name { get; set; } //название каталога
            public string SubName { get; set; } // подзаголовок каталога
            public string URLPictureCatalog { get; set; } // иконка каталога 
            public string URLSearch { get; set; } // ссылка на поиск  

            public Url URLPicturesC { get; set; }
            public Url URLSearchC { get; set; }
          
        }
        public Url StringToURL(string s)
        {
            Url uri = new Url(s);
            return uri;
        }
        public class Genre
        {
            public string Name { get; set; }
            public string URLGenre { get; set; }
            public string Content { get; set; }
            public string ID { get; set; }
        }

        public class Book
        {
            public string Name { get; set; }            
            public string ID { get; set; }
            public string Content { get; set; }
            public string URLPicturesBook { get; set; }
            public string URLSaveBook { get; set; }
        }

        public void ReadXMLAll()
        {
            // загружаем библиотеку
            XmlDocument doc = new XmlDocument();
            doc.Load("https://dimonvideo.ru/lib.xml"); 
            Catalog catalog = new Catalog(); // создаем каталог
            List <Genre> genres = new List<Genre>(); // создаем список сортировок и выборок
            List <Book>book=new List<Book>(); // создаем список книг
            XmlElement element = doc.DocumentElement;
            foreach (XmlNode node in element)
            {   
                //Считываем Данные о библиотеке

                if (node.Name == "title") { catalog.Name = node.InnerText; }
                if (node.Name == "subtitle") catalog.SubName = node.InnerText;
                if (node.Name == "icon") catalog.URLPictureCatalog = node.InnerText;
                if (node.Name=="link"&& node.Attributes.GetNamedItem("type").InnerText=="application/atom+xml")
                {
                    catalog.URLSearch = node.Attributes.GetNamedItem("href").InnerText;
                }

                //Считываем данные о разделах

                if (node.Name== "entry"&& node.ChildNodes.Count==6)
                {
                    Genre gen= new Genre();
                    foreach(XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name=="title") gen.Name= node2.InnerText;
                        if (node2.Name == "link" && node2.Attributes.GetNamedItem("type").InnerText == "application/atom+xml;profile=opds-catalog")
                        {
                            gen.URLGenre = node2.Attributes.GetNamedItem("href").InnerText;
                        }
                        if (node2.Name == "id") gen.ID = node2.InnerText;
                        if (node2.Name == "content") gen.Content = node2.InnerText;
                    }
                    genres.Add(gen);
                }

                //Считываем данные о разделах

                if (node.Name == "entry" && node.ChildNodes.Count == 11)
                {
                    Book bo = new Book();
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "title") bo.Name = node2.InnerText;
                        if (node2.Name == "id") bo.ID = node2.InnerText;
                        if (node2.Name == "content") bo.Content = node2.InnerText;
                        if (node2.Name == "link" && node2.Attributes.GetNamedItem("type").InnerText == "image/jpg")
                        {
                            bo.URLPicturesBook = node2.Attributes.GetNamedItem("href").InnerText;
                        }
                        if (node2.Name == "link" && node2.Attributes.GetNamedItem("type").InnerText == "application/fb2+zip")
                        {
                            bo.URLSaveBook= node2.Attributes.GetNamedItem("href").InnerText;
                        }

                    }
                    book.Add(bo);
                }            

            }

            //Console.WriteLine(genres.Count);
            //Console.WriteLine(genres.Count);

            //for (int i = 0; i < genres.Count; i++)
            //{
            //    Console.WriteLine(genres[i].Name);
            //    Console.WriteLine(genres[i].ID);
            //    Console.WriteLine(genres[i].Content);
            //    Console.WriteLine(genres[i].URLGenre);
            //}


            //Console.WriteLine(catalog.Name);
            //Console.WriteLine(catalog.SubName);
            //Console.WriteLine(catalog.URLPictureCatalog);
            //Console.WriteLine(catalog.URLSearch);

            Url t = StringToURL(genres[0].URLGenre);
            Console.WriteLine(t);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ReadXMLAll();
        }
    }
}
