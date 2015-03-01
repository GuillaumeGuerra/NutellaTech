//*****************************************************************************
//* Class: XamlToPngConverter
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2008
//*****************************************************************************
//* Project                 : GalaSoft.Wpf.Utilities
//* Author                  : Laurent Bugnion, GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 03.04.2009
//*****************************************************************************
//* Last base level: BL0001
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.Utilities.Attributes;

namespace GalaSoft.Utilities.Wpf
{
  /// <summary>
  /// Takes a Stream containing XAML content, and converts it in a PNG image.
  /// </summary>
  
  [ClassInfo(typeof(XamlToPngConverter),
  VersionString = "V1.0.0",
  DateString = "200904040955",
  Description = "Takes a Stream containing XAML content, and converts it in a PNG image",
  UrlContacts = "http://www.galasoft.ch/contact_en.html",
  Email = "laurent@galasoft.ch")]
  public class XamlToPngConverter
  {
    /// <summary>
    /// Takes a Stream containing XAML content, and converts it in a PNG image.
    /// </summary>
    /// <param name="xamlInput">The Stream containing the XAML content.</param>
    /// <param name="width">The desired width of the PNG image.</param>
    /// <param name="height">The desired height of the PNG image.</param>
    /// <param name="pngOutput">The Stream that will contain the PNG image.</param>
    /// <param name="replacements">A list of DependencyPropertyReplacement objects
    /// that define which DependencyProperty will be replaced in the XAML stream.
    /// This allows, for example, localizing an element.</param>
    public void Convert(Stream xamlInput,
      double width, 
      double height, 
      Stream pngOutput, 
      List<DependencyPropertyReplacement> replacements)
    {
      // Round width and height to simplify
      width = Math.Round(width);
      height = Math.Round(height);

      Thread pngCreationThread = new Thread((ThreadStart) delegate()
      {
        FrameworkElement element = XamlReader.Load(xamlInput) as FrameworkElement;

        if (replacements != null)
        {
          foreach (DependencyPropertyReplacement replacement in replacements)
          {
            DependencyObject replacementElement = element.FindName(replacement.ElementName) as DependencyObject;

            if (replacementElement != null)
            {
              Type t = replacementElement.GetType();
              FieldInfo fieldInfo = t.GetField(replacement.PropertyName,
                 BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);

              if (fieldInfo != null)
              {
                DependencyProperty dp = (DependencyProperty) fieldInfo.GetValue(null);
                replacementElement.SetValue(dp, replacement.Value);
              }
            }
          }
        }

        Size renderingSize = new Size(width, height);
        element.Measure(renderingSize);
        Rect renderingRectangle = new Rect(renderingSize);
        element.Arrange(renderingRectangle);

        BitmapSource xamlBitmap = RenderToBitmap(element);

        try
        {
          PngBitmapEncoder enc = new PngBitmapEncoder();
          enc.Frames.Add(BitmapFrame.Create(xamlBitmap));
          enc.Save(pngOutput);
        }
        catch (ObjectDisposedException)
        {
          // IF the operation lasted too long, the object might be disposed already
        }
      });

      pngCreationThread.IsBackground = true;
      pngCreationThread.SetApartmentState(ApartmentState.STA);
      pngCreationThread.Start();
      pngCreationThread.Join();

      try
      {
        if (pngOutput.Length == 0)
        {
          throw new TimeoutException();
        }
      }
      catch (ObjectDisposedException)
      {
        // If the object was disposed, it means that the Stream is ready.
      }
    }

    private BitmapSource RenderToBitmap(FrameworkElement target)
    {
      Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
      RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int) target.ActualWidth,
        (int) target.ActualHeight,
        96, 96, PixelFormats.Pbgra32);

      DrawingVisual visual = new DrawingVisual();
      using (DrawingContext context = visual.RenderOpen())
      {
        VisualBrush brush = new VisualBrush(target);
        context.DrawRectangle(brush, null, new Rect(new Point(), bounds.Size));
      }
      renderBitmap.Render(visual);
      return renderBitmap;
    }
  }

  public class DependencyPropertyReplacement
  {
    public string ElementName
    {
      get;
      set;
    }
    private string _propertyName;
    public string PropertyName
    {
      get { return _propertyName; }
      set
      {
        if (value == null)
        {
          value = "";
        }
        if (!value.EndsWith("Property"))
        {
          value += "Property";
        }
        _propertyName = value;
      }
    }
    public object Value
    {
      get;
      set;
    }
  }
}
