using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;

namespace pili_sdk_csharp.pili
{
    public class PiliException : Exception
    {
        public readonly HttpWebResponse response;

        private readonly string mDetails;

        public PiliException(HttpWebResponse response)
        {
            this.response = response;

            try
            {
                var reader = new StreamReader(response.GetResponseStream());
                var text = reader.ReadToEnd();
                var jsonObj = JObject.Parse(text);

                mDetails = Utils.JsonEncode(jsonObj);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                mDetails += e.Message;
            }
        }

        public PiliException(string msg)
            : base(msg)
        {
            response = null;
        }

        public PiliException(Exception e)
        {
            response = null;
        }

        public override string Message => response == null ? base.Message : response.ToString();

        public virtual string Details => mDetails;

        public virtual int code()
        {
            return response == null ? -1 : (int)response.StatusCode;
        }
    }
}
