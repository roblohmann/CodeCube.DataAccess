using System;
using System.Linq.Expressions;
using CodeCube.DataAccess.EntityFrameworkCore.Extensions;
using Xunit;

namespace CodeCube.DataAccess.EntityFrameworkCore.Tests
{
    public class ExpressionExtensionsTests
    {
        private interface IHasName
        {
            string Name { get; }
        }

        private class Person : IHasName
        {
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void Convert_ChangesParameterTypeFromInterfaceToConcreteType()
        {
            Expression<Func<IHasName, bool>> filter = x => x.Name == "Rob";

            var converted = filter.Convert<IHasName, Person>();

            Assert.Equal(typeof(Person), converted.Parameters[0].Type);

            var compiled = converted.Compile();
            Assert.True(compiled(new Person { Name = "Rob" }));
            Assert.False(compiled(new Person { Name = "Other" }));
        }
    }
}
