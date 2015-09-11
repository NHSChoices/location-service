namespace GoatTrip.Common.Formatters
{
    public interface IFormatConditions<in T>
    {
        bool ShouldFormat(T inputType);
    }
}