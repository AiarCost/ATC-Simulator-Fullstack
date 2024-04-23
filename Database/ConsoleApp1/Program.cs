
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Aspose.Cells;
using System;
using System.Reflection;

namespace Selenium_Automation
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Version 1.02");

            //User Input
            Console.WriteLine("What Year? (1987-2023)");
            int YearInt = int.Parse(Console.ReadLine());
            YearInt -= 1987;

            Console.WriteLine("What month? (1-12)");
            int MonthInt = int.Parse(Console.ReadLine());
            MonthInt -= 1;

            Console.WriteLine("What Day? 1-31");
            int DayInt = int.Parse(Console.ReadLine());
            DayInt -= 1;

            Console.WriteLine("Editor (0) or Build (1)?");
            int isBuild = int.Parse(Console.ReadLine());

            /******************************************/
            ChromeOptions options = new ChromeOptions();

            string fullFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;

            string codeBase = Path.GetDirectoryName(fullFilePath);

            //Directing File downloads
            string[] ParsedCodeBase = codeBase.Split("\\");
            string DownloadBase = "";

            for(int x  = 0; x < ParsedCodeBase.Length-5; x++)
            {
                if (x == 0)
                    DownloadBase = ParsedCodeBase[x];
                else
                    DownloadBase += "\\" + ParsedCodeBase[x];
            }

            string EditorDirectory = "\\ATC Simulator Fullstack\\Assets\\Resources";
            string BuildDirectory = "\\Builds\\ATC Simulator Fullstack_Data\\Resources";

            if (isBuild == 1)
                DownloadBase += BuildDirectory;
            else
                DownloadBase += EditorDirectory;

            options.AddUserProfilePreference("download.default_directory", DownloadBase);

            Console.WriteLine(DownloadBase);


            IWebDriver ArrivalDataDriver = new ChromeDriver(codeBase + "\\chromedriver-win64", options);
            ArrivalDataDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            ArrivalDataDriver.Manage().Window.Minimize();

            // This will open up the URLs for arrival and departure
            ArrivalDataDriver.Url = "https://www.transtats.bts.gov/ontime/Arrivals.aspx";
            ArrivalDataDriver.FindElement(By.Id("chkStatistics_0")).Click();
            ArrivalDataDriver.FindElement(By.Id("chkStatistics_1")).Click();
            ArrivalDataDriver.FindElement(By.Id("chkStatistics_4")).Click();


            //Arrival Preset Keys
            IWebElement ArrivalAirportSelection = ArrivalDataDriver.FindElement(By.Id("cboAirport"));
            ArrivalAirportSelection.SendKeys("LAS");

            IWebElement ArrivalAirlineSelection = ArrivalDataDriver.FindElement(By.Id("cboAirline"));
            ArrivalAirlineSelection.SendKeys("Southwest");

            //Element pushing to website for autodata fill
            ArrivalDataDriver.FindElement(By.Id("chkMonths_" + MonthInt)).Click();
            ArrivalDataDriver.FindElement(By.Id("chkDays_" + DayInt)).Click();
            ArrivalDataDriver.FindElement(By.Id("chkYears_" + YearInt)).Click();


            //Submit Data Query to site
            ArrivalDataDriver.FindElement(By.Id("btnSubmit")).Click();
            //Download and close
            ArrivalDataDriver.FindElement(By.Id("DL_CSV")).Click();

            WebDriverWait ArrivalWait = new WebDriverWait(ArrivalDataDriver, TimeSpan.FromSeconds(5));
            ArrivalWait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException), typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            try
            {
                ArrivalWait.Until(ExpectedConditions.ElementExists(By.Id("TESTONLYFORWAITINGTIME")));

            }
            catch
            {
                Console.WriteLine("Continuing to Departure");
            }

            ArrivalDataDriver.Close();



            IWebDriver DepartureDataDriver = new ChromeDriver(codeBase + "\\chromedriver-win64", options);
            DepartureDataDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            DepartureDataDriver.Manage().Window.Minimize();

            DepartureDataDriver.Url = "https://www.transtats.bts.gov/ontime/Departures.aspx";
            DepartureDataDriver.FindElement(By.Id("chkStatistics_0")).Click();
            DepartureDataDriver.FindElement(By.Id("chkStatistics_1")).Click();
            DepartureDataDriver.FindElement(By.Id("chkStatistics_4")).Click();

            //Departure Preset Keys
            IWebElement DepartureAirportSelection = DepartureDataDriver.FindElement(By.Id("cboAirport"));
            DepartureAirportSelection.SendKeys("LAS");

            IWebElement DepartureAirlineSelection = DepartureDataDriver.FindElement(By.Id("cboAirline"));
            DepartureAirlineSelection.SendKeys("Southwest");

            //Departure Data
            DepartureDataDriver.FindElement(By.Id("chkMonths_" + MonthInt)).Click();
            DepartureDataDriver.FindElement(By.Id("chkDays_" + DayInt)).Click();
            DepartureDataDriver.FindElement(By.Id("chkYears_" + YearInt)).Click();

            //Submite Data Query to site
            DepartureDataDriver.FindElement(By.Id("btnSubmit")).Click();
            //Download and close
            DepartureDataDriver.FindElement(By.Id("DL_CSV")).Click();

            WebDriverWait DepartureWait = new WebDriverWait(DepartureDataDriver, TimeSpan.FromSeconds(5));
            DepartureWait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException), typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            try
            {
                DepartureWait.Until(ExpectedConditions.ElementExists(By.Id("TESTONLYFORWAITINGTIME")));

            }
            catch
            {
                Console.WriteLine("Finished, going to close now.");
            }

            DepartureDataDriver.Close();



            //File.Delete(DownloadBase + "\\Detailed_Statistics_Arrivals.csv");
            var Arrival = new HashSet<string>(File.ReadAllLines(DownloadBase + "\\Detailed_Statistics_Arrivals.csv").Skip(8));
            File.WriteAllLines(DownloadBase+"\\Detailed_Statistics_Arrivals.csv", Arrival);

            //Workbook arrivalBook = new Workbook(DownloadBase + "\\Detailed_Statistics_Arrivals.csv");
            //arrivalBook.Save(DownloadBase + "\\ArrivalJSON.json", SaveFormat.Json);


            //File.Delete(DownloadBase + "\\Detailed_Statistics_Departures.csv");
            var Departure = new HashSet<string>(File.ReadAllLines(DownloadBase + "\\Detailed_Statistics_Departures.csv").Skip(8));
            File.WriteAllLines(DownloadBase + "\\Detailed_Statistics_Departures.csv", Departure);

            //Workbook departureBook = new Workbook(DownloadBase + "\\Detailed_Statistics_Departures.csv");
            //departureBook.Save(DownloadBase + "\\DepartureJSON.json", SaveFormat.Json);

            return;

        }
    }
}