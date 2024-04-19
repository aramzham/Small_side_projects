using Nito.AsyncEx;

namespace EntainTasks.Task2;

public class WebPageDownloader : IDisposable
{
    private readonly HttpClient _client;
    private readonly IEnumerable<string> _websites;

    public WebPageDownloader(IEnumerable<string> websites)
    {
        _websites = websites;
        
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("user-agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/000000000 Safari/537.36 Edg/000000000");
    }

    public Task DownloadWebPages1()
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = 5 };
        return Parallel.ForEachAsync(_websites, options, async (url, token) =>
        {
            var response = await _client.GetAsync(url, token);
            var content = await response.Content.ReadAsStringAsync(token);
            Console.WriteLine($"{url} - {content.Length}");
        });
    }

    public async Task DownloadWebPages2()
    {
        var tasks = _websites.Select(url => _client.GetAsync(url)).ToList();
        
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            var response = await finishedTask;
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{response.RequestMessage.RequestUri} - {content.Length}");
        }
    }

    public async Task DownloadWebPages3()
    {
        var tasks = _websites.Select(url => _client.GetAsync(url)).ToList();
        
        await foreach (var response in WhenEach(tasks))
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{response.RequestMessage.RequestUri} - {content.Length}");
        }
    }
    
    private async IAsyncEnumerable<T> WhenEach<T>(IEnumerable<Task<T>> myTasks)
    {
        foreach (var task in myTasks.OrderByCompletion())
        {
            yield return await task;
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}