using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace ArtSolution.ComponentModel
{
    /// <summary>
    /// 泛型列表类型转换
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class GenericListTypeConverter<T> : TypeConverter
    {
        protected readonly TypeConverter typeConverter;

        /// <summary>
        /// 构造
        /// </summary>
        public GenericListTypeConverter()
        {
            typeConverter = TypeDescriptor.GetConverter(typeof(T));
            if (typeConverter == null)
                throw new InvalidOperationException(string.Format("类型转换器不存在，该类型为{0} ", typeof(T).FullName));
        }

        /// <summary>
        /// 从逗号分隔的字符串中获取字符串数组
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Array</returns>
        protected virtual string[] GetStringArray(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                var result = input
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToArray();
                return result;
            }

            return new string[0];
        }

        /// <summary>
        /// 该转换器是否可以将给定的源类型中的对象转换为使用上下文的本地类型的转换器
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="sourceType">Source type</param>
        /// <returns>Result</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {

            if (sourceType == typeof(string))
            {
                string[] items = GetStringArray(sourceType.ToString());
                return items.Any();
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 将给定的对象转换为转换器的本机类型.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="culture">Culture</param>
        /// <param name="value">Value</param>
        /// <returns>Result</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] items = GetStringArray((string)value);
                var result = new List<T>();
                Array.ForEach(items, s =>
                {
                    object item = typeConverter.ConvertFromInvariantString(s);
                    if (item != null)
                    {
                        result.Add((T)item);
                    }
                });

                return result;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// 使用指定的上下文和参数将给定的值对象转换为指定的目标类型
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="culture">Culture</param>
        /// <param name="value">对象</param>
        /// <param name="destinationType">目标类型</param>
        /// <returns>Result</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string result = string.Empty;
                if (value != null)
                {
                    //不使用 string.Join()因为它不支持不变的文化
                    for (int i = 0; i < ((IList<T>)value).Count; i++)
                    {
                        var str1 = Convert.ToString(((IList<T>)value)[i], CultureInfo.InvariantCulture);
                        result += str1;
                        // 最后元素不加“,”
                        if (i != ((IList<T>)value).Count - 1)
                            result += ",";
                    }
                }
                return result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
