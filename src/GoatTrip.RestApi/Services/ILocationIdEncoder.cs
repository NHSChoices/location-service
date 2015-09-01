namespace GoatTrip.RestApi.Services
{
    public interface ILocationIdEncoder
    {
        string Encode(string id);
        string Decode(string encodedId);
    }
}