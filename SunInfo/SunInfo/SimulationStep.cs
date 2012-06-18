using System;

namespace SunInfo
{
    public class SimulationStep
    {
        public enum Direction
        {
            Undefined = 0,
            Forward,
            Backward
        }

        public enum Quantity
        {
            Undefined = 0,
            OneSecond,
            OneMinute,
            OneHour,
            OneDay,
            OneMonth,
            OneYear,
        }

        public Direction Dir { get; set; }
        public Quantity Qty { get; set; }

        public SimulationStep()
        {
            Normal();
        }

        public override string ToString()
        {
            string qty = "";
            switch (Qty)
            {
                case Quantity.OneSecond:
                    qty = "1 sec";
                    break;
                case Quantity.OneMinute:
                    qty = "1 min";
                    break;
                case Quantity.OneHour:
                    qty = "1 hr";
                    break;
                case Quantity.OneDay:
                    qty = "1 day";
                    break;
                case Quantity.OneMonth:
                    qty = "1 month";
                    break;
                case Quantity.OneYear:
                    qty = "1 year";
                    break;
                case Quantity.Undefined:
                default:
                    qty = "";
                    break;
            }
            string dir = "";
            switch(Dir)
            {
                case Direction.Forward:
                    dir = "fwd";
                    break;
                case Direction.Backward:
                    dir = "bkwd";
                    break;
                case Direction.Undefined:
                    dir = "still";
                    break;
            }
            return String.Format("{0} {1}", qty, dir).Trim();
        }

        public bool IsStill()
        {
            return Dir == Direction.Undefined || Qty == Quantity.Undefined;
        }

        public bool IsNormal()
        {
            return Dir == Direction.Forward || Qty == Quantity.OneSecond;
        }

        public void Normal()
        {
            Dir = Direction.Forward;
            Qty = Quantity.OneSecond;
        }

        public DateTime Step(DateTime currDateTime)
        {
            DateTime newDateTime;
            int unit;
            switch (Dir)
            {
                case Direction.Backward:
                    unit = -1;
                    break;
                case Direction.Forward:
                    unit = 1;
                    break;
                case Direction.Undefined:
                default:
                    unit = 0;
                    break;
            }
            switch(Qty)
            {
                case Quantity.OneSecond:
                    newDateTime = currDateTime.AddSeconds(unit);
                    break;
                case Quantity.OneMinute:
                    newDateTime = currDateTime.AddMinutes(unit);
                    break;
                case Quantity.OneHour:
                    newDateTime = currDateTime.AddHours(unit);
                    break;
                case Quantity.OneDay:
                    newDateTime = currDateTime.AddDays(unit);
                    break;
                case Quantity.OneMonth:
                    newDateTime = currDateTime.AddMonths(unit);
                    break;
                case Quantity.OneYear:
                    newDateTime = currDateTime.AddYears(unit);
                    break;
                case Quantity.Undefined:
                default:
                    newDateTime = currDateTime;
                    break;
            }
            return newDateTime;
        }

        public bool Rew()
        {
            if (Dir == Direction.Backward)
            {
                if (Qty == Quantity.OneYear)
                    return false;
                Qty++;
            }
            else if (Dir == Direction.Undefined)
            {
                Dir = Direction.Backward;
                Qty = Quantity.OneSecond;
            }
            else if (Dir == Direction.Forward)
            {
                Qty--;
                if(Qty == Quantity.Undefined)
                {
                    Dir = Direction.Undefined;
                }
            }
            return true;
        }

        public bool Ff()
        {
            if (Dir == Direction.Forward)
            {
                if (Qty == Quantity.OneYear)
                    return false;
                Qty++;
            }
            else if (Dir == Direction.Undefined)
            {
                Dir = Direction.Forward;
                Qty = Quantity.OneSecond;
            }
            else if (Dir == Direction.Backward)
            {
                Qty--;
                if (Qty == Quantity.Undefined)
                {
                    Dir = Direction.Undefined;
                }
            }
            return true;
        }
    }
}
