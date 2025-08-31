using System.Text.RegularExpressions;

namespace Banking.Api.Domain.Identity
{
    public static class NicaraguanNationalId
    {
        // 001-071296-0003X  (DD=07, MM=12, YY=96)
        private static readonly Regex Pattern = new(
            @"^\d{3}-(?<DD>\d{2})(?<MM>\d{2})(?<YY>\d{2})-\d{4}[A-Za-z]$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static bool IsValidFormat(string nid) => Pattern.IsMatch(nid);

        public static bool DateMatches(string nid, DateTime birthDate)
        {
            var m = Pattern.Match(nid);
            if (!m.Success) return false;

            var dd = m.Groups["DD"].Value;
            var mm = m.Groups["MM"].Value;
            var yy = m.Groups["YY"].Value;

            var bdd = birthDate.Day.ToString("00");
            var bmm = birthDate.Month.ToString("00");
            var byy = (birthDate.Year % 100).ToString("00");

            return dd == bdd && mm == bmm && yy == byy;
        }
    }
}
