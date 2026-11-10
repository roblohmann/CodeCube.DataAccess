using System.Linq;
using CodeCube.DataAccess.EntityFrameworkCore;
using Xunit;

namespace CodeCube.DataAccess.EntityFrameworkCore.Tests
{
    public class EnumQueryFilterHelperTests
    {
        private enum Color
        {
            Red,
            Green,
            Blue
        }

        [Fact]
        public void ApplyFilters_WithoutFilters_ReturnsAllEnumValues()
        {
            var result = EnumQueryFilterHelper<Color>.ApplyFilters(new string[0]);

            Assert.Equal(new[] { Color.Red, Color.Green, Color.Blue }, result.ToArray());
        }

        [Fact]
        public void ApplyFilters_WithFilters_ReturnsOnlyMatchingValues()
        {
            var result = EnumQueryFilterHelper<Color>.ApplyFilters(new[] { "Blue", "Red" });

            Assert.Equal(new[] { Color.Blue, Color.Red }, result.ToArray());
        }

        [Fact]
        public void ApplyFilters_IgnoresUnknownFilters()
        {
            var result = EnumQueryFilterHelper<Color>.ApplyFilters(new[] { "Purple", "Green" });

            Assert.Equal(new[] { Color.Green }, result.ToArray());
        }

        [Fact]
        public void ApplyFilters_WithOnlyUnknownFilters_ReturnsEmpty()
        {
            Assert.Empty(EnumQueryFilterHelper<Color>.ApplyFilters(new[] { "Purple" }));
        }
    }
}
