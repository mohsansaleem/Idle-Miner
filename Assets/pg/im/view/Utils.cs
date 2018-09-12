using System.Globalization;

namespace pg.im.view
{
    public static class Utils
    {
        public static string ToShort (this double num)
        {
            if (num > 999999999999999999999999.0)
            {
                return num.ToString("0,,,,,,,.##ad", CultureInfo.InvariantCulture);
            }
            else if (num > 999999999999999999999.0)
            {
                return num.ToString("0,,,,,,,.##ac", CultureInfo.InvariantCulture);
            }
            else if (num > 999999999999999999.0)
            {
                return num.ToString("0,,,,,,.##ab", CultureInfo.InvariantCulture);
            }
            else if (num > 999999999999999.0)
            {
                return num.ToString("0,,,,,.##aa", CultureInfo.InvariantCulture);
            }
            else if (num > 999999999999.0)
            {
                return num.ToString("0,,,,.##T", CultureInfo.InvariantCulture);
            }
            else if (num > 999999999.0)
            {
                return num.ToString("0,,,.##B", CultureInfo.InvariantCulture);
            }
            else if (num > 999999.0)
            {
                return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
            }
            else if (num > 999)
            {
                return num.ToString("0,.##K", CultureInfo.InvariantCulture);
            }
            else
            {
                return num.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}