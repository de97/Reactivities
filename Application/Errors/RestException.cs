using System;
using System.Net;

namespace Application.Errors
{
    //used to make likely errors such as not finding a resource in database
    public class RestException : Exception
    {

        public RestException(HttpStatusCode code, object erros = null)
        {
            Code = code;

            Errors = Errors;

        }

        public HttpStatusCode Code { get; }

        public object Errors { get; }
    }
}