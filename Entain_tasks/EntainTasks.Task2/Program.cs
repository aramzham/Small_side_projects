using System.Diagnostics;
using EntainTasks.Task2;

string[] websites =
[
    "https://www.gov.am/en/", "https://www.flashscore.com/", "https://www.youtube.com/", "https://www.temu.com/", "https://www.goal.com/es"
];

var sw = new Stopwatch();
var webPageDownloader = new WebPageDownloader(websites);

sw.Start();
await webPageDownloader.DownloadWebPages1();
Console.WriteLine($"Executed DownloadWebPages1 in {sw.ElapsedMilliseconds}ms ---------------------");

sw.Restart();
await webPageDownloader.DownloadWebPages2();
Console.WriteLine($"Executed DownloadWebPages2 in {sw.ElapsedMilliseconds}ms ---------------------");

sw.Restart();
await webPageDownloader.DownloadWebPages3();
Console.WriteLine($"Executed DownloadWebPages3 in {sw.ElapsedMilliseconds}ms ---------------------");

sw.Stop();
