using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;

namespace pili_sdk_csharp.pili
{
    public class PiliException : Exception
    {
        private readonly string _mDetails;
        public readonly HttpWebResponse Response;

        public PiliException(HttpWebResponse response)
        {
            Response = response;

            try
            {
                var reader = new StreamReader(response.GetResponseStream());
                var text = reader.ReadToEnd();
                var jsonObj = JObject.Parse(text);

                _mDetails = Utils.JsonEncode(jsonObj);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                _mDetails += e.Message;
            }
        }

        public PiliException(string msg)
            : base(msg)
        {
            Response = null;
        }

        public PiliException(Exception e)
        {
            Response = null;
        }

        public override string Message => Response == null ? base.Message : Response.ToString();

        public virtual string Details => _mDetails;

        public virtual int Code()
        {
            return Response == null ? -1 : (int)Response.StatusCode;
        }
    }
}
