using CCJUnitFramework;
using System;

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
            if (CanBeCompacted())
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

        private bool CanBeCompacted()
        {
            return expected != null && actual != null && !AreStringsEqual();
        }

        private string Compact(string s)
        {
            string result = DELTA_START + s.Substring(prefixLength, s.Length - suffixLength - prefixLength) + DELTA_END;

            if (prefixLength > 0)
                result = ComputeCommonPrefix() + result;
            if (suffixLength > 0)
                result = result + ComputeCommonSuffix();

            return result;
        }

        private string ComputeCommonPrefix()
        {
            return (prefixLength > contextLength ? ELLIPSIS : "") + expected.Substring(Math.Max(0, prefixLength - contextLength), prefixLength - Math.Max(0, prefixLength - contextLength));
        }

        private string ComputeCommonSuffix()
        {
            int start = expected.Length - suffixLength;
            int length = Math.Min(expected.Length - suffixLength + contextLength, expected.Length) - start;
            
            return expected.Substring(expected.Length - suffixLength, length) + (expected.Length - suffixLength < expected.Length - contextLength ? ELLIPSIS : "");
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
