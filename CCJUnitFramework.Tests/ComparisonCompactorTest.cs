using Xunit;

namespace CCJUnitFramework.Tests
{
    public class ComparisonCompactorTest
    {
        [Fact]
        public void TestMessage()
        {
            string failure = new ComparisonCompactor(0, "b", "c").Compact("a");
            Assert.Equal("a expected <[b]> but was <[c]>", failure);
        }
    }
}
