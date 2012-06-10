using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
            ThirtyDays,
            ThreeHundredSixtyFiveDays,
        }

        public Direction Dir { get; set; }
        public Quantity Qty { get; set; }

        public SimulationStep()
        {
            Normal();
        }

        public bool IsStill()
        {
            return Dir == Direction.Undefined || Qty == Quantity.Undefined;
        }

        public void Normal()
        {
            Dir = Direction.Forward;
            Qty = Quantity.OneSecond;
        }

        public TimeSpan ToTimeSpan()
        {
            TimeSpan timeSpan;
            switch(Qty)
            {
                case Quantity.OneSecond:
                    timeSpan = new TimeSpan(0, 0, 1);
                    break;
                case Quantity.OneMinute:
                    timeSpan = new TimeSpan(0, 1, 0);
                    break;
                case Quantity.OneHour:
                    timeSpan = new TimeSpan(1, 0, 0);
                    break;
                case Quantity.OneDay:
                    timeSpan = new TimeSpan(1, 0, 0, 0);
                    break;
                case Quantity.ThirtyDays:
                    timeSpan = new TimeSpan(30, 0, 0, 0);
                    break;
                case Quantity.ThreeHundredSixtyFiveDays:
                    timeSpan = new TimeSpan(365, 0, 0, 0);
                    break;
                case Quantity.Undefined:
                default:
                    timeSpan = new TimeSpan(0);
                    break;
            }
            if (Dir == Direction.Backward)
            {
                timeSpan = -timeSpan;
            }
            return timeSpan;
        }

        public bool Rew()
        {
            if (Dir == Direction.Backward)
            {
                if (Qty == Quantity.ThreeHundredSixtyFiveDays)
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
                if (Qty == Quantity.ThreeHundredSixtyFiveDays)
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
