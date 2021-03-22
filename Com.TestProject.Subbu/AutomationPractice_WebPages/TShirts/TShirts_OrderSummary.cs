using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Com.TestProject.Subbu.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace Com.TestProject.Subbu.AutomationPractice_WebPages.TShirts
{
    public class TShirts_OrderSummary
    {
        IWebDriver _driver;
        IWebElement webElement;
        WebConnector selenium = new WebConnector();

        public TShirts_OrderSummary(IWebDriver _driver)
        {
            this._driver = _driver;
            PageFactory.InitElements(_driver, this);
        }

        Dictionary<string, string> dictOrderSummary = new Dictionary<string, string>();
        WebDriverWait wait;


        
        //Order History and Details
        [FindsBy(How = How.XPath, Using = "//*[@id='center_column']/div/div[1]/ul/li[1]/a")]
        public IWebElement OrderHisAndDetailsButton { get; set; }


        //Order History and Details
        [FindsBy(How = How.XPath, Using = "//*[@id='center_column']/div/div[1]/ul/li[1]/a")]
        public IWebElement OrderListTable { get; set; }


        //DEtails button from grid
        [FindsBy(How = How.XPath, Using = "/html/body/div/div[2]/div/div[3]/div/div/table/tbody/tr/td[1]")]
        public IWebElement OrderSumaryTable_OrderReferenceLink { get; set; }

        //Oder details table
        //  
        [FindsBy(How = How.XPath, Using = "//*[@id='order-list']/tbody")]
        public IWebElement OrderDetailsTable { get; set; }



        //ProductName  
        [FindsBy(How = How.XPath, Using = "/html/body/div/div[2]/div/div[3]/div/div/div/form[1]/div/table/tbody/tr/td[2]/label")]
        public IWebElement ProductName { get; set; }

        //Quantity 
        [FindsBy(How = How.XPath, Using = "/html/body/div/div[2]/div/div[3]/div/div/div/form[1]/div/table/tbody/tr/td[3]/label")]
        public IWebElement Quantity { get; set; }

        //Unit Price 
        [FindsBy(How = How.XPath, Using = "/html/body/div/div[2]/div/div[3]/div/div/div/form[1]/div/table/tbody/tr/td[4]/label")]
        public IWebElement UnitPrice { get; set; }

        //TOtal Price 
        [FindsBy(How = How.XPath, Using = "/html/body/div/div[2]/div/div[3]/div/div/div/form[1]/div/table/tbody/tr/td[5]/label")]
        public IWebElement TotalPrice { get; set; }


        public List<Dictionary<string, string>> GetOrderSummaryDetails( string strOrderReferenceNumber)
        {
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));

            List<Dictionary<string, string>> dctOrderDetailsList = new List<Dictionary<string, string>>();

            OrderHisAndDetailsButton.Click();
            

            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='order-list']")));
            OrderSumaryTable_OrderReferenceLink.Click();



            IWebElement myOrderListTable = _driver.FindElement(By.XPath("//*[@id='order-list']/tbody"));
            List<IWebElement> rows_table = myOrderListTable.FindElements(By.TagName("tr")).ToList();
            int i = 0;
            for (int row =1; row <= rows_table.Count(); row++)
            {
                Dictionary<string, string> dictRowData = new Dictionary<string, string>();
                List<IWebElement> rowData = rows_table[i].FindElements(By.TagName("td")).ToList();

                for (int col = 0; col < rowData.Count(); col++)
                {
                    if (col == 0)
                    {
                        string strColOrderReferenceNameLink = "//*[@id='order-list']/tbody/tr[" + row + "]/ td[" + 1 + "]/a";
                        wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(strColOrderReferenceNameLink)));
                        IWebElement orderReferenceLink = _driver.FindElement(By.XPath(strColOrderReferenceNameLink));
                        if (orderReferenceLink.Text == strOrderReferenceNumber)
                        {
                            orderReferenceLink.Click();

                            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='order-detail-content']")));
                            IWebElement myOrderInfoTable = _driver.FindElement(By.XPath("//*[@id='order-detail-content']/table/tbody"));
                            List<IWebElement> rowsOrderInfo_table = myOrderInfoTable.FindElements(By.TagName("tr")).ToList();
                            for (int rowOrderInfoData = 0; rowOrderInfoData < rowsOrderInfo_table.Count(); rowOrderInfoData++)
                            {
                                List<IWebElement> rowColData = rowsOrderInfo_table[0].FindElements(By.TagName("td")).ToList();
                                string strProductName = string.Empty;
                                string strQuantity = string.Empty;
                                string strUnitPrice = string.Empty;
                                string strTotalPrice = string.Empty;

                                for (int colData = 1; colData <= rowColData.Count(); colData++)
                                {
                                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='order-detail-content']/table/tbody/tr/td[" + colData + "]/label")));
                                    IWebElement myOrderInfoColValues = _driver.FindElement(By.XPath("//*[@id='order-detail-content']/table/tbody/tr/td[" + colData + "]/label"));
                                    if (colData == 2)
                                    {
                                        dictRowData.Add("productName", myOrderInfoColValues.Text);
                                    }
                                    else if (colData == 3)
                                    {
                                        dictRowData.Add("qty", myOrderInfoColValues.Text);
                                    }
                                    else if (colData == 4)
                                    {
                                        dictRowData.Add("unitPrice", myOrderInfoColValues.Text);
                                    }
                                    else if (colData == 5)
                                    {
                                        dictRowData.Add("total", myOrderInfoColValues.Text);
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
                if (dictRowData.Count > 0)
                {
                    dctOrderDetailsList.Add(dictRowData);
                }
                i++;
            }
            return dctOrderDetailsList;
        }
    }
}
