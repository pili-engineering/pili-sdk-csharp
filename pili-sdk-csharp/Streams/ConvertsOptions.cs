namespace Qiniu.Pili.Streams
{
    internal class ConvertsOptions
    {
        public ConvertsOptions(string[] converts)
        {
            Converts = converts;
        }

        internal string[] Converts { get; set; }
    }
}
