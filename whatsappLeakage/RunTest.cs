using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace whatsappLeakage
{
     class RunTest
    {
        private QueueObj obj;
        ChromeDriver driver;
        String Number;
        String Text;
        Driver dd;
        public RunTest(QueueObj obj,String Number,String Text,String TerID,Driver dd)
        {
            this.obj = obj;
            this.Number = Number;
            this.Text = Text;
            this.dd = dd;
            driver = dd.GetDriver();
        }
        public void run()
        {
            AppDomain.CurrentDomain.UnhandledException += new
             UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            try
            {
                TimeSpan.FromSeconds(130);
                driver.Navigate().GoToUrl("https://web.whatsapp.com/send?phone=" + Number + "&text=" + Text);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#app > div._1ADa8._3Nsgw.app-wrapper-web.font-fix.os-win > span:nth-child(2) > div._209uk > span > div:nth-child(1) > div > div > div > div > div._2Nr6U, #main > footer > div._2BU3P.tm2tP.copyable-area > div > div > div._2lMWa > div._3HQNh._1Ae7k > button")));
                if (driver.FindElements(By.CssSelector("#app > div._1ADa8._3Nsgw.app-wrapper-web.font-fix.os-win > span:nth-child(2) > div._209uk > span > div:nth-child(1) > div > div > div > div > div._2Nr6U")).Count > 0)
                {
                    if (driver.FindElements(By.XPath("//div[contains(text(), 'Phone number shared via url is invalid.')]")).Count > 0)
                    {
                        Console.WriteLine("Message Undelivered: Invalid Number");
                    }
                }

                if (driver.FindElements(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/div/div[2]/div[2]/button")).Count > 0)
                {

                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/div/div[2]/div[2]/button"))).Click();
                    Console.WriteLine("Message Sent");
                    Thread.Sleep(600);
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[aria-label=' Sent '],span[aria-label=' Delivered ']")));
                    Console.WriteLine("Message Delivered");
                }
                obj.releasDriver(dd);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("Profile: "+dd.getProfile());
                Console.WriteLine("Message Undelivered");
                obj.releasDriver(dd);
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
