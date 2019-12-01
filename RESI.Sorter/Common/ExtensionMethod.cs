using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using Season;
using RESI.Sorter;

public static class Extend
{
    #region 基本


    // Extend ObservableCollection<T> Class
    public static void AddRange<T>(this System.Collections.ObjectModel.ObservableCollection<T> o, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            o.Add(item);
        }
    }
    /// <summary>
    /// 返回枚举的文字说明
    /// </summary>
    /// <param name="eEnum"></param>
    /// <returns></returns>
    public static string GetEnumDescription(this Enum eEnum)
    {
        FieldInfo fi = eEnum.GetType().GetField(eEnum.ToString());
        if (fi != null)
        {
            // 获取描述的属性。
            DescriptionAttribute attr = Attribute.GetCustomAttribute(fi,
                typeof(DescriptionAttribute), false) as DescriptionAttribute;
            if (attr != null)
            {
                return attr.Description;
            }
        }
        //。Net4.5
        //DescriptionAttribute attribute = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
        return "";
    }
    /// <summary>
    /// 返回枚举的文字说明
    /// </summary>
    /// <param name="eEnum"></param>
    /// <returns></returns>
    public static string GetClassDescription(this object eEnum)
    {
        FieldInfo fi = eEnum.GetType().GetField(eEnum.ToString());
        if (fi != null)
        {
            // 获取描述的属性。
            DescriptionAttribute attr = Attribute.GetCustomAttribute(fi,
                typeof(DescriptionAttribute), false) as DescriptionAttribute;
            if (attr != null)
            {
                return attr.Description;
            }
        }
        //。Net4.5
        //DescriptionAttribute attribute = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
        return "";
    }
    /// <summary>
    /// 将集合展开并以ToString形式拼接
    /// </summary>
    /// <param name="间隔字符">拼接时的间隔字符</param>
    /// <returns>拼接后的字符串</returns>
    public static string ExpandAndToString(this IEnumerable s, string 间隔字符)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var f in s)
        {
            if (sb.Length > 0) sb.Append(间隔字符);
            sb.Append(f.ToString());
        }
        return sb.ToString();
    }

    /// <summary>
    /// 左边开始取字串子集
    /// </summary>
    /// <param name="strSrc">字符串</param>
    /// <param name="iCount">位数</param>
    /// <returns></returns>
    public static string Left(this string strSrc, int iCount)
    {
        if (strSrc == null || strSrc.Length <= iCount)
            return strSrc;
        return strSrc.Substring(0, iCount);
    }
    /// <summary>
    /// 从右边开始取字串子集
    /// </summary>
    /// <param name="strSrc">字符串</param>
    /// <param name="iCount">位数</param>
    /// <returns></returns>
    public static string Right(this string strSrc, int iCount)
    {
        if (strSrc == null || strSrc.Length <= iCount)
            return strSrc;
        return strSrc.Substring(strSrc.Length - iCount);
    }
    #region 生成十六进制字符串 格式“00 AA AB。。。”

    public static int byteToInt2(this byte[] b)
    {

        int mask = 0xff;
        int temp = 0;
        int n = 0;
        for (int i = 0; i < b.Length; i++)
        {
            n <<= 8;
            temp = b[i] & mask;
            n |= temp;
        }
        return n;
    }

    /// <summary>
    /// 16进制字节数组转字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="delimiter">每个字节之间的分隔符</param>
    /// <returns></returns>
    public static string ByteToHexStr(this byte[] bytes, string delimiter = "")
    {
        string returnStr = "";
        if (bytes != null)
        {
            returnStr = bytes.Aggregate(returnStr, (current, t) => current + delimiter + t.ToString("X2"));
        }
        return returnStr;
    }

    /// <summary>
    /// 字符串转16进制字节数组
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    public static byte[] StrToToHexByte(this string hexString)
    {

        hexString = hexString.Replace(" ", "");
        if ((hexString.Length % 2) != 0)
            hexString += " ";
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
        return returnBytes;
    }
    public static ushort[] ToShortArray(this byte[] src)
    {

        int count = src.Length >> 1;
        ushort[] dest = new ushort[count];
        for (int i = 0; i < count; i++)
        {
            dest[i] = (ushort)((src[i * 2] & 0xff) | ((src[2 * i + 1] & 0xff) << 8));
        }
        return dest;
    }
    public static byte[] ToByteArray(this ushort[] src)
    {

        int count = src.Length;
        byte[] dest = new byte[count << 1];
        for (int i = 0; i < count; i++)
        {
            dest[i * 2] = (byte)(src[i]);
            dest[i * 2 + 1] = (byte)(src[i] >> 8);
        }

        return dest;
    }
    /// <summary>
    /// 计算校验码
    /// </summary>
    /// <param name="Instruct"></param>
    /// <returns></returns>
    public static string CalcationCRC(this string Instruct)
    {
        try
        {
            var sByte = Instruct.Split(' ');
            var nSum = 0;
            for (var j = 0; j < sByte.Length - 1; j++)
            {
                nSum = nSum + int.Parse(sByte[j], NumberStyles.HexNumber);
            }
            nSum = nSum % 256;
            return nSum.ToString("X").Right(2).PadLeft(2, '0');
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 计算校验码
    /// </summary>
    /// <param name="Instruct"></param>
    /// <returns></returns>
    /// beginPos 从这一位开校验
    public static byte CalCRC(this byte[] Instruct, byte beginPos = 0)
    {
        try
        {
            var nSum = 0x00;
            for (var j = beginPos; j < Instruct.Length; j++)
            {
                nSum = nSum + Instruct[j];
            }
            //return nSum;
            return Convert.ToByte(nSum % 256);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// 计算校验(采用异或校验)
    /// </summary>
    /// <param name="pBuf"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static byte CalcationCheckout(this byte[] Instruct, byte beginPos = 0)
    {
        try
        {
            byte nSum = 0x00;
            for (var j = beginPos; j < Instruct.Length; j++)//从第beginPos位开始计算
            {
                nSum ^= Instruct[j];
            }
            return nSum;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// 计算校验(采用CRC16算法校验)
    /// </summary>
    /// <param name="pBuf"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static ushort CaluationCRC16(this byte[] Instruct, int beginPos, int count)
    {
        try
        {
            ushort m_Crc;
            ushort m_InitCrc = 0xffff;
            ushort j;
            int nSize = count;
            for (int i = beginPos; i < nSize; i++)
            {
                m_InitCrc ^= Instruct[i];
                for (j = 0; j < 8; j++)
                {
                    m_Crc = m_InitCrc;
                    m_InitCrc >>= 1;
                    if ((m_Crc & 0x0001) == 1)
                        m_InitCrc ^= 0xa001;
                }
            }
            return m_InitCrc;
        }
        catch (Exception)
        {
            return 0;
        }
    }
    /// <summary>
    /// 字符串分隔
    /// </summary>
    /// <param name="sourceStr">源字符串</param>
    /// <param name="splitStr">间隔字符</param>
    /// <returns></returns>
    public static string[] SplitStr(this string sourceStr, string splitStr)
    {
        return Regex.Split(sourceStr, splitStr, RegexOptions.IgnoreCase);
    }
    /// <summary>
    /// 截取指定字符串中间的字符串
    /// </summary>
    /// <param name="str">源字符串</param>
    /// <param name="s">头字符串</param>
    /// <param name="e">尾字符串</param>
    /// <returns></returns>
    public static string RegexStringRegion(this string str, string s, string e)
    {
        Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
        return rg.Match(str).Value;
    }
    public static string SpliteHEX(this int nSrc, bool flag)
    {
        return SpliteHEX(nSrc.ToString("X"), flag);
    }

    public static string SpliteHEX(this int nSrc, int nByteNumber, bool flag)
    {
        return SpliteHEX(nSrc.ToString("X"), nByteNumber, flag);
    }

    public static string SpliteHEX(this double nSrc, bool flag)
    {
        return SpliteHEX(nSrc.ToString("X"), flag);
    }

    public static string SpliteHEX(this double nSrc, int nByteNumber, bool flag)
    {
        return SpliteHEX(nSrc.ToString("X"), nByteNumber, flag);
    }

    public static string SpliteHEX(this string nSrc, bool flag)
    {
        var sTemp = nSrc.Trim();
        if (sTemp.Length % 2 == 1)
        {
            sTemp = sTemp.PadLeft(sTemp.Length + 1, '0');
        }
        return SpliteHEX(sTemp, sTemp.Length / 2, flag);
    }

    private static string SpliteHEX(this string HEXParameter, int ByteNumber, bool LwFlag)
    {
        string sResutrn = string.Empty, sTemp;
        sTemp = HEXParameter.PadLeft(ByteNumber * 2, '0');

        for (int i = 0; i < (HEXParameter.Length / 2); i++)
        {
            if (LwFlag)
            {
                sResutrn = sResutrn + " " + sTemp.Left(2);
                sTemp = sTemp.Right(sTemp.Length - 2);
            }
            else
            {
                sResutrn = sResutrn + " " + sTemp.Right(2);
                sTemp = sTemp.Left(sTemp.Length - 2);
            }
        }
        return sResutrn.Trim();
    }
    #endregion
    /// <summary>
    /// 用MD5加密字符串
    /// </summary>
    /// <param name="password">待加密的字符串</param>
    /// <returns></returns>
    public static string MD5Encrypt(this string password)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        byte[] hashedDataBytes;
        hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
        StringBuilder tmp = new StringBuilder();
        foreach (byte i in hashedDataBytes)
        {
            tmp.Append(i.ToString("x2"));
        }
        return tmp.ToString();
    }

    /// <summary>
    ///  AES 加密
    /// </summary>
    /// <param name="str">明文（待加密）</param>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    public static string AesEncrypt(this string str, string key)
    {
        if (string.IsNullOrEmpty(str)) return null;
        Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

        RijndaelManaged rm = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(key),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        ICryptoTransform cTransform = rm.CreateEncryptor();
        Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    ///  AES 解密
    /// </summary>
    /// <param name="str">明文（待解密）</param>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    public static string AesDecrypt(this string str, string key)
    {
        if (string.IsNullOrEmpty(str)) return null;
        Byte[] toEncryptArray = Convert.FromBase64String(str);

        RijndaelManaged rm = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(key),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        ICryptoTransform cTransform = rm.CreateDecryptor();
        Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Encoding.UTF8.GetString(resultArray);
    }

    #region "中文转GBK"
    /// <summary>
    /// 中文转GBK
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static byte[] CHConvertGBK(string ch)
    {
        byte[] buff = Encoding.GetEncoding("GBK").GetBytes(ch);
        return buff;
    }
    #endregion

    /// <summary>
    /// 检测字符串是否为null或空字符串
    /// </summary>
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    /// <summary>
    /// 当字符串为null或空字符串时执行自定义表达式
    /// </summary>
    public static void IsNullOrEmptyThen(this string s, Action<string> 表达式)
    {
        if (string.IsNullOrEmpty(s)) 表达式(s);
    }

    /// <summary>
    /// 当字符串为null或空字符串时执行自定义表达式，并返回处理后的字符串
    /// </summary>
    public static string IsNullOrEmptyThen(this string s, Func<string, string> 表达式)
    {
        if (string.IsNullOrEmpty(s)) return 表达式(s);
        return s;
    }

    /// <summary>
    /// 将字符串格式化并返回
    /// </summary>
    public static string FormatWith(this string s, params object[] 格式化参数)
    {
        return string.Format(s, 格式化参数);
    }

    /// <summary>
    /// 将字符串格式化并返回
    /// </summary>
    public static string FormatWith(this string s, object 格式化参数1)
    {
        return string.Format(s, 格式化参数1);
    }

    /// <summary>
    /// 将字符串格式化并返回
    /// </summary>
    public static string FormatWith(this string s, object 格式化参数1, object 格式化参数2)
    {
        return string.Format(s, 格式化参数1, 格式化参数2);
    }

    /// <summary>
    /// 将字符串格式化并返回
    /// </summary>
    public static string FormatWith(this string s, object 格式化参数1, object 格式化参数2, object 格式化参数3)
    {
        return string.Format(s, 格式化参数1, 格式化参数2, 格式化参数3);
    }

    /// <summary>
    /// 验证是否匹配
    /// </summary>
    public static bool RegexIsMatch(this string s, string 表达式)
    {
        return Regex.IsMatch(s, 表达式);
    }

    /// <summary>
    /// 验证是否匹配
    /// </summary>
    public static bool RegexIsMatch(this string s, string 表达式, RegexOptions 选项)
    {
        return Regex.IsMatch(s, 表达式, 选项);
    }

    /// <summary>
    /// 获取一个匹配项
    /// </summary>
    public static Match RegexMatch(this string s, string 表达式)
    {
        return Regex.Match(s, 表达式);
    }

    /// <summary>
    /// 获取一个匹配项
    /// </summary>
    public static Match RegexMatch(this string s, string 表达式, RegexOptions 选项)
    {
        return Regex.Match(s, 表达式, 选项);
    }

    /// <summary>
    /// 获取所有匹配项
    /// </summary>
    public static MatchCollection RegexMatches(this string s, string 表达式)
    {
        return Regex.Matches(s, 表达式);
    }

    /// <summary>
    /// 获取所有匹配项
    /// </summary>
    public static MatchCollection RegexMatches(this string s, string 表达式, RegexOptions 选项)
    {
        return Regex.Matches(s, 表达式, 选项);
    }

    /// <summary>
    /// 以匹配项拆分字符串
    /// </summary>
    public static string[] RegexSplit(this string s, string 表达式)
    {
        return Regex.Split(s, 表达式);
    }

    /// <summary>
    /// 以匹配项拆分字符串
    /// </summary>
    public static string[] RegexSplit(this string s, string 表达式, RegexOptions 选项)
    {
        return Regex.Split(s, 表达式, 选项);
    }

    /// <summary>
    /// 替换匹配项为新值
    /// </summary>
    public static string RegexReplace(this string s, string 表达式, string 替换值)
    {
        return Regex.Replace(s, 表达式, 替换值);
    }

    /// <summary>
    /// 替换匹配项为新值
    /// </summary>
    public static string RegexReplace(this string s, string 表达式, string 替换值, RegexOptions 选项)
    {
        return Regex.Replace(s, 表达式, 替换值, 选项);
    }

    /// <summary>
    /// 判断一个值是否存在于提供的多个值中
    /// </summary>
    public static bool In<T>(this T t, params T[] list)
    {
        return list.Contains(t);
    }

    /// <summary>
    /// 根据表达式判定是否符合判断依据中的某一项
    /// </summary>
    /// <param name="判断表达式">第一个参数为原值，第二个参数为判断依据</param>
    public static bool In<T, C>(this T t, Func<T, C, bool> 判断表达式, params C[] 判断依据)
    {
        return 判断依据.Any(f => 判断表达式(t, f));
    }

    ///// <summary>
    ///// 判断一个值是否存在于提供的多个值中
    ///// </summary>
    //public static bool In<T>(this T t, T 值1, T 值2)
    //{
    //    return t.In(值1, 值2);
    //}

    ///// <summary>
    ///// 判断一个值是否存在于提供的多个值中
    ///// </summary>
    //public static bool In<T>(this T t, T 值1, T 值2, T 值3)
    //{
    //    return t.In(值1, 值2, 值3);
    //}

    ///// <summary>
    ///// 判断一个值是否存在于提供的多个值中
    ///// </summary>
    //public static bool In<T>(this T t, T 值1, T 值2, T 值3, T 值4)
    //{
    //    return t.In(值1, 值2, 值3, 值4);
    //}

    ///// <summary>
    ///// 判断一个值是否存在于提供的多个值中
    ///// </summary>
    //public static bool In<T>(this T t, T 值1, T 值2, T 值3, T 值4, T 值5)
    //{
    //    return t.In(值1, 值2, 值3, 值4, 值5);
    //}

    /// <summary>
    /// 判断一个值是否介于两值之间（与两值中的任意一个相等也返回true）
    /// </summary>
    public static bool InRange<T>(this IComparable<T> t, T 最小值, T 最大值)
    {
        return t.CompareTo(最小值) >= 0 && t.CompareTo(最大值) <= 0;
    }

    /// <summary>
    /// 判断一个值是否介于两值之间（与两值中的任意一个相等也返回true）
    /// </summary>
    public static bool InRange(this IComparable t, object 最小值, object 最大值)
    {
        return t.CompareTo(最小值) >= 0 && t.CompareTo(最大值) <= 0;
    }

    /// <summary>
    /// 遍历集合，执行传入表达式
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> 操作)
    {
        foreach (T element in source)
            操作(element);
    }

    /// <summary>
    /// 遍历集合，执行传入表达式
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> 操作)
    {
        int i = 0;
        foreach (T element in source)
            操作(element, i++);
    }

    /// <summary>
    /// 将对象的字符串形式输出到调试输出窗口中，并将该对象返回
    /// </summary>
    public static T Trace<T>(this T t)
    {
        System.Diagnostics.Trace.WriteLine(t == null ? "[Null]" : t.ToString());
        return t;
    }

    /// <summary>
    /// 将对象的字符串形式输出到调试输出窗口中，并将该对象返回
    /// </summary>
    public static T Trace<T>(this T t, Func<T, object> 表达式)
    {
        var o = 表达式(t);
        System.Diagnostics.Trace.WriteLine(o == null ? "[Null]" : o.ToString());
        return t;
    }

    /// <summary>
    /// 将对象的字符串形式输出到调试输出窗口中，并将该对象返回
    /// </summary>
    public static T Trace<T>(this T t, string 分类)
    {
        System.Diagnostics.Trace.WriteLine(t == null ? "[Null]" : t.ToString(), 分类);
        return t;
    }

    /// <summary>
    /// 将对象的字符串形式输出到调试输出窗口中，并将该对象返回
    /// </summary>
    public static T Trace<T>(this T t, Func<T, object> 表达式, string 分类)
    {
        var o = 表达式(t);
        System.Diagnostics.Trace.WriteLine(o == null ? "[Null]" : o.ToString(), 分类);
        return t;
    }

    /// <summary>
    /// 将对象的字符串形式输出到调试输出窗口中，并将该对象返回
    /// </summary>
    ///<param name="格式化字符串">用于格式化的字符串，以{0}表示此对象的显示位置</param>
    public static T TraceFormat<T>(this T t, string 格式化字符串)
    {
        System.Diagnostics.Trace.WriteLine(string.Format(格式化字符串, t == null ? "[Null]" : t.ToString()));
        return t;
    }

    /// <summary>
    /// 将对象的字符串形式输出到调试输出窗口中，并将该对象返回
    /// </summary>
    ///<param name="格式化字符串">用于格式化的字符串，以{0}表示此对象的显示位置</param>
    public static T TraceFormat<T>(this T t, string 格式化字符串, string 分类)
    {
        System.Diagnostics.Trace.WriteLine(string.Format(格式化字符串, t == null ? "[Null]" : t.ToString()), 分类);
        return t;
    }

    /// <summary>
    /// 循环所有子项，递归选取并返回所有后代项，针对IEnumerable
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="递归项选取表达式">通过此表达式选取要返回的子项</param>
    /// <returns>选取的子项</returns>
    public static IEnumerable<T> RecursionEachSelect<T>(this IEnumerable o, Func<T, IEnumerable<T>> 递归项选取表达式)
    {
        return RecursionEachSelect(o, 递归项选取表达式, null);
    }

    /// <summary>
    /// 循环所有子项，递归选取并返回所有后代项，针对IEnumerable
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="递归项选取表达式">通过此表达式选取要返回的子项</param>
    /// <param name="检验表达式">対返回项进行检验，返回代入此表达式后表达式成立的项</param>
    /// <returns>选取的子项</returns>
    public static IEnumerable<T> RecursionEachSelect<T>(this IEnumerable o, Func<T, IEnumerable<T>> 递归项选取表达式, Predicate<T> 检验表达式)
    {
        return RecursionEachSelect(o.Cast<T>(), 递归项选取表达式, 检验表达式);
    }

    /// <summary>
    /// 循环所有子项，递归选取并返回所有后代项，针对IEnumerable泛型形式
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="递归项选取表达式">通过此表达式选取要返回的子项</param>
    /// <returns>选取的子项</returns>
    public static IEnumerable<T> RecursionEachSelect<T>(this IEnumerable<T> o, Func<T, IEnumerable<T>> 递归项选取表达式)
    {
        return RecursionEachSelect(o, 递归项选取表达式, null);
    }

    /// <summary>
    /// 循环所有子项，递归选取并返回所有后代项，针对IEnumerable泛型形式
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="递归项选取表达式">通过此表达式选取要返回的子项</param>
    /// <param name="检验表达式">対返回项进行检验，返回代入此表达式后表达式成立的项</param>
    /// <returns>选取的子项</returns>
    public static IEnumerable<T> RecursionEachSelect<T>(this IEnumerable<T> o, Func<T, IEnumerable<T>> 递归项选取表达式, Predicate<T> 检验表达式)
    {
        foreach (var f in o)
        {
            if (检验表达式 == null || 检验表达式(f)) yield return f;
            foreach (var d in RecursionSelect(f, 递归项选取表达式, 检验表达式))
            {
                yield return d;
            }
        }
    }

    /// <summary>
    /// 递归选取并返回后代项
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="递归项选取表达式">通过此表达式选取要返回的子项</param>
    /// <returns>选取的子项</returns>
    public static IEnumerable<T> RecursionSelect<T>(this T o, Func<T, IEnumerable<T>> 递归项选取表达式)
    {
        return RecursionSelect(o, 递归项选取表达式, null);
    }

    /// <summary>
    /// 递归选取并返回后代项
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="递归项选取表达式">通过此表达式选取要返回的子项</param>
    /// <param name="检验表达式">対返回项进行检验，返回代入此表达式后表达式成立的项</param>
    /// <returns>选取的子项</returns>
    public static IEnumerable<T> RecursionSelect<T>(this T o, Func<T, IEnumerable<T>> 递归项选取表达式, Predicate<T> 检验表达式)
    {
        foreach (var f in 递归项选取表达式(o))
        {
            if (检验表达式 == null || 检验表达式(f)) yield return f;
            foreach (var d in RecursionSelect(f, 递归项选取表达式, 检验表达式))
            {
                yield return d;
            }
        }
    }


    #endregion
    /// <summary>
    /// 日期转换成unix时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTimeToUnixTimestamp(this DateTime dateTime)
    {
        var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
        return Convert.ToInt64((dateTime - start).TotalSeconds);
    }

    /// <summary>
    /// 日期转换成时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTimeToTimestamp(this DateTime dateTime)
    {

        return Convert.ToInt64((dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000);
    }
    /// <summary>
    /// unix时间戳转换成日期
    /// </summary>
    /// <param name="unixTimeStamp">时间戳（秒）</param>
    /// <returns></returns>
    public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
    {
        var start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
        return start.AddSeconds(timestamp);
    }
    #region 特殊字符串
    /// <summary>
    /// 正则匹配IP
    /// </summary>
    /// <param name="SourceIP"></param>
    /// <returns></returns>
    public static string RegexIP(this string SourceIP)
    {
        string regexIp = "((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)";
        Regex ips = new Regex(regexIp);
        if (ips.IsMatch(SourceIP))
        {
            return ips.Match(SourceIP).Value;
        }
        return "";
    }
    /// <summary>
    /// yyyy-MM-dd HH:mm:ss
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToLongString(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }
    //精确到毫秒
    /// <summary>
    /// yyyy-MM-dd HH:mm:ss.fff
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToLongerString(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
    /// <summary>
    /// yyyyMMddHHmmssfff
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToLongerString4JD(this DateTime dt)
    {
        return dt.ToString("yyyyMMddHHmmssfff");
    }
    /// <summary>
    /// yyyy-MM-dd HH:00:00
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime ToHourDate(this DateTime dt)
    {
        return DateTime.Parse(dt.ToString("yyyy-MM-dd HH:00:00"));
    }

    public interface ISpecialString
    {
        string Value { get; set; }
    }

    /// <summary>
    /// 特殊字符串基类
    /// </summary>
    public abstract class SpecialString : ISpecialString
    {
        public SpecialString()
        {

        }

        public SpecialString(string value)
        {
            _Value = value;
        }

        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        private string _Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// 转换为特殊字符串类型
    /// </summary>
    public static T As<T>(this string s) where T : SpecialString, new()
    {
        var o = new T();
        o.Value = s;
        return o;
    }

    public class UriString : SpecialString
    {
        /// <summary>
        /// 将绝对Uri与相对路径组合。
        /// 如果传入的是绝对路径，则原样返回。
        /// 通常用于处理网页内的相对路径超链接，如将“http://abc.com”或“http://abc.com/index.htm”与“info.htm”组合的话，就会生成“http://abc.com/info.htm”
        /// </summary>
        /// <param name="RelativePath">待组合的相对路径，可以是“../abc.htm”形式</param>
        public string CombineRelativePath(string RelativePath)
        {
            //try
            //{
            //    return new Uri(RelativePath).AbsoluteUri;
            //}
            //catch
            //{
            //var u = new Uri(Value);
            //return new Uri(u.LocalPath.EndsWith("/") ? u : new Uri(Path.GetDirectoryName(Value).Replace(@"\", "/").Replace(":/", "://")), RelativePath).AbsoluteUri;
            return new Uri(new Uri(Value), RelativePath).AbsoluteUri;
            //}
        }

        /// <summary>
        /// 转换本地文件Uri为本地路径格式。
        /// 如“file:///C:/abc/avatar.xml”将被转换为“C:\abc\avatar.xml”。
        /// 如果该Uri不是本地文件Uri，那么将抛出异常。
        /// </summary>
        public string ToLocalFilePath()
        {
            if (!Value.ToLower().StartsWith("file:///")) throw new Exception("这不是一个本地文件Uri");
            return Value.Substring(8).Replace("/", "\\");
        }

    }

    public static UriString AsUriString(this string s)
    {
        return s.As<UriString>();
    }

    public class PathString : SpecialString
    {
        /// <summary>
        /// 转换本地路径为本地文件Uri格式。
        /// 如“C:\abc\avatar.xml”将被转换为“file:///C:/abc/avatar.xml”。
        /// </summary>
        public string ToLocalFileUri()
        {
            return "file:///" + Value.Replace("\\", "/");
        }

        /// <summary>
        /// 将绝对路径与相对路径组合。
        /// 如果传入的是绝对路径，则原样返回。
        /// 通常用于处文件相对路径计算，如将“C:\abc\”或“C:\abc\a.txt”与“info.htm”组合的话，就会生成“C:\abc\info.htm”
        /// </summary>
        /// <param name="RelativePath">待组合的相对路径，可以是“..\abc.htm”形式</param>
        public string CombineRelativePath(string RelativePath)
        {
            //return ToLocalFileUri().AsUriString().CombineRelativePath(RelativePath.Replace("\\", "/")).AsUriString().ToLocalFilePath();
            return Value.AsPathString().Combine(RelativePath).AsPathString().FullPath;
        }

        /// <summary>
        /// 获取完整路径，等同于Path.GetFullPath()
        /// </summary>
        public string FullPath
        {
            get
            {
                return Path.GetFullPath(Value);
            }
        }
        //private string _FullPath;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.GetFileName(Value);
            }
        }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension
        {
            get
            {
                return Path.GetExtension(Value);
            }
        }

        /// <summary>
        /// 所在目录名，等同于Path.GetDirectoryName()
        /// </summary>
        public string DirectoryName
        {
            get
            {
                return Path.GetDirectoryName(Value);
            }
        }

        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        public bool FileExists
        {
            get
            {
                return File.Exists(Value);
            }
        }

        /// <summary>
        /// 返回目录是否存在
        /// </summary>
        public bool DirectoryExists
        {
            get
            {
                return Directory.Exists(Value);
            }
        }

        /// <summary>
        /// 返回是否为绝对路径
        /// </summary>
        public bool IsPathRooted
        {
            get
            {
                return Path.IsPathRooted(Value);
            }
        }

        /// <summary>
        /// 拼接路径
        /// </summary>
        public string Combine(string 待拼接路径)
        {
            return Path.Combine(Value, 待拼接路径);
        }

        /// <summary>
        /// 不带扩展名的文件名
        /// </summary>
        public string FileNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Value);
            }
        }
    }

    public static PathString AsPathString(this string s)
    {
        return s.As<PathString>();
    }

    public interface IHtmlString : ISpecialString
    {

    }

    public class HtmlString : SpecialString, IHtmlString
    {

    }

    public static HtmlString AsHtmlString(this string s)
    {
        return s.As<HtmlString>();
    }

    public interface IXmlString : ISpecialString
    {

    }

    public class XmlString : SpecialString, IXmlString
    {

    }

    public static XmlString AsXmlString(this string s)
    {
        return s.As<XmlString>();
    }

    public class XHtmlString : SpecialString, IXmlString, IHtmlString
    {

    }

    public static XHtmlString AsXHtmlString(this string s)
    {
        return s.As<XHtmlString>();
    }
    #endregion

    #region 其它
    /// <summary>
    /// 执行Switch操作
    /// </summary>
    public static Switch<T> Switch<T>(this T v)
    {
        return new Switch<T>(v);
    }

    /// <summary>
    /// 执行Switch操作，并传入一个方法用于处理返回结果
    /// </summary>
    /// <param name="Do">处理返回结果的方法，该方法将在每次执行CaseReturn并匹配成功时或执行DefaultReturn时调用，方法的第一个参数是新传入的返回值，第二个参数是当前的返回值</param>
    public static Case<T, R> Switch<T, R>(this T v, Func<R, R, R> Do)
    {
        return new Case<T, R>(v, Do);
    }
    #endregion
    /// <summary>
    /// 根据字符串取Type
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static Type Gettypen(this string typeName)
    {
        Type type = null;
        Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
        int assemblyArrayLength = assemblyArray.Length;
        for (int i = 0; i < assemblyArrayLength; ++i)
        {
            type = assemblyArray[i].GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        for (int i = 0; (i < assemblyArrayLength); ++i)
        {
            Type[] typeArray = assemblyArray[i].GetTypes();
            int typeArrayLength = typeArray.Length;
            for (int j = 0; j < typeArrayLength; ++j)
            {
                if (typeArray[j].Name.Equals(typeName))
                {
                    return typeArray[j];
                }
            }
        }
        return type;
    }
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}

public abstract class SwitchCaseBase<T>
{
    protected bool DefaultSet;

    protected void CheckDefaultSet()
    {
        if (DefaultSet) throw new Exception("Default操作必须在方法链末端执行，在其后不得执行Case操作。");
    }

    protected virtual T Value
    {
        get
        {
            return _Value;
        }
    }
    protected T _Value;

    protected virtual bool IsBroke
    {
        get
        {
            return _IsBroke;
        }
    }
    protected bool _IsBroke;

    internal void Break()
    {
        _IsBroke = true;
    }
}

public class Switch<T> : SwitchCaseBase<T>
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="Value">原始值</param>
    public Switch(T Value)
    {
        _Value = Value;
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    public Case<T, R> CaseReturn<R>(T Value, Func<T, R> Run)
    {
        return CaseReturn(f => f.Equals(Value), Run);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="ReturnValue">返回结果</param>
    public Case<T, R> CaseReturn<R>(T Value, R ReturnValue)
    {
        return CaseReturn(f => f.Equals(Value), f => ReturnValue);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="ReturnValue">返回结果</param>
    public Case<T, R> CaseReturn<R>(Predicate<T> Check, R ReturnValue)
    {
        return CaseReturn(Check, f => ReturnValue, true);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    public Case<T, R> CaseReturn<R>(Predicate<T> Check, Func<T, R> Run)
    {
        return CaseReturn(Check, Run, true);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseReturn<R>(T Value, Func<T, R> Run, bool Break)
    {
        return CaseReturn(f => f.Equals(Value), Run, Break);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="ReturnValue">返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseReturn<R>(T Value, R ReturnValue, bool Break)
    {
        return CaseReturn(f => f.Equals(Value), f => ReturnValue, Break);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="ReturnValue">返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseReturn<R>(Predicate<T> Check, R ReturnValue, bool Break)
    {
        return CaseReturn(Check, f => ReturnValue, Break);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param> 
    public Case<T, R> CaseReturn<R>(Predicate<T> Check, Func<T, R> Run, bool Break)
    {
        CheckDefaultSet();
        var r = new Case<T, R>(this.Value, this.IsBroke, this.DefaultSet);
        if (IsBroke)
        {
            return r;
        }
        if (Check(Value))
        {
            r.SetReturnValue(Run(Value));
            if (Break) r.Break();
        }
        return r;
    }

    /// <summary>
    /// 默认生成的返回值，该方法必须在Switch操作方法链末端执行，其后不得再执行Case操作，但可以执行其它的Default操作
    /// </summary>
    /// <param name="ReturnValue">返回结果</param>
    public Case<T, R> DefaultReturn<R>(R ReturnValue)
    {
        return DefaultReturn(f => ReturnValue);
    }

    /// <summary>
    /// 默认生成的返回值，该方法必须在Switch操作方法链末端执行，其后不得再执行Case操作，但可以执行其它的Default操作
    /// </summary>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    public Case<T, R> DefaultReturn<R>(Func<T, R> Run)
    {
        DefaultSet = true;
        var r = new Case<T, R>(this.Value, this.IsBroke, this.DefaultSet);
        if (IsBroke)
        {
            return r;
        }
        r.SetReturnValue(Run(this.Value));
        return r;
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    public Switch<T> CaseRun(T Value, Action<T> Run)
    {
        return CaseRun(f => f.Equals(Value), Run);
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Switch<T> CaseRun(T Value, Action<T> Run, bool Break)
    {
        return CaseRun(f => f.Equals(Value), Run, Break);
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    public Switch<T> CaseRun(Predicate<T> Check, Action<T> Run)
    {
        return CaseRun(Check, Run, true);
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Switch<T> CaseRun(Predicate<T> Check, Action<T> Run, bool Break)
    {
        CheckDefaultSet();
        if (IsBroke) return this;
        if (Check(this.Value))
        {
            Run(this.Value);
            if (Break) _IsBroke = true;
        }
        return this;
    }

    /// <summary>
    /// 默认执行方法，该方法必须在Switch操作方法链末端执行，其后不得再执行Case操作，但可以执行其它的Default操作
    /// </summary>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    public void DefaultRun(Action<T> Run)
    {
        DefaultSet = true;
        if (IsBroke) return;
        Run(this.Value);
    }
}

public class Case<T, R> : SwitchCaseBase<T>
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="Value">原始值</param>
    public Case(T Value)
    {
        _Value = Value;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="Value">原始值</param>
    /// <param name="Do">处理返回结果的方法，该方法将在每次执行CaseReturn并匹配成功时或执行DefaultReturn时调用，方法的第一个参数是新传入的返回值，第二个参数是当前的返回值</param>
    public Case(T Value, Func<R, R, R> Do)
    {
        _Value = Value;
        this.Do = Do;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="Value">原始值</param>
    /// <param name="IsBroke">是否已结束</param>
    /// <param name="DefaultSet">是否已执行过默认操作</param>
    public Case(T Value, bool IsBroke, bool DefaultSet)
    {
        _Value = Value;
        _IsBroke = IsBroke;
        this.DefaultSet = DefaultSet;
    }

    protected Func<R, R, R> Do;

    /// <summary>
    /// 最终返回结果
    /// </summary>
    public R ReturnValue
    {
        get
        {
            return _ReturnValue;
        }
    }
    private R _ReturnValue;

    internal void SetReturnValue(R Value)
    {
        if (Do == null)
            _ReturnValue = Value;
        else
            _ReturnValue = Do(Value, ReturnValue);
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    public Case<T, R> CaseRun(T Value, Action<T> Run)
    {
        return CaseRun(f => f.Equals(Value), Run);
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseRun(T Value, Action<T> Run, bool Break)
    {
        return CaseRun(f => f.Equals(Value), Run, Break);
    }


    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    public Case<T, R> CaseRun(Predicate<T> Check, Action<T> Run)
    {
        return CaseRun(Check, Run, true);
    }

    /// <summary>
    /// 匹配并执行传入方法
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseRun(Predicate<T> Check, Action<T> Run, bool Break)
    {
        CheckDefaultSet();
        if (IsBroke) return this;
        if (Check(this.Value))
        {
            Run(this.Value);
            if (Break) _IsBroke = true;
        }
        return this;
    }

    /// <summary>
    /// 默认执行方法，该方法必须在Switch操作方法链末端执行，其后不得再执行Case操作，但可以执行其它的Default操作
    /// </summary>
    /// <param name="Run">执行的方法，其参数为原始值</param>
    public Case<T, R> DefaultRun(Action<T> Run)
    {
        DefaultSet = true;
        if (IsBroke) return this;
        Run(this.Value);
        return this;
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    public Case<T, R> CaseReturn(T Value, Func<T, R> Run)
    {
        return CaseReturn(f => f.Equals(Value), Run);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="ReturnValue">返回结果</param>
    public Case<T, R> CaseReturn(T Value, R ReturnValue)
    {
        return CaseReturn(f => f.Equals(Value), f => ReturnValue);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    public Case<T, R> CaseReturn(Predicate<T> Check, Func<T, R> Run)
    {
        return CaseReturn(Check, Run, true);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="ReturnValue">返回结果</param>
    public Case<T, R> CaseReturn(Predicate<T> Check, R ReturnValue)
    {
        return CaseReturn(Check, f => ReturnValue, true);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseReturn(T Value, Func<T, R> Run, bool Break)
    {
        return CaseReturn(f => f.Equals(Value), Run, Break);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Value">匹配值</param>
    /// <param name="ReturnValue">返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseReturn(T Value, R ReturnValue, bool Break)
    {
        return CaseReturn(f => f.Equals(Value), f => ReturnValue, Break);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="ReturnValue">返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param>
    public Case<T, R> CaseReturn(Predicate<T> Check, R ReturnValue, bool Break)
    {
        return CaseReturn(Check, f => ReturnValue, Break);
    }

    /// <summary>
    /// 匹配并生成返回值。如果通过Switch指定了返回结果处理方法，那么在匹配成功时会将ReturnValue的值设为经过处理后的结果，否则将直接覆盖原有的ReturnValue值
    /// </summary>
    /// <param name="Check">匹配验证方法</param>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    /// <param name="Break">是否在匹配成功后结束</param> 
    public Case<T, R> CaseReturn(Predicate<T> Check, Func<T, R> Run, bool Break)
    {
        CheckDefaultSet();
        if (IsBroke)
        {
            return this;
        }
        if (Check(Value))
        {
            SetReturnValue(Run(Value));
            if (Break) this.Break();
        }
        return this;
    }

    /// <summary>
    /// 默认生成的返回值，该方法必须在Switch操作方法链末端执行，其后不得再执行Case操作，但可以执行其它的Default操作
    /// </summary>
    /// <param name="ReturnValue">返回结果</param>
    public Case<T, R> DefaultReturn(R ReturnValue)
    {
        return DefaultReturn(f => ReturnValue);
    }

    /// <summary>
    /// 默认生成的返回值，该方法必须在Switch操作方法链末端执行，其后不得再执行Case操作，但可以执行其它的Default操作
    /// </summary>
    /// <param name="Run">执行的方法，其参数为原始值，其返回值即为返回结果</param>
    public Case<T, R> DefaultReturn(Func<T, R> Run)
    {
        DefaultSet = true;
        if (IsBroke)
        {
            return this;
        }
        SetReturnValue(Run(this.Value));
        return this;
    }
}

public static class IHtmlStringExtend
{
    /// <summary>
    /// 获取标题
    /// </summary>
    public static string GetTitle(this Extend.IHtmlString s)
    {
        return s.Value.RegexMatch(@"<title.*?>(.+?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[1].Value.Trim();
    }

    /// <summary>
    /// 获取所有超链接（a）匹配项，其中组1为链接地址值，组2为链接显示内容
    /// </summary>
    public static IEnumerable<Match> GetLinks(this Extend.IHtmlString s)
    {
        return s.Value.RegexMatches(@"<a.+?href=[""'](.+?)[""'].*?>(.+?)</\s*?a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Cast<Match>();
    }

    /// <summary>
    /// 获取所有图像（img）匹配项，其中组1为图像地址值
    /// </summary>
    public static IEnumerable<Match> GetImages(this Extend.IHtmlString s)
    {
        return s.Value.RegexMatches(@"<img.+?src=[""'](.+?)[""'].*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Cast<Match>();
    }
    //********************************************************************************
    /// <summary>      
    /// description： 深克隆对象，拷贝对象包含标记"[DataMember]"的属性值,复制包含所有深度层级  
    /// add by：YHJ
    /// date：2015-3-9
    /// </summary>   
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="obj">泛型变量</param>
    /// <returns>拷贝对象</returns>
    //public static T DeepClone<T>(this T obj)
    //{
    //    T cloned = default(T);
    //    var serializer = new DataContractSerializer(typeof(T));

    //    using (Stream ms = new MemoryStream())
    //    {
    //        serializer.WriteObject(ms, obj);
    //        ms.Position = 0;
    //        cloned = (T)serializer.ReadObject(ms);
    //        return cloned;
    //    }
    //}
    //******************************************************
    public static object DepthClone(this object obj)
    {
        object clone = new object();
        using (Stream stream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                clone = formatter.Deserialize(stream);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
        }
        return clone;
    }

    public static string GetHost(this string URI)
    {
        try
        {
            Uri uri = new Uri(URI);
            string host = uri.Host;
            return host;
        }
        catch (Exception ex)
        {
            Log.Error("parse URI:", ex);
            return "";
        }

    }

}
