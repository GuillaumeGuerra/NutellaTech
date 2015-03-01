//*****************************************************************************
//* Object Name: CSessionControl
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2003 - 2006
//*****************************************************************************
//* Project                 : GalaSoft.Utilities
//* Target                  : .NET Framework 2.0
//* Language/Compiler       : C#
//* Author                  : Laurent Bugnion (LBu), GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 22.02.2006
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
using System.Collections;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities
{
  // Class definition *********************************************************
  /// <summary>
  /// Allows to keep a list of active sessions, and to check which are still active
  /// and which expired already.
  /// <para>Primarily intended for ASP.NET sessions, this class can also be used
  /// for any kind of session, as long as they can be uniquely identified
  /// by a string identifier.</para>
  /// <para>If the working process is stopped and restarted, the sessions are
  /// not valid anymore. This class allows to check this too, because the session
  /// list will be cleared in such a case.</para>
  /// </summary>
  [ClassInfo( typeof( CSessionControl ),
    VersionString = "V01.00",
    DateString = "200602222138",
    Description = "Allows to check if a session expired or is still active.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch" )]
  public class CSessionControl
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
    /// Static list of all sessions.
    /// </summary>
    private static Hashtable s_htSessions;

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

    // ------------------------------------------------------------------------
    /// <summary>
    /// To be called when the session is started, for example in Global.Session_Start
    /// for ASP.NET.
    /// </summary>
    /// <param name="tyType">The type responsible for the session control.</param>
    /// <param name="strSessionId">The unique identifier for this session.</param>
    public static void InitSession( Type tyType, string strSessionId )
    {
      if ( s_htSessions == null )
      {
        s_htSessions = new Hashtable();
      }

      string strTypeId = tyType.FullName.ToLower();
      ArrayList alSessionIds;
      if ( s_htSessions.Contains( strTypeId ) )
      {
        alSessionIds = (ArrayList) s_htSessions[ strTypeId ];
      }
      else
      {
        alSessionIds = new ArrayList();
        s_htSessions.Add( strTypeId, alSessionIds );
      }

      if ( !alSessionIds.Contains( strSessionId ) )
      {
        alSessionIds.Add( strSessionId );
      }
    }
    
    // ------------------------------------------------------------------------
    /// <summary>
    /// To be called when the session is started, for example in Global.Session_Start
    /// for ASP.NET.
    /// </summary>
    /// <param name="strSessionId">The unique identifier for this session.</param>
    public static void InitSession( string strSessionId )
    {
      InitSession( typeof( CSessionControl ), strSessionId );
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// To be called when the session is stopped, for example in Global.Session_End
    /// for ASP.NET.
    /// </summary>
    /// <param name="tyType">The type responsible for the session control.</param>
    /// <param name="strSessionId">The unique identifier for this session.</param>
    public static void EndSession( Type tyType, string strSessionId )
    {
      if ( s_htSessions == null )
      {
        return;
      }

      string strTypeId = tyType.FullName.ToLower();

      if ( !s_htSessions.Contains( strTypeId ) )
      {
        return;
      }

      ArrayList alSessionIds = (ArrayList) s_htSessions[ strTypeId ];

      if ( !alSessionIds.Contains( strSessionId ) )
      {
        return;
      }

      alSessionIds.Remove( strSessionId );
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// To be called when the session is stopped, for example in Global.Session_End
    /// for ASP.NET.
    /// </summary>
    /// <param name="strSessionId">The unique identifier for this session.</param>
    public static void EndSession( string strSessionId )
    {
      EndSession( typeof( CSessionControl ), strSessionId );
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Checks if a session is still active.
    /// </summary>
    /// <param name="tyType">The type responsible for the session control.</param>
    /// <param name="strSessionId">The unique identifier for this session.</param>
    /// <returns>True if the session is still active. False if EndSession has
    /// already been called, or if the working process has been stopped
    /// and restarted.</returns>
    public static bool IsSessionActive( Type tyType, string strSessionId )
    {
      if ( s_htSessions == null )
      {
        return false;
      }

      string strTypeId = tyType.FullName.ToLower();

      if ( !s_htSessions.Contains( strTypeId ) )
      {
        return false;
      }

      ArrayList alSessionIds = (ArrayList) s_htSessions[ strTypeId ];

      if ( !alSessionIds.Contains( strSessionId ) )
      {
        return false;
      }

      return true;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Checks if a session is still active.
    /// </summary>
    /// <param name="strSessionId">The unique identifier for this session.</param>
    /// <returns>True if the session is still active. False if EndSession has
    /// already been called, or if the working process has been stopped
    /// and restarted.</returns>
    public static bool IsSessionActive( string strSessionId )
    {
      return IsSessionActive( typeof( CSessionControl ), strSessionId );
    }

    #endregion

    //*************************************************************************
    //* Constructor & generated code ******************************************
    //*************************************************************************

    #region Constructors

    // ------------------------------------------------------------------------
    /// <summary>
    /// This class is a facade --> forbid construction.
    /// </summary>
    private CSessionControl()
    {
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
    //* Operators *************************************************************
    //*************************************************************************

    #region Operators
    #endregion
  }
}
