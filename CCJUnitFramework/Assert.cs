namespace CCJUnitFramework
{
    class Assert
    {
        public static string Format (string message, string expected, string actual)
        {
            return $"{message} expected <{expected}> but was <{actual}>";
        }
    }
}
