using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Linkedin
{
    public partial class Form1 : Form
    {
        private IWebDriver _driver { get; set; }
        private const string UserName = "akarelin19821982@gmail.com";
        private const string Password = "RPR4J3Jjpv";
        private List<string> _accountLinks = new List<string>();
        private int maxNumberOfAccount = 100;

        public Form1()
        {
            InitializeComponent();
            Start(null, null);
        }

        private void Start(object sender, EventArgs e)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(chromeDriverService, new ChromeOptions());
            _driver.Url = "https://www.linkedin.com/login";
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            IWait<IWebDriver> wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30.00));

            wait.Until(driver1 =>
                ((IJavaScriptExecutor) _driver).ExecuteScript("return document.readyState").Equals("complete"));

            var emailInput = _driver.FindElement(By.Id("username"));
            emailInput.SendKeys(UserName);
            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(Password);

            var signInButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            signInButton.Click();

            _driver.Url =
                "https://www.linkedin.com/search/results/people/?geoUrn=%5B%22101282230%22%5D&keywords=dpo&network=%5B%22S%22%2C%22O%22%5D&origin=FACETED_SEARCH";

            do
            {
                GetAllAccountsFromCurrentPage();
                GoToAnotherPage();
            } while (_accountLinks.Count < maxNumberOfAccount);

            MainLabel.Text = "Sex";
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

        private void GoToAnotherPage()
        {
            var element = _driver.FindElement(By.Id("li-modal-container"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(element);
            actions.Perform();

            var nextPageButton = _driver.FindElement(By.CssSelector("button[aria-label=\"Далее\"]"));
            nextPageButton.Click();
        }
    }
}