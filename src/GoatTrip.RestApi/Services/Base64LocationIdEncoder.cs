using System;
using System.Text;

namespace GoatTrip.RestApi.Services
{
    public class Base64LocationIdEncoder
        : ILocationIdEncoder {

        public string Encode(string id) {
            var bytes = Encoding.Unicode.GetBytes(id);
            var encodedId = Convert.ToBase64String(bytes);
            return encodedId;
        }

        public string Decode(string encodedId) {
            var bytes = Convert.FromBase64String(encodedId);
            var decodedId = Encoding.Unicode.GetString(bytes);
            return decodedId;
        }
    }
}