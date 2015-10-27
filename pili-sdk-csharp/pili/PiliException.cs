using System;
using System.Net;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;
using System.IO;
namespace pili_sdk_csharp.pili
{



    public class PiliException : Exception
    {
        public readonly HttpWebResponse response;

        private string mDetails = null;

        public PiliException(HttpWebResponse response)
        {
            this.response = response;

            try
            {

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string text = reader.ReadToEnd();
                JObject jsonObj = JObject.Parse(text);

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
            this.response = null;
        }

        public virtual int code()
        {
            return response == null ? -1 : (int)response.StatusCode;
        }

        public override string Message
        {
            get
            {
                return response == null ? base.Message : response.ToString();
            }
        }

        public virtual string Details
        {
            get
            {
                return mDetails;
            }
        }
    }

}