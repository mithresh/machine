using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Core.Services.Impl
{
  public class Namer : INamer
  {
    readonly Random _random = new Random();

    public string ToCamelCase(string source)
    {
      StringBuilder sb = new StringBuilder();
      bool upper = true;
      foreach (char c in source)
      {
        if (c == '_')
        {
          upper = true;
          continue;
        }
        sb.Append(upper ? Char.ToUpper(c) : c);
        upper = false;
      }
      return sb.ToString();
    }

    public string ToLowerCamelCase(string source)
    {
      string camelCase = ToCamelCase(source);
      return camelCase.Substring(0, 1).ToLower() + camelCase.Substring(1);
    }

    public string ToUnderscoreDelimited(string source)
    {
      return ToDelimited(source, '_');
    }

    public string ToDashDelimited(string source)
    {
      return ToDelimited(source, '-');
    }

    public string ToDelimited(string source, char delimiter)
    {
      StringBuilder sb = new StringBuilder();
      bool previousWasDelimiter = true;
      foreach (char c in source)
      {
        if (Char.IsUpper(c) && !previousWasDelimiter)
        {
          sb.Append(delimiter);
        }
        sb.Append(c);
        previousWasDelimiter = (c == delimiter);
      }
      return sb.ToString().ToLower();
    }

    public string MakeRandomName()
    {
      string lowercase = "abcdefghijklmnopqrstuvwxyz";
      string uppercase = lowercase.ToUpper();
      string everthing = lowercase + uppercase;
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < 8; i++)
      {
        sb.Append(everthing[_random.Next(everthing.Length)]);
      }
      return sb.ToString();
    }
  }
}