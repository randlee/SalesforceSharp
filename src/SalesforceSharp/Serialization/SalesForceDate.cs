using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Class SalesForceDate.
    /// </summary>
    public class SalesForceDate
    {
        /// <summary>
        /// Converts DateTime object to salesforce datetime string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToSalesforce(DateTime dateTime)
        {
            if (dateTime != null)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            return null;
        }
        /// <summary>
        /// Converts to date time.
        /// </summary>
        /// <param name="sfDateString">The sf date string.</param>
        /// <returns>DateTime.</returns>
        public static DateTime?  ConvertToDateTime(string sfDateString)
        {
            DateTime date;
            if(DateTime.TryParse(sfDateString,out date))
            {
                return date;
            }
            return null;
        }
    }

}
