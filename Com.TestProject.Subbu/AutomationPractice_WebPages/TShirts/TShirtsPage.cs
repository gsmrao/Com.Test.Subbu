using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Com.TestProject.Subbu.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;



namespace Com.TestProject.Subbu.AutomationPractice_WebPages.TShirts
{
    public class TShirtsPage
    {
        IWebDriver _driver;
        IWebElement webElement;
        WebConnector selenium = new WebConnector();

        public TShirtsPage(IWebDriver _driver)
        {
            this._driver = _driver;
            PageFactory.InitElements(_driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='center_column']/ul/li/div/div[1]/div/a[1]/img")]
        public IWebElement TShirts_ImageLink { get; set; }


        [FindsBy(How = How.XPath, Using = "html/body/div/div[2]/div/div[3]/div[2]/ul/li/div/div[2]/div[1]")]
        public IWebElement TShirts_ImageArea { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div/div[2]/div/div[3]/div/div/div/div[4]/form/div/div[3]/div[1]/p/button")]
        public IWebElement TShirts_AddCartButton { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div/div[1]/header/div[3]/div/div/div[4]/div[1]/div[2]/div[4]/a")]
        public IWebElement TShirts_ProceedToCheckOutButton { get; set; }


        
        [FindsBy(How = How.XPath, Using = "//*[@id='center_column']/p[2]/a[1]")]
        public IWebElement TShirts_SummaryTab_ProceedToCheckOutButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='center_column']/form/p/button")]
        public IWebElement TShirts_AddressTab_ProceedToCheckOutButton { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@id='cgv']")]
        public IWebElement TShirts_ShippingTab_AgreeTermsCheckBox { get; set; }

        
        [FindsBy(How = How.XPath, Using = "//*[@id='form']/p/button")]
        public IWebElement TShirts_ShippingTab_ProceedToCheckOutButton { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@id='HOOK_PAYMENT]/div[1]/div/p/a")]
        public IWebElement TShirts_ShippingTab_PayByBankwireButton { get; set; }

        
        [FindsBy(How = How.XPath, Using = "//*[@id='cart_navigation']/button")]
        public IWebElement TShirts_PaymentTab_ConfirmOrder { get; set; }

        

        
        WebDriverWait wait;
        public string OrderTShirtAndGetOrderDetails()
        {
            Dictionary<string, string> dicOrderConformationDetails = new Dictionary<string, string>();
            IWebElement myOrderCompleteMsg;

            TShirts_ImageArea.Click();

            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));

            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(200));

            
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/div/div[1]/header/div[3]/div/div/div[4]/div[1]/div[2]/div[4]/a")));
            TShirts_ProceedToCheckOutButton.Click();

            //Proceed to checkout Cart Summary
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='center_column']/p[2]/a[1]")));
            TShirts_SummaryTab_ProceedToCheckOutButton.Click();
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(200));


            //Click on Proceed to check out Order in Address tab
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='center_column']/form/p/button")));
            TShirts_AddressTab_ProceedToCheckOutButton.Click();


            //Click on Proceed to check out Order in Shipping tab
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='uniform-cgv']")));

            TShirts_ShippingTab_AgreeTermsCheckBox.Click();

            TShirts_ShippingTab_ProceedToCheckOutButton.Click();

            IWebElement PayByBankwireButton = _driver.FindElement(By.XPath("//*[@id='HOOK_PAYMENT']/div[1]/div/p/a"));
            PayByBankwireButton.Click();

            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='cart_navigation']/button")));
            TShirts_PaymentTab_ConfirmOrder.Click();

            //Get Order confirmation details
            myOrderCompleteMsg = _driver.FindElement(By.XPath("//*[@id='center_column']/div/p/strong"));
            dicOrderConformationDetails.Add("orderConfirmMessage", myOrderCompleteMsg.Text);
            //*[@id='center_column']/div				
            myOrderCompleteMsg = _driver.FindElement(By.XPath("//*[@id='center_column']/div"));

            string[] stringSeparators = new string[] { "\r\n" };
            string[] orderInfMsgs =  myOrderCompleteMsg.Text.Split(stringSeparators, StringSplitOptions.None);

            string strOrderReferenceNumber = string.Empty; 
            foreach (var item in orderInfMsgs)
            {
                if (item.Contains("- Do not forget to insert your order reference"))
                {
                    var orderRefNumber = Regex.Matches(item.ToString(), @"(\b[A-Z][A-Z]+|\b[A-Z]\b)").Cast<Match>().Select(m => m.Value);
                    foreach (var refData in orderRefNumber)
                    {
                        dicOrderConformationDetails.Add("orderReferenceNo", refData.ToString());
                        strOrderReferenceNumber = refData.ToString();
                    }
                }
            }
           
           

            //Amount
            myOrderCompleteMsg = _driver.FindElement(By.XPath("//*[@id='center_column']/div/span"));
            dicOrderConformationDetails.Add("orderAmount", myOrderCompleteMsg.Text.ToString());

            return strOrderReferenceNumber;
        }

    }
}
