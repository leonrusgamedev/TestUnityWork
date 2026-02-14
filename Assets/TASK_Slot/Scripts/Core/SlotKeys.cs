namespace Rusleo.TestTask.Core
{
    public static class SlotKeys
    {
        // UIButtonDataBind: Btn{buttonName}Enable
        public const string BtnStartEnable = "BtnStartEnable";
        public const string BtnStopEnable = "BtnStopEnable";

        // Debug / UI Text (если надо)
        public const string SlotState = "SlotState";

        // Result
        public const string ResultIndex = "SlotResultIndex";

        // Events: Model.EventManager.Invoke(...)
        public const string SlotConfig = "SlotConfig";

    }
}