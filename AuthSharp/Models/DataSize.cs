using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthSharp.Models
{
    public class DataSize
    {
        long val;
        public DataSize(long val)
        {
            this.val = val;
        }
        public override string ToString()
        {
            var intervals = new [] {
                    new { Base = 1d,                  Max = 1000L,            Unit = "B" },       
                    new { Base = 1024d,               Max = 1000L*1000,       Unit = "KB" },
                    new { Base = 1024d * 1024,        Max = 1000L*1000*1000,  Unit = "MB" },
                    new { Base = 1024d * 1024 * 1024, Max = long.MaxValue,    Unit = "GB" }
                };
            var interval = intervals.Where(item => Math.Abs(val) < item.Max).OrderBy(item => item.Max).Take(1).Single();

            return (val / interval.Base).ToString("G3") + " " + interval.Unit;
        }
    }

    public class DataSizeFormatter : IFormatProvider, ICustomFormatter
    {

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            long? val = arg as long?;
            if (val != null && val >= 0)
            {
                var intervals = new[] {
                    new { Base = 1d,                  Max = 1000L,            Unit = "B" },       
                    new { Base = 1024d,               Max = 1000L*1000,       Unit = "KB" },
                    new { Base = 1024d * 1024,        Max = 1000L*1000*1000,  Unit = "MB" },
                    new { Base = 1024d * 1024 * 1024, Max = long.MaxValue,    Unit = "GB" }
                };
                var interval = intervals.Where(item => val < item.Max).OrderBy(item => item.Max).Take(1).Single();

                return val / interval.Base + interval.Unit;
            }

            return null;
        }
    }
}