namespace EntainTasks.Task1;

public class Compressor
{
    // example: "aabbbccc" -> "2a3b3c"
    // example: "aaaabbbccc" -> "4a3b3c"
    // example: "a" -> "1a"
    public string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
    
        if (input.Length == 1)
            return $"1{input}";
    
        var currentSymbol = default(char);
        var currentSymbolCount = 0;
        var result = string.Empty;
    
        foreach (var symbol in input)
        {
            if (symbol == currentSymbol)
                currentSymbolCount++;
            else
            {
                if (currentSymbolCount > 0)
                    result += $"{currentSymbolCount}{currentSymbol}";
    
                currentSymbol = symbol;
                currentSymbolCount = 1;
            }
        }
        
        return result + $"{currentSymbolCount}{currentSymbol}";
    }
    
    // ----------- recursive way to do -----------
    
    // public string Compress(string input)
    // {
    //     if (string.IsNullOrEmpty(input))
    //         return input;
    //
    //     return input.Length == 1 
    //         ? $"1{input}" 
    //         : CompressHelper(input, 0, input[0], 0);
    // }
    //
    // private string CompressHelper(string input, int index, char currentSymbol, int currentSymbolCount)
    // {
    //     if (index >= input.Length)
    //         return $"{currentSymbolCount}{currentSymbol}";
    //
    //     if (input[index] == currentSymbol)
    //     {
    //         return CompressHelper(input, index + 1, currentSymbol, currentSymbolCount + 1);
    //     }
    //
    //     return $"{currentSymbolCount}{currentSymbol}" + CompressHelper(input, index, input[index], 0);
    // }
}