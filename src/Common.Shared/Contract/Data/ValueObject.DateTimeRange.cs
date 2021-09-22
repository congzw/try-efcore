using System;

namespace Common.Shared.Contract.Data
{
    /// <summary>
    /// 自定义的时间段模型，可用于有开始时间和结束时间的场合
    /// 例如课节等
    /// </summary>
    public class DateTimeRange : ValueObject<DateTimeRange>
    {
        public DateTimeOffset Start { get; private set; }
        public DateTimeOffset End { get; private set; }

        public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
        }
        public DateTimeRange(DateTimeOffset start, TimeSpan duration)
        {
            Start = start;
            End = start.Add(duration);
        }
        protected DateTimeRange() { }
        public DateTimeRange NewEnd(DateTimeOffset newEnd)
        {
            return new DateTimeRange(this.Start, newEnd);
        }
        public DateTimeRange NewDuration(TimeSpan newDuration)
        {
            return new DateTimeRange(this.Start, newDuration);
        }
        public DateTimeRange NewStart(DateTimeOffset newStart)
        {
            return new DateTimeRange(newStart, this.End);
        }

        public static DateTimeRange CreateMinuteRange(DateTimeOffset startDate, int minutes)
        {
            return new DateTimeRange(startDate, startDate.AddMinutes(minutes));
        }
        public static DateTimeRange CreateHourRange(DateTimeOffset startDate, int hour)
        {
            return new DateTimeRange(startDate, startDate.AddHours(hour));
        }
        public static DateTimeRange CreateOneDayRange(DateTimeOffset day)
        {
            return new DateTimeRange(day, day.AddDays(1));
        }
        public static DateTimeRange CreateOneWeekRange(DateTimeOffset startDay)
        {
            return new DateTimeRange(startDay, startDay.AddDays(7));
        }
        /// <summary>
        /// 本周
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTimeRange CreateWeekRange(DateTimeOffset now)
        {
            var startWeek = now.AddDays(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))).Date;
            var endWeek = startWeek.AddDays(6).Date;
            return new DateTimeRange(startWeek, endWeek);
        }
        /// <summary>
        /// 本月
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTimeRange CreateMonthRange(DateTimeOffset now)
        {
            var startMonth = now.AddDays(1 - now.Day).Date;
            var endMonth = startMonth.AddMonths(1).AddDays(-1).Date;
            return new DateTimeRange(startMonth, endMonth);
        }


        /// <summary>
        /// 保留时间，把日期部分自动修正为指定的时间的日期
        /// </summary>
        /// <param name="dayDate"></param>
        /// <returns></returns>
        public DateTimeRange FixDateTimeRangeOfSomeDay(DateTimeOffset dayDate)
        {
            var defaultDayDate = dayDate.Date;
            var startFix = defaultDayDate.Add(this.Start.TimeOfDay);
            var endFix = defaultDayDate.Add(this.End.TimeOfDay);
            var dateTimeRange = new DateTimeRange(startFix, endFix);
            return dateTimeRange;
        }

        /// <summary>
        /// 时间间隔的分钟数部分。
        /// </summary>
        /// <returns></returns>
        public int DurationInMinutes()
        {
            return (End - Start).Minutes;
        }
        /// <summary>
        /// 查看两个时间段是否重叠
        /// </summary>
        /// <param name="dateTimeRange"></param>
        /// <returns></returns>
        public bool Overlaps(DateTimeRange dateTimeRange)
        {
            return this.Start < dateTimeRange.End &&
                   this.End > dateTimeRange.Start;
        }
    }
}