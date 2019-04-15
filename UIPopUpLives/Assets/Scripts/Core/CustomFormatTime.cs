public static class CustomFormatTime
{
    public static string formatTimeLive(string minutes, string seconds)
    {
        return  string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
}