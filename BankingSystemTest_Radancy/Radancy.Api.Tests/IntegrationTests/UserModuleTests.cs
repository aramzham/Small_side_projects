using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using HashidsNet;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Radancy.Api.Models;

namespace Radancy.Api.Tests.IntegrationTests;

public class UserModuleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _httpClient;

    public UserModuleTests(WebApplicationFactory<Program> app)
    {
        _app = app;
        _httpClient = _app.CreateClient();
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        // arrange
        var hashIds = _app.Services.GetRequiredService<IHashids>();

        // act
        var response = await _httpClient.PostAsync("/user", new StringContent(string.Empty));
        var result = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseModel>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty();
        hashIds.Decode(user.Id).Should().HaveCountGreaterThan(0);
    }
}