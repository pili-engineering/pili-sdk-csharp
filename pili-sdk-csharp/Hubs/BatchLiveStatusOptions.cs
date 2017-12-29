namespace Qiniu.Pili.Hubs
{
    internal class BatchLiveStatusOptions
    {
        public BatchLiveStatusOptions(string[] items)
        {
            Items = items;
        }

        public string[] Items { get; set; }
    }
}
