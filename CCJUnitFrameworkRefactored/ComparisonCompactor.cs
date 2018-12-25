using CCJUnitFramework;
using System;
using System.Text;

namespace CCJUnitFrameworkRefactored
{
    public class ComparisonCompactor
    {
        public static readonly string ELLIPSIS = "...";
        public static readonly string DELTA_END = "]";
        public static readonly string DELTA_START = "[";

        private int contextLength;
        private string expected;
        private string actual;
        private string compactExpected;
        private string compactActual;
        private int prefixLength;
        private int suffixLength;

        public ComparisonCompactor(int contextLength, string expected, string actual)
        {
            this.contextLength = contextLength;
            this.expected = expected;
            this.actual = actual;
            compactExpected = expected;
            compactActual = actual;
        }

        public string FormatCompactedComparison(string message)
        {
            if (ShouldBeCompacted())
            {
                CompactExpectedAndActual();
                return JUnitAssert.Format(message, compactExpected, compactActual);
            }

            return JUnitAssert.Format(message, compactExpected, compactActual);
        }

        private void CompactExpectedAndActual()
        {
            FindCommonPrefixAndSuffix();
            compactExpected = Compact(expected);
            compactActual = Compact(actual);
        }

        private void FindCommonPrefixAndSuffix()
        {
            FindCommonPrefix();
            suffixLength = 0;
            for (; !SuffixOverlapsPrefix(); suffixLength++)
            {
                if (charFromEnd(expected, suffixLength) != charFromEnd(actual, suffixLength))
                    break;
            }
        }

        private char charFromEnd(string s, int i)
        {
            return s[s.Length - i - 1];
        }

        private bool SuffixOverlapsPrefix()
        {
            return actual.Length - suffixLength <= prefixLength || expected.Length - suffixLength <= prefixLength;
        }

        private bool ShouldBeCompacted()
        {
            return expected != null && actual != null && !AreStringsEqual();
        }

        private string Compact(string s)
        {
            return new StringBuilder().Append(StartingEllipses())
                                      .Append(StartingContext())
                                      .Append(DELTA_START)
                                      .Append(Delta(s))
                                      .Append(DELTA_END)
                                      .Append(EndingContext())
                                      .Append(EndingEllipses())
                                      .ToString();
        }

        private string StartingEllipses()
        {
            return prefixLength > contextLength ? ELLIPSIS : "";
        }

        private string StartingContext()
        {
            int contextStart = Math.Max(0, prefixLength - contextLength);
            int length = prefixLength - contextStart;
            return expected.Substring(contextStart, length);
        }

        private string Delta(string s)
        {
            int deltaStart = prefixLength;
            int deltaLength = s.Length - suffixLength - prefixLength;
            return s.Substring(deltaStart, deltaLength);
        }

        private string EndingContext()
        {
            int contextStart = expected.Length - suffixLength;
            int length = suffixLength > contextLength ? contextLength : suffixLength;
            return expected.Substring(contextStart, length);
        }

        private string EndingEllipses()
        {
            return suffixLength > contextLength ? ELLIPSIS : "";
        }
        
        private void FindCommonPrefix()
        {
            prefixLength = 0;
            int end = Math.Min(expected.Length, actual.Length);
            for (; prefixLength < end; prefixLength++)
            {
                if (expected[prefixLength] != actual[prefixLength])
                    break;
            }
        }
        
        private bool AreStringsEqual()
        {
            return expected.Equals(actual);
        }
    }
}
