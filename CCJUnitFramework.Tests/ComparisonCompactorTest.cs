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

        [Fact]
        public void TestStartSame()
        {
            string failure = new ComparisonCompactor(1, "ba", "bc").Compact(null);
            Assert.Equal("expected <b[a]> but was <b[c]>", failure);
        }
    }
}
