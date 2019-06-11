using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Services.Utils
{
    public class HttpClientManagerException : Exception
    {
        public HttpClientManagerException():base()
        {
        }

        public HttpClientManagerException(string message) : base(message)
        {

        }
        public HttpContent ResponseConetent { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
