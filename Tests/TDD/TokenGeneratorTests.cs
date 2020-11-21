using AddressBook.Domain;
using AddressBook.Service.Common;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;

namespace AddressBook.Tests.TDD
{
    [TestFixture]
    public class TokenGeneratorTests
    {
        [Test]
        public void Should_generate_a_valid_token_when_calling_Generate()
        {
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(x => x.Value).Returns("A VALID SUPER SECRET :)");

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x.GetSection("Secret")).Returns(mockConfigurationSection.Object);

            var tokenGenerator = new TokenGenerator(mockConfiguration.Object);
            var user = new User
            {
                FirstName = "Ali",
                LastName = "Baba",
                Email = "alibaba@test.com",
                Id = Guid.NewGuid().ToString()
            };

            var token = tokenGenerator.Generate(user);
            Assert.AreNotEqual(null, token);
        }
    }
}
