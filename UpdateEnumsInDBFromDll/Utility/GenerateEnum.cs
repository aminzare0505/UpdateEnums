using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateEnumsInDBFromDll.Utility
{
    public static class GenerateEnum
    {
        public static List<EnumValueDto> GetEnums(Type t)
        {
            var enumValues = System.Enum.GetValues(t);
            List<EnumValueDto> EnumValueDtoLs = new List<EnumValueDto>();
            foreach (var val in enumValues)
            {
                Type type = System.Enum.GetUnderlyingType(t);
                var id = Convert.ChangeType(val, type);
                string name = System.Enum.GetName(t, id);
                if (name == "Unknown")
                    name = "نامشخص";
                EnumValueDtoLs.Add(new EnumValueDto() { Key = int.Parse(id.ToString()), Value = name.ToString().Replace('_', ' ') });
            }
            return EnumValueDtoLs;
        }
    }
}
