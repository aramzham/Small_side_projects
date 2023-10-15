using System.Net;
using System.Text;
using System.Text.Json;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Radancy.Api.Models;
using IHashids = HashidsNet.IHashids;

namespace Radancy.Api.Tests.IntegrationTests;

public class AccountModuleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _httpClient;
    private readonly Faker _faker;

    public AccountModuleTests(WebApplicationFactory<Program> app)
    {
        _app = app;
        _httpClient = _app.CreateClient();
        _faker = new Faker();
    }

    [Fact]
    public async Task Create_WhenIdIsNotHash_ReturnsNotFound()
    {
        // arrange
        var request = new Faker<CreateAccountRequestModel>()
            .CustomInstantiator(x => new CreateAccountRequestModel(x.Random.String()))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response =
            await _httpClient.PostAsync("/account", new StringContent(content, Encoding.UTF8, "application/json"));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenIdIsHash_ReturnsOk()
    {
        // arrange
        var hashIds = _app.Services.GetRequiredService<IHashids>();
        var userResponse = await _httpClient.PostAsync("/user", new StringContent(string.Empty));
        var userResult = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseModel>(userResult, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        var request = new Faker<CreateAccountRequestModel>()
            .CustomInstantiator(_ => new CreateAccountRequestModel(user.Id))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response =
            await _httpClient.PostAsync("/account", new StringContent(content, Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var account = JsonSerializer.Deserialize<AccountResponseModel>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        account.Should().NotBeNull();
        account.Id.Should().NotBeEmpty();
        hashIds.Decode(account.Id).Should().HaveCountGreaterThan(0);
        account.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task Create_WhenUserIdIsNotCorrect_ReturnsBadRequestWithValidationFailed()
    {
        // arrange
        var hashIds = _app.Services.GetRequiredService<IHashids>();
        var request = new Faker<CreateAccountRequestModel>()
            .CustomInstantiator(f => new CreateAccountRequestModel(hashIds.Encode(f.Random.Int())))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response =
            await _httpClient.PostAsync("/account", new StringContent(content, Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var validationFailed = JsonSerializer.Deserialize<ValidationFailedResponseModel>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationFailed.Should().NotBeNull();
        validationFailed.Message.Should().Be("User not found");
        validationFailed.PropertyName.Should().Be(nameof(AccountResponseModel.UserId));
    }

    [Fact]
    public async Task Withdraw_WhenIdIsNotHash_ReturnsNotFound()
    {
        // arrange
        var request = new Faker<WithdrawRequestModel>()
            .CustomInstantiator(x => new WithdrawRequestModel(x.Random.String(), x.Random.Decimal()))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response = await _httpClient.PostAsync("/account/withdraw",
            new StringContent(content, Encoding.UTF8, "application/json"));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Withdraw_WhenAmountIsIncorrect_ReturnsBadRequestWithValidationFailure()
    {
        // arrange
        var userResponse = await _httpClient.PostAsync("/user", new StringContent(string.Empty));
        var userResult = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseModel>(userResult, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        var accountCreateContent = JsonSerializer.Serialize(new CreateAccountRequestModel(user.Id));
        await _httpClient.PostAsync("/account",
            new StringContent(accountCreateContent, Encoding.UTF8, "application/json"));

        var withdrawAmount = _faker.Random.Decimal();
        var request = new Faker<WithdrawRequestModel>()
            .CustomInstantiator(_ => new WithdrawRequestModel(user.Id, withdrawAmount))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response = await _httpClient.PostAsync("/account/withdraw",
            new StringContent(content, Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var validationFailed = JsonSerializer.Deserialize<ValidationFailedResponseModel>(result,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationFailed.Should().NotBeNull();
        validationFailed.Message.Should().NotBeEmpty();
        validationFailed.PropertyName.Should().Be("amount");
    }

    [Fact]
    public async Task Withdraw_WhenAmountIsCorrect_ReturnsOk()
    {
        // arrange
        var userResponse = await _httpClient.PostAsync("/user", new StringContent(string.Empty));
        var userResult = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseModel>(userResult, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        var accountCreateContent = JsonSerializer.Serialize(new CreateAccountRequestModel(user.Id));
        var accountResponse = await _httpClient.PostAsync("/account",
            new StringContent(accountCreateContent, Encoding.UTF8, "application/json"));
        var accountResult = await accountResponse.Content.ReadAsStringAsync();
        var accountResponseModel = JsonSerializer.Deserialize<AccountResponseModel>(accountResult,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        var depositAmount = _faker.Random.Decimal(1000, 2000);
        var depositRequest = new Faker<DepositRequestModel>()
            .CustomInstantiator(_ => new DepositRequestModel(accountResponseModel.Id, depositAmount))
            .Generate();
        var depositContent = JsonSerializer.Serialize(depositRequest);
        await _httpClient.PostAsync("/account/deposit",
            new StringContent(depositContent, Encoding.UTF8, "application/json"));

        var withdrawAmount = _faker.Random.Decimal(0, depositAmount / 2);
        var request = new Faker<WithdrawRequestModel>()
            .CustomInstantiator(_ => new WithdrawRequestModel(user.Id, withdrawAmount))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response = await _httpClient.PostAsync("/account/withdraw",
            new StringContent(content, Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var account = JsonSerializer.Deserialize<AccountResponseModel>(result,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        account.Should().NotBeNull();
        account.Id.Should().Be(accountResponseModel.Id);
        account.UserId.Should().Be(user.Id);
        account.Balance.Should().Be(depositAmount + 100 - withdrawAmount);
    }
    
    [Fact]
    public async Task Deposit_WhenIdIsNotHash_ReturnsNotFound()
    {
        // arrange
        var request = new Faker<DepositRequestModel>()
            .CustomInstantiator(x => new DepositRequestModel(x.Random.String(), x.Random.Decimal()))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response =
            await _httpClient.PostAsync("/account/deposit", new StringContent(content, Encoding.UTF8, "application/json"));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Deposit_WhenAmountIsNotCorrect_ReturnsBadRequestWithValidationFailed()
    {
        // arrange
        var userResponse = await _httpClient.PostAsync("/user", new StringContent(string.Empty));
        var userResult = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseModel>(userResult, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        var accountCreateContent = JsonSerializer.Serialize(new CreateAccountRequestModel(user.Id));
        var accountResponse = await _httpClient.PostAsync("/account",
            new StringContent(accountCreateContent, Encoding.UTF8, "application/json"));
        var accountResult = await accountResponse.Content.ReadAsStringAsync();
        var accountResponseModel = JsonSerializer.Deserialize<AccountResponseModel>(accountResult,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        
        var request = new Faker<DepositRequestModel>()
            .CustomInstantiator(f => new DepositRequestModel(accountResponseModel.Id, f.Random.Decimal(10001, decimal.MaxValue)))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response =
            await _httpClient.PostAsync("/account/deposit", new StringContent(content, Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var validationFailed = JsonSerializer.Deserialize<ValidationFailedResponseModel>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationFailed.Should().NotBeNull();
        validationFailed.Message.Should().NotBeEmpty();
        validationFailed.PropertyName.Should().Be("amount");
    }
    
    [Fact]
    public async Task Deposit_WhenAmountIsCorrect_ReturnsOk()
    {
        // arrange
        var userResponse = await _httpClient.PostAsync("/user", new StringContent(string.Empty));
        var userResult = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseModel>(userResult, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        var accountCreateContent = JsonSerializer.Serialize(new CreateAccountRequestModel(user.Id));
        var accountResponse = await _httpClient.PostAsync("/account",
            new StringContent(accountCreateContent, Encoding.UTF8, "application/json"));
        var accountResult = await accountResponse.Content.ReadAsStringAsync();
        var accountResponseModel = JsonSerializer.Deserialize<AccountResponseModel>(accountResult,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        
        var request = new Faker<DepositRequestModel>()
            .CustomInstantiator(f => new DepositRequestModel(accountResponseModel.Id, f.Random.Decimal(10000)))
            .Generate();
        var content = JsonSerializer.Serialize(request);

        // act
        var response =
            await _httpClient.PostAsync("/account/deposit", new StringContent(content, Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var account = JsonSerializer.Deserialize<AccountResponseModel>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        account.Should().NotBeNull();
        account.Id.Should().Be(accountResponseModel.Id);
        account.UserId.Should().Be(user.Id);
        account.Balance.Should().Be(accountResponseModel.Balance + request.Amount);
    }
}