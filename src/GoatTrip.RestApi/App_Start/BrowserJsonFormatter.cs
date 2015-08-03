using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GoatTrip.RestApi {
    public class BrowserJsonFormatter
        : JsonMediaTypeFormatter {

        public BrowserJsonFormatter(bool identResponse) {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            if (identResponse)
                SerializerSettings.Formatting = Formatting.Indented;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType) {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}