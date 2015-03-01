//*****************************************************************************
//* Object Name: ResourceFactory
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2006 - 2008
//*****************************************************************************
//* Project                 : GalaSoft.Utilities.Resources
//* Target                  : .NET Framework 2.0
//* Language/Compiler       : C#
//* Author                  : Laurent Bugnion (LBu), GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 05.01.2006
//*****************************************************************************
//* Description:
//* See the class definition here under.
//* Last base level: BL0007.
//*****************************************************************************

//*****************************************************************************
//* Imports *******************************************************************
//*****************************************************************************

#region Imports

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;

using GalaSoft.Utilities.Attributes;

#endregion

namespace GalaSoft.Utilities.Resources
{
  // Class definition *********************************************************
  /// <summary>
  /// Allows easy access to various Resource related functions.
  /// </summary>
  [ClassInfo(typeof(ResourceFactory),
      DisplayName = "Resource factory",
      VersionString = "V01.05.01",
      DateString = "20061214160000",
      Description = "Allows easy access to various Resource related functions.",
      UrlContacts = "http://www.galasoft.ch/contact_en.html",
      Email = "laurent@galasoft.ch")]
  public static class ResourceFactory
  {
    //*************************************************************************
    //* Enums *****************************************************************
    //*************************************************************************

    #region Enums

    // ------------------------------------------------------------------------
    /// <summary>
    /// Embedded file format.
    /// </summary>
    private enum FileFormat : int
    {
      /// <summary>
      /// Unknown type.
      /// </summary>
      Unknown = 0,
      /// <summary>
      /// Icon.
      /// </summary>
      Icon = 1,
      /// <summary>
      /// Image with EXIF embedded information.
      /// </summary>
      ImageExif = 2,
      /// <summary>
      /// Image without EXIF embedded information.
      /// </summary>
      ImageNonExif = 3,
      /// <summary>
      /// Text.
      /// </summary>
      Text = 4,
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Defines how the embedded resources should be extracted.
    /// </summary>
    public enum ExtractMode : int
    {
      /// <summary>
      /// No exception will be thrown if the versions differ.
      /// </summary>
      NoThrow = 0,
      /// <summary>
      /// An <see cref="VersionException" /> will be thrown if an existing file
      /// is found, and its version is older than the embedded file's version.
      /// </summary>
      ThrowIfOlder = 1,
      /// <summary>
      /// An <see cref="VersionException" /> will be thrown if an existing file
      /// is found, and its version is newer than the embedded file's version.
      /// </summary>
      ThrowIfNewer = 2,
      /// <summary>
      /// An <see cref="VersionException" /> will be thrown if an existing file
      /// is found, and its version is older or newer than the embedded file's version.
      /// </summary>
      ThrowIfOlderOrNewer = 3,
    }

    #endregion

    //*************************************************************************
    //* Constants *************************************************************
    //*************************************************************************

    #region Constants

    /// <summary>
    /// Tag used to enter a version information (XML) into text files extracted
    /// from the resources.
    /// </summary>
    public const string TAG_VERSION = "gslb_version";
    /// <summary>
    /// Attribute used to enter a version information (XML) into text files extracted
    /// from the resources.
    /// </summary>
    public const string ATTR_VERSION = "version";

    #endregion

    //*************************************************************************
    //* Static attributes *****************************************************
    //*************************************************************************

    #region StaticAttributes

    /// <summary>
    /// Holds the ResourceManager instances already created.
    /// </summary>
    private static Dictionary<Type, ResourceManager> s_resourceCache;

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

    #region Resources strings and managers

    // ------------------------------------------------------------------------
    /// <summary>
    /// Returns (after creating if needed) a ResourceManager corresponding
    /// to the given type. The managers are cached.
    /// </summary>
    /// <param name="resourceType">The Type for which the ResourceManager must
    /// be returned.</param>
    /// <returns>A ResourceManager corresponding to the given type.</returns>
    public static ResourceManager GetResourceManager(Type resourceType)
    {
      return GetResourceManager(resourceType, true);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Returns (after creating if needed) a ResourceManager corresponding
    /// to the given type. Depending on the parameter cache, the managers are
    /// cached or not.
    /// </summary>
    /// <param name="resourceType">The Type for which the ResourceManager must
    /// be returned.</param>
    /// <param name="cache">If true, the managers are cached. In that case,
    /// use <see cref="CleanUp" /> to delete the cached instances.</param>
    /// <returns>A ResourceManager corresponding to the given type.</returns>
    public static ResourceManager GetResourceManager(Type resourceType, bool cache)
    {
      if (s_resourceCache == null)
      {
        s_resourceCache = new Dictionary<Type, ResourceManager>();
      }

      if (!s_resourceCache.ContainsKey(resourceType))
      {
        s_resourceCache.Add(resourceType, new ResourceManager(resourceType));
      }

      return s_resourceCache[resourceType];
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Returns a string extracted from the resource file.
    /// </summary>
    /// <param name="resourceType">The <see cref="Type" /> from which the class
    /// derives all information</param>
    /// <param name="name">The name of the string in the resource.</param>
    /// <returns>The extracted string.</returns>
    public static string GetString(Type resourceType, string name)
    {
      return GetResourceManager(resourceType).GetString(name);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Cleans up cached resources. This method must be called to free the resources
    /// if <see cref="GetResourceManager(Type)" /> or <see cref="GetResourceManager(Type,bool)" /> 
    /// with the second parameter set to "true" was called.
    /// </summary>
    /// <param name="resourceType">The <see cref="Type" /> from which the class
    /// derives all information</param>
    public static void CleanUp(Type resourceType)
    {
      if (s_resourceCache == null)
      {
        return;
      }

      if (s_resourceCache.ContainsKey(resourceType))
      {
        s_resourceCache[resourceType].ReleaseAllResources();
        s_resourceCache.Remove(resourceType);
      }

      if (s_resourceCache.Count == 0)
      {
        s_resourceCache = null;
      }
    }

    #endregion

    #region Embedded resource files

    // ------------------------------------------------------------------------
    // TODO Test with strDirectoryInResource = ""
    /// <summary>
    /// Check the Assembly in which the <see cref="Type" /> resourceType is included, and
    /// returns a list of all the files embedded in the Assembly under the same namespace.
    /// </summary>
    /// <param name="resourceType">The Type defining the namespace for which the
    /// embedded file list must be returned.</param>
    /// <param name="directoryInResource">The directory in which the files
    /// are placed in the Resources assembly.</param>
    /// <returns>An array of <see cref="ResourceFileInfo" />.</returns>
    public static CResourceFileInfo[] GetFiles(Type resourceType,
      string directoryInResource)
    {
      Assembly assembly = Assembly.GetAssembly(resourceType);
      string[] resourceNames = assembly.GetManifestResourceNames();
      List<CResourceFileInfo> foundNames = new List<CResourceFileInfo>();
      foreach (string fullName in resourceNames)
      {
        string parentInResource
          = Path.Combine(resourceType.Namespace, directoryInResource).Replace("/", ".").Replace("\\", ".")
          + "."; // TODO Put this out of the loop

        if (fullName.StartsWith(parentInResource))
        {
          foundNames.Add(new CResourceFileInfo(resourceType, directoryInResource, fullName));
        }
      }
      return foundNames.ToArray();
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Extracts a resource file embedded in an Assembly. The embedded file is handled as
    /// a binary file.
    /// <param>For text files, use
    /// <see cref="ExtractFile(Type,string,bool,string,string,ExtractMode)" /> instead.</param>
    /// </summary>
    /// <param name="resourceType">The <see cref="Type" /> from which the class
    /// derives all information</param>
    /// <param name="pathInResource">The path in the resource file, which may be
    /// relative (eg "images.myFile.bmp") or absolute
    /// (eg "GalaSoft.MyType.images.myFile.bmp").</param>
    /// <param name="fullPath">The full path under which the file must be extracted
    /// (including the file name).</param>
    /// <param name="mode">Specifies if Exceptions should be thrown in case
    /// a newer or older file already exists in the same target directory.</param>
    /// <returns>A <see cref="FileInfo" /> instance pointing to the target file.</returns>
    /// <remarks>The following table describes in which case an embedded file will
    /// be extracted and versioned.
    /// <code>
    /// [1]   [2]   [3]   [4] | [5]               [6]               [7]
    /// no    no    no    n/a | no                yes  
    /// yes   no    no    no  | no                no  
    /// yes   no    no    yes | no                yes               1)
    /// no    no    yes   n/a | yes               yes  
    /// yes   no    yes   no  | yes               yes  
    /// yes   no    yes   yes | If VFile&lt;VType If VFile&lt;VType 4A)4B)
    /// no    yes   no    n/a | no                yes  
    /// yes   yes   no    no  | no                yes               2)
    /// yes   yes   no    yes | no                If VFile&lt;VRes  3A)3B)
    /// no    yes   yes   n/a | no                yes  
    /// yes   yes   yes   no  | no                yes  
    /// yes   yes   yes   yes | no                If VFile&lt;VRes  3A)3B)
    /// 
    /// [1] = Target file exists
    /// [2] = Image in resource is versioned
    /// [3] = Type is versioned
    /// [4] = Target file is versioned
    /// [5] = File must be versioned
    /// [6] = File must be extracted
    /// [7] = Remarks
    /// VFile = Version in Target file
    /// VType = Version in Type
    /// VRes = Version in embedded file
    /// 
    /// Remarks:
    /// 1) This case exists only if the type or the image in the resource was versioned before, but then not anymore.
    /// 2) This case exists only if the image in the resource has been versioned since the last time that the control was run.
    /// 3A) If VFile &gt; VRes, throw Exception if eMode == eThrowIfNewer or eThrowIfOlderOrNewer
    /// 3B) If VFile &lt; VRes, throw Exception if eMode == eThrowIfOlder or eThrowIfOlderOrNewer
    /// 4A) If VFile &gt; VType, throw Exception if eMode == eThrowIfNewer or eThrowIfOlderOrNewer
    /// 4B) If VFile &lt; VType, throw Exception if eMode == eThrowIfOlder or eThrowIfOlderOrNewer
    /// </code>
    /// </remarks>
    public static FileInfo ExtractFile(Type resourceType,
      string pathInResource,
      string fullPath,
      ExtractMode mode)
    {
      return ExtractFile(resourceType, pathInResource, false, fullPath, null, mode);
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Extracts and versions a resource file embedded in an Assembly. See the table
    /// listed under "Remarks" for details.
    /// </summary>
    /// <param name="resourceType">The <see cref="Type" /> from which the class
    /// derives all information</param>
    /// <param name="pathInResource">The path in the resource file, which may be
    /// relative (eg "images.myFile.bmp") or absolute
    /// (eg "GalaSoft.MyType.images.myFile.bmp").</param>
    /// <param name="fileIsText">If true, the resource file is handled as a text file.</param>
    /// <param name="fullPath">The full path under which the file must be extracted
    /// (including the file name).</param>
    /// <param name="textVersionPattern">For text files, an XML version node
    /// is added on top of the file. If a special form must be used, this parameter
    /// allows to specifiy it.
    /// <example>/* {0} */ produces an XML version node formatted as follows:
    /// /* &lt; gslb_version version="V1.2.3" &gt; */</example>
    /// </param>
    /// <param name="mode">Specifies if Exceptions should be thrown in case
    /// a newer or older file already exists in the same target directory.</param>
    /// <returns>A <see cref="FileInfo" /> instance pointing to the target file.</returns>
    /// <remarks>The following table describes in which case an embedded file will
    /// be extracted and versioned.
    /// <code>
    /// [1]   [2]   [3]   [4] | [5]               [6]               [7]
    /// no    no    no    n/a | no                yes  
    /// yes   no    no    no  | no                no  
    /// yes   no    no    yes | no                yes               1)
    /// no    no    yes   n/a | yes               yes  
    /// yes   no    yes   no  | yes               yes  
    /// yes   no    yes   yes | If VFile&lt;VType If VFile&lt;VType 4A)4B)
    /// no    yes   no    n/a | no                yes  
    /// yes   yes   no    no  | no                yes               2)
    /// yes   yes   no    yes | no                If VFile&lt;VRes  3A)3B)
    /// no    yes   yes   n/a | no                yes  
    /// yes   yes   yes   no  | no                yes  
    /// yes   yes   yes   yes | no                If VFile&lt;VRes  3A)3B)
    /// 
    /// [1] = Target file exists
    /// [2] = Image in resource is versioned
    /// [3] = Type is versioned
    /// [4] = Target file is versioned
    /// [5] = File must be versioned
    /// [6] = File must be extracted
    /// [7] = Remarks
    /// VFile = Version in Target file
    /// VType = Version in Type
    /// VRes = Version in embedded file
    /// 
    /// Remarks:
    /// 1) This case exists only if the type or the image in the resource was versioned before, but then not anymore.
    /// 2) This case exists only if the image in the resource has been versioned since the last time that the control was run.
    /// 3A) If VFile &gt; VRes, throw Exception if eMode == eThrowIfNewer or eThrowIfOlderOrNewer
    /// 3B) If VFile &lt; VRes, throw Exception if eMode == eThrowIfOlder or eThrowIfOlderOrNewer
    /// 4A) If VFile &gt; VType, throw Exception if eMode == eThrowIfNewer or eThrowIfOlderOrNewer
    /// 4B) If VFile &lt; VType, throw Exception if eMode == eThrowIfOlder or eThrowIfOlderOrNewer
    /// </code>
    /// </remarks>
    public static FileInfo ExtractFile(Type resourceType,
      string pathInResource,
      bool fileIsText,
      string fullPath,
      string textVersionPattern,
      ExtractMode mode)
    {
      // http://www.codeproject.com/dotnet/Extracting_Embedded_Image.asp

      //Console.WriteLine( "** Extracting " + strFullPath );

      FileInfo targetFile = new FileInfo(fullPath);
      if (!targetFile.Directory.Exists)
      {
        targetFile.Directory.Create();
      }

      if (!pathInResource.StartsWith(resourceType.Namespace))
      {
        pathInResource = resourceType.Namespace + "." + pathInResource;
      }
      pathInResource = pathInResource.Replace("/", ".").Replace("\\", ".");

      Assembly assembly = Assembly.GetAssembly(resourceType);

      Stream inputStream = assembly.GetManifestResourceStream(pathInResource);
      if (inputStream == null)
      {
        // No such file in resources
        //Console.WriteLine( "** No such file" );
        return null;
      }

      FileInfo resultFile = null;
      FileFormat format = FileFormat.Unknown;
      VersionEx versionInType = ClassInfoAttribute.GetClassInfo(resourceType, false).Version;

      //Console.WriteLine( "Version in Type: " + ( ( oVersionInType == null ) ? "null" : oVersionInType.strFull ) );

      // Try a text file ------------------------------------------------------
      if (fileIsText)
      {
        // According to user, it's a text file --> save as such.
        // Note: It is not possible to determine programatically if a file is
        // binary or text.

        //Console.WriteLine( "** File is text" );

        // Text files are versioned by including an XML fragment in
        // /**/ comments on top of the file
        StreamReader reader = null;
        StreamWriter writer = null;
        try
        {
          format = FileFormat.Text;
          resultFile = new FileInfo(fullPath);

          VersionEx versionInResource = GetVersion(ref inputStream);
          //Console.WriteLine( "Version in Resource: " + ( ( oVersionInResource == null ) ? "null" : oVersionInResource.strFull ) );

          bool version = false;
          bool extract = false;
          DecideAction(resultFile,
            versionInResource,
            versionInType,
            format,
            mode,
            out version,
            out extract);

          //Console.WriteLine( "** bVersion = " + bVersion.ToString() );
          //Console.WriteLine( "** bExtract = " + bExtract.ToString() );

          if (extract)
          {
            writer = new StreamWriter(fullPath);
            inputStream.Position = 0;
            reader = new StreamReader(inputStream);

            if (version)
            {
              XmlDocument doc = new XmlDocument();
              XmlElement versionElement = doc.CreateElement(TAG_VERSION);
              versionElement.SetAttribute(ATTR_VERSION, versionInType.Full);
              if (textVersionPattern != null
                && textVersionPattern.IndexOf("{0}") > -1)
              {
                writer.WriteLine(string.Format(textVersionPattern, versionElement.OuterXml));
              }
              else
              {
                writer.WriteLine(versionElement.OuterXml);
              }

              writer.WriteLine();
            }

            string line;
            while ((line = reader.ReadLine()) != null)
            {
              writer.WriteLine(line);
            }
          }

          return resultFile;
        }
        catch (Exception)
        {
          throw;
        }
        finally
        {
          if (reader != null)
          {
            reader.Close();
          }
          if (writer != null)
          {
            writer.Close();
          }
        }
      }

      // Try an icon ----------------------------------------------------------
      try
      {
        format = FileFormat.Icon;
        VersionInXmlFile(resultFile);
        Icon icon = new Icon(inputStream);

        //Console.WriteLine( "** File is Icon" );

        icon.Save(inputStream);
        resultFile = new FileInfo(fullPath);
        return resultFile;
      }
      catch (Exception)
      {
        format = FileFormat.Unknown;
        inputStream.Position = 0;
      }

      // Try an image ---------------------------------------------------------
      // Images are versioned in meta information (EXIF) if available,
      // or through a XML file.
      Image resource = null;

      try
      {
        format = FileFormat.ImageExif;
        resultFile = new FileInfo(fullPath);
        resource = Image.FromStream(inputStream);
        VersionEx versionInResource = GetVersion(resource);
        //Console.WriteLine( "Version in Resource: " + ( ( oVersionInResource == null ) ? "null" : oVersionInResource.strFull ) );
        //Console.WriteLine( "** File is Image" );

        bool version = false;
        bool extract = false;
        DecideAction(resultFile,
          versionInResource,
          versionInType,
          format,
          mode,
          out version,
          out extract);

        if (version)
        {
          // Version file
          string existingDescription = PropertyItemFactory.GetImageDescription(resource);
          existingDescription = versionInType.Full
            + (existingDescription.Length > 0 ? "\r\n" : "")
            + existingDescription;

          PropertyItem descriptionItem = PropertyItemFactory.GetNewImageDescription(existingDescription);

          try
          {
            resource.SetPropertyItem(descriptionItem);
          }
          catch (ArgumentException)
          {
            // It's an image file, but the format doesn't support EXIF
            format = FileFormat.ImageNonExif;

            //Console.WriteLine( "** Impossible to version (non-EXIF)" );

            VersionInXmlFile(resultFile);

            // TEMPO Extract only if file was not extracted already
            extract = !resultFile.Exists;
            version = false;
          }
        }

        if (extract)
        {
          resource.Save(fullPath);
        }

        //Console.WriteLine( "** bVersion = " + bVersion.ToString() );
        //Console.WriteLine( "** bExtract = " + bExtract.ToString() );
        return resultFile;
      }
      catch (VersionException)
      {
        throw;
      }
      catch (Exception)
      {
        format = FileFormat.Unknown;
        inputStream.Position = 0;
      }
      finally
      {
        if (resource != null)
        {
          resource.Dispose();
        }
      }

      // No other type was successful --> Simply extract resource using binary writer
      BinaryWriter binaryWriter = null;
      BinaryReader binaryReader = null;
      try
      {
        //Console.WriteLine( "** Extracting binary" );

        Stream outputStream = File.Create(fullPath);
        binaryWriter = new BinaryWriter(outputStream);
        binaryReader = new BinaryReader(inputStream);

        byte[] bytes = new byte[1000];
        int count = 0;

        while ((count = binaryReader.Read(bytes, 0, 1000)) > 0)
        {
          binaryWriter.Write(bytes, 0, count);
        }

        VersionInXmlFile(resultFile);
        return resultFile;
      }
      catch (Exception)
      {
        //Console.WriteLine( "** Exception thrown" );
      }
      finally
      {
        if (binaryWriter != null)
        {
          binaryWriter.Close();
        }
        if (binaryReader != null)
        {
          binaryReader.Close();
        }
      }

      return null;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Adds version information to an external XML file located in the target
    /// directory.
    /// </summary>
    /// <param name="outputFile">The target extracted file.</param>
    /// <remarks>Not implemented yet</remarks>
    private static void VersionInXmlFile(FileInfo outputFile)
    {
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Computes which action must be executed (extract file, version file)
    /// according to the following table:
    /// <code>
    /// [1]   [2]   [3]   [4] | [5]               [6]               [7]
    /// no    no    no    n/a | no                yes  
    /// yes   no    no    no  | no                no  
    /// yes   no    no    yes | no                yes               1)
    /// no    no    yes   n/a | yes               yes  
    /// yes   no    yes   no  | yes               yes  
    /// yes   no    yes   yes | If VFile&lt;VType If VFile&lt;VType 4A)4B)
    /// no    yes   no    n/a | no                yes  
    /// yes   yes   no    no  | no                yes               2)
    /// yes   yes   no    yes | no                If VFile&lt;VRes  3A)3B)
    /// no    yes   yes   n/a | no                yes  
    /// yes   yes   yes   no  | no                yes  
    /// yes   yes   yes   yes | no                If VFile&lt;VRes  3A)3B)
    /// 
    /// [1] = Target file exists
    /// [2] = File in resource is versioned
    /// [3] = Type is versioned
    /// [4] = Target file is versioned
    /// [5] = File must be versioned
    /// [6] = File must be extracted
    /// [7] = Remarks
    /// VFile = Version in Target file
    /// VType = Version in Type
    /// VRes = Version in embedded file
    /// 
    /// Remarks:
    /// 1) This case exists only if the type or the image in the resource was versioned before, but then not anymore.
    /// 2) This case exists only if the image in the resource has been versioned since the last time that the control was run.
    /// 3A) If VFile &gt; VRes, throw Exception if eMode == eThrowIfNewer or eThrowIfOlderOrNewer
    /// 3B) If VFile &lt; VRes, throw Exception if eMode == eThrowIfOlder or eThrowIfOlderOrNewer
    /// 4A) If VFile &gt; VType, throw Exception if eMode == eThrowIfNewer or eThrowIfOlderOrNewer
    /// 4B) If VFile &lt; VType, throw Exception if eMode == eThrowIfOlder or eThrowIfOlderOrNewer
    /// </code>
    /// </summary>
    /// <param name="targetFile">The target file.</param>
    /// <param name="versionInResource">The version of the embedded resource file (or null if it is not versioned).</param>
    /// <param name="versionInType">The version of the Type (or null if it is not versioned).</param>
    /// <param name="format">The file's format.</param>
    /// <param name="mode">The Exception mode.</param>
    /// <param name="versionFile">If true, the file must be versioned before extraction.</param>
    /// <param name="extractFile">If true, the file must be extracted.</param>
    private static void DecideAction(FileInfo targetFile,
      VersionEx versionInResource,
      VersionEx versionInType,
      FileFormat format,
      ExtractMode mode,
      out bool versionFile,
      out bool extractFile)
    {
      versionFile = false;
      extractFile = true;

      if (targetFile.Exists)
      {
        VersionEx versionInFile = null;

        switch (format)
        {
          case FileFormat.ImageExif:
            Image image = Image.FromFile(targetFile.FullName);
            versionInFile = GetVersion(image);
            image.Dispose();
            break;

          case FileFormat.Text:
            StreamReader reader = null;
            try
            {
              reader = new StreamReader(targetFile.FullName);
              Stream fileStream = reader.BaseStream;
              versionInFile = GetVersion(ref fileStream);
            }
            catch (Exception)
            {
              throw;
            }
            finally
            {
              reader.Close();
            }
            break;

          default:
            return;
        }

        //Console.WriteLine( "Version in File: " + ( ( oVersionInFile == null ) ? "null" : oVersionInFile.strFull ) );

        if (versionInResource != null
          && versionInFile != null)
        {
          // Check versions. Depending on the mode, an exception is thrown
          // if the control's version is newer or older than the existing file.
          if (((mode == ExtractMode.ThrowIfNewer
                || mode == ExtractMode.ThrowIfOlderOrNewer)
              && versionInFile > versionInResource)
            || ((mode == ExtractMode.ThrowIfOlder
                || mode == ExtractMode.ThrowIfOlderOrNewer)
              && versionInFile < versionInResource))
          {
            throw new VersionException("Existing file has version " + versionInFile.Full
              + "; Embedded file has version " + versionInResource.Full);
          }
          extractFile = (versionInFile < versionInResource);
        }

        if (versionInResource == null)
        {
          if (versionInType != null)
          {
            if (versionInFile != null)
            {
              // Check versions. Depending on the mode, an exception is thrown
              // if the control's version is newer or older than the existing file.
              if (((mode == ExtractMode.ThrowIfNewer
                    || mode == ExtractMode.ThrowIfOlderOrNewer)
                  && versionInFile > versionInType)
                || ((mode == ExtractMode.ThrowIfOlder
                    || mode == ExtractMode.ThrowIfOlderOrNewer)
                  && versionInFile < versionInType))
              {
                throw new VersionException("File has version " + versionInFile.Full
                  + "; Control has version " + versionInType.Full);
              }
              extractFile = versionFile = (versionInFile < versionInType);
            }
            else
            {
              versionFile = true;
            }
          }

          if (versionInType == null
            && versionInFile == null)
          {
            extractFile = false;
          }
        }
      }
      else
      {
        if (versionInResource == null
          && versionInType != null)
        {
          versionFile = true;
        }
      }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Creates a <see cref="VersionEx" /> object according to the version information
    /// embedded in the text file.
    /// <para>This method attempts to find an XML version tag in the first line of the
    /// file. If available, the XML version tag is: <c>&lt; gslb_version version="V1.2.3" &gt;</c></para>
    /// </summary>
    /// <param name="textStream">A Stream with a text file content.</param>
    /// <returns>A VersionEx object with the embedded information, or null if nothing is found.</returns>
    public static VersionEx GetVersion(ref Stream textStream)
    {
      try
      {
        string versionLine = "";
        int character = -1;
        while ((character = textStream.ReadByte()) != '\n'
          && character != '\r'
          && character != '<'
          && character > -1)
        {
        }

        versionLine += "<";
        int trimIndex = 0;
        int current = 1;
        while ((character = textStream.ReadByte()) != '\n'
          && character != '\r'
          && character != '>'
          && character > -1)
        {
          versionLine += (char) character;
          if (character == '<')
          {
            // There is another '<' in the string --> trim
            trimIndex = current;
          }
          current++;
        }

        versionLine += (char) character;

        if (trimIndex > 0)
        {
          versionLine = versionLine.Substring(trimIndex);
        }

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(versionLine);
        string version = doc.FirstChild.Attributes[ATTR_VERSION].Value;

        if (VersionEx.IsValidVersion(version))
        {
          return new VersionEx(version);
        }
        return null;
      }
      catch (Exception)
      {
      }
      finally
      {
        if (textStream != null)
        {
          textStream.Position = 0;
        }
      }
      return null;
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// Creates a <see cref="VersionEx" /> object according to the version information
    /// embedded in the Image file.
    /// <para>This method attempts to find a version string in a <see cref="PropertyItem" />
    /// object with ID=0x010E. If available, the first line of the description contains
    /// the version information.</para>
    /// </summary>
    /// <param name="image">The Image containing the embedded version string.</param>
    /// <returns>A VersionEx object with the embedded information, or null if nothing is found.</returns>
    /// See also: <seealso cref="PropertyItemFactory" />
    public static VersionEx GetVersion(Image image)
    {
      string versionInResource = PropertyItemFactory.GetImageDescription(image);
      if (versionInResource != null
        && versionInResource.Length > 0)
      {
        if (versionInResource.IndexOf("\r") > -1)
        {
          versionInResource = versionInResource.Split(new char[] { '\r' })[0];
        }
        if (versionInResource.IndexOf("\n") > -1)
        {
          versionInResource = versionInResource.Split(new char[] { '\n' })[0];
        }
        if (VersionEx.IsValidVersion(versionInResource))
        {
          return new VersionEx(versionInResource);
        }
      }
      return null;
    }

    #endregion

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

    // Separator **************************************************************
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------
    /// <summary>
    /// This exception is thrown when a newer or older version of an embedded
    /// resource file is found in the target directory.
    /// </summary>
    public class VersionException : ApplicationException
    {
      /// <summary>
      /// Initializes a new instance of the VersionException class.
      /// </summary>
      public VersionException()
        : base()
      {
      }

      /// <summary>
      /// Initializes a new instance of the VersionException class with a specified error message.
      /// </summary>
      /// <param name="message">The error message that explains the reson for the exception.</param>
      public VersionException(string message)
        : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the VersionException class with a specified error message
      /// and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The error message that explains the reson for the exception.</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VersionException(string message, Exception innerException)
        : base(message, innerException)
      {
      }
    }

    #endregion
  }
}