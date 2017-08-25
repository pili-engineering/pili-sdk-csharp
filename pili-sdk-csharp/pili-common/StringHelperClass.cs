//-------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2015 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some aspects of the Java String class.
//-------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Text.RegularExpressions;

internal static class StringHelperClass
{
    internal static string SubstringSpecial(this string self, int start, int end)
    {
        return self.Substring(start, end - start);
    }


    internal static bool StartsWith(this string self, string prefix, int toffset)
    {
        return self.IndexOf(prefix, toffset, StringComparison.Ordinal) == toffset;
    }


    internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
    {
        var splitArray = Regex.Split(self, regexDelimiter);

        if (trimTrailingEmptyStrings)
        {
            if (splitArray.Length > 1)
            {
                for (var i = splitArray.Length; i > 0; i--)
                {
                    if (splitArray[i - 1].Length > 0)
                    {
                        if (i < splitArray.Length)
                        {
                            Array.Resize(ref splitArray, i);
                        }

                        break;
                    }
                }
            }
        }

        return splitArray;
    }


    internal static string NewString(byte[] bytes)
    {
        return NewString(bytes, 0, bytes.Length);
    }

    internal static string NewString(byte[] bytes, int index, int count)
    {
        return Encoding.UTF8.GetString(bytes, index, count);
    }

    internal static string NewString(byte[] bytes, string encoding)
    {
        return NewString(bytes, 0, bytes.Length, encoding);
    }

    internal static string NewString(byte[] bytes, int index, int count, string encoding)
    {
        return Encoding.GetEncoding(encoding).GetString(bytes, index, count);
    }


    internal static byte[] GetBytes(this string self)
    {
        return GetbytesForEncoding(Encoding.UTF8, self);
    }

    internal static byte[] GetBytes(this string self, string encoding)
    {
        return GetbytesForEncoding(Encoding.GetEncoding(encoding), self);
    }

    private static byte[] GetbytesForEncoding(Encoding encoding, string s)
    {
        var bytes = new byte[encoding.GetByteCount(s)];
        encoding.GetBytes(s, 0, s.Length, bytes, 0);
        return bytes;
    }
}
