namespace Magnum.CommandLine
{
    using System;
    using System.Text.RegularExpressions;

    public class Use_types_name_lowercased_removing_Command :
        ICommandNamingConvention
    {
        public string GetName<T>()
        {
            return GetName(typeof (T));
        }

        public string GetName(Type t)
        {
            string result = t.Name.Replace("Command", "");
            if(t.IsGenericType)
            {
                result = new Regex("`\\w*").Replace(result, "");
            }
            return result.ToLower();
        }
    }
}