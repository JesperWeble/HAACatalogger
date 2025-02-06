using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace HAACatalogger
{
    public class SeleniumCrawler // A crawler that iterates through each card on my Dulst game to draw their properties and such and place them into my excel document.
    {
        // Set up webdriver
        private WebDriver WebDriver { get; set; } = null!;
        private string DriverPath { get; set; } = @"C:\Users\62864\OneDrive\My Own Projects\HAACatalogger";
        private string BaseUrl { get; set; } = "https://dulst.com";

        WriteToExcel writer = new WriteToExcel(); // Adds an instance of WriteToExcel so that this can call its method.

        public List<Card> cards = new List<Card>();

        public void DulstToExcel()
        {
            // ---Making The Webdriver---
            var options = new ChromeOptions(); 
            //options.AddArguments("--headless"); // Hide the browser from the user.

            WebDriver = new ChromeDriver(DriverPath, options);
            WebDriver.Navigate().GoToUrl(BaseUrl + "/haa/cards");
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60); // WebDriver will wait up to (FromSeconds) for any element to be present on the page before throwing an error




            // ---Variables---
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(60)); // creates an object that can used to wait for specific conditions
            IWebElement elementToExtractFrom; // Variable that will be set to the current location that we are extracting data from later.
            IWebElement elementCheckpoint;

            var cardListContainer = WebDriver.FindElement(By.CssSelector("div.mainCards > div[data-cardtype='unit'] > div.card-list-container")); // find the container for unit cards.

            wait.Until(WebDriver => cardListContainer.Displayed);
            Thread.Sleep(1000);
            var cardList = cardListContainer.FindElements(By.XPath(".//a[@href]")); // Find the actual list of cards in the container
            foreach (var card in cardList)
            {
                string cardUrl = card.GetAttribute("href"); // Find the name of the card as written in the url
                if (cardUrl.EndsWith("Template-Unit") == false) //Ensure that the name is not "Template Unit"
                {
                    card.Click();
                    Thread.Sleep(100);
                    Card cardObject = new Card();
                    try
                    {
                        elementToExtractFrom = WebDriver.FindElement(By.CssSelector("div.cardFullInfo.unit.card-info-inner"));
                        cardObject.cost = int.Parse(elementToExtractFrom.GetAttribute("data-cost"));
                        cardObject.atk = int.Parse(elementToExtractFrom.GetAttribute("data-atk"));
                        cardObject.health = int.Parse(elementToExtractFrom.GetAttribute("data-health"));
                        cardObject.expansion = elementToExtractFrom.GetAttribute("data-expansion");

                        elementCheckpoint = elementToExtractFrom; // Checkpoint at CardFullInfo
                        
                        elementToExtractFrom = elementCheckpoint.FindElement(By.CssSelector("div[rarity]")); // Get rarity of card
                        cardObject.rarity = elementToExtractFrom.GetAttribute("rarity");

                        elementToExtractFrom = elementCheckpoint.FindElement(By.CssSelector("h1.name")); // Get name of card.
                        cardObject.name = elementToExtractFrom.Text;

                        elementCheckpoint = elementCheckpoint.FindElement(By.CssSelector("ul.dataPane")); // Checkpoint at dataPane

                        elementToExtractFrom = elementCheckpoint.FindElement(By.CssSelector("li.type > div.data > span.type.tooltip")); // Get cardtype.
                        cardObject.cardtype = elementToExtractFrom.Text;

                        elementToExtractFrom = elementCheckpoint.FindElement(By.CssSelector("li.type > div.data > div.individualClasses")); // Get Groups.
                        var elementArrayToExtractFrom = elementToExtractFrom.FindElements(By.CssSelector("span"));
                        cardObject.group = elementArrayToExtractFrom.Select(span => span.Text).ToArray(); // Takes all the spans extracted and extracts their content into an array.

                        elementToExtractFrom = elementCheckpoint.FindElement(By.CssSelector("div.mainProperties > li[data-name='reqSource'] > div.data")); // Get Sources.
                        elementArrayToExtractFrom = elementToExtractFrom.FindElements(By.CssSelector("div.item"));
                        cardObject.reqSources = elementArrayToExtractFrom.Select(item => item.Text).ToArray();
                        if (cardObject.reqSources[0] == "")
                        {
                            cardObject.reqSources[0] = "Neutral";
                        }


                        Console.WriteLine("\n");
                        Console.WriteLine($"Name: {cardObject.name} | Type: {cardObject.cardtype} | Group: {string.Join(", ", cardObject.group)} | Source: {string.Join(", ", cardObject.reqSources)}");
                        Console.WriteLine("Cost: " + cardObject.cost + " | " + "Stats: " + cardObject.atk + "/" + cardObject.health);
                        Console.WriteLine("Expansion: " + cardObject.expansion + " | " + "Rarity: " + cardObject.rarity);
                        Console.WriteLine("\n");
                        cards.Add(cardObject);
                        
                        

                        var returnElement = WebDriver.FindElement(By.Id("card-properties")); // Finds the clickable bit that when clicked will go back to the full list of cards.
                        returnElement.Click();;
                    






                        //elementToExtractFrom = elementToExtractFrom.FindElement(By.CssSelector("cardDisplayInner"));

                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                }

            }





        }
    }
}