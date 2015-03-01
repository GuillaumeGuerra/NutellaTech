//*****************************************************************************
//* Object Name: PropertyItemFactory    
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2006 - 2008
//*****************************************************************************
//* Project                 : GalaSoft.Utilities.Resources
//* Target                  : .NET Framework 2.0
//* Language/Compiler       : C#
//* Author                  : Laurent Bugnion (LBu), GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 03.10.2006
//*****************************************************************************
//* Description:
//* See the class definition here under.
//* Last base level: BL0001
//*****************************************************************************

//*****************************************************************************
//* Imports *******************************************************************
//*****************************************************************************

#region Imports

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities.Resources
{
  // Class definition *********************************************************
  /// <summary>
  /// High level API to handle <see cref="PropertyItem" />. For the moment,
  /// only PropertyTagImageDescription (0x010E) is handled.
  /// </summary>
  [ClassInfo( typeof( PropertyItemFactory ),
    DisplayName = "PropertyItem factory for Images",
    VersionString = "V00.01.00",
    DateString = "20061024075000",
    Description = "Creates and initializes PropertyItem objects to store metainformation in Images",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch" )]
  public abstract class PropertyItemFactory
  {
    //*************************************************************************
    //* Enums *****************************************************************
    //*************************************************************************

    #region Enums

    /// <summary>
    /// PropertyItem types. For more information, see
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagtypeconstants.asp">MSDN</a>.
    /// </summary>
    public enum Type : short
    {
      None = 0,
      Ascii = 2,
    }

    #endregion

    //*************************************************************************
    //* Constants *************************************************************
    //*************************************************************************

    #region Constants

    /// <summary>
    /// Path of the predefined image with the PropertyItem used to
    /// construct others.
    /// </summary>
    private const string PATH_PROPERTY_IMAGE = "Resources.images.item.gif";
    /// <summary>
    /// Tag used for Image Description.
    /// See <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdicpp/gdiplus/gdiplusreference/constants/imagepropertytagconstants/propertyitemdescriptions.asp">MSDN</a>
    /// <para>Value: 0x010E</para>
    /// </summary>
    public const int ID_PROPERTY_IMAGEDESCRIPTION = 0x010E;
    /// <summary>
    /// Tag used for EXIF user comment.
    /// See <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdicpp/gdiplus/gdiplusreference/constants/imagepropertytagconstants/propertyitemdescriptions.asp">MSDN</a>
    /// <para>Value: 0x9286</para>
    /// </summary>
    public const int ID_PROPERTY_EXIFUSERCOMMENT = 0x9286;

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
    /// Extracts the description field enclosed in the Image passed as parameter,
    /// corresponding to the tag <see cref="ID_PROPERTY_IMAGEDESCRIPTION" />.
    /// </summary>
    /// <param name="img">The Image containing the description to extract.</param>
    /// <returns>The extracted description. If nothing is found, or if an error
    /// occurs, returns an empty string.</returns>
    public static string GetImageDescription( Image img )
    {
      try
      {
        PropertyItem item = img.GetPropertyItem( ID_PROPERTY_IMAGEDESCRIPTION );
        if ( item != null
          && item.Value != null )
        {
          string description = ASCIIEncoding.ASCII.GetString( item.Value );
          if ( description.EndsWith( "\0" ) )
          {
            description = description.Substring( 0, description.Length - 1 );
          }
          return description;
        }
        return "";
      }
      catch ( Exception ex )
      {
        System.Diagnostics.Trace.WriteLine( ex.Message );
        return "";
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Creates a new PropertyItem object.
    /// </summary>
    /// <returns>A newly created PropertyItem object with its Id, Type and
    /// Len properties set to 0. Its Value property is set to an empty byte array.</returns>
    public static PropertyItem GetNewPropertyItem()
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream imageStream = assembly.GetManifestResourceStream( assembly.GetName().Name
        + "." + PATH_PROPERTY_IMAGE );

      if ( imageStream != null )
      {
        Image resource = Image.FromStream( imageStream );
        PropertyItem item = resource.GetPropertyItem( ID_PROPERTY_EXIFUSERCOMMENT );
        if ( item != null )
        {
          item.Id = 0;
          item.Type = 0;
          item.Len = 0;
          item.Value = new byte[] { };
          return item;
        }
      }
      return null;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Creates a new PropertyItem object according to the parameters.
    /// </summary>
    /// <param name="type">The PropertyItem type. See
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagtypeconstants.asp">MSDN</a>.
    /// </param>
    /// <param name="id">The PropertyItem's ID according to
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdicpp/gdiplus/gdiplusreference/constants/imagepropertytagconstants/propertyitemdescriptions.asp">this page</a>.
    /// </param>
    /// <returns>A new PropertyItem instance, set according to the parameters.</returns>
    public static PropertyItem GetNewPropertyItem( Type type, int id )
    {
      PropertyItem item = GetNewPropertyItem();
      item.Type = (short) type;
      item.Id = id;
      return item;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Get a new PropertyItem, with its ID set to <see cref="ID_PROPERTY_IMAGEDESCRIPTION" />.
    /// </summary>
    /// <param name="description">The description to write in the PropertyItem.</param>
    /// <returns>A new PropertyItem which can be used to set an Image description.</returns>
    public static PropertyItem GetNewImageDescription( string description )
    {
      PropertyItem item = GetNewStringProperty( Type.Ascii, description );
      item.Id = ID_PROPERTY_IMAGEDESCRIPTION;
      return item;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Creates a new PropertyItem for string properties.
    /// </summary>
    /// <param name="type">The PropertyItem's type.</param>
    /// <param name="property">The property to write.</param>
    /// <returns>A new PropertyItem set according to the parameters.</returns>
    private static PropertyItem GetNewStringProperty( Type type, string property )
    {
      PropertyItem item = GetNewPropertyItem( type, 0 );
      byte[] descriptionBytes = ASCIIEncoding.ASCII.GetBytes( property );
      item.Len = descriptionBytes.Length;
      item.Value = descriptionBytes;
      return item;
    }

    #endregion

    //*************************************************************************
    //* Constructor & generated code ******************************************
    //*************************************************************************

    #region Constructors

    /// <summary>
    /// Prevents instantiation.
    /// </summary>
    private PropertyItemFactory()
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
    //* Classes ***************************************************************
    //*************************************************************************

    #region Classes
    #endregion
  }
}
