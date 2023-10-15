using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Radancy.Api.Data;
using Radancy.Api.Repositories;
using Radancy.Api.Repositories.Contracts;

namespace Radancy.Api.Tests.RepositoriesTests;

public class UserRepositoryTests
{
    private IUserRepository _sut;

    [Fact]
    public async Task Create_ShouldCreateUser()
    {
        // arrange
        var options = new DbContextOptionsBuilder<RadancyDbContext>()
            .UseInMemoryDatabase(databaseName:"testInMemoryDb")
            .Options;

        await using var context = new RadancyDbContext(options);
        
        _sut = new UserRepository(context);
        
        // act
        var result = await _sut.Create();
        
        // assert
        result.Should().BeOfType<User>();
        result.Should().NotBeNull();
        result.Id.Should().Be(1);

        var users = context.Users;
        users.Should().HaveCount(1);

        var firstUser = await users.FirstOrDefaultAsync();
        firstUser.Should().NotBeNull();
        firstUser.Id.Should().Be(result.Id);
    }
}