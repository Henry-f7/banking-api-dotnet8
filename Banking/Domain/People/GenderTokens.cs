namespace Banking.Api.Domain.People
{
    public enum Gender { Male, Female, Other }
    public static class GenderTokens
    {
        private static readonly Dictionary<string, Gender> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["M"] = Gender.Male,
            ["MALE"] = Gender.Male,
            ["F"] = Gender.Female,
            ["FEMALE"] = Gender.Female,
            ["O"] = Gender.Other,
            ["OTHER"] = Gender.Other
        };

        public static bool TryNormalize(string token, out Gender gender)
            => _map.TryGetValue(token, out gender);
    }
}
