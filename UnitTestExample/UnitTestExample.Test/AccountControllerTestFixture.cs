using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {

        [
        Test,
        TestCase("password", false),
        TestCase("pelda@gmail", false),
        TestCase("peldagmail.com", false),
        TestCase("pelda@gmail.com", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.ValidateEmail(email);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test,
         TestCase("ABCD1234", false),
         TestCase("Ab1234", false),
         TestCase("Abcd1234", true),
         TestCase("abcd1234", false),
         TestCase("ABCDabcd", false),
         ]

        public void TestValidatePassword(string password, bool expectedResult)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.ValidatePassword(password);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [
    Test,
    TestCase("pelda@gmail.com", "Abcd1234"),
    TestCase("pelda2@gmail.com", "Abcd12345"),
]
        public void TestRegisterHappyPath(string email, string password)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.Register(email, password);

            // Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }

    }

}
