using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoatTrip.RestApi.Services
{
    public class UPRNEncoder : ILocationIdEncoder 
    {
        public string Encode(string id)
        {
            return id;
        }

        public string Decode(string encodedId)
        {
            return encodedId;
        }
    }
}