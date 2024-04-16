using FluentAssertions;

namespace EntainTasks.Task1.Tests;

public class CompressorTests
{
    private Compressor _sut = new();

    [Theory]
    [InlineData("AA", "2A")]
    [InlineData("AAB", "2A1B")]
    [InlineData("AAABBCCCCDDABC", "3A2B4C2D1A1B1C")]
    [InlineData("CCCAcABBDD", "3C1A1c1A2B2D")]
    [InlineData("E", "1E")]
    public void Compress_ShouldReturnCompressedString(string input, string output)
    {
        // arrange
        
        // act
        var result = _sut.Compress(input);
        
        // assert
        result.Should().Be(output);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void Compress_ShouldReturnEmpty(string input, string output)
    {
        // arrange

        // act
        var result = _sut.Compress(input);
        
        // assert
        result.Should().BeNullOrEmpty(output);
    }
}