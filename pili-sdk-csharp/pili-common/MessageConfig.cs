namespace pili_sdk_csharp.pili_common
{

    public class MessageConfig
    {
        public const string NULL_STREAM_ID_EXCEPTION_MSG = "FATAL EXCEPTION: streamId is null!";
        public const string NULL_HUBNAME_EXCEPTION_MSG = "FATAL EXCEPTION: hubName is null!";
        public const string NULL_CREDENTIALS_EXCEPTION_MSG = "FATAL EXCEPTION: credentials is null!";
        public const string ILLEGAL_RTMP_PUBLISH_URL_MSG = "Illegal rtmp publish url!";
        public const string ILLEGAL_TIME_MSG = "Illegal startTime or endTime!";
        public static readonly string ILLEGAL_TITLE_MSG = "The length of title should be at least:" + Config.TITLE_MIN_LENGTH + ",or at most:" + Config.TITLE_MAX_LENGTH;
        public const string ILLEGAL_FILE_NAME_EXCEPTION_MSG = "Illegal file name !";
        public const string ILLEGAL_FORMAT_EXCEPTION_MSG = "Illegal format !";
    }

}