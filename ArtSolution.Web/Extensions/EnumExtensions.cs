using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Extensions
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
              bool markCurrentAsSelected = true, int[] valuesToExclude = null) 
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("参数必须为枚举类型", "enumObj");

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select new
                         {
                             ID = Convert.ToInt32(enumValue),
                             //Name = enumValue.GetDescripion(),
                         };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("参数必须为枚举类型", "enumObj");

            var values = from Enum enumValue in Enum.GetValues(typeof(TEnum))
                         select new
                         {
                             ID = Convert.ToInt32(enumValue),
                             Name = enumValue.GetDescription()//.GetDescription(),
                         };
            object selectedValue = null;
            return new SelectList(values, "ID", "Name", selectedValue);
        }


        public static SelectList EnumToDictionary<TEnum>(this TEnum enumObj, Func<Enum, String> getText,bool isSelected = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }
            Dictionary<Int32, String> enumDic = new Dictionary<int, string>();

            var values = Enum.GetValues(typeof(TEnum));

            foreach (Enum enumValue in values)
            {
                Int32 key = Convert.ToInt32(enumValue);
                String value = getText(enumValue);
                enumDic.Add(key, value);
            }

            var i = enumDic.Select(x => new
            {
                ID = x.Key,
                Name = x.Value
            });

            if (isSelected)
                return new SelectList(i, "Id", "Name", enumObj);
            else
            return new SelectList(i, "Id", "Name", null);
        }

        public static string GetDescription(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }
    }
}