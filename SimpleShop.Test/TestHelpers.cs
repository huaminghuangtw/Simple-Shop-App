// ############################################################################################################################
//
// These function are needed for testing and replace the #define of the original answer.
// However they are not common practice.
// And not written to be understood at this point in time.
//
// ############################################################################################################################

using System;
using System.Reflection;

namespace SimpleShop.Test
{
public static class TestHelpers
{
    private static Type? DeterminType<T>(string classname, string @namespace = "", Assembly assembly=null) where T : new()
    {
        assembly ??= typeof(T).Assembly;
        var type_sting = String.Format("{0}.{1}", @namespace, classname);
        Type? class_type = assembly.GetType(type_sting);
        class_type ??= Type.GetType(type_sting);
        return class_type;
    }

    public static bool IsClassDefined<T>(string classname, string @namespace="") where T : new()
    {
        return DeterminType<T>(classname, @namespace) != null;
    }

    public static T CreateClassIfDefinedOrDefault<T>(string classname, string @namespace="") where T : new()
    {
        var class_type =  DeterminType<T>(classname, @namespace);
        if (class_type == null || class_type.IsSubclassOf(typeof(T))) {
            return (T) Activator.CreateInstance(class_type);
        }
        return new T();
    }

    public static void Shuffle(this KeywordPair[] pairs)
    {
        var rnd = new Random();
        foreach (var p in pairs)
        {
            pairs.Swap(rnd.Next(0,pairs.Length), rnd.Next(0,pairs.Length));
        }
    }

    public static void Swap(this KeywordPair[] pairs, int first, int second)
    {
        var tmp = pairs[first];
        pairs[first] = pairs[second];
        pairs[second] = tmp;
    }
}   // class static class TestHelpers
}   // namespace SimpleShop.Test