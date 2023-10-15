using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Radancy.Api.Data;
using Radancy.Api.Repositories;
using Radancy.Api.Repositories.Contracts;

namespace Radancy.Api.Tests.RepositoriesTests;

public class AccountRepositoryTests
{
    private IAccountRepository _sut;
    private readonly DbContextOptions<RadancyDbContext> _options;
    private readonly Faker _faker;

    public AccountRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<RadancyDbContext>()
            .UseInMemoryDatabase(databaseName: "testInMemoryDb")
            .Options;
        _faker = new Faker();
    }

    [Fact]
    public async Task Get_WhenAccountExists_ReturnsAccount()
    {
        // Arrange
        await using var context = new RadancyDbContext(_options);
        var account = new Account()
        {
            UserId = _faker.Random.Int()
        };
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
        _sut = new AccountRepository(context);

        // Act
        var result = await _sut.Get(account.Id);

        // Assert
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();
        result.AsT1.Balance.Should().Be(100m);
        result.AsT1.Id.Should().Be(account.Id);
        result.AsT1.UserId.Should().Be(account.UserId);
    }

    [Fact]
    public async Task Get_WhenAccountDoesExist_ReturnsValidationFailed()
    {
        // Arrange
        await using var context = new RadancyDbContext(_options);
        _sut = new AccountRepository(context);

        // Act
        var result = await _sut.Get(_faker.Random.Int());

        // Assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be(nameof(Account.UserId));
        result.AsT0.Message.Should().Be("Account not found");
    }

    [Fact]
    public async Task Create_WhenAccountNotFound_ReturnsValidationFailed()
    {
        // arrange
        await using var context = new RadancyDbContext(_options);
        _sut = new AccountRepository(context);

        // act
        var result = await _sut.Create(_faker.Random.Int());

        // assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.AsT0.PropertyName.Should().Be(nameof(Account.UserId));
        result.AsT0.Message.Should().Be("User not found");
    }

    [Fact]
    public async Task Create_WhenNoValidationError_ReturnsAccount()
    {
        // arrange
        await using var context = new RadancyDbContext(_options);
        var user = new User { Id = _faker.Random.Int() };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        _sut = new AccountRepository(context);

        // act
        var result = await _sut.Create(user.Id);
        
        // arrange
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();
        result.AsT1.Balance.Should().Be(100m);
        result.AsT1.Id.Should().NotBe(0);
        result.AsT1.UserId.Should().Be(user.Id);
    }
}