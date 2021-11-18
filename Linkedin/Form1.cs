using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linkedin.Entities;
using Linkedin.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Linkedin
{
    public partial class Form1 : Form
    {
        private IWebDriver _driver { get; set; }
        private List<string> _accountLinks = new List<string>();
        private decimal maxNumberOfAccount;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Start(object sender, EventArgs e)
        {
            maxNumberOfAccount = numericUpDown1.Value;
            MessageLabel.Text = "Getting accounts...";
            await Task.Run(GetAccountLinks);

            MessageLabel.Text = $"Getting mails...";
            await Task.Delay(200);
            var profiles = await Task.Run(GetProfileInfoForEveryLink);
            MessageLabel.Text = $"Saving into Excel file...";
            SaveIntoExcelFile(profiles);
            OpenResultFile();
            MessageLabel.Text = $"Done!";
        }

        private void OpenResultFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            var directory = new DirectoryInfo(path);
            var myFile = directory.GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First();
            System.Diagnostics.Process.Start(myFile.DirectoryName);
            System.Diagnostics.Process.Start(myFile.DirectoryName + $"/{myFile.Name}");
        }

        private List<Profile> GetProfileInfoForEveryLink()
        {
            AccountDetailer detailer = new AccountDetailer(rocketApiKey.Text);
            var profiles = detailer.GetDetailsForLinks(_accountLinks);
            return profiles;
        }

        private void SaveIntoExcelFile(List<Profile> profiles)
        {
            var excelWorker = new ExcelWorker();
            excelWorker.SaveIntoExcel(profiles);
        }

        private void GetAccountLinks()
        {
            try
            {
                InitializeDriver();
                LogIn();

                _driver.Url = linkBox.Text;
                //https://www.linkedin.com/search/results/people/?geoUrn=%5B%22101282230%22%5D&keywords=dpo&network=%5B%22S%22%2C%22O%22%5D&origin=FACETED_SEARCH
                do
                {
                    GetAllAccountsFromCurrentPage();
                    GoToAnotherPage();
                } while (_accountLinks.Count < maxNumberOfAccount);
            }
            finally
            {
                _driver.Close();
                _driver.Quit();
            }
        }

        private void GetAllAccountsFromCurrentPage()
        {
            var accounts = _driver.FindElements(By.ClassName("entity-result__image")).ToList()
                .Select(elem => elem.FindElement(By.ClassName("app-aware-link"))).ToList();
            foreach (var account in accounts)
            {
                var link = account.GetAttribute("href");
                _accountLinks.Add(link);
                if (_accountLinks.Count == maxNumberOfAccount) return;
            }
        }

        private void InitializeDriver()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            if (silentCheckBox.Checked)
                options.AddArgument("headless");

            _driver = new ChromeDriver(chromeDriverService, options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        private void LogIn()
        {
            _driver.Url = "https://www.linkedin.com/login";

            IWait<IWebDriver> wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30.00));

            wait.Until(driver1 =>
                ((IJavaScriptExecutor) _driver).ExecuteScript("return document.readyState").Equals("complete"));

            var emailInput = _driver.FindElement(By.Id("username"));
            emailInput.SendKeys(userNameBox.Text);
            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(passwordBox.Text);

            var signInButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            signInButton.Click();
        }

        private void GoToAnotherPage()
        {
            var element = _driver.FindElement(By.Id("li-modal-container"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(element);
            actions.Perform();

            var nextPageButton = _driver.FindElement(By.CssSelector("button[aria-label=\"Далее\"]"));
            nextPageButton.Click();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void findFromFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FindFromFileForm form = new FindFromFileForm();
            form.ShowDialog();
        }
    }
}