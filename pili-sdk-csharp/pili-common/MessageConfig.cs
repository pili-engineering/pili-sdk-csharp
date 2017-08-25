namespace pili_sdk_csharp.pili_common
{
    public class MessageConfig
    {
        public const string NullStreamIdExceptionMsg = "FATAL EXCEPTION: streamId is null!";
        public const string NullHubnameExceptionMsg = "FATAL EXCEPTION: hubName is null!";
        public const string NullCredentialsExceptionMsg = "FATAL EXCEPTION: credentials is null!";
        public const string IllegalRtmpPublishUrlMsg = "Illegal rtmp publish url!";
        public const string IllegalTimeMsg = "Illegal startTime or endTime!";
        public const string IllegalFileNameExceptionMsg = "Illegal file name !";
        public const string IllegalFormatExceptionMsg = "Illegal format !";
        public static readonly string IllegalTitleMsg = "The length of title should be at least:" + Config.TitleMinLength + ",or at most:" + Config.TitleMaxLength;
    }
}
