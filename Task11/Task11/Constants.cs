namespace Task11
{
    public class Constants
    {
        public const string API_TOKEN = "ab457116572fa95d76686be84420dd7f";
        public const string TELEGRAM_TOKEN = "1961626635:AAHoazzOmqi-WNn2Xm4hdChjpJ41_3oh9zQ";

        public const string API_HTTP_PATH = @"http://api.exchangeratesapi.io/v1/";
        public const string API_HTTP_SUFX = "?access_key=";
        public const string API_DATE_FORMAT = "yyyy-MM-dd";
        public const string API_LAST_VALUE = "latest";

        public const string FIRST_SELECT_HEADER = "Select basic currency:\n";
        public const string SECOND_SELECT_HEADER = "Select your currency:\n";
        public const string DATE_SELECT_HEADER = "Select rate's date or click \"NOW\". Example: 2020-01-01:\n";
        public const string AMOUNT_SELECT_HEADER = "Enter the amount of currency. Example: 23,45";
        public const string ERROR_HEADER = "Error while process. Try later.";
        public const string ERROR_DATE_FORMAT = "Wrong date format";
        public const string FINISH_MESS_HEAD = "Rate for: ";
    }
}
