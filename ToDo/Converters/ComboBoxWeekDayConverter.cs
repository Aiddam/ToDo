namespace ToDo.Converters
{
    public class ComboBoxWeekDayConverter : ComboBoxConverter
    {
        public ComboBoxWeekDayConverter() : base(new WeekDayConverter())
        {
        }
    }
}
