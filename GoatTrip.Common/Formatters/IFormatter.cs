namespace GoatTrip.Common.Formatters
{
    public interface IFormatter<T>
    {
        T Format(T input);
    }
}
