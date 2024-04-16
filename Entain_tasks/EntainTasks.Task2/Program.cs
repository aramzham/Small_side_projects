string[] websites =
[
    "https://www.gov.am/en/", "https://www.flashscore.com/", "https://www.youtube.com/", "https://www.temu.com/",
    "https://www.goal.com/es"
];

var client = new HttpClient();
client.DefaultRequestHeaders.Add("user-agent",
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36 Edg/123.0.0.0");

await Parallel.ForEachAsync(websites, async (url, token) =>
{
    try
    {
        var response = await client.GetAsync(url, token);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(token);
            Console.WriteLine($"{url} - {content.Length}");
        }
        else
        {
            Console.WriteLine($"{url} - {response.StatusCode}");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"{url} - failed with [{e.Message}]");
    }
});