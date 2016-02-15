using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Use to manage how each fields are managed when communicate with Salesforce
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true,AllowMultiple = false)]
    public class SalesforceAttribute : Attribute
    {
        /// <summary>
        /// Exclude this field when pulling or updating to salesforce
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Exclude this field when serializing data to update to salesforce
        /// </summary>
        public bool IgnoreUpdate{ get; set; }

        /// <summary>
        /// FieldId in Salesforce for this property.  If none, then it will use the property name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get; set; }

        /// <summary>
        /// Gets the name of the field from a decorated property.
        /// </summary>
        /// <param name="prop">The decorated property.</param>
        /// <returns>FieldId in Salesforce for this property.  If none, then it will use the property name.</returns>
        public string GetFieldName(PropertyInfo prop)
        {
            if(!string.IsNullOrWhiteSpace(FieldName))
            {
                return FieldName;
            }
            return prop.Name;
        }

        /// <summary>
        /// Gets the properties decorated with SalesforceAttribute
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <param name="rank">The desired rank. Rank=0 will return all non-ignored properties. Rank>0 will </param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetProperties<T>(int rank=0) where T : new()
        {
            var properties = new List<string>();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var sfIdAttrs = prop.GetCustomAttributes(typeof(SalesforceAttribute), true);

                if (sfIdAttrs.Any())
                {
                    var sfAttr = sfIdAttrs.FirstOrDefault() as SalesforceAttribute;
                    // If Ignore then we shouldn't include it.
                    if (sfAttr != null && !sfAttr.Ignore)
                    {
                        if(rank == 0 || (sfAttr.Rank > 0 && sfAttr.Rank <= rank))
                        {
                            properties.Add(sfAttr.GetFieldName(prop));
                            continue;
                        }
                    }
                }
            }
            return properties;
        }

        /// <summary>
        /// Gets the desired properties and associated values from specified record.
        /// </summary>
        /// <typeparam name="T">Type for generic</typeparam>
        /// <param name="record">The record to analyze.</param>
        /// <param name="rank">The desired rank. Rank=0 will return all non-ignored properties. Rank>0 will </param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public static Dictionary<string,string> GetPropertyValues<T>(T record, int rank = 0) where T : new()
        {
            var properties = new Dictionary<string,string>();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var sfIdAttrs = prop.GetCustomAttributes(typeof(SalesforceAttribute), true);

                if (sfIdAttrs.Any())
                {
                    var sfAttr = sfIdAttrs.FirstOrDefault() as SalesforceAttribute;
                    // If Ignore then we shouldn't include it.
                    if (sfAttr != null && !sfAttr.Ignore)
                    {
                        if (rank == 0 || (sfAttr.Rank > 0 && sfAttr.Rank <= rank))
                        {
                            var value = prop.GetValue(record, null);
                            properties.Add(sfAttr.GetFieldName(prop),value==null?"":value.ToString());
                            continue;
                        }
                    }
                }
            }

            return properties;
        }
    }
}
