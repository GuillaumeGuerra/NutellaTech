//*****************************************************************************
//* Class: Wpf.Utility
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2008 - 2009
//*****************************************************************************
//* Project                 : GalaSoft.Wpf.Utilities
//* Author                  : Laurent Bugnion, GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 03.04.2009
//*****************************************************************************
//* Last base level: BL0001
//*****************************************************************************

using System;
using System.Windows.Media;
using GalaSoft.Utilities.Attributes;

namespace GalaSoft.Utilities.Wpf
{
  /// <summary>
  /// A class of Utilities for WPF applications
  /// </summary>
  [ClassInfo(typeof(Utility),
  VersionString = "V1.0.0",
  DateString = "200904040954",
  Description = "A set of utility methods specially for WPF",
  UrlContacts = "http://www.galasoft.ch/contact_en.html",
  Email = "laurent@galasoft.ch")]
  public static class Utility
  {
    /// <summary>
    /// Takes a color name (see the Colors enumeration) or a color code
    /// (#RRGGBB or #AARRGGBB) and converts it in a System.Windows.Media.Color
    /// object.
    /// </summary>
    public static object MakeColor(string colorNameOrCode)
    {
      if (colorNameOrCode == null)
      {
        throw new ArgumentNullException("colorNameOrCode");
      }

      if (colorNameOrCode.StartsWith("#"))
      {
        byte[] bytes = HexToData(colorNameOrCode.Substring(1));

        if (bytes == null
          || bytes.Length < 3
          || bytes.Length > 4)
        {
          throw new ArgumentException("Parameter must be formatted as #AARRGGBB or #RRGGBB", "colorNameOrCode");
        }

        if (bytes.Length == 3)
        {
          return MakeColor(255, bytes[0], bytes[1], bytes[2]);
        }
        else
        {
          return MakeColor(bytes[0], bytes[1], bytes[2], bytes[3]);
        }
      }
      else
      {
        System.Drawing.Color drawingColor = System.Drawing.Color.FromName(colorNameOrCode);
        return MakeColor(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
      }
    }

    /// <summary>
    /// Takes 4 bytes (alpha channel, red, green and blue) and converts them into
    /// the corresponding System.Windows.Media.Color object.
    /// </summary>
    public static object MakeColor(byte a, byte r, byte g, byte b)
    {
      return System.Windows.Media.Color.FromArgb(a, r, g, b);
    }

    public static object MakeSolidColorBrush(string colorNameOrCode)
    {
      return MakeSolidColorBrush(colorNameOrCode, false);
    }

    /// <summary>
    /// Takes a color name (see the Colors enumeration) or a color code
    /// (#RRGGBB or #AARRGGBB) and converts it in a System.Windows.Media.SolidColorBrush
    /// object.
    /// </summary>
    /// <param name="freeze">If true, the Brush object will be frozen before being returned.</param>
    public static object MakeSolidColorBrush(string colorNameOrCode, bool freeze)
    {
      if (colorNameOrCode == null)
      {
        throw new ArgumentNullException("colorNameOrCode");
      }

      object color = MakeColor(colorNameOrCode);
      SolidColorBrush brush = new SolidColorBrush((System.Windows.Media.Color) color);
      
      if (freeze)
      {
        brush.Freeze();
      }
      
      return brush;
    }

    /// <summary>
    /// Takes 4 bytes (alpha channel, red, green and blue) and converts them into
    /// the corresponding System.Windows.Media.Color object.
    /// </summary>
    public static object MakeSolidColorBrush(byte a, byte r, byte g, byte b)
    {
      return MakeSolidColorBrush(a, r, g, b, false);
    }

    /// <summary>
    /// Takes 4 bytes (alpha channel, red, green and blue) and converts them into
    /// the corresponding System.Windows.Media.Color object.
    /// </summary>
    /// <param name="freeze">If true, the Brush object will be frozen before being returned.</param>
    public static object MakeSolidColorBrush(byte a, byte r, byte g, byte b, bool freeze)
    {
      SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(a, r, g, b));

      if (freeze)
      {
        brush.Freeze();
      }

      return brush;
    }

    /// <summary>
    /// Creates a clone of a Point.
    /// </summary>
    public static object MakePoint(System.Drawing.Point originalPoint)
    {
      return new System.Windows.Point((double) originalPoint.X, (double) originalPoint.Y);
    }

    /// <summary>
    /// Create a Point according to coordinates.
    /// </summary>
    public static object MakePoint(double[] coords)
    {
      return new System.Windows.Point(coords[0], coords[1]);
    }

    /// <summary>
    /// Creates a Point according to the X and Y coordinates.
    /// </summary>
    public static object MakePoint(double x, double y)
    {
      return new System.Windows.Point(x, y);
    }

    // http://www.codeproject.com/KB/recipes/hexencoding.aspx
    private static byte[] HexToData(string hexString)
    {
      if (hexString == null)
        return null;

      if (hexString.Length % 2 == 1)
        hexString = '0' + hexString;

      byte[] data = new byte[hexString.Length / 2];

      for (int i = 0; i < data.Length; i++)
        data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

      return data;
    }
  }
}
