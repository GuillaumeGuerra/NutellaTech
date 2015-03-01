//*****************************************************************************
//* Class: Utility
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2008
//*****************************************************************************
//* Project                 : GalaSoft.Utilities
//* Author                  : Laurent Bugnion, GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 27.10.2003
//*****************************************************************************
//* Last base level: BL0008
//*****************************************************************************

#region Imports

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GalaSoft.Utilities.Attributes;

#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif

#endregion

namespace GalaSoft.Utilities
{
  /// <summary>
  /// Set of utility functions.
  /// </summary>
  [ClassInfo(typeof(Utility),
    VersionString = "1.10.1",
    DateString = "200902011436",
    Description = "Set of utility functions.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch")]
  public static class Utility
  {
    #region Constructors

    #region Static Constructors
    #endregion

    #region Public Constructors
    #endregion

    #region Private Constructors
    #endregion

    #region Protected Constructors
    #endregion

    #region Internal Constructors
    #endregion

    #endregion

    #region Static Members

    #region Static Public Properties
    #endregion

    #region Static Private Properties
    #endregion

    #region Static Public Methods

    // ------------------------------------------------------------------------
    /// <summary>
    /// Get a number with N digits.
    /// </summary>
    /// <param name="number">The number which must be formatted.</param>
    /// <param name="digits">The number of digits to return.</param>
    /// <returns>The encoded number as a string.
    /// If the number is negative, returns "-xxx" where xxx is the encoded
    /// absolute value.
    /// For example, GetNDigits( -21, 3 ) return "-021".
    /// If the number is too big to be encoded with the
    /// number of digits, returns the number as a string.</returns>
    public static string GetNDigits(int number, int digits)
    {
      if ((number < 0))
      {
        return "-" + GetNDigits(Math.Abs(number), digits);
      }
      else
      {
        if (number < Math.Pow(10, digits))
        {
          StringBuilder builder = new StringBuilder();
          builder.Append(number);

          for (int index = 1; index < digits; index++)
          {
            builder.Insert(0, ((number < Math.Pow(10, index)) ? "0" : ""));
          }

          return builder.ToString();
        }
        else
        {
          return number.ToString(CultureInfo.InvariantCulture);
        }
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Converts a DateTime in a sortable string, without any separators.
    /// </summary>
    /// <param name="toConvert">The DateTime object to convert.</param>
    /// <param name="format">The desired output format.</param>
    /// <returns>The DateTime as a sortable string, without any separators.</returns>
    public static string GetSortableDateTimeString(DateTime toConvert,
      DateTimeFormat format)
    {
      StringBuilder builder = new StringBuilder();
      builder.Append(GetNDigits(toConvert.Year, 4));
      builder.Append(GetNDigits(toConvert.Month, 2));
      builder.Append(GetNDigits(toConvert.Day, 2));

      if ((format == DateTimeFormat.DateTimeMinutes)
        || (format == DateTimeFormat.DateTimeSeconds)
        || (format == DateTimeFormat.DateTimeMillis))
      {
        builder.Append(GetNDigits(toConvert.Hour, 2));
        builder.Append(GetNDigits(toConvert.Minute, 2));
      }

      if ((format == DateTimeFormat.DateTimeSeconds)
        || (format == DateTimeFormat.DateTimeMillis))
      {
        builder.Append(GetNDigits(toConvert.Second, 2));
      }

      if (format == DateTimeFormat.DateTimeMillis)
      {
        builder.Append(GetNDigits(toConvert.Millisecond, 3));
      }

      return builder.ToString();
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Converts a sortable date-time string (as produced by
    /// <see cref="GetSortableDateTimeString"/>) in the corresponding DateTime.
    /// </summary>
    /// <param name="sortableString">A string as produced by
    /// <see cref="GetSortableDateTimeString"/></param>
    /// <returns>The DateTime instance corresponding to the Stirng parameter.</returns>
    /// <exception cref="ArgumentException">If the string has an invalid format.</exception>
    public static DateTime GetDateTimeFromSortableString(string sortableDateTime)
    {
      if ((sortableDateTime == null)
        || (sortableDateTime.Length == 0))
      {
        return DateTime.MinValue;
      }

      if (sortableDateTime.Length < 8)
      {
        throw new ArgumentException("Invalid sortable DateTime string");
      }

      int year = Int32.Parse(sortableDateTime.Substring(0, 4), CultureInfo.InvariantCulture);
      int month = Int32.Parse(sortableDateTime.Substring(4, 2), CultureInfo.InvariantCulture);
      int day = Int32.Parse(sortableDateTime.Substring(6, 2), CultureInfo.InvariantCulture);
      int hour = 0;
      int minute = 0;
      int second = 0;
      int millisecond = 0;

      if (sortableDateTime.Length > 8)
      {
        if (sortableDateTime.Length < 12)
        {
          throw new ArgumentException("Invalid sortable DateTime string");
        }

        hour = Int32.Parse(sortableDateTime.Substring(8, 2), CultureInfo.InvariantCulture);
        minute = Int32.Parse(sortableDateTime.Substring(10, 2), CultureInfo.InvariantCulture);
      }

      if (sortableDateTime.Length > 12)
      {
        if (sortableDateTime.Length < 14)
        {
          throw new ArgumentException("Invalid sortable DateTime string");
        }

        second = Int32.Parse(sortableDateTime.Substring(12, 2), CultureInfo.InvariantCulture);
      }

      if (sortableDateTime.Length > 14)
      {
        if (sortableDateTime.Length != 17)
        {
          throw new ArgumentException("Invalid sortable DateTime string");
        }

        millisecond = Int32.Parse(sortableDateTime.Substring(14, 3), CultureInfo.InvariantCulture);
      }

      return new DateTime(year, month, day, hour, minute, second, millisecond);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Parses a query string starting with a '?' or not.
    /// </summary>
    /// <param name="queryString">The query string to be parsed. The initial '?' can be
    /// included or not.</param>
    /// <returns>A Dictionary of key/value pairs corresponding to the query string. If queryString
    /// is null, returns null. If queryString is an empty string, returns an empty Dictionary.</returns>
    public static Dictionary<string, string> ExtractQuery(string queryString)
    {
      if (queryString == null)
      {
        return null;
      }

      Dictionary<string, string> result = new Dictionary<string, string>();

      if (queryString.Length == 0)
      {
        return result;
      }

      if (queryString.StartsWith("?"))
      {
        queryString = queryString.Substring(1);
      }

      string[] splitByParameters = queryString.Split(new char[] { '&' });
      foreach (string byParameter in splitByParameters)
      {
        string[] pair = byParameter.Split(new char[] { '=' });

        result.Add(pair[0], HttpUtility.UrlDecode(pair[1]));
      }

      return result;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Parses a URI's query string.
    /// </summary>
    /// <param name="queryString">The URI of which the query string must be parsed.</param>
    /// <returns>A Dictionary of key/value pairs corresponding to the query string. If queryUri
    /// is null, returns null. If queryUri doesn't have a query string, returns an empty Dictionary.</returns>
    public static Dictionary<string, string> ExtractQuery(Uri queryUri)
    {
      if (queryUri == null)
      {
        return null;
      }

      string queryString = queryUri.Query;
      return ExtractQuery(queryString);
    }

    #endregion

    #region Static Private Methods
    #endregion

    #region Static Private Fields
    #endregion

    #endregion

    #region Dependency Properties
    #endregion

    #region Properties

    #region Public Properties
    #endregion

    #region Private Properties
    #endregion

    #region Protected Properties
    #endregion

    #region Internal Properties
    #endregion

    #endregion

    #region Methods

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Internal Methods
    #endregion

    #endregion

    #region Operators
    #endregion

    #region Events

    #region Public Events
    #endregion

    #region Private Events
    #endregion

    #region Protected Events
    #endregion

    #region Internal Events
    #endregion

    #endregion

    #region Interfaces Implementation
    #endregion

    #region Nested Types
    
    // ------------------------------------------------------------------------
    /// <summary>
    /// Use with method <see cref="GetSortableDateTimeString" />.
    /// </summary>
    public enum DateTimeFormat : int
    {
      /// <summary>
      /// Format yyyyMMdd.
      /// </summary>
      DateOnly = 0,
      /// <summary>
      /// Format yyyyMMddhhmm.
      /// </summary>
      DateTimeMinutes = 1,
      /// <summary>
      /// Format yyyyMMddhhmmss.
      /// </summary>
      DateTimeSeconds = 2,
      /// <summary>
      /// Format yyyyMMddhhmmssmmm.
      /// </summary>
      DateTimeMillis = 3,
    }

    #endregion

    #region Constants
    #endregion

    #region Private Fields
    #endregion

  }
}
