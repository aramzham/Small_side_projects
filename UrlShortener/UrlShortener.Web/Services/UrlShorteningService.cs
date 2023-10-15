using Microsoft.EntityFrameworkCore;
using UrlShortener.Web.Data;

namespace UrlShortener.Web.Services;

public class UrlShorteningService
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
    private readonly Random _random = new();
    private readonly ApplicationDbContext _dbContext;
    
    public const int NumberOfCharsInShortLink = 7;

    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateUniqueCode()
    {
        while (true)
        {
            var codeChars = new char[NumberOfCharsInShortLink];
            for (var i = 0; i < NumberOfCharsInShortLink; i++)
            {
                codeChars[i] = Alphabet[_random.Next(Alphabet.Length)];
            }

            var code = new string(codeChars); // convert char array to string

            // check if the code doesn't exist in the database
            if (await _dbContext.ShortenedUrls.AnyAsync(url => url.ShortUrl == code)) // you may think about generating codes in advance to not to hit database too often
                continue;
            
            return code;
        }
    }
}