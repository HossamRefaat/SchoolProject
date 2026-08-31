using System.Globalization;

namespace SchoolProject.Data.Commons
{
    public class LocalizalbeEntity
    {
        public string GetLocal(string ar, string en)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            if (culture.TwoLetterISOLanguageName.ToLower().Equals("ar"))
                return ar;
            return en;
        }
    }
}
