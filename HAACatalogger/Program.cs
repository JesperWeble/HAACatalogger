namespace HAACatalogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] sources = ["Neutral", "Idol", "Madoushi", "Senshi", "Gunshi", "Banchou", "Okashii"];
            SeleniumCrawler crawler = new SeleniumCrawler();
            WriteToExcel writer = new WriteToExcel();
            crawler.DulstToExcel();
            //writer.ImportToExcel(crawler.cards);
           
            
        }
    }
}
