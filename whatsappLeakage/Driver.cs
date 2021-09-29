using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace whatsappLeakage
{
    class Driver
    {

        ChromeDriver driver;
        int profile;

        public Driver(int pp)
        {
            profile = pp;
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions chromeOptions = new ChromeOptions();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            chromeOptions.AddArguments("user-data-dir=" + path + "\\Profiles\\Profile " + pp);
            chromeOptions.AddArguments("--incognito");
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            driver = new ChromeDriver(driverService, chromeOptions);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["source"] = "Object.defineProperty(navigator, 'webdriver', {get: () => undefined})";
            driver.ExecuteChromeCommand("Page.addScriptToEvaluateOnNewDocument", parameters);
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl("https://web.whatsapp.com/");
        }

        public ChromeDriver GetDriver()
        {
            return driver;
        }
        public int getProfile()
        {
            return profile;
        }
    }
}
