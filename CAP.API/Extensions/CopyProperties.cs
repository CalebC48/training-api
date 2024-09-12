using System.Linq;

namespace CAP.API.Extensions
{
    /// <summary>
    /// A class to facilitate copying properties from one object to another
    /// </summary>
    public static class CopyProperties
    {
        /// <summary>
        /// Copies all the properties that have the same name between two objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source">The source object to copy from</param>
        /// <param name="dest">The destination object to copy to</param>
        /// <param name="ignoreId">Ignore Id when copying</param>
        public static void CopyPropertiesTo<T, TU>(this T source, TU dest, bool ignoreId = false)
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var sourceProp in sourceProps)
            {
                var destProp = destProps.FirstOrDefault(x =>
                    x.Name == sourceProp.Name && x.PropertyType == sourceProp.PropertyType &&
                    (!ignoreId || x.Name != "Id"));
                if (destProp != null)
                {
                    destProp.SetValue(dest, sourceProp.GetValue(source, null));
                }
                // Allow calling the new object constructor if it takes the source object as a parameter
                else
                {
                    // Find a destination property with the same name as the source property
                    var matchingDestProp =
                        destProps.FirstOrDefault(x => x.Name == sourceProp.Name && (!ignoreId || x.Name != "Id"));

                    // Check if the destination property is not null and is a reference type
                    if (matchingDestProp == null || !matchingDestProp.PropertyType.IsClass) continue;

                    // Check if the destination property already exists
                    var existingValue = matchingDestProp.GetValue(dest);

                    if (existingValue is not null && existingValue.GetType() == sourceProp.PropertyType)
                    {
                        // Copy properties from the source object to the existing destination object
                        sourceProp.GetValue(source)?.CopyPropertiesTo(existingValue);
                    }
                    else
                    {
                        // Check if the destination property's type has a constructor that takes the source property's type as a parameter
                        var constructor =
                            matchingDestProp.PropertyType.GetConstructor(new[] { sourceProp.PropertyType });
                        if (constructor == null) continue;
                        // Create a new instance of the destination property's type and set its value to the new instance
                        var newValue = constructor.Invoke(new[] { sourceProp.GetValue(source, null) });
                        matchingDestProp.SetValue(dest, newValue);
                    }
                }
            }
        }
    }
}