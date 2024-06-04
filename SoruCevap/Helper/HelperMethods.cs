namespace SoruCevap.Helper
{
    public class HelperMethods
    {
        public static int GetUnixTimeUT3()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSinceEpoch = utcNow - unixEpoch;
            int unixTimeUT3 = (int)(timeSinceEpoch.TotalMilliseconds / 1000);
            return unixTimeUT3;
        }
    }
}
