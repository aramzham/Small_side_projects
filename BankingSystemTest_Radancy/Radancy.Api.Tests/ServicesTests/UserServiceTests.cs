using Bogus;
using FluentAssertions;
using NSubstitute;
using Radancy.Api.Data;
using Radancy.Api.Repositories.Contracts;
using Radancy.Api.Services;
using Radancy.Api.Services.Contracts;

namespace Radancy.Api.Tests.ServicesTests;

public class UserServiceTests
{
    private readonly IUserService _sut;
    private readonly IUserRepository _userRepositoryMock;

    public UserServiceTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _sut = new UserService(_userRepositoryMock);
    }

    [Fact]
    public async Task CreateUser_Should_Return_User()
    {
        // arrange
        var id = new Faker().Random.Int();
        _userRepositoryMock.Create().Returns(new User{Id = id});
        
        // act
        var result = await _sut.Create();
        
        // assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        _userRepositoryMock.Received(1);
    }
}