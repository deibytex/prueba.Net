using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Syscaf.Helpers
{
    public class PropertyHelper
    {

        private static readonly ConcurrentDictionary<Type, PropertyHelper[]> Cache = new ConcurrentDictionary<Type, PropertyHelper[]>();
        private static readonly MethodInfo CallInnerDelegateMethod = typeof(PropertyHelper).GetMethod(nameof(CallInnerDelegate), BindingFlags.NonPublic | BindingFlags.Static);
        public string Name { get; set; }
        public Func<object, object> Getter { get; set; }
        public static PropertyHelper[] GetProperties(Type type)
        => Cache
        .GetOrAdd(type, _ => type
        .GetProperties()
        .Select(property =>
        {
            var getMethod = property.GetMethod;
            var declaringClass = property.DeclaringType;
            var typeOfResult = property.PropertyType;
            // Func<Type, TResult>
            var getMethodDelegateType = typeof(Func<,>).MakeGenericType(declaringClass, typeOfResult);
            // c => c.Data
            var getMethodDelegate = getMethod.CreateDelegate(getMethodDelegateType);

            // CallInnerDelegate<Type, TResult>
            var callInnerGenericMethodWithTypes = CallInnerDelegateMethod
            .MakeGenericMethod(declaringClass, typeOfResult);
            // Func<object, object>
            var result = (Func<object, object>)callInnerGenericMethodWithTypes.Invoke(null, new[] { getMethodDelegate });
            return new PropertyHelper
            {
                Name = property.Name,
                Getter = result
            };
        }).ToArray());

        // trae el valor de la propiedad 
        public static PropertyHelper GetProperty(Type type, string name)
        {
            PropertyHelper[] propiedades = GetProperties(type);
            return propiedades.Where(w => w.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase)).First();
        }

        // trae el valor de la propiedad en la cache 
        public static object GetValueProperty(string name, object x)
        {
            PropertyHelper[] propiedades = GetProperties(x.GetType());
            return propiedades.Where(w => w.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase)).First().Getter(x);
        }

        // Called via reflection.
        private static Func<object, object> CallInnerDelegate<TClass, TResult>(
        Func<TClass, TResult> deleg)
        => instance => deleg((TClass)instance);
    }
}