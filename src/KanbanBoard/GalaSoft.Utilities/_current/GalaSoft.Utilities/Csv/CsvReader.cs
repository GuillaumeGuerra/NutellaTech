//*****************************************************************************
//* Object Name: CsvReader
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
using System.Collections;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities.Csv
{
  // Class definition *********************************************************
  /// <summary>
  /// Reader for CSV lines.
  /// <para>Allows to define any separator. Uses ';' as default separator if nothing else is defined.</para>
  /// </summary>
  [ClassInfo( typeof( CsvReader ),
    DisplayName="CSV reader",
    VersionString = "V01.00",
    DateString = "200602172233",
    Description = "Reader for CSV lines.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch" )]
  public class CsvReader : CsvBase
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
    /// The reader for the CSV lines.
    /// </summary>
    private StreamReader _reader = null;

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
    /// Constructor for this reader.
    /// </summary>
    /// <param name="path">The input file's full path.</param>
    public CsvReader( string path )
    {
      _reader = new StreamReader( path );
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
    /// Reads a CSV line.
    /// </summary>
    /// <returns>The fields parsed in an array.</returns>
    public string[] ReadLine()
    {
      string newLine = _reader.ReadLine();

      if ( newLine == null )
      {
        return null;
      }
      if ( newLine.Length == 0 )
      {
        return new string[ 1 ] { "" };
      }

      ArrayList result = new ArrayList();
      
      bool inQuote = false;
      char currentChar;
      string currentField = "";
      for ( int index = 0; index < newLine.Length; index++ )
      {
        currentChar = newLine[ index ];
        if ( currentChar == '"' )
        {
          inQuote = !inQuote;
          continue;
        }

        if ( !inQuote
          && currentChar == _separator )
        {
          // If not in a quote, then it's the end of the field.
          result.Add( currentField );
          currentField = "";
        }
        else
        {
          currentField += currentChar;
        }
      }

      result.Add( currentField );
      return (string[]) result.ToArray( typeof( string ) );
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Closes the stream. This method must be called before the file
    /// can be used by other applications.
    /// </summary>
    public override void Close()
    {
      if ( _reader != null )
      {
        _reader.Close();
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
