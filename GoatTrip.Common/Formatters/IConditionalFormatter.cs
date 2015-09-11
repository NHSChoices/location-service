namespace GoatTrip.Common.Formatters
{
    public interface IConditionalFormatter<TF, in TC>
    {
        TF DetermineConditionsAndFormat(TF field, TC fieldType);
    }
}