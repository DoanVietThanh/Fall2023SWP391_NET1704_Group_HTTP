using DocumentFormat.OpenXml.Bibliography;
using DriverLicenseLearningSupport.Models;
using System.Runtime.CompilerServices;

namespace DriverLicenseLearningSupport.Utils
{
    public class DateTimeHelper
    {
        public static int ToSpecificMonthValid(int totalMonth)
        {
            int currMonth = DateTime.Now.Month;
            int count = currMonth + totalMonth;
            if(count > 12)
            {
                return count = count - 12;
            }
            return -1;
        }

        public static IEnumerable<DateTime> GetDates(int month, int year)
        {
            //yield return Enumerable.Range(1, DateTime.DaysInMonth(year, month)) // Days: 1,2,3...etc
            //                       .Select(day => new DateTime(year, month, day))
            //                       .FirstOrDefault(); // Init DateTime foreach day
            for (int i=1; i<=DateTime.DaysInMonth(year, month); ++i)
            {
                yield return new DateTime(year, month, i);
            }
        }

        public static IEnumerable<DateTime> GenerateRangeFromToDateTime(int totalMonth, DateTime startDate)
        {
            // get start month
            int startMonth = startDate.Month;
            // get start year
            int startYear = startDate.Year;
            for (int i = 0; i <= totalMonth; ++i)
            {
                if (startMonth + i <= 12)
                {
                    foreach (DateTime dt in GetDates(startMonth + i, startYear))
                    {
                        if(dt >= startDate && dt < startDate.AddMonths(totalMonth))
                        {
                            yield return dt;
                        }
                    }
                }
            }

            //int remainingDays = DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day;
            //if(remainingDays > 0)
            //{
            //    foreach (DateTime dt in GetDates(startMonth + totalMonth , startYear))
            //    {
            //        //var endDate = startDate.AddMonths(3).Subtract(TimeSpan.FromDays(1));
            //        if (dt >= startDate.AddMonths(totalMonth - 1) && dt.Day <= startDate.Day)
            //        {
            //            yield return dt;
            //        }
            //    }
            //}
            

            int isOverValidMonth = DateTimeHelper.ToSpecificMonthValid(totalMonth);
            if (isOverValidMonth > 0)
            {
                //var tempList = new List<DateTime>();
                for (int i = 1; i <= isOverValidMonth; ++i)
                {
                    //tempList = DateTimeHelper.GetDates(i, currYear);
                    foreach (DateTime dt in GetDates(i, startYear))
                    {
                        yield return dt;
                    }
                }
            }
        }

        public static IEnumerable<WeekdayScheduleModel> GenerateRangeWeekday(int totalMonth, DateTime startDate,
            Guid courseId)
        {
            // get all date time from current to next months
            var dateTimes = GenerateRangeFromToDateTime(totalMonth, startDate).ToList();

            // first weekday 
            var firstDay = Convert.ToInt32(dateTimes.First().DayOfWeek);
            // last weekday 
            var lastDay = Convert.ToInt32(dateTimes.Last().DayOfWeek);

            // init dates list
            List<DateTime> dates = new List<DateTime>();

            // Sunday <- weekday = 0
            if(firstDay == 0)
            {
                // Add more 6 date before to complete weekday schedule
                firstDay = 7;
            }
            // get first day of month
            var firstDayOfMonth = dateTimes.First();
            // generate week schedule from frist day to prev
            for(int i = firstDay - 1; i >= 1; --i)
            {
                // if first start date is 30 || 31
                if(firstDayOfMonth.Day == 30 || 
                    firstDayOfMonth.Day == 31)
                {
                    // add date before first month's day to complete weekday schedule
                    dates.Add(new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month - 1,
                        // get prev days by timespan value
                        firstDayOfMonth.Subtract(TimeSpan.FromDays(i)).Day));
                }
                else // start date < 30
                {
                    var substractDate = firstDayOfMonth.Subtract(TimeSpan.FromDays(i)).Day;

                    // substract date 30 || 31
                    if(substractDate == 30 || substractDate == 31)
                    {
                        // add date before first month's day to complete weekday schedule
                        dates.Add(new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month - 1,
                            // get prev days by timespan value
                            firstDayOfMonth.Subtract(TimeSpan.FromDays(i)).Day));
                    }
                    else // substract date < 30
                    {
                        // add date before first month's day to complete weekday schedule
                        dates.Add(new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month,
                            // get prev days by timespan value
                            firstDayOfMonth.Subtract(TimeSpan.FromDays(i)).Day));
                    }
                }

            }

            // add range datetimes
            dates.AddRange(dateTimes);

            // get last day of month
            var lastDayOfMonth = dateTimes.Last();
            for(int i = 1; i <= 7 - lastDay; ++i)
            {
                // add date before first month's day to complete weekday schedule
                dates.Add(new DateTime(lastDayOfMonth.Year, lastDayOfMonth.Month,
                    // get prev days by timespan value
                    lastDayOfMonth.Add(TimeSpan.FromDays(i)).Day));
            }

            // get total week 
            var totalWeekday = dates.Count / 7;

            // init offset
            var offset = 0;
            // each week -> week schedule
            for (int i = 0; i < totalWeekday; ++i)
            {
                // skip offset element and get next 7 day
                // to create weekday schedule
                var weeks = dates.Skip(offset).Take(7).ToList();
                // increase offset to next weekday
                offset += 7;

                //// generate other date
                //if(weeks.Count <= 7)
                //{
                //    for(int j = 1; j <= 7 -weeks.Count; ++j) 
                //    {
                //        weeks.Add(new DateTime)
                //    }
                //}
                
                // Format datetime (dd) (MM)
                var startday = weeks[0].ToString("dd");
                var endday = weeks[6].ToString("dd");
                var monthOfStartDay = weeks[0].ToString("MM");
                var monthOfEndDay = weeks[6].ToString("MM");
                
                // return weekday schedule model
                yield return new WeekdayScheduleModel
                {
                    Monday = weeks[0],
                    Tuesday = weeks[1],
                    Wednesday = weeks[2],
                    Thursday = weeks[3],
                    Friday = weeks[4],
                    Saturday = weeks[5],
                    Sunday = weeks[6],
                    CourseId = courseId.ToString(),
                    WeekdayScheduleDesc = $"{startday}/{monthOfStartDay} To {endday}/{monthOfEndDay}"
                };
            }
        }

        public static IEnumerable<DateTime> GenerateDateTimesFromWeekDay(WeekdayScheduleModel weekday)
        {
            var dates = new List<DateTime>()
            { weekday.Monday, weekday.Tuesday, weekday.Wednesday,
                weekday.Thursday, weekday.Friday, weekday.Saturday, weekday.Sunday};
            return dates;
        }

        public static List<double> GenerateMonthlyIncome(List<CoursePackageReservationModel> reservations)
        {
            var januaryIncome = reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 1)
                .Select(x => x.PaymentAmmount).Sum();
            var februaryIncome = reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 2)
                .Select(x => x.PaymentAmmount).Sum();
            var marchIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 3)
                .Select(x => x.PaymentAmmount).Sum();
            var aprilIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 4)
                .Select(x => x.PaymentAmmount).Sum();
            var mayIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 5)
                .Select(x => x.PaymentAmmount).Sum();
            var juneIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 6)
                .Select(x => x.PaymentAmmount).Sum();
            var julyIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 7)
                .Select(x => x.PaymentAmmount).Sum();
            var augustIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 8)
                .Select(x => x.PaymentAmmount).Sum();
            var septemberIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 9)
                .Select(x => x.PaymentAmmount).Sum();
            var octoberIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 10)
                .Select(x => x.PaymentAmmount).Sum();
            var novemberIncome =reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 11)
                .Select(x => x.PaymentAmmount).Sum();
            var decemberIncome = reservations.Where(x => Convert.ToDateTime(x.CreateDate).Month == 12)
                .Select(x => x.PaymentAmmount).Sum();

            return new List<Double>()
            {
                Convert.ToDouble(januaryIncome),
                Convert.ToDouble(februaryIncome),
                Convert.ToDouble(marchIncome),
                Convert.ToDouble(aprilIncome),
                Convert.ToDouble(mayIncome),
                Convert.ToDouble(juneIncome),
                Convert.ToDouble(julyIncome),
                Convert.ToDouble(augustIncome),
                Convert.ToDouble(septemberIncome),
                Convert.ToDouble(octoberIncome),
                Convert.ToDouble(novemberIncome),
                Convert.ToDouble(decemberIncome)
            };
        }

        public static List<int> GenerateWeeklySlots(List<TeachingScheduleModel> schedules)
        {
            var totalInMonday = 0;
            var totalInTuesday = 0;
            var totalInWednesday = 0;
            var totalInThursday = 0;
            var totalInFriday = 0;
            var totalInSaturday = 0;
            var totalInSunday = 0;

            foreach(var schedule in schedules)
            {
                // schedule in monday
                if ((int)schedule.TeachingDate.DayOfWeek == 1)
                {
                    ++totalInMonday;
                }
                // schedule in tuesday
                else if ((int)schedule.TeachingDate.DayOfWeek == 2)
                {
                    ++totalInTuesday;
                }
                // schedule in wednesday
                else if ((int)schedule.TeachingDate.DayOfWeek == 3)
                {
                    ++totalInWednesday;
                }
                // schedule in thursday
                else if ((int)schedule.TeachingDate.DayOfWeek == 4)
                {
                    ++totalInThursday;
                }
                // schedule in friday
                else if ((int)schedule.TeachingDate.DayOfWeek == 5)
                {
                    ++totalInFriday;
                }
                // schedule in saturday
                else if ((int)schedule.TeachingDate.DayOfWeek == 6)
                {
                    ++totalInSaturday;
                }
                // schedule in sunday
                else
                {
                    ++totalInSunday;
                }
            }

            return new List<int> 
            { totalInMonday, totalInTuesday, totalInWednesday,
                totalInThursday, totalInFriday, totalInSaturday, totalInSunday };
        }

        public static List<int> MultipleWeeklySlots(List<int> prevList, List<TeachingScheduleModel> weekdaySlots)
        {
            var nextList = GenerateWeeklySlots(weekdaySlots);

            for(int i=0 ; i<prevList.Count; i++)
            {
                prevList[i] += nextList[i];
            }

            return prevList;
        }
    }
}
