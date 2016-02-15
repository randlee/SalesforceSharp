using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Class SalesforceIdAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,Inherited = true,AllowMultiple = false)]
    public class SalesforceIdAttribute : SalesforceAttribute
    {
        /// <summary>
        /// Gets the object type the salesforce Id it is decorating refers to
        /// </summary>
        /// <value>The type of the identifier.</value>
        public Type IdType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesforceIdAttribute"/> class.
        /// </summary>
        /// <param name="type">The object type the salesforce Id it is decorating refers to.</param>
        public SalesforceIdAttribute(Type type)
        {
            IdType = type;
        }

        /// <summary>
        /// Gets all IDs of the specified object type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="idType">Type of the identifier.</param>
        /// <returns>Dictionary&lt;System.String, Type&gt;.</returns>
        public static Dictionary<string,Type> GetIdsofType<T>(T record, Type idType=null) where T : new()
        {
            var properties = new Dictionary<string, Type>();

            var props = record.GetType().GetProperties();
            foreach (var prop in props)
            {
                var sfIdAttrs = prop.GetCustomAttributes(typeof(SalesforceIdAttribute), true);
                
                if (sfIdAttrs.Any())
                {
                    var sfAttr = sfIdAttrs.FirstOrDefault() as SalesforceIdAttribute;
                    // If Ignore then we shouldn't include it.
                    if (sfAttr != null && !sfAttr.Ignore)
                    {
                        if (idType == null || sfAttr.IdType == idType)
                        {
                            properties.Add(sfAttr.GetFieldName(prop), sfAttr.IdType);
                            continue;
                        }
                    }
                }
            }

            return properties;
        }
    }
}