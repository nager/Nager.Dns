namespace Nager.Dns.Helpers
{
    public static class MxHelper
    {
        public static string GetHostname(string dohMxData)
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
