﻿using FCG.Domain.Modules.Users;

namespace UnitTests.User
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            Name name = new("João");
            Email email = new("joão@example.com");
            Password password = new("SenhaValida@123");
            UserRole role = UserRole.Admin;

            // Act
            FCG.Domain.Modules.Users.User user = new(name, email, password, role);

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
            Assert.Equal(role, user.Role);
            Assert.True((DateTime.Now - user.CreateDate).TotalSeconds < 1);
        }
    }
}
