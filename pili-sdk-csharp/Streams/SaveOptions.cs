namespace Qiniu.Pili.Streams
{
    public class SaveOptions
    {
        public SaveOptions()
        {
        }

        public SaveOptions(long start, long end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        ///     End unix time. 0 means current time
        /// </summary>
        public long End { get; set; }

        /// <summary>
        ///     Get or set the expiration days of ts.
        ///     -1 means no change of ts's expiration;
        ///     0 means storing forever;
        ///     any other positive number can change the ts's expiration days.
        /// </summary>
        public long ExpireDays { get; set; }

        /// <summary>
        ///     The saved file name
        /// </summary>
        public string Fname { get; set; }

        /// <summary>
        ///     File format. default in m3u8
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        ///     URL address. After dora asynchronous operation is done, will notify this address
        /// </summary>
        public string Notify { get; set; }

        /// <summary>
        ///     If qiniu dora pipeline is needed, assign this value
        /// </summary>
        public string Pipeline { get; set; }

        /// <summary>
        ///     Start unix time
        /// </summary>
        public long Start { get; set; }
    }
}
