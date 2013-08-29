using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

public static class Utils
{
    public static int FromString(string x)
    {
        if (x.StartsWith("0x")) return int.Parse(x.Substring(2), System.Globalization.NumberStyles.HexNumber);
        else return int.Parse(x);
    }
    public static string To4Hex(short x)
    {
        return "0x" + x.ToString("x4");
    }

    public static string GetCommaSepString<T>(T[] arr)
    {
        StringBuilder ret = new StringBuilder();
        for (var i = 0; i < arr.Length; i++)
        {
            if (i != 0) ret.Append(", ");
            ret.Append(arr[i].ToString());
        }
        return ret.ToString();
    }

    public static List<int> StringListToIntList(List<string> strList)
    {
        List<int> ret = new List<int>();
        foreach (var i in strList)
        {
            try { ret.Add(Convert.ToInt32(i)); }
            catch { }
        }
        return ret;
    }

    public static int[] FromCommaSepString32(string x)
    {
        return x.Split(',').Select(_ => FromString(_.Trim())).ToArray();
    }

    public static short[] FromCommaSepString16(string x)
    {
        return x.Split(',').Select(_ => (short)FromString(_.Trim())).ToArray();
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}