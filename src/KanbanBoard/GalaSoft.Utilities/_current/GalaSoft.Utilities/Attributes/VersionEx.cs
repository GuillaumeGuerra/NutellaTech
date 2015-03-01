//*****************************************************************************
//* Object Name: VersionEx
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2003 - 2008
//*****************************************************************************
//* Project                 : GalaSoft.Utilities.Attributes
//* Target                  : .NET Framework 2.0
//* Language/Compiler       : C#
//* Author                  : Laurent Bugnion (LBu), GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 17.02.2006
//*****************************************************************************
//* Description:
//* See the class definition here under.
//* Last Base Level: BL0006
//*****************************************************************************

//*****************************************************************************
//* Imports *******************************************************************
//*****************************************************************************

#region Imports

using System;
using System.Text.RegularExpressions;

#endregion

namespace GalaSoft.Utilities.Attributes
{
  // Class definition *********************************************************
  /// <summary>
  /// Defines a VersionEx object as used by GalaSoft objects, including
  /// a BaseLevel version.
  /// </summary>
  [ClassInfo(typeof(VersionEx),
    DisplayName = "Extended version object",
    VersionString = "V1.0.5",
    DateString = "200902011436",
    Description = "Defines a VersionEx object as used by GalaSoft objects, including a BaseLevel version.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch")]
  public class VersionEx
  {
    //*************************************************************************
    //* Enums *****************************************************************
    //*************************************************************************

    #region Enums
    #endregion

    //*************************************************************************
    //* Constants *************************************************************
    //*************************************************************************

    #region Constants

    /// <summary>
    /// The length of the "Major" part in the <see cref="Normalized" /> string.
    /// </summary>
    private const int LENGTH_MAJOR = 2;
    /// <summary>
    /// The length of the "Minor" part in the <see cref="Normalized" /> string.
    /// </summary>
    private const int LENGTH_MINOR = 2;
    /// <summary>
    /// The length of the "Build" part in the <see cref="Normalized" /> string.
    /// </summary>
    private const int LENGTH_BUILD = 3;
    /// <summary>
    /// The length of the "Revision" part in the <see cref="Normalized" /> string.
    /// </summary>
    private const int LENGTH_REVISION = 4;
    /// <summary>
    /// The length of the "BaseLevel" part in the <see cref="Normalized" /> string.
    /// </summary>
    private const int LENGTH_BASE_LEVEL = 4;
    /// <summary>
    /// The length of the "Letter" part in the <see cref="Normalized" /> string.
    /// </summary>
    private const int LENGTH_BASE_LEVEL_LETTER = 1;

    #endregion

    //*************************************************************************
    //* Static attributes *****************************************************
    //*************************************************************************

    #region StaticAttributes

    /// <summary>
    /// Regular expression used to test if a version string is valid.
    /// </summary>
    private static Regex s_versionRegEx
      = new Regex(@"^[Vv]?\d+\.\d+(\.\d+){0,2}(/BL\d{"
      + LENGTH_BASE_LEVEL
      + @"}[A-Z]?)?");

    /// <summary>
    /// Regular expression used to test if a normalized version string is valid.
    /// </summary>
    private static Regex s_versionNormalizedRegEx
      = new Regex(@"^[Vv]?\d{" + LENGTH_MAJOR + @"}\d{"
      + LENGTH_MINOR + @"}(\d{"
      + LENGTH_BUILD + @"}(\d{"
      + LENGTH_REVISION + @"})?)?(BL\d{"
      + LENGTH_BASE_LEVEL + @"}[A-Z]?)?");

    #endregion

    //*************************************************************************
    //* Attributes ************************************************************
    //*************************************************************************

    #region Attributes

    /// <summary>
    /// The basis Version object as defined by the .NET framework.
    /// </summary>
    private Version _version;
    /// <summary>
    /// A Base Level: BLdddd[L] where L is an optional letter from A to Z.
    /// </summary>
    private string _baseLevel = "";

    #endregion

    //*************************************************************************
    //* Properties ************************************************************
    //*************************************************************************

    #region Properties

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the VersionEx object in normalized form: Vxxyy[zzz[rrrr]][BLdddd[L]]
    /// where L is an optional letter from A to Z.
    /// </summary>
    public string Normalized
    {
      get
      {
        if (_version == null
          || _version.Major < 0
          || _version.Minor < 0)
        {
          return "";
        }

        string result = "V"
          + Utility.GetNDigits(_version.Major, LENGTH_MAJOR)
          + Utility.GetNDigits(_version.Minor, LENGTH_MINOR);

        if (_version.Build >= 0)
        {
          result += Utility.GetNDigits(_version.Build, LENGTH_BUILD);
        }
        if (_version.Revision >= 0)
        {
          result += Utility.GetNDigits(_version.Revision, LENGTH_REVISION);
        }
        result += _baseLevel;
        return result;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the VersionEx object in full: [V][xx.yy[.zz[.rr]]][/BLdddd[L]]
    /// where L is an optional letter from A to Z.
    /// </summary>
    public string Full
    {
      get
      {
        if (_version == null
          || _version.Major < 0
          || _version.Minor < 0)
        {
          return "";
        }
        string result = MajorMinorBuildRevision;
        if (_baseLevel.Length > 0)
        {
          result += "/" + _baseLevel;
        }
        return result;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the "Major" part of the VersionEx.
    /// <para>If it is undefined, returns -1.</para>
    /// </summary>
    public int Major
    {
      get
      {
        if (_version == null)
        {
          return -1;
        }
        return _version.Major;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the "Minor" part of the VersionEx.
    /// <para>If it is undefined, returns -1.</para>
    /// </summary>
    public int Minor
    {
      get
      {
        if (_version == null)
        {
          return -1;
        }
        return _version.Minor;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the "Build" part of the VersionEx.
    /// <para>If it is undefined, returns -1.</para>
    /// </summary>
    public int Build
    {
      get
      {
        if (_version == null)
        {
          return -1;
        }
        return _version.Build;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the "Revision" part of the VersionEx.
    /// <para>If it is undefined, returns -1.</para>
    /// </summary>
    public int Revision
    {
      get
      {
        if (_version == null)
        {
          return -1;
        }
        return _version.Revision;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the VersionEx object in Major.Minor form: Vx.y.
    /// <para>If Major or Minor are undefined, returns an empty string.</para>
    /// </summary>
    public string MajorMinor
    {
      get
      {
        if (Major >= 0
          && Minor >= 0)
        {
          return "V" + Major
            + "." + Minor;
        }
        return "";
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the VersionEx object in Major.Minor.Build form: Vx.y[.z].
    /// <para>If Build is undefined, it is omitted.</para>
    /// </summary>
    public string MajorMinorBuild
    {
      get
      {
        string version = MajorMinor;
        if (version.Length == 0)
        {
          return "";
        }
        else
        {
          if (Build >= 0)
          {
            return version + "." + Build;
          }
          return version;
        }
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the VersionEx object in Major.Minor.Build.Revision form: Vx.y[.z[.r]].
    /// <para>If Revision is undefined, it is omitted.</para>
    /// </summary>
    public string MajorMinorBuildRevision
    {
      get
      {
        string version = MajorMinorBuild;
        if (version.Length == 0)
        {
          return "";
        }
        else
        {
          if (Revision >= 0)
          {
            return version + "." + Revision;
          }
          return version;
        }
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the Base Level part of the VersionEx: BLdddd[L] where L is an optional
    /// letter from A to Z.
    /// <para>If it is not defined, or if the Assembly is a release version,
    /// returns an empty string.</para>
    /// </summary>
    public string BaseLevel
    {
      get
      {
        if (_baseLevel.Length > 0)
        {
#if DEBUG
          return _baseLevel;
#else
          return "";
#endif
        }
        return "";
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Regular expression used to test if a normalized version string is valid.
    /// </summary>
    public static Regex s_VersionRegEx
    {
      get
      {
        return s_versionRegEx;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Regular expression used to test if a normalized version string is valid.
    /// </summary>
    public static Regex s_VersionNormalizedRegEx
    {
      get
      {
        return s_versionNormalizedRegEx;
      }
    }

    #endregion

    //*************************************************************************
    //* Static methods ********************************************************
    //*************************************************************************

    #region Static methods

    // ------------------------------------------------------------------------
    /// <summary>
    /// Tests if a string is a valid version string.
    /// <para>A version string is valid if it is in the form </para>
    /// <list type="bullet">
    /// <item>[V][xx.yy[.zz[.rr]]][/BLdddd[L]]</item>
    /// <item>Vxxyy[zzz[rrrr]][BLdddd[L]]</item>
    /// </list>
    /// </summary>
    /// <param name="versionString">The version string to be tested</param>
    /// <returns>True if it is a valid version string, false otherwise.</returns>
    public static bool IsValidVersion(string versionString)
    {
      return (s_versionRegEx.Match(versionString).Value == versionString
        || s_versionNormalizedRegEx.Match(versionString).Value == versionString);
    }

    #endregion

    //*************************************************************************
    //* Constructor & generated code ******************************************
    //*************************************************************************

    #region Constructors

    // ------------------------------------------------------------------------
    /// <summary>
    /// Takes a Version string and constructs a VersionEx object.
    /// </summary>
    /// <param name="version">Supports the following formats:
    /// <list type="bullet">
    /// <item>[V][xx.yy[.zz[.rr]]][/BLdddd[L]]</item>
    /// <item>Vxxyy[zzz[rrrr]][BLdddd[L]]</item>
    /// </list>
    /// </param>
    /// <exception cref="ArgumentException">Thrown when the argument is not in the
    /// correct form.</exception>
    public VersionEx(string version)
    {
      int major = -1;
      int minor = -1;
      int build = -1;
      int revision = -1;
      string tempVersion = version;
      if (version.ToLower().StartsWith("v"))
      {
        tempVersion = tempVersion.Substring(1);
      }

      if (tempVersion != null)
      {
        if (s_versionRegEx.Match(tempVersion).Value != tempVersion)
        {
          if (s_versionNormalizedRegEx.Match(tempVersion).Value != tempVersion)
          {
            throw new ArgumentException("Invalid version string" + Environment.NewLine
              + "(should be [V][xx.yy[.zz[.rr]]][/BLdddd[L]]" + Environment.NewLine
              + "or Vxxyy[zzz[rrrr]][BLdddd[L]]): " + version,
              "version");
          }
          else
          {
            string temp = tempVersion.Substring(0, LENGTH_MAJOR);
            major = Int32.Parse(temp);
            tempVersion = tempVersion.Substring(LENGTH_MAJOR);
            temp = tempVersion.Substring(0, LENGTH_MINOR);
            minor = Int32.Parse(temp);
            tempVersion = tempVersion.Substring(LENGTH_MINOR);
            if (tempVersion.Length > 0
              && !tempVersion.StartsWith("D"))
            {
              temp = tempVersion.Substring(0, LENGTH_BUILD);
              build = Int32.Parse(temp);
              tempVersion = tempVersion.Substring(LENGTH_BUILD);
            }
            if (tempVersion.Length > 0
              && !tempVersion.StartsWith("D"))
            {
              temp = tempVersion.Substring(0, LENGTH_REVISION);
              revision = Int32.Parse(temp);
              tempVersion = tempVersion.Substring(LENGTH_REVISION);
            }
            if (tempVersion.Length > 0
              && tempVersion.StartsWith("BL"))
            {
              _baseLevel = tempVersion;
            }
          }
        }
        else
        {
          if (tempVersion.IndexOf("/") > -1)
          {
            string[] versions = tempVersion.Split(new char[] { '/' });
            tempVersion = versions[0];
            _baseLevel = versions[1].Trim();
          }
          string[] valueStrings = tempVersion.Split(new char[] { '.' });
          int[] valueInts = new int[4] { -1, -1, -1, -1 };
          for (int index = 0; index < valueStrings.Length; index++)
          {
            valueInts[index] = Int32.Parse(valueStrings[index]);
          }
          major = valueInts[0];
          minor = valueInts[1];
          build = valueInts[2];
          revision = valueInts[3];
        }
      }

      _version = (major < 0 || minor < 0) ? null
        : (build < 0) ? new Version(major, minor)
        : (revision < 0) ? new Version(major, minor, build)
        : new Version(major, minor, build, revision);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a VersionEx object with Major.Minor parts
    /// </summary>
    /// <param name="major">The Major part.</param>
    /// <param name="minor">The Minor part.</param>
    public VersionEx(int major, int minor)
    {
      _version = new Version(major, minor);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a VersionEx object with Major.Minor.Build parts
    /// </summary>
    /// <param name="major">The Major part.</param>
    /// <param name="minor">The Minor part.</param>
    /// <param name="build">The Build part.</param>
    public VersionEx(int major, int minor, int build)
    {
      _version = new Version(major, minor, build);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a VersionEx object with Major.Minor.Build.Revision parts
    /// </summary>
    /// <param name="major">The Major part.</param>
    /// <param name="minor">The Minor part.</param>
    /// <param name="build">The Build part.</param>
    /// <param name="revision">The Revision part.</param>
    public VersionEx(int major, int minor, int build, int revision)
    {
      _version = new Version(major, minor, build, revision);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a VersionEx object with Major.Minor/BaseLevel parts
    /// </summary>
    /// <param name="major">The Major part.</param>
    /// <param name="minor">The Minor part.</param>
    /// <param name="baseLevel">The Base Level part BLdddd[L] where L is an optional
    /// letter from A to Z.</param>
    public VersionEx(int major, int minor, string baseLevel)
    {
      _version = new Version(major, minor);
      _baseLevel = baseLevel;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a VersionEx object with Major.Minor.Build/BaseLevel parts
    /// </summary>
    /// <param name="major">The Major part.</param>
    /// <param name="minor">The Minor part.</param>
    /// <param name="build">The Build part.</param>
    /// <param name="baseLevel">The Base Level part BLdddd[L] where L is an optional
    /// letter from A to Z.</param>
    public VersionEx(int major, int minor, int build, string baseLevel)
    {
      _version = new Version(major, minor, build);
      _baseLevel = baseLevel;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a VersionEx object with Major.Minor.Build.Revision/BaseLevel parts
    /// </summary>
    /// <param name="major">The Major part.</param>
    /// <param name="minor">The Minor part.</param>
    /// <param name="build">The Build part.</param>
    /// <param name="revision">The Revision part.</param>
    /// <param name="baseLevel">The Base Level part BLdddd[L] where L is an optional
    /// letter from A to Z.</param>
    public VersionEx(int major, int minor, int build, int revision, string baseLevel)
    {
      _version = new Version(major, minor, build, revision);
      _baseLevel = baseLevel;
    }

    #endregion

    //*************************************************************************
    //* Event handlers ********************************************************
    //*************************************************************************

    #region Event handlers
    #endregion

    //*************************************************************************
    //* Methods ***************************************************************
    //*************************************************************************

    #region Methods

    // ------------------------------------------------------------------------
    /// <summary>
    /// Overrides <see cref="Object.GetHashCode" />.
    /// </summary>
    /// <returns>A unique hashcode for this instance.</returns>
    public override int GetHashCode()
    {
      return _version.GetHashCode() ^ _baseLevel.GetHashCode();
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Overrides <see cref="Object.Equals(object)" />.
    /// </summary>
    /// <param name="obj">A VersionEx instance.</param>
    /// <returns>True if both versions are equal.</returns>
    /// <exception cref="InvalidCastException">Thrown if obj is not
    /// a VersionEx instance.</exception>
    public override bool Equals(object obj)
    {
      return (this == (VersionEx) obj);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Overrides <see cref="Object.ToString"/>
    /// </summary>
    /// <returns>A formatted string representing this VersionEx object</returns>
    public override string ToString()
    {
      return Full;
    }

    #endregion

    //*************************************************************************
    //* Operators *************************************************************
    //*************************************************************************

    #region Operators

    // ------------------------------------------------------------------------
    /// <summary>
    /// Compares two VersionEx objects and returns true if the first one is equal
    /// to the second one.
    /// </summary>
    /// <param name="version1">The first VersionEx object.</param>
    /// <param name="version2">The second VersionEx object.</param>
    /// <returns>True if version1 is equal to version2. False otherwise.</returns>
    public static bool operator ==(VersionEx version1, VersionEx version2)
    {
      Version v1 = null;
      Version v2 = null;
      bool v1IsNull = false;
      bool v2IsNull = false;
      try
      {
        v1 = version1._version;
      }
      catch (NullReferenceException)
      {
        v1IsNull = true;
      }
      try
      {
        v2 = version2._version;
      }
      catch (NullReferenceException)
      {
        v2IsNull = true;
      }
      if (v1IsNull)
      {
        if (v2IsNull)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      else
      {
        if (v2IsNull)
        {
          return false;
        }
      }

      if (version1._version == version2._version)
      {
        return (version1._baseLevel == version2._baseLevel);
      }
      return false;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Compares two VersionEx objects and returns true if the first one is bigger
    /// than the second one.
    /// <para>Note: By convention, a version with a BaseLevel string is always
    /// bigger than the same version without BaseLevel string.</para>
    /// </summary>
    /// <param name="version1">The first VersionEx object.</param>
    /// <param name="version2">The second VersionEx object.</param>
    /// <returns>True if version1 is bigger than version2. False otherwise.</returns>
    public static bool operator >(VersionEx version1, VersionEx version2)
    {
      if (version1 == null
        && version2 != null)
      {
        return false;
      }
      if (version1 != null
        && version2 == null)
      {
        return true;
      }
      if (version1 == null
        && version2 == null)
      {
        return false;
      }
      if (version1._version > version2._version)
      {
        return true;
      }
      if (version1._version == version2._version)
      {
        return (string.Compare(version1._baseLevel, version2._baseLevel, StringComparison.OrdinalIgnoreCase) > 0);
      }
      return false;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Compares two VersionEx objects and returns true if the first one is bigger or equal
    /// to the second one.
    /// <para>Note: By convention, a version with a Base Level string is always
    /// bigger than the same version without Base Level string.</para>
    /// </summary>
    /// <param name="version1">The first VersionEx object.</param>
    /// <param name="version2">The second VersionEx object.</param>
    /// <returns>True if version1 is bigger or equal to version2. False otherwise.</returns>
    public static bool operator >=(VersionEx version1, VersionEx version2)
    {
      if (version1 == null
        && version2 != null)
      {
        return false;
      }
      if (version1 != null
        && version2 == null)
      {
        return true;
      }
      if (version1 == null
        && version2 == null)
      {
        return true;
      }
      if (version1._version > version2._version)
      {
        return true;
      }
      if (version1._version == version2._version)
      {
        return (string.Compare(version1._baseLevel, version2._baseLevel, StringComparison.OrdinalIgnoreCase) >= 0);
      }
      return false;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Compares two VersionEx objects and returns true if the first one is smaller
    /// than the second one.
    /// <para>Note: By convention, a version with a Base Level string is always
    /// bigger than the same version without Base Level string.</para>
    /// </summary>
    /// <param name="version1">The first VersionEx object.</param>
    /// <param name="version2">The second VersionEx object.</param>
    /// <returns>True if version1 is smaller than version2. False otherwise.</returns>
    public static bool operator <(VersionEx version1, VersionEx version2)
    {
      if (version1 == null
        && version2 != null)
      {
        return true;
      }
      if (version1 != null
        && version2 == null)
      {
        return false;
      }
      if (version1 == null
        && version2 == null)
      {
        return false;
      }
      if (version1._version < version2._version)
      {
        return true;
      }
      if (version1._version == version2._version)
      {
        return (string.Compare(version1._baseLevel, version2._baseLevel, StringComparison.OrdinalIgnoreCase) < 0);
      }
      return false;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Compares two VersionEx objects and returns true if the first one is smaller or equal
    /// to the second one.
    /// <para>Note: By convention, a version with a Base Level string is always
    /// bigger than the same version without Base Level string.</para>
    /// </summary>
    /// <param name="version1">The first VersionEx object.</param>
    /// <param name="version2">The second VersionEx object.</param>
    /// <returns>True if version1 is smaller or equal to version2. False otherwise.</returns>
    public static bool operator <=(VersionEx version1, VersionEx version2)
    {
      if (version1 == null
        && version2 != null)
      {
        return true;
      }
      if (version1 != null
        && version2 == null)
      {
        return false;
      }
      if (version1 == null
        && version2 == null)
      {
        return true;
      }
      if (version1._version < version2._version)
      {
        return true;
      }
      if (version1._version == version2._version)
      {
        return (string.Compare(version1._baseLevel, version2._baseLevel, StringComparison.OrdinalIgnoreCase) <= 0);
      }
      return false;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Compares two VersionEx objects and returns true if the first one is not equal
    /// to the second one.
    /// </summary>
    /// <param name="version1">The first VersionEx object.</param>
    /// <param name="version2">The second VersionEx object.</param>
    /// <returns>True if version1 is not equal to version2. False otherwise.</returns>
    public static bool operator !=(VersionEx version1, VersionEx version2)
    {
      Version v1 = null;
      Version v2 = null;
      bool v1IsNull = false;
      bool v2IsNull = false;
      try
      {
        v1 = version1._version;
      }
      catch (NullReferenceException)
      {
        v1IsNull = true;
      }
      try
      {
        v2 = version2._version;
      }
      catch (NullReferenceException)
      {
        v2IsNull = true;
      }
      if (v1IsNull)
      {
        if (v2IsNull)
        {
          return false;
        }
        else
        {
          return true;
        }
      }
      else
      {
        if (v2IsNull)
        {
          return true;
        }
      }

      if (v1 != v2)
      {
        return true;
      }
      return (version1._baseLevel != version2._baseLevel);
    }

    #endregion
  }
}
