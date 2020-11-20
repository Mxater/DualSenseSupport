namespace DualSenseSupport
{
    public static class Util
    {
        public static int InRange(this int these, int min, int max)
        {
            if (these > max)
            {
                return max;
            }

            if (these < min)
            {
                return min;
            }

            return these;
        }
    }
}