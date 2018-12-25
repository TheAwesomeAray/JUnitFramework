namespace CCJUnitFramework
{
    public class JUnitAssert
    {
        public static string Format (string message, string expected, string actual)
        {
            return $"{message} expected <{expected ?? "null"}> but was <{actual ?? "null"}>".Trim();
        }
    }
}
