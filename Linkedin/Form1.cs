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
using OpenQA.Selenium.Support.UI;

namespace Linkedin
{
    public partial class Form1 : Form
    {
        private IWebDriver _driver { get; set; }
        private const string UserName = "akarelin19821982@gmail.com";
        private const string Password = "RPR4J3Jjpv";

        public Form1()
        {
            InitializeComponent();
            Start(null,null);
        }

        private void Start(object sender, EventArgs e)
        {
            _driver = new ChromeDriver();
            _driver.Url = "https://www.linkedin.com/login";

            IWait<IWebDriver> wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30.00));

            wait.Until(driver1 => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));

            var emailInput = _driver.FindElement(By.Id("username"));
            emailInput.SendKeys(UserName);
            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(Password);

            var signInButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            signInButton.Click();




        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
