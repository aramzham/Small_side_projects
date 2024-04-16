using EntainTasks.Task1;

var compressor = new Compressor();
var compressed = compressor.Compress("Hello world!");
Console.WriteLine(compressed);
