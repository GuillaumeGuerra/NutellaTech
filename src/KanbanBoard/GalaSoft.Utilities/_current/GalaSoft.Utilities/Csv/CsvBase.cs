//*****************************************************************************
//* Object Name: CsvBase
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2003 - 2008
//*****************************************************************************
//* Project                 : GalaSoft.Utilities
//* Target                  : .NET Framework 2.0
//* Language/Compiler       : C#
//* Author                  : Laurent Bugnion (LBu), GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 17.02.2006
//*****************************************************************************
//* Description:
//* See the class definition here under.
//* Last base level: BL0001.
//*****************************************************************************

//*****************************************************************************
//* Imports *******************************************************************
//*****************************************************************************

#region Imports

using System;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities.Csv
{
  // Class definition *********************************************************
  /// <summary>
  /// Basis class for CSV writer and reader.
  /// <para>Allows to define any separator. Uses ';' as default separator if nothing else is defined.</para>
  /// </summary>
  [ClassInfo( typeof( CsvBase ),
    DisplayName = "CSV base class",
    VersionString = "V01.00",
    DateString = "200602172237",
    Description = "Basis class for CSV writer and reader.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch" )]
  public abstract class CsvBase : IDisposable
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
    /// Default separator for CSV fields. Set to ';'.
    /// </summary>
    public const char DEFAULT_SEPARATOR = ';';

    #endregion

    //*************************************************************************
    //* Static attributes *****************************************************
    //*************************************************************************

    #region StaticAttributes
    #endregion

    //*************************************************************************
    //* Attributes ************************************************************
    //*************************************************************************

    #region Attributes

    /// <summary>
    /// Separator for CSV fields.
    /// </summary>
    protected char _separator = DEFAULT_SEPARATOR;

    #endregion

    //*************************************************************************
    //* Properties ************************************************************
    //*************************************************************************

    #region Properties

    /// <summary>
    /// Separator for CSV fields. Set by default to ';'. Can be set to anything
    /// by the user.
    /// </summary>
    public char Separator
    {
      get { return _separator; }
      set { _separator = value; }
    }

    #endregion

    //*************************************************************************
    //* Static methods ********************************************************
    //*************************************************************************

    #region Static methods
    #endregion

    //*************************************************************************
    //* Constructor & generated code ******************************************
    //*************************************************************************

    #region Constructors
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

    /// <summary>
    /// Closes the stream.
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// Closes the stream and disposes this object.
    /// </summary>
    public abstract void Dispose();

    #endregion

    //*************************************************************************
    //* Operators *************************************************************
    //*************************************************************************

    #region Operators
    #endregion
  }
}
