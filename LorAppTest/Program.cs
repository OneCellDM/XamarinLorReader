using LorParser;

namespace LorConsoleTest;

public class Program
{
    public static void Main()
    {
        try
        {
            var res = LOR.GetNewsList().GetAwaiter().GetResult();

            Console.WriteLine("Смещение по записям: {0}", res.Offset);
            Console.ReadLine();
            var re = LOR.GetNews(res.Items[0].NewsUri).GetAwaiter().GetResult();
            foreach (var item in res.Items)
            {
                Console.WriteLine("Title: {0} ( {1} )", item.Title, item.CommentsCount);
                Console.WriteLine("Text: {0}", item.Text);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка!: {0}", ex.Message);
        }

        Console.WriteLine("ok");
        Console.ReadLine();
    }
}