using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace RecEngine
{
    public partial class Form1 : Form
    {
        public string sampletext = "Type here a small text";
        public bool editsampletext;
        public Form1()
        {
            InitializeComponent();
            textBox2.Text = sampletext;
            textBox2.ForeColor = Color.Black;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void txt_Enter(object sender, EventArgs e)
        {
            if (!editsampletext)
            {
                textBox2.Clear();
                textBox2.ForeColor = Color.Black;
            }
        }
        private void button_click(object sender, EventArgs e)
        {
            makeFile();
            
            string[] lines = File.ReadLines("C:\\ChristmasList.txt").ToArray();
            string checkGender = checkMaleOrFemale(lines[0]);
            textBox1.Text = checkGender;

            string posOrNeg = isWordNegativeOrPositive(lines[1]);
            textBox4.Text = posOrNeg;

            List<string> gift = new List<string>(lines);
            gift.RemoveAt(0);
            gift.RemoveAt(0);

            string result = String.Join(" ", gift.ToArray());
            textBox5.Text = result;

            string finalgift;
            
            if (textBox4.Text == "Negative")
            {
                string neg = textBox1.Text;
                finalgift = scrapeAmazon(neg);
                textBox6.Text = finalgift;
            }
            else
            {
                finalgift = scrapeAmazon(result);
                textBox6.Text = finalgift;
            }


        }
        public void makeFile()
        {
            StreamWriter sw = new StreamWriter("C:\\ChristmasList.txt");

            string words = textBox2.Text;

            string[] space = words.Split(' ');

            foreach (var item in space)
            {
                sw.WriteLine(item);
            }

            sw.Close();
        }
        public string checkMaleOrFemale(string gender)
        {
            string data = getAPIDataForGender(gender);
            List<string> info = parseDataForGender(data);
            return info[0];
        }
        public string isWordNegativeOrPositive(string word)
        {
            string[] posWords = {"like","likes", "love", "loves", "adore", "adores", 
                 "want", "wants", "need", "needs", "enjoy", "enjoys", "prefer", "prefers" };

            if (posWords.Contains(word))
            {
                return "Positive";
            }
            else
            {
                return "Negative";
            }
        }
        private static string getAPIDataForGender(string name)
        {
            WebRequest wr = WebRequest.Create("https://api.genderize.io?name=" + name);
            WebResponse response = wr.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader dataReader = new System.IO.StreamReader(stream);
            string allData = dataReader.ReadToEnd();
            return allData;
        }
        private static List<String> parseDataForGender(string data)
        {
            List<String> information = new List<String>();
            dynamic jsonData = JObject.Parse(data);
            string probableGender = jsonData.gender;
            information.Add(probableGender);
            return information;
        }
        public static string scrapeAmazon(string gift)
        {
            HtmlAgilityPack.HtmlWeb website = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument document = website.Load("https://www.amazon.nl/s?k=" + gift + " gift");
            var datalist = document.DocumentNode.SelectNodes("//span[@class='a-size-base-plus a-color-base a-text-normal']").ToList();
            string url = "https://www.amazon.nl/s?k=" + gift + " gift";


            foreach (var item in datalist)
            {
                if (true)
                {
                    string s = datalist[0].InnerHtml;
                    string ss = datalist[1].InnerHtml;
                    string sss = datalist[2].InnerHtml;

                    return "1) " + s + "---------------" + "2) " + ss + "---------------" + "3) " + sss + "---------------" + url;


                }
                
            }
            return "nothing found";
        }
    }
}
