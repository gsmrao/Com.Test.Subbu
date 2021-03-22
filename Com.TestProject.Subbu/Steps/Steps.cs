using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;
using Com.TestProject.Subbu.AutomationPractice_WebPages.TShirts;
using TechTalk.SpecFlow.NUnit;
using NUnit.Framework;
using Com.TestProject.Subbu.AutomationPractice_WebPages;
using System.Text.RegularExpressions;

namespace Com.TestProject.Subbu.Steps
{
    [Binding]
    public class Steps
    {
        IWebDriver driver;

        IndexPage indexPage;
        TShirtsPage tShirtsPage;
        TShirt_OrderHistory shirt_OrderHistory;
        TShirts_OrderSummary shirts_OrderSummary;
        public List<Dictionary<string, string>> dictOrderSummaryDetails = new List<Dictionary<string, string>>();

        SignInPage signInPage;
        PersonalInformation personalInformation;


        IEnumerable<OrderDetails> orderDetails = null;

        public string OrderReferenceNumber { get; set; }

        [Given(@"I launch below application using ""(.*)"" and ""(.*)"" browser")]
        public void GivenILaunchBelowApplicationUsingAndBrowser(string strSiteURL, string strBrowserType)
        {
            

            if (strBrowserType == "Chrome")
            {
                driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            driver.Navigate().GoToUrl(strSiteURL);

            driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);

        }

        [Then(@"I click on T-SHIRTS tab")]
        public void ThenIClickOnT_SHIRTSTab()
        {
            indexPage = new IndexPage(driver);
            indexPage.TShirtsLinkClick();

        }
        [Then(@"I select and click on T-Shirts AddCart")]
        public void ThenISelectAndClickOnT_ShirtsAddCart()
        {
            tShirtsPage = new TShirtsPage(driver);
            tShirtsPage.OrderTShirtAndGetOrderDetails();
            //tShirtsPage.AddCart_Click();
        }

        [Then(@"I validae Order Summary")]
        public void ThenIValidaeOrderSummary(Table table)
        {
            orderDetails = table.CreateSet<OrderDetails>();

            foreach (var expOrderDetails in orderDetails)
            {
                for (int i = 0; i < dictOrderSummaryDetails.Count; i++)
                {
                    foreach (var actOrderDetails in dictOrderSummaryDetails[i])
                    {
                        //| productName                 | unitPrice | qty | total  |
                        if (nameof(expOrderDetails.productName) == actOrderDetails.Key)
                        {
                            if (actOrderDetails.Value.Contains(expOrderDetails.productName))
                            {
                                Assert.AreEqual(true, true);
                            }
                            else
                            {
                                Assert.AreEqual(false, true);
                            }

                        }
                        else if (nameof(expOrderDetails.unitPrice) == actOrderDetails.Key)
                        {
                            Assert.AreEqual(expOrderDetails.unitPrice, actOrderDetails.Value);

                        }
                        else if (nameof(expOrderDetails.qty) == actOrderDetails.Key)
                        {
                            Assert.AreEqual(expOrderDetails.qty, actOrderDetails.Value);

                        }
                        else if (nameof(expOrderDetails.total) == actOrderDetails.Key)
                        {
                            Assert.AreEqual(expOrderDetails.total, actOrderDetails.Value);

                        }
                    }
                }
            
            }
            driver.Quit();
        }

        [Then(@"I click on Sigin button")]
        public void ThenIClickOnSiginButton()
        {
            signInPage = new SignInPage(driver);
            signInPage.ClickOnSiginTab();
        }
        [Then(@"Enter login details ""(.*)"" and ""(.*)""")]
        public void ThenEnterLoginDetailsAnd(string userName, string password)
        {
            signInPage = new SignInPage(driver);
            signInPage.ClickOnSignInButton(userName, password);
        }

        [Then(@"Update Personal Information ""(.*)"" and ""(.*)""")]
        public void ThenUpdatePersonalInformationAnd(string strFirstName, string strCurrentPassword)
        {
            personalInformation = new PersonalInformation(driver);
            personalInformation.UpdatePersonalInformation(strFirstName, strCurrentPassword);
            driver.Quit();
        }

        [Then(@"I select T-Shirt and I Order the T-Shirts")]
        public void ThenISelectT_ShirtAndIOrderTheT_Shirts()
        {
            tShirtsPage = new TShirtsPage(driver);
            // Add to shopping Cart and ProceedtoCheckout
            OrderReferenceNumber = tShirtsPage.OrderTShirtAndGetOrderDetails();

            //
        }

        [Then(@"Click on login id from menu link and Click on Order Summary details")]
        public void ThenClickOnLoginIdFromMenuLinkAndClickOnOrderSummaryDetails()
        {
            signInPage = new SignInPage(driver);
            signInPage.ClickOnUserNameButton();

            //Click on Order Summary Details
            shirts_OrderSummary = new TShirts_OrderSummary(driver);
            dictOrderSummaryDetails = shirts_OrderSummary.GetOrderSummaryDetails(OrderReferenceNumber);

        }

    }

    public class OrderDetails
    { 
        public string productName { get; set; }
        public string colorAndSize { get; set; }
        public string unitPrice { get; set; }
        public string qty { get; set; }
        public string total { get; set; }
        public OrderDetails() { }
    }
    public class OrderReferenceDetails
    {
        public string orderConfirmMessage { get; set; }
        public string orderAmount { get; set; }
        public string orderReferenceNo { get; set; }
    }


}
