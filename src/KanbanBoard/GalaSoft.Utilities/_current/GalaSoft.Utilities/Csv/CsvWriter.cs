//*****************************************************************************
//* Object Name: CsvWriter
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
using System.IO;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities.Csv
{
  // Class definition *********************************************************
  /// <summary>
  /// Writer for CSV lines.
  /// <para>Allows to define any separator. Uses ';' as default separator if nothing else is defined.</para>
  /// <para>If a field includes the defined separator, it will be enclosed in quotes.</para>
  /// </summary>
  [ClassInfo( typeof( CsvWriter ),
    DisplayName="CSV writer",
    VersionString = "V01.00",
    DateString = "200602172221",
    Description = "Writer for CSV lines.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch" )]
  public class CsvWriter : CsvBase
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
    /// The writer for the CSV lines.
    /// </summary>
    private StreamWriter _writer = null;
    
    #endregion

    //*************************************************************************
    //* Properties ************************************************************
    //*************************************************************************

    #region Properties
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

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructor for this writer.
    /// </summary>
    /// <param name="fullPath">The output file's full path.</param>
    /// <param name="append">If true, the new lines will be appended to the file,
    /// in case it existed already.</param>
    public CsvWriter( string fullPath, bool append )
    {
      _writer = new StreamWriter( fullPath, append );
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
    /// Writes a CSV line to the file.
    /// <para>If a field includes the defined separator, it will be enclosed in quotes.</para>
    /// </summary>
    /// <param name="columns">The fields to write to the file.</param>
    public void WriteLine( string[] columns )
    {
      if ( columns == null )
      {
        return;
      }

      string line = "";

      foreach ( string column in columns )
      {
        string quote = "";
        if ( column.IndexOf( _separator ) > -1 )
        {
          quote = "\"";
        }
        line += quote + column + quote + _separator;
      }

      line = line.Substring( 0, line.Length - 1 );
      _writer.WriteLine( line );
    }
  
    // ------------------------------------------------------------------------
    /// <summary>
    /// Closes the stream. This method must be called before the file
    /// can be used by other applications.
    /// </summary>
    public override void Close()
    {
      if ( _writer != null )
      {
        _writer.Close();
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Closes the stream. This method must be called before the file
    /// can be used by other applications.
    /// </summary>
    public override void Dispose()
    {
      this.Close();
    }

    #endregion

    //*************************************************************************
    //* Operators *************************************************************
    //*************************************************************************

    #region Operators
    #endregion
  }
}
