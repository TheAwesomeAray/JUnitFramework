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

        [Fact]
        public void TestEndSame()
        {
            string failure = new ComparisonCompactor(1, "ab", "cb").Compact(null);
            Assert.Equal("expected <[a]b> but was <[c]b>", failure);
        }

        [Fact]
        public void TestSame()
        {
            string failure = new ComparisonCompactor(1, "ab", "ab").Compact(null);
            Assert.Equal("expected <ab> but was <ab>", failure);
        }

        [Fact]
        public void TestNoContextStartAndEndSame()
        {
            string failure = new ComparisonCompactor(0, "abc", "adc").Compact(null);
            Assert.Equal("expected <...[b]...> but was <...[d]...>", failure);
        }

        [Fact]
        public void TestStartAndEndContext()
        {
            string failure = new ComparisonCompactor(1, "abc", "adc").Compact(null);
            Assert.Equal("expected <a[b]c> but was <a[d]c>", failure);
        }

        [Fact]
        public void TestComparisonErrorStartSameComplete()
        {
            string failure = new ComparisonCompactor(2, "ab", "abc").Compact(null);
            Assert.Equal("expected <ab[]> but was <ab[c]>", failure);
        }

        [Fact]
        public void TestComparisonErrorEndSameComplete()
        {
            string failure = new ComparisonCompactor(0, "bc", "abc").Compact(null);
            Assert.Equal("expected <[]...> but was <[a]...>", failure);
        }
    }
}
