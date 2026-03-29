namespace Nager.Dns.Helpers
{
    /// <summary>
    /// MX Helper
    /// </summary>
    public static class MxHelper
    {
        /// <summary>
        /// Extracts the hostname from a raw MX record string returned by a DoH query.
        /// The input is expected in the format "priority hostname".
        /// For example: "10 mail.example.com".
        /// </summary>
        /// <param name="dohMxData">The MX record string from a DoH response.</param>
        /// <returns>
        /// The hostname part of the MX record, or <c>null</c> if the input is not in the expected format.
        /// </returns>
        public static string? GetHostname(string dohMxData)
        {
            var prioritySplitPosition = dohMxData.IndexOf(' ');
            if (prioritySplitPosition == -1)
            {
                return null;
            }

            return dohMxData.Substring(prioritySplitPosition + 1);
        }
    }
}
