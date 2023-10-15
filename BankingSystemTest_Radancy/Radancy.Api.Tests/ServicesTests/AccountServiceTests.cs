using Bogus;
using FluentAssertions;
using NSubstitute;
using Radancy.Api.Data;
using Radancy.Api.Models;
using Radancy.Api.Repositories.Contracts;
using Radancy.Api.Services;
using Radancy.Api.Services.Contracts;

namespace Radancy.Api.Tests.ServicesTests;

public class AccountServiceTests
{
    private readonly IAccountService _sut;
    private readonly IAccountRepository _accountRepositoryMock;
    private readonly Faker _faker;

    public AccountServiceTests()
    {
        _faker = new Faker();
        _accountRepositoryMock = Substitute.For<IAccountRepository>();
        _sut = new AccountService(_accountRepositoryMock);
    }

    [Fact]
    public async Task Create_Should_Return_User()
    {
        // arrange
        var userId = _faker.Random.Int();
        var balance = _faker.Random.Decimal();
        _accountRepositoryMock.Create(Arg.Any<int>()).Returns(new Account { UserId = userId, Balance = balance });

        // act
        var result = await _sut.Create(default);

        // assert
        result.IsT1.Should().BeTrue();
        result.AsT1.UserId.Should().Be(userId);
        result.AsT1.Balance.Should().Be(balance);
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Withdraw_WhenAccountNotFound_ReturnsValidationFailure()
    {
        // arrange
        var validationFailed = new Faker<ValidationFailed>()
            .CustomInstantiator(f => new ValidationFailed(f.Random.String(), f.Random.String()))
            .Generate();
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(validationFailed);

        // act
        var result = await _sut.Withdraw(default, default);

        // assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be(validationFailed.PropertyName);
        result.AsT0.Message.Should().Be(validationFailed.Message);
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Withdraw_WhenAmountIsNegative_ReturnsValidationFailed()
    {
        // arrange
        var account = new Faker<Account>().Generate();
        var invalidAmount = _faker.Random.Decimal(int.MinValue, 0);
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(account);

        // act
        var result = await _sut.Withdraw(default, invalidAmount);

        // arrange
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be("amount");
        result.AsT0.Message.Should().Be("Withdrawal amount cannot be less than 0.");
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Withdraw_WhenMoreThan90PercentIsRequested_ThrowsException()
    {
        // arrange
        var account = new Faker<Account>().Generate();
        var invalidAmount = _faker.Random.Decimal(account.Balance * 0.9m);
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(account);

        // act
        var result = await _sut.Withdraw(default, invalidAmount);

        // assert 
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be("amount");
        result.AsT0.Message.Should().Be("Withdrawal amount is greater than 90% of the account balance.");
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Withdraw_WhenBalanceIsLessThan100_ReturnsValidationFailed()
    {
        // arrange
        var account = new Faker<Account>()
            .RuleFor(a => a.Balance, f => f.Random.Decimal(100, 1000))
            .Generate();
        var invalidAmount = _faker.Random.Decimal(account.Balance - 100, account.Balance);
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(account);

        // act
        var result = await _sut.Withdraw(default, invalidAmount);

        // assert 
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be("amount");
        result.AsT0.Message.Should().Be("Account balance cannot be less than 100.");
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Withdraw_WhenAmountIsValid_ReturnsAccount()
    {
        // arrange
        var initialBalance = _faker.Random.Decimal(1000, 2000);
        var account = new Faker<Account>()
            .RuleFor(a => a.Balance, _ => initialBalance)
            .Generate();
        var amount = _faker.Random.Decimal(0, account.Balance / 2);
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(account);

        // act
        var result = await _sut.Withdraw(default, amount);

        // assert
        result.IsT1.Should().BeTrue();
        result.IsT0.Should().BeFalse();
        result.AsT1.Id.Should().Be(account.Id);
        result.AsT1.UserId.Should().Be(account.UserId);
        result.AsT1.Balance.Should().Be(initialBalance - amount);
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Deposit_WhenAccountNotFound_ReturnsValidationFailure()
    {
        // arrange
        var validationFailed = new Faker<ValidationFailed>()
            .CustomInstantiator(f => new ValidationFailed(f.Random.String(), f.Random.String()))
            .Generate();
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(validationFailed);

        // act
        var result = await _sut.Deposit(default, default);

        // assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be(validationFailed.PropertyName);
        result.AsT0.Message.Should().Be(validationFailed.Message);
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Deposit_WhenAmountIsMoreThan10000_ReturnsValidationFailed()
    {
        // arrange
        var invalidAmount = _faker.Random.Decimal(10000, decimal.MaxValue);
        var account = new Faker<Account>().Generate();
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(account);

        // act
        var result = await _sut.Deposit(default, invalidAmount);

        // assert 
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be("amount");
        result.AsT0.Message.Should().Be("Deposit amount is greater than 10000.");
        _accountRepositoryMock.Received(1);
    }

    [Fact]
    public async Task Deposit_WhenAmountIsValid_ReturnsAccount()
    {
        // arrange
        var amount = _faker.Random.Decimal(1, 10000);
        var account = new Faker<Account>().Generate();
        var initialBalance = account.Balance;
        _accountRepositoryMock.Get(Arg.Any<int>()).Returns(account);

        // act
        var result = await _sut.Deposit(default, amount);

        // assert
        result.IsT1.Should().BeTrue();
        result.IsT0.Should().BeFalse();
        result.AsT1.Id.Should().Be(account.Id);
        result.AsT1.UserId.Should().Be(account.UserId);
        result.AsT1.Balance.Should().Be(initialBalance + amount);
        _accountRepositoryMock.Received(1);
    }
}