namespace Helpers
{
    public static class MathHelper
    {
        public static decimal RefineValue(decimal value)
            => Math.Truncate(1000 * value) / 1000;

        public static double RefineValue(double value)
            => Math.Truncate(1000 * value) / 1000;
    }
}
