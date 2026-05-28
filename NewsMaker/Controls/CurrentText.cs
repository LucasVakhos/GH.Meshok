namespace NewsMaker
{
    public class CurrentText : SimpleInfoText
    {
        protected CurrentCounter typeCounter;
        public CurrentText(InfoType infoType) : base(infoType)
        {
            switch (infoType)
            {
                case InfoType.Sended:
                    typeCounter = new CurrentCounter(this);
                    break;
                case InfoType.Errors:
                    typeCounter = new CurrentCounter(this);
                    break;
                case InfoType.Elapsed:
                    typeCounter = new ElapsedCounter(this);
                    break;
                case InfoType.WorkStatus:
                    typeCounter = new WorkStatusCounter(this);
                    break;
                default:
                    break;
            }
            //typeCounter.DefaultText();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                if (_infoType == InfoType.WorkStatus)
                    (typeCounter as WorkStatusCounter).Dispose();
            base.Dispose(disposing);
        }
        public override void IncCurrent()
        {
            typeCounter.IncCurrent();
        }
        public override void Reset()
        {
            typeCounter.Reset();
        }
        internal override string GetText()
        {
            return typeCounter.GetText();
        }
        public override void RestoreText()
        {
            SetText(GetText());
        }
    }
}
