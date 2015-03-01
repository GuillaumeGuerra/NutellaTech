//*****************************************************************************
//* Object Name: CResourceFileInfo    
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2006
//*****************************************************************************
//* Project                 : GalaSoft.Utilities.Resources
//* Target                  : .NET Framework 2.0
//* Language/Compiler       : C#
//* Author                  : Laurent Bugnion (LBu), GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 01.10.2006
//*****************************************************************************
//* Description:
//* See the class definition here under.
//*****************************************************************************

//*****************************************************************************
//* Imports *******************************************************************
//*****************************************************************************

#region Imports

using System;
using System.IO;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities.Resources
{
  // Class definition *********************************************************
  /// <summary>
  /// Delivers information about files embedded in the Resources.
  /// </summary>
  [ClassInfo( typeof( ResourceFactory ) )]
  public class CResourceFileInfo
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
    /// Embedded file's full name.
    /// <example>GalaSoft.Utilities.Images.myimage.gif</example>
    /// </summary>
    private string m_strFullName = "";
    /// <summary>
    /// The Type for which this resource is embedded.
    /// </summary>
    private Type m_tyType = null;
    /// <summary>
    /// The directory in the Assembly in which this file is embedded.
    /// <example>For a resource located in GalaSoft.Utilities.Images.myimage.gif,
    /// with a type of GalaSoft.Utilities.CUtilities,
    /// the namespace is "GalaSoft.Utilities" and the directory is "Images".</example>
    /// </summary>
    private string m_strDirectory = null;
    /// <summary>
    /// The resource's file name, including the extension.
    /// </summary>
    private string m_strFileName = null;
    /// <summary>
    /// The resource's file name, without the extension.
    /// </summary>
    private string m_strFileNameWithoutExtension = null;
    /// <summary>
    /// The file's extension, including the leading '.'.
    /// </summary>
    private string m_strExtension = null;

    #endregion

    //*************************************************************************
    //* Properties ************************************************************
    //*************************************************************************

    #region Properties

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the embedded file's full name.
    /// <example>GalaSoft.Utilities.Images.myimage.gif</example>
    /// </summary>
    public string strFullName
    {
      get
      {
        return m_strFullName;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the Type for which this resource is embedded.
    /// </summary>
    public Type tyType
    {
      get
      {
        return tyType;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the directory in the Assembly in which this file is embedded.
    /// <example>For a resource located in GalaSoft.Utilities.Images.myimage.gif,
    /// with a type of GalaSoft.Utilities.CUtilities,
    /// the namespace is "GalaSoft.Utilities" and the directory is "Images".</example>
    /// </summary>
    public string strDirectory
    {
      get
      {
        return m_strDirectory;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the resource's file name, including the extension.
    /// </summary>
    public string strFileName
    {
      get
      {
        if ( m_strFileName == null )
        {
          // Add 2 to length for '.'
          m_strFileName = m_strFullName.Substring( strNameSpace.Length + strDirectory.Length + 2 );
        }
        return m_strFileName;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the resource's file name, without the extension.
    /// </summary>
    public string strFileNameWithoutExtension
    {
      get
      {
        if ( m_strFileNameWithoutExtension == null )
        {
          m_strFileNameWithoutExtension = Path.GetFileNameWithoutExtension( strFileName );
        }
        return m_strFileNameWithoutExtension;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the file's extension, including the leading '.'.
    /// </summary>
    public string strExtension
    {
      get
      {
        if ( m_strExtension == null )
        {
          m_strExtension = Path.GetExtension( m_strFullName );
        }
        return m_strExtension;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type for which this file is embedded.
    /// <example>For a resource located in GalaSoft.Utilities.Images.myimage.gif,
    /// with a type of GalaSoft.Utilities.CUtilities,
    /// the namespace is "GalaSoft.Utilities" and the directory is "Images".</example>
    /// </summary>
    public string strNameSpace
    {
      get
      {
        return m_tyType.Namespace;
      }
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

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="tyType">The type for which this resource is embedded.</param>
    /// <param name="strDirectory">The directory in the resource under which
    /// this resource is embedded.</param>
    /// <param name="strFullName">The embedded resource's full name.</param>
    /// <example>For a resource located in GalaSoft.Utilities.Images.myimage.gif,
    /// with a type of GalaSoft.Utilities.CUtilities,
    /// the namespace is "GalaSoft.Utilities" and the directory is "Images".</example>
    public CResourceFileInfo( Type tyType, string strDirectory, string strFullName )
    {
      m_tyType = tyType;
      m_strDirectory = strDirectory;
      m_strFullName = strFullName;
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
    #endregion

    //*************************************************************************
    //* Classes ***************************************************************
    //*************************************************************************

    #region Classes
    #endregion
  }
}