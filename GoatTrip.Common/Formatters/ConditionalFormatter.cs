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
            if (field == null || fieldType == null || !_conditions.ShouldFormat(fieldType))
                return field;

            return _formatter.Format(field);
        }
    }
}