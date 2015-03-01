//*****************************************************************************
//* Class: FlagsExtracter
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2006 - 2008
//*****************************************************************************
//* Project                 : GalaSoft.Utilities
//* Author                  : Laurent Bugnion, GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 15.09.2006
//*****************************************************************************
//* Last base level: BL0005
//*****************************************************************************

#region Imports

using System;
using System.Collections.Generic;
using System.Reflection;
using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities
{
  /// <summary>
  /// Converts an integer value into the corresponding enumerated flags,
  /// returned in an array.
  /// </summary>
  /// <typeparam name="T">An Enumeration with the <see cref="FlagsAttribute"/> set.
  /// <para>The enumeration constants must be in powers of two, that is, 1, 2, 4, 8,
  /// and so on. This means the individual flags in combined enumeration constants do
  /// not overlap.</para></typeparam>
  [ClassInfo( typeof( FlagsExtracter<> ),
    VersionString = "V1.2.1",
    DateString = "200809302301",
    Description = "Extracts flags from an integer value and gives the corresponding enum values back.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch")]
  public class FlagsExtracter<T>
  {
    private List<T> _flagsList = new List<T>();
    private int _originalValue = 0;
    private T[] _enumValues = null;
    private int[] _integerValues = null;

    // ------------------------------------------------------------------------
    /// <summary>
    /// Contains the enumerated flags corresponding to the integer value
    /// passed to the constructor.
    /// </summary>
    public T[] Flags
    {
      get
      {
        return _flagsList.ToArray();
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Contains the original value passed to the constructor.
    /// </summary>
    public int OriginalValue
    {
      get
      {
        return _originalValue;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Converts an integer value into the corresponding enumerated flags,
    /// returned in an array.
    /// </summary>
    /// <param name="flags">An integer corresponding to the flags set. For example,
    /// binary 100101101 = decimal 301</param>
    /// <returns>An array of enumerated values of the type parameter T.</returns>
    public static T[] ExtractFlags( int flags )
    {
      return new FlagsExtracter<T>( flags ).Flags;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Checks if a given MemberInfo is decorated with <see cref="FlagsAttribute" />.
    /// Typically, this method is used on "enum" types.
    /// <example>
    /// if (HasFlagsAttribute(typeof(AnyEnum))
    /// {
    ///   // ...
    /// }
    /// </example>
    /// </summary>
    /// <returns>true if T is decorated with FlagsAttribute, false otherwise.</returns>
    public static bool HasFlagsAttribute(MemberInfo type)
    {
      return (type.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the extracter with its property
    /// <see cref="Flags" /> set accordingly to the parameter iFlags.
    /// </summary>
    /// <param name="flags">An integer corresponding to the flags set. For example,
    /// binary 100101101 = decimal 301</param>
    /// <exception cref="ArgumentException">If T is not decorated with
    /// <see cref="FlagsAttribute" />.</exception>
    public FlagsExtracter( int flags )
    {
      // Check if the "T" Enum is correctly qualified as "Flags"
      Type typeOfEnum = typeof( T );
      if ( !HasFlagsAttribute(typeOfEnum) )
      {
        // RESX
        throw new ArgumentException( string.Format( "The type {0} is not an enum, or not a Flags enumeration",
          typeOfEnum.FullName ) );
      }

      // GetValues returns the values sorted by the binary value of the enum
      Array values = Enum.GetValues( typeof( T ) );
      _enumValues = (T[]) values;
      _integerValues = (int[]) values;

      AddFlags( flags );
    }
    
    // ------------------------------------------------------------------------
    /// <summary>
    /// Adds a serie of flags to the already extracted values.
    /// </summary>
    /// <param name="flags">An integer corresponding to the flags set. For example,
    /// binary 100101101 = decimal 301</param>
    public void AddFlags( int flags )
    {
      for ( int index = 0; index < _integerValues.Length; index++ )
      {
        if ( _integerValues[ index ] > 0 )
        {
          if ( _integerValues[ index ] > flags )
          {
            break;
          }

          int testValue = _integerValues[ index ] & flags;
          if ( testValue == _integerValues[ index ]
            && !_flagsList.Contains( _enumValues[ index ] ) )
          {
            _flagsList.Add( _enumValues[ index ] );
            _originalValue += _integerValues[ index ];
          }
        }
      }
    }
  }
}