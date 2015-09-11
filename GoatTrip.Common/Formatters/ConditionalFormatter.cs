namespace GoatTrip.Common.Formatters
{
    public class ConditionalFormatter<TF, TC> : IConditionalFormatter<TF, TC>
    {
        private readonly IFormatConditions<TC> _conditions;
        private readonly IFormatter<TF> _formatter;

        public ConditionalFormatter(IFormatter<TF> formatter, IFormatConditions<TC> conditions)
        {
            _formatter = formatter;
            _conditions = conditions;
        }

        public TF DetermineConditionsAndFormat(TF field, TC fieldType)
        {
            return _conditions.ShouldFormat(fieldType) ? _formatter.Format(field) : field;
        }
    }
}