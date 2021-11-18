using Linkedin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Linkedin
{
    public partial class FindFromFileForm : Form
    {
        public FindFromFileForm()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.xlsx)|*.xlsx";
            Start("C:\\Users\\дима\\Downloads\\Копия European VC list for Defendocs.xlsx");
        }

        

        //"C:\\Users\\дима\\Downloads\\Копия European VC list for Defendocs.xlsx"
        private void AddFileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            fileNameLabel.Text = Path.GetFileName(filename);
            MessageBox.Show("Файл открыт");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = openFileDialog1.FileName;
            
        }
        private void Start(string fileName)
        {
            ExcelWorker exWorker = new ExcelWorker();
            var list = exWorker.GetListOfNames(fileName);

        }
    }
}
