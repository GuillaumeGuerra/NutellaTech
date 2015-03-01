//*****************************************************************************
//* Object Name: ClassInfoAttribute
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
//* Last base level: BL0007
//*****************************************************************************

//*****************************************************************************
//* Imports *******************************************************************
//*****************************************************************************

#region Imports

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Globalization;

#endregion

namespace GalaSoft.Utilities.Attributes
{
  // Class definition *********************************************************
  /// <summary>
  /// Allows to define metainformation about a class, much like the AssemblyInfo.
  /// <example>To use this, add an attribute before a class definition:
  /// <code>
  /// [ClassInfo( typeof( ClassName ),
  ///    VersionString = "V01.00",
  ///    Description = "Description for this class" )]
  /// public class ClassName...</code></example>
  /// <para>Note: Only the "owner type" is mandatory, additional information
  /// is optional.</para>
  /// <para>By setting the <see cref="DefaultNamespaceType" /> property, you
  /// define a type containing default information. For example, if you
  /// create a control using other accessory classes, you can define metainformation
  /// for this control (eg version, description...), and refer all other accessory
  /// classes to the default class by using this property. If the default type is defined,
  /// and a property is undefined, then the default type's metainformation will be
  /// returned.</para>
  /// <para>To get the ClassInfo corresponding to a given type,
  /// use <see cref="GetClassInfo(Type)" /></para>
  /// <para>Note: <see cref="GetClassInfo(Assembly)" /> also works with Assemblies, and returns
  /// an instance of ClassInfoAttribute filled with the AssemblyInfo. This allows
  /// handling Assemblies and Types in a similar way.</para>
  /// <para>To get the metainformation in an easier way, use
  /// <see cref="Utilities.GetProductInfo(Type,Utilities.ProductInfo,bool)" /></para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
  [ClassInfo(typeof(ClassInfoAttribute),
    DisplayName = "Class Info Attribute",
    VersionString = "V1.1.5",
    DateString = "200902011436",
    Description = "Allows to define metainformation about a class, much like the AssemblyInfo.",
    UrlContacts = "http://www.galasoft.ch/contact_en.html",
    Email = "laurent@galasoft.ch")]
  public class ClassInfoAttribute : Attribute
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
    /// The default type for this class. If defined, and if a metainformation is missing,
    /// the default attribute will be searched in order to return the corresponding information.
    /// </summary>
    private Type _default = null;
    /// <summary>
    /// The ClassInfoAttribute corresponding to the default type.
    /// </summary>
    private ClassInfoAttribute _defaultTypeAttribute = null;
    /// <summary>
    /// The ClassInfoAttribute corresponding to the assembly.
    /// </summary>
    private ClassInfoAttribute _assemblyAttribute = null;

    /// <summary>
    /// This type's full name (including full namespace).
    /// </summary>
    private string _name = "";
    /// <summary>
    /// This type's display name (for example for About forms).
    /// </summary>
    private string _displayName = "";
    /// <summary>
    /// If true, a ClassInfoAttribute was added to the given type. If false, nothing was defined.
    /// </summary>
    private bool _exists = true;

    /// <summary>
    /// The class' version.
    /// </summary>
    private VersionEx _version = null;
    /// <summary>
    /// The class' date.
    /// </summary>
    private DateTime _date = DateTime.MinValue;
    /// <summary>
    /// The class' path.
    /// <para>Corresponds to the assembly's path.</para>
    /// </summary>
    private string _path = "";
    /// <summary>
    /// The class' description.
    /// </summary>
    private string _description = "";
    /// <summary>
    /// Legal copyright.
    /// </summary>
    private string _copyright = "";
    /// <summary>
    /// Legal trademark.
    /// </summary>
    private string _trademark = "";
    /// <summary>
    /// The company having created this class.
    /// </summary>
    private string _company = "";
    /// <summary>
    /// The company's website.
    /// </summary>
    private string _url = "";
    /// <summary>
    /// The company's "contacts" URL.
    /// </summary>
    private string _urlContacts = "";
    /// <summary>
    /// The company's support email.
    /// </summary>
    private string _email = "";
    /// <summary>
    /// The company's product URL.
    /// </summary>
    private string _urlProduct = "";

    #endregion

    //*************************************************************************
    //* Properties ************************************************************
    //*************************************************************************

    #region Properties

    // ------------------------------------------------------------------------
    /// <summary>
    /// Returns true if the metainformation were entered for the corresponding type.
    /// Returns false if nothing was entered.
    /// </summary>
    public bool Exists
    {
      get { return _exists; }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the corresponding type's full name.
    /// </summary>
    public string Name
    {
      get
      {
        if (_displayName.Length == 0)
        {
          // Construct _defaultAttribute using property, however
          // further access is done usng attribute in order to
          // optimize speed.
          if (DefaultTypeAttribute != null
            && _defaultTypeAttribute.Name != null)
          {
            return _defaultTypeAttribute.Name;
          }
          if (AssemblyAttribute != null
            && _assemblyAttribute.Name != null)
          {
            return _assemblyAttribute.Name;
          }
          return "";
        }
        return _name;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the corresponding type's display name.
    /// <para>The display name can be used, for example, for About forms.</para>
    /// <para>If the display name was not set, this property returns the same
    /// value as <see cref="Name" /></para>
    /// </summary>
    public string DisplayName
    {
      get
      {
        if (_displayName.Length == 0)
        {
          // Construct _defaultAttribute using property, however
          // further access is done usng attribute in order to
          // optimize speed.
          if (DefaultTypeAttribute != null
            && _defaultTypeAttribute.DisplayName != null)
          {
            return _defaultTypeAttribute.DisplayName;
          }
          if (AssemblyAttribute != null
            && _assemblyAttribute.DisplayName != null)
          {
            return _assemblyAttribute.DisplayName;
          }
          return Name;
        }
        return _displayName;
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _displayName = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the corresponding type's version.
    /// </summary>
    public VersionEx Version
    {
      get
      {
        if (_version != null)
        {
          return _version;
        }
        // Construct _defaultAttribute using property, however
        // further access is done usng attribute in order to
        // optimize speed.
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Version != null)
        {
          return _defaultTypeAttribute.Version;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Version != null)
        {
          return _assemblyAttribute.Version;
        }
        return null;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the default type for this class. If defined, and if a
    /// metainformation is missing, the default attribute will be searched in order
    /// to return the corresponding information.
    /// </summary>
    public Type DefaultNamespaceType
    {
      get { return _default; }
      set
      {
        _default = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the class' version as a string.
    /// <para>Supports the following formats: [V][xx.yy[.zz[.rr]]][/Ddddd[++]]</para>
    /// </summary>
    public string VersionString
    {
      get
      {
        if (_version != null)
        {
          return _version.Full;
        }
        // Construct _defaultAttribute using property, however
        // further access is done usng attribute in order to
        // optimize speed.
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Version != null)
        {
          return _defaultTypeAttribute.Version.Full;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Version != null)
        {
          return _assemblyAttribute.Version.Full;
        }
        return "";
      }
      set
      {
        _version = new VersionEx(value);
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the class' description.
    /// </summary>
    public string Description
    {
      get
      {
        if (_description.Length > 0)
        {
          return _description;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Description.Length > 0)
        {
          return _defaultTypeAttribute.Description;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Description.Length > 0)
        {
          return _assemblyAttribute.Description;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _description = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets of gets the class' date as a string.
    /// </summary>
    /// <exception cref="FormatException">Thrown if the string is not recognized as a
    /// valid date.</exception>
    public string DateString
    {
      get
      {
        if (_date != DateTime.MinValue)
        {
          return _date.ToString();
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute._date != DateTime.MinValue)
        {
          return _defaultTypeAttribute.DateString;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute._date != DateTime.MinValue)
        {
          return _assemblyAttribute.DateString;
        }
        return "";
      }
      set
      {
        if (value == null
          || value.Length == 0)
        {
          _date = DateTime.MinValue;
        }
        else
        {
          _date = Utility.GetDateTimeFromSortableString(value);
        }
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets the class' path.
    /// <para>Corresponds to the assembly's path.</para>
    /// </summary>
    public string Path
    {
      get
      {
#if SILVERLIGHT
        throw new NotSupportedException("Not supported in Silverlight");
#else
        if (_path.Length == 0)
        {
          // Construct _defaultAttribute using property, however
          // further access is done usng attribute in order to
          // optimize speed.
          if (DefaultTypeAttribute != null
            && _defaultTypeAttribute.Path != null)
          {
            return _defaultTypeAttribute.Path;
          }
          if (AssemblyAttribute != null
            && _assemblyAttribute.Path != null)
          {
            return _assemblyAttribute.Path;
          }
        }
        return _path;
#endif
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the legal copyright.
    /// </summary>
    public string Copyright
    {
      get
      {
        if (_copyright.Length > 0)
        {
          return _copyright;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Copyright.Length > 0)
        {
          return _defaultTypeAttribute.Copyright;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Copyright.Length > 0)
        {
          return _assemblyAttribute.Copyright;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _copyright = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the legal trademark.
    /// </summary>
    public string Trademark
    {
      get
      {
        if (_trademark.Length > 0)
        {
          return _trademark;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Trademark.Length > 0)
        {
          return _defaultTypeAttribute.Trademark;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Trademark.Length > 0)
        {
          return _assemblyAttribute.Trademark;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _trademark = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the company having created this class.
    /// </summary>
    public string Company
    {
      get
      {
        if (_company.Length > 0)
        {
          return _company;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Company.Length > 0)
        {
          return _defaultTypeAttribute.Company;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Company.Length > 0)
        {
          return _assemblyAttribute.Company;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _company = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the company's website.
    /// </summary>
    public string Url
    {
      get
      {
        if (_url.Length > 0)
        {
          return _url;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Url.Length > 0)
        {
          return _defaultTypeAttribute.Url;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Url.Length > 0)
        {
          return _assemblyAttribute.Url;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _url = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the company's contacts webpage.
    /// </summary>
    public string UrlContacts
    {
      get
      {
        if (_urlContacts.Length > 0)
        {
          return _urlContacts;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.UrlContacts.Length > 0)
        {
          return _defaultTypeAttribute.UrlContacts;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.UrlContacts.Length > 0)
        {
          return _assemblyAttribute.UrlContacts;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _urlContacts = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the company's support email.
    /// </summary>
    public string Email
    {
      get
      {
        if (_email.Length > 0)
        {
          return _email;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.Email.Length > 0)
        {
          return _defaultTypeAttribute.Email;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.Email.Length > 0)
        {
          return _assemblyAttribute.Email;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _email = value;
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Sets or gets the company's product webpage.
    /// </summary>
    public string UrlProduct
    {
      get
      {
        if (_urlProduct.Length > 0)
        {
          return _urlProduct;
        }
        if (DefaultTypeAttribute != null
          && _defaultTypeAttribute.UrlProduct.Length > 0)
        {
          return _defaultTypeAttribute.UrlProduct;
        }
        if (AssemblyAttribute != null
          && _assemblyAttribute.UrlProduct.Length > 0)
        {
          return _assemblyAttribute.UrlProduct;
        }
        return "";
      }
      set
      {
        if (value == null)
        {
          value = "";
        }
        _urlProduct = value;
      }
    }

    // ------------------------------------------------------------------------
    private ClassInfoAttribute DefaultTypeAttribute
    {
      get
      {
        if (_default != null
          && _defaultTypeAttribute == null)
        {
          _defaultTypeAttribute = GetClassInfo(_default, false);
        }
        return _defaultTypeAttribute;
      }
    }

    // ------------------------------------------------------------------------
    private ClassInfoAttribute AssemblyAttribute
    {
      get
      {
        if (_default != null
          && _assemblyAttribute == null)
        {
          _assemblyAttribute = GetClassInfo(_default.Assembly);
        }
        return _assemblyAttribute;
      }
    }

    #endregion

    //*************************************************************************
    //* Static methods ********************************************************
    //*************************************************************************

    #region Static methods

    public static ClassInfoAttribute GetClassInfo()
    {
      return new ClassInfoAttribute();
    }


    // ------------------------------------------------------------------------
    /// <summary>
    /// This method never returns null, but might return a ClassInfo with
    /// <see cref="Exists" /> == false.
    /// </summary>
    /// <param name="type">The type for which the ClassInfo must be returned.</param>
    /// <param name="useAssemblyInfo">If true, the given type's Assembly will be used
    /// for missing parameters.</param>
    /// <returns>The ClassInfo corresponding to the type.</returns>
    public static ClassInfoAttribute GetClassInfo(Type type, bool useAssemblyInfo)
    {
      if (type == null)
      {
        return new ClassInfoAttribute();
      }

      ClassInfoAttribute[] classInfos
        = (ClassInfoAttribute[]) type.GetCustomAttributes(typeof(ClassInfoAttribute),
        false);

      if (classInfos.Length == 0)
      {
        ClassInfoAttribute result = new ClassInfoAttribute();
        if (useAssemblyInfo)
        {
          result._assemblyAttribute = GetClassInfo(type.Assembly);
        }
        return result;
      }
      else
      {
        if (classInfos.Length > 1)
        {
          throw new Exception("Too many ClassInfoAttribute found in type " + type.FullName);
        }
        else
        {
          if (useAssemblyInfo)
          {
            classInfos[0]._assemblyAttribute = GetClassInfo(type.Assembly);
          }
          return classInfos[0];
        }
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Gets a ClassInfo corresponding to the assembly.
    /// </summary>
    /// <param name="assembly">The Assembly from which the metainformation must be read.</param>
    /// <returns>A ClassInfo filled with the metainformation taken from the Assembly.</returns>
    public static ClassInfoAttribute GetClassInfo(Assembly assembly)
    {
      if (assembly == null)
      {
        return new ClassInfoAttribute();
      }

      return new ClassInfoAttribute(assembly);
    }

    #endregion

    //*************************************************************************
    //* Constructor & generated code ******************************************
    //*************************************************************************

    #region Constructors

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a ClassInfo with default values. Used when the metainformation
    /// for a given type cannot be found.
    /// </summary>
    private ClassInfoAttribute()
    {
      _exists = false;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a ClassInfo with all values.
    /// </summary>
    /// <param name="defaultNamespaceType">The default type for this class. If defined, and if a metainformation is missing,
    /// the default attribute will be searched in order to return the corresponding information.</param>
    /// <param name="displayName">The class' display name, for example for About forms.</param>
    /// <param name="version">The class' version.</param>
    /// <param name="date">The class' date.</param>
    /// <param name="description">The class' description.</param>
    /// <param name="copyright">Legal copyright.</param>
    /// <param name="trademark">Legal trademark.</param>
    /// <param name="company">The company having created this class.</param>
    /// <param name="url">The company's website.</param>
    /// <param name="urlContacts">The company's contacts webpage.</param>
    /// <param name="email">The company's support email.</param>
    /// <param name="urlProduct">The company's product webpage.</param>
    public ClassInfoAttribute(Type defaultNamespaceType,
      string displayName,
      string version,
      string date,
      string description,
      string copyright,
      string trademark,
      string company,
      string url,
      string urlContacts,
      string email,
      string urlProduct)
    {
      this.DefaultNamespaceType = defaultNamespaceType;
      this.DisplayName = displayName;
      this.VersionString = version;
      this.DateString = date;
      this.Description = description;
      this.Copyright = copyright;
      this.Trademark = trademark;
      this.Company = company;
      this.Url = url;
      this.UrlContacts = urlContacts;
      this.Email = email;
      this.UrlProduct = urlProduct;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a ClassInfo corresponding to a type.
    /// </summary>
    /// <param name="ownerType">The type which this ClassInfo describes.</param>
    public ClassInfoAttribute(Type ownerType)
    {
      _name = ownerType.FullName;

#if !SILVERLIGHT
      _path = ownerType.Assembly.Location.Substring(0,
        ownerType.Assembly.Location.LastIndexOf("\\") + 1);
#endif
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Constructs a ClassInfo corresponding to an assembly.
    /// <para>Note: Normally, ClassInfo correspond to types, but using them
    /// to describe Assemblies allow a uniform handling.</para>
    /// </summary>
    /// <param name="assembly">The assembly which this ClassInfo describes.</param>
    public ClassInfoAttribute(Assembly assembly)
    {
#if SILVERLIGHT

      // UNSUPPORTED IN SILVERLIGHT
      throw new NotSupportedException("This feature is not supported in the Silverlight version");

#else
      
      FileVersionInfo fileVersionInfo
        = FileVersionInfo.GetVersionInfo(assembly.Location);

      _name = fileVersionInfo.InternalName;
      _displayName = fileVersionInfo.ProductName;
      _description = fileVersionInfo.Comments;
      _date = File.GetLastWriteTime(assembly.Location);
      _path = assembly.Location.Substring(0,
        assembly.Location.LastIndexOf("\\") + 1);
      _copyright = fileVersionInfo.LegalCopyright;
      _trademark = fileVersionInfo.LegalTrademarks;

      string companyName = fileVersionInfo.CompanyName;
      string[] companyInfos = companyName.Split(new char[] { '@' });
      if (companyInfos.Length > 0)
      {
        _company = companyInfos[0].Trim();
        if (companyInfos.Length > 1)
        {
          _url = companyInfos[1].Trim();
        }
      }
      else
      {
        _company = companyName;
        _url = "";
      }

      _urlContacts = "";
      _email = "";

      string debug;
#if DEBUG
      string productVersion = fileVersionInfo.ProductVersion;
      if (productVersion.IndexOf("BL") != -1)
      {
        debug = productVersion.Substring(productVersion.LastIndexOf("BL"));
      }
      else
      {
        debug = "";
      }
#else
      debug = "";
#endif

      _version = new VersionEx(fileVersionInfo.FileMajorPart,
        fileVersionInfo.FileMinorPart,
        fileVersionInfo.FileBuildPart,
        fileVersionInfo.FilePrivatePart,
        debug);

#endif
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

  }
}
