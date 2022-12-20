

using CsvHelper;
using HtmlAgilityPack;
using System.Globalization;
using System.Reflection.Metadata;

runProgram.main();
class Book
{
    public string Title { get; set; }

    public string Price { get; set; }
}
//HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb;
//HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument;
class runProgram
{
   public static void main()
    {
        List<string> booklinks = GetBookLinks("https://books.toscrape.com/catalogue/category/books/mystery_3/index.html");
        Console.WriteLine("Found {0} links", booklinks.Count);
        List<Book> books = GetBookDetails(booklinks);
        foreach(Book b in books)
        {
            Console.WriteLine(b.Title);
            Console.WriteLine(b.Price);
        }
        exportToCSV(books);
    }
    static void exportToCSV(List<Book> books)
    {
        using (var writer = new StreamWriter(path: "books.csv")) 
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(books);
        }

    }
    static List<Book> GetBookDetails(List<string> urls)
    {
        List<Book> books = new List<Book>();
        foreach(string url in urls)
        {
            HtmlDocument document = GetDoc(url);
            string titleXPath = "//h1";
            string priceXpath = "//div[contains(@class,\"product_main\")]/p[@class=\"price_color\"]";
            Book book = new Book();
            book.Title = document.DocumentNode.SelectSingleNode(titleXPath).InnerText;
            book.Price = document.DocumentNode.SelectSingleNode(priceXpath).InnerText;
            books.Add(book);

        }
        return books;

    }

    static HtmlDocument GetDoc(string url)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);
        return doc;
    }

    static List<string> GetBookLinks(string url)
    {
        List<string> booklist = new List<string>();
        HtmlDocument doc = GetDoc(url);
        HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//h3/a");

        Uri baseURI = new Uri(uriString: url);

        foreach (HtmlNode link in linkNodes)
        {
            string href = link.Attributes["href"].Value;
            booklist.Add(item: new Uri(baseURI, relativeUri: href).AbsoluteUri);
        }
        return booklist;
    }
    //"//*[@id=\"default\"]/div/div/div/div/section/div[2]/ol/li[3]/article/h3/a"
}