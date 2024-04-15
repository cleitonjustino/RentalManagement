namespace RentalManagement.Domain.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
            {
                if (!nullOrWhitespaceInputReturnsNull)
                {
                    return new List<string>();
                }

                return Enumerable.Empty<string>();
            }

            return (from s in csvList.TrimEnd(',').Split(',').AsEnumerable()
                    select s.Trim()).ToList();
        }

        public static bool IsNullOrWhitespace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
    }
}
