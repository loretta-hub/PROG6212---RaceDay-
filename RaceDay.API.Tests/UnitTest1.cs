using System.Linq;
using Xunit;

namespace RaceDay.API.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestProject_IsWorking()
        {
            // Arrange
            int expected = 2;

            // Act
            int actual = 1 + 1;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Organiser")]
        [InlineData("Participant")]
        public void ValidRoles_ShouldBeAccepted(string role)
        {
            // Arrange
            string[] validRoles = { "Organiser", "Participant" };

            // Act
            bool isValid = validRoles.Contains(role);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("Guest")]
        [InlineData("")]
        public void InvalidRoles_ShouldNotBeAccepted(string role)
        {
            // Arrange
            string[] validRoles = { "Organiser", "Participant" };

            // Act
            bool isValid = validRoles.Contains(role);

            // Assert
            Assert.False(isValid);
        }
    }
}