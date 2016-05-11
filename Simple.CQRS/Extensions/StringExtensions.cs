namespace Simple.CQRS.Extensions
{
    public static class StringExtensions
    {
        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
