using System;

namespace Task11.TBot
{
    public class UserState
    {
        public Step CurrentStep { get; set; }
        public string FirstCurrency { get; set; }
        public string SecondCurrency { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public DateTime RateDate { get; set; }
        public bool DateNow { get; set; }
        public bool Success { get; set; }
    }

    public enum Step
    {
        FIRST_CURR,
        SECOND_CURR,
        DATE,
        AMOUNT
    }
}
