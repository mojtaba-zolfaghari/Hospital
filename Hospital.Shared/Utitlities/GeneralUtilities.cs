using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Hospital.Shared.Utitlities
{
    public static class GeneralUtilities
    {
        public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return condition ? transform(source) : source;
        }
        public static bool IsValidIranianNationalCode(this string input)
        {
            if (!Regex.IsMatch(input, @"^\d{10}$"))
                return false;

            var check = Convert.ToInt32(input.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return sum < 2 && check == sum || sum >= 2 && check + sum == 11;
        }

        /// <summary>
        /// only 2022-01-14 22:14:57 returns
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>2022-01-14</returns>
        /// <returns>2022-01-14 22:14:57</returns>
        public static string ToDateString(DateTime dateTime, bool fullDateAndTime)
        {
            if (fullDateAndTime)
                return dateTime.ToString("yyyy/MM/dd HH:mm:ss");

            return dateTime.ToString("yyyy/MM/dd");
        }

        public static bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static DateTime[] PrintDateList(DateTime StartDate, DateTime EndDate)
        {
            List<DateTime> allDates = new();
            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates.ToArray();
        }

        public static bool CheckIp(string UserIpList, string RequestedIp)
        {
            var userIpaddress = UserIpList.Split(",").Any(c => c == RequestedIp);
            if (userIpaddress)
                return true;
            return false;
        }

        public static Guid GetCurrentUserId(IHttpContextAccessor context)
        {
            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            var UserIdClaimn = Guid.Parse(claimsIdentity.FindFirst("UserId").Value);
            return UserIdClaimn;
        }

        public static string GetNullPropertiesMessage<T>(T obj, params string[] ignoreProperties)
        {
            List<string> nullProperties = new();

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (!ignoreProperties.Contains(property.Name))
                {
                    object value = property.GetValue(obj);

                    if (value == null || value is Guid && (Guid)value == Guid.Empty)
                    {
                        nullProperties.Add(property.Name);
                    }
                }
            }

            if (nullProperties.Count == 0)
            {
                return null;
            }
            else if (nullProperties.Count == 1)
            {
                return "The " + nullProperties[0] + " property is null.";
            }
            else
            {
                return "These properties NOT received, please check them: [" + string.Join(", ", nullProperties) + "]";
            }
        }

    }
}
