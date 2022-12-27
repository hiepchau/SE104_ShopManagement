using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace SE104_OnlineShopManagement.Test.UI_Test
{
    internal class AuthenticateScreenTest
    {
        private string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private string CalculatorAppId = AppDomain.CurrentDomain.BaseDirectory + "SE104_OnlineShopManagement.exe";

        protected static WindowsDriver<WindowsElement> desktopSession;
        [SetUp]
        public void Setup()
        {
            if (desktopSession == null)
            {
                AppiumOptions _appCapabilities = new AppiumOptions();
                _appCapabilities.AddAdditionalCapability("app", CalculatorAppId);
                _appCapabilities.AddAdditionalCapability("deviceName", "WindowsPC");
                desktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), _appCapabilities);
                Assert.IsNotNull(desktopSession);
                desktopSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }
        }
        [TearDown]
        public void TearDown()
        {
            desktopSession.Quit();
            desktopSession = null;
        }
        [Test]
        public void TestLogin1()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtCompany\"]").SendKeys("123");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtUsername\"]").SendKeys("1");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"PasswordBox\"]").SendKeys("1");
            Assert.IsTrue(desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"LOG IN\"][@AutomationId=\"loginBtn\"]").Enabled);
        }
        [Test]
        public void TestLogin2()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtCompany\"]").SendKeys("321");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtUsername\"]").SendKeys("hehe");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"PasswordBox\"]").SendKeys("Test");
            Assert.IsTrue(desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"LOG IN\"][@AutomationId=\"loginBtn\"]").Enabled);
        }

        [Test]
        public void TestFailLogin1()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtCompany\"]").SendKeys("123");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtUsername\"]").SendKeys("1");
            Assert.IsFalse(desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"LOG IN\"][@AutomationId=\"loginBtn\"]").Enabled);
        }
        [Test]
        public void TestFailLogin2()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtCompany\"]").SendKeys("123");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"PasswordBox\"]").SendKeys("1");
            Assert.IsFalse(desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"LOG IN\"][@AutomationId=\"loginBtn\"]").Enabled);
        }
        [Test]
        public void TestFailLogin3()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Edit[@AutomationId=\"txtUsername\"]").SendKeys("1");
            Assert.IsFalse(desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"LOG IN\"][@AutomationId=\"loginBtn\"]").Enabled);
        }
        [Test]
        public void TestRegister()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"Create Account\"][@AutomationId=\"signupBtn\"]").Click();
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"FirstNameBox\"]").SendKeys("Test");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Custom[@AutomationId=\"BirthdayPicker\"]/Edit[@AutomationId=\"PART_TextBox\"]").SendKeys("20/12/2005");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/ComboBox[@AutomationId=\"CbSex\"]").Click();
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Window[@ClassName=\"Popup\"]/ListItem[@ClassName=\"ListBoxItem\"][@Name=\"Male\"]").Click(); desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"UsernameBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"PasswordBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"VerifyPasswordBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"CompanyBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Button[@Name=\"Dang ky\"][@AutomationId=\"signUpBtn\"]").Click();
        }
        [Test]
        public void TestFailRegister()
        {
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"LoginWindow\"]/Button[@Name=\"Create Account\"][@AutomationId=\"signupBtn\"]").Click();
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"FirstNameBox\"]").SendKeys("Test");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"LastNameBox\"]").SendKeys("Test");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Custom[@AutomationId=\"BirthdayPicker\"]/Edit[@AutomationId=\"PART_TextBox\"]").SendKeys("20/12/2005");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/ComboBox[@AutomationId=\"CbSex\"]").Click();
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Window[@ClassName=\"Popup\"]/ListItem[@ClassName=\"ListBoxItem\"][@Name=\"Male\"]").Click();
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"UsernameBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"PasswordBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"VerifyPasswordBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Edit[@AutomationId=\"CompanyBox\"]").SendKeys("TESTING");
            desktopSession.FindElementByXPath("/Window[@ClassName=\"Window\"]/Custom[@ClassName=\"RegisterForm\"]/Button[@Name=\"Dang ky\"][@AutomationId=\"signUpBtn\"]").Click();
            Thread.Sleep(5000);
            Assert.IsNotNull(desktopSession.FindElementByClassName("Window"));
        }
    }
}