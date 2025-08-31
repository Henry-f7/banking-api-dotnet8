namespace Banking.Api.Domain.Primitives
{
    public static class Currency
    {
        private static readonly HashSet<string> _all =
        new(StringComparer.OrdinalIgnoreCase) { "NIO", "USD" };

        public static bool IsSupported(string code) => _all.Contains(code);

        public static IReadOnlyCollection<string> All => _all;
    }
}
