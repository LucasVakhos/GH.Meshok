using System.Collections.Generic;
namespace NewsMaker
{
    public class TotalCounter : CurrentCounter
    {
        private static int _totalCount;
        protected static List<TotalCounter> Counters = new List<TotalCounter>();
        public TotalCounter(SimpleInfoText text) : base(text)
        {
            Counters.Add(this);
        }
        public static int TotalCount
        {
            get => _totalCount;
            set
            {
                Reset();
                _totalCount = value;
                if (_totalCount != value)
                    foreach (var item in Counters)
                        item.SetText();
            }
        }
        public static int Current { get; private set; }
        internal new static void IncCurrent()
        {
            Current++;
            foreach (var item in Counters) item.SetText();
        }
        internal new static void Reset()
        {
            Current = 0;
            _totalCount = 0;
            foreach (var item in Counters) item.SetText();
        }
        internal override string GetText()
        {
            return $"{_totalCount}";
        }
    }
}
