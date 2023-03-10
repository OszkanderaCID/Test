using Test;

namespace UnitTest
{
    public class UserFinderTests
    {
        private List<User> _userList = new List<User> {
            new User{ Id = 1, Username = @"domain1\Test1" },
            new User{ Id = 2, Username = @"domain2\Test1" },
            new User{ Id = 3, Username = @"Test1" },
            new User{ Id = 4, Username = @"OTest1" },
            new User{ Id = 5, Username = @"Test123" },
            new User{ Id = 6, Username = @"ABCTest123" },
            new User{ Id = 7, Username = @"domain\Test2" },
            new User{ Id = 8, Username = @"Test2" },
            new User{ Id = 9, Username = @"Test3" },
            new User{ Id = 10, Username = @"domain\Test4" },
            new User{ Id = 11, Username = @"domain\OXTest455" },
            new User{ Id = 12, Username = @"domain\OXTest4" },
            new User{ Id = 13, Username = @"domain\Test465" },
        };

        private UserFinder _userFinder = new UserFinder();

        [Theory]
        [InlineData(@"domain\Test4", 10)]
        [InlineData(@"domain1\Test1", 1)]
        [InlineData(@"domain1\Test123", 5)]
        public void UserWithDomain_ShouldBeFound(string username, int expectedId)
        {
            var id = _userFinder.GetUserId(_userList, username);

            Assert.Equal(expectedId, id);
        }

        [Theory]
        [InlineData("Test4", 10)]
        [InlineData("Test3", 9)]
        [InlineData("Test123", 5)]
        public void UserWithoutDomain_ShouldBeFound(string username, int expectedId)
        {           
            var id = _userFinder.GetUserId(_userList, username);

            Assert.Equal(expectedId, id);
        }

        [Theory]
        [InlineData(@"domain1\Test8")]
        [InlineData(@"domainABC\Test1")]
        public void UserWithDomain_ShouldNotBeFound(string username)
        {
            var id = _userFinder.GetUserId(_userList, username);

            Assert.Null(id);
        }
        [Theory]
        [InlineData(@"Test8")]
        [InlineData(@"Test1")]
        [InlineData(@"Test2")]
        public void UserWithoutDomain_ShouldNotBeFound(string username)
        {
            var id = _userFinder.GetUserId(_userList, username);

            Assert.Null(id);
        }

    }
}