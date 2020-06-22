using System;
using CodeCube.DataAccess.EntityFrameworkCore.Attributes;
using Xunit;

namespace CodeCube.DataAccess.EntityFrameworkCore.Tests
{
    public class NotEmptyTests
    {
        [Fact]
        public void EmptyGuidIsNotAllowed()
        {
            //Setup
            var validator = new NotEmptyAttribute();

            //Act
            bool isValid = validator.IsValid(Guid.Empty);

            //Assert
            Assert.False(isValid);
        }

        [Fact]
        public void NullIsAllowed()
        {
            //Setup
            var validator = new NotEmptyAttribute();

            //Act
            bool isValid = validator.IsValid(null);

            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void EmptyStringIsAllowed()
        {
            //Setup
            var validator = new NotEmptyAttribute();

            //Act
            bool isValid = validator.IsValid(string.Empty);

            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void DateTimeMinValueIsNotAllowed()
        {
            //Setup
            var validator = new NotEmptyAttribute();

            //Act
            bool isValid = validator.IsValid(DateTime.MinValue);

            //Assert
            Assert.False(isValid);
        }
    }
}
