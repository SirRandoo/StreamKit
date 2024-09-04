// See https://aka.ms/new-console-template for more information

using System;
using System.Runtime.InteropServices;
using System.Text;

// Console.WriteLine(Marshal.SizeOf(typeof(TooltipData)));
// Console.WriteLine(Marshal.SizeOf(typeof(TooltipMetadata)));


unsafe
{
    char[] text = "Hello world how are you".ToCharArray();
    Console.WriteLine(Encoding.UTF8.GetByteCount(text));
    Console.WriteLine(text.Length * sizeof(char));

    Console.WriteLine(sizeof(TooltipData));
    Console.WriteLine(sizeof(TooltipMetadata));
    Console.WriteLine(sizeof(TaggedString));
    Console.WriteLine(Marshal.SizeOf(new TaggedString(new string(text))));
}

public enum TooltipPriority
{
    Default = 0,
    Pawn = 1,
    Ideo = 2
}

public struct TooltipMetadata
{
    public string Text;
    public Func<string> TextGetter;
    public int UniqueId;
    public TooltipPriority Priority;
    public float Delay;
}

public struct TooltipData
{
    public string Text;
    public float Delay;
}

public struct TaggedString(string dat)
{
    public static readonly TaggedString Empty = new("");

    public string RawText => dat;

    public char this[int i] => RawText[i];

    public int Length => RawText.Length;

    public int StrippedLength => RawText.Length;

    public TaggedString CapitalizeFirst()
    {
        if (string.IsNullOrEmpty(dat))
        {
            return this;
        }
        int num = FirstLetterBetweenTags();
        if (char.ToUpper(dat[num]) == dat[num])
        {
            return this;
        }
        if (dat.Length == 1)
        {
            return new TaggedString(dat.ToUpper());
        }
        if (num == 0)
        {
            return new TaggedString(char.ToUpper(dat[num]) + dat.Substring(num + 1));
        }
        return new TaggedString(dat.Substring(0, num) + char.ToUpper(dat[num]) + dat.Substring(num + 1));
    }

    public TaggedString EndWithPeriod()
    {
        if (string.IsNullOrEmpty(dat))
        {
            return this;
        }
        if (dat[dat.Length - 1] == '.')
        {
            return this;
        }
        return dat + ".";
    }

    private int FirstLetterBetweenTags()
    {
        bool flag = false;
        for (int i = 0; i < dat.Length - 1; i++)
        {
            if (dat[i] == '(' && dat[i + 1] == '*')
            {
                flag = true;
                continue;
            }
            if (flag && dat[i] == ')' && dat[i + 1] != '(')
            {
                return i + 1;
            }
            if (!flag)
            {
                return i;
            }
        }
        return 0;
    }

    public bool NullOrEmpty()
    {
        return string.IsNullOrEmpty(RawText);
    }

    public TaggedString Trim()
    {
        return new TaggedString(RawText.Trim());
    }

    public TaggedString ToLower()
    {
        return new TaggedString(RawText.ToLower());
    }

    public TaggedString Replace(string oldValue, string newValue)
    {
        return new TaggedString(RawText.Replace(oldValue, newValue));
    }

    public static implicit operator TaggedString(string str)
    {
        return new TaggedString(str);
    }

    public static TaggedString operator +(TaggedString t1, TaggedString t2)
    {
        return new TaggedString(t1.RawText + t2.RawText);
    }

    public static TaggedString operator +(string t1, TaggedString t2)
    {
        return new TaggedString(t1 + t2.RawText);
    }

    public static TaggedString operator +(TaggedString t1, string t2)
    {
        return new TaggedString(t1.RawText + t2);
    }

    public override string ToString()
    {
        return RawText;
    }
}
