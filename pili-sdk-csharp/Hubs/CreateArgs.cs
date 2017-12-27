namespace Qiniu.Pili.Hubs
{
    /// <summary>
    ///     Create
    /// </summary>
    internal class CreateArgs
    {
        public CreateArgs(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}
