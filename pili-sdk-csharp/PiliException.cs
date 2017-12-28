using System;
using System.Net.Http;

namespace Qiniu.Pili
{
    public class PiliException : Exception
    {
        private const string ErrNotFound = "stream not found";
        private const string ErrDuplicate = "stream already exists";
        private const string ErrNotLive = "no data";

        public PiliException(HttpResponseMessage response)
        {
            Response = response;
        }

        public PiliException(string msg) : base(msg)
        {
        }

        public PiliException(Exception e) : base(null, e)
        {
        }

        public HttpResponseMessage Response { get; }

        public virtual int Code => Response == null ? -1 : (int)Response.StatusCode;

        public virtual bool Duplicate => Code == 614;

        public virtual bool NotFound => Code == 612;

        public virtual bool NotInLive => Code == 619;

        public override string Message
        {
            get
            {
                if (Response == null)
                {
                    return base.Message;
                }

                switch (Code)
                {
                    case 614:
                        return ErrDuplicate;
                    case 612:
                        return ErrNotFound;
                    case 619:
                        return ErrNotLive;
                    default:
                        return Response.ReasonPhrase;
                }
            }
        }
    }
}
