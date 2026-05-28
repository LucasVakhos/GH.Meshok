using System;
namespace NewsMaker
{
    public class ElapsedCounter : TotalCounter
    {
        private readonly DateTime startTime;
        private DateTime endTime;
        public ElapsedCounter(SimpleInfoText text) : base(text)
        {
            startTime = DateTime.Now;
        }
        internal override string GetText()
        {
            endTime = DateTime.Now;
            var fromTime = endTime.Subtract(startTime);
            var fromTicks = fromTime.Ticks / Math.Max(1, Current);
            var forTicks = fromTicks * (TotalCount - Current);
            var forTime = new TimeSpan(forTicks);
            return $"Прошло : {fromTime:hh\\:mm\\:ss}, Осталось:  {forTime:hh\\:mm\\:ss}";
        }
    }
}
