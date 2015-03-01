//*****************************************************************************
//* Class: ZoomExtension
//*****************************************************************************
//* Copyright © GalaSoft Laurent Bugnion 2009
//*****************************************************************************
//* Project                 : GalaSoft.Utilities.Wpf.Zoom
//* Author                  : Laurent Bugnion, GalaSoft
//* Web                     : http://www.galasoft.ch
//* Contact info            : laurent@galasoft.ch
//* Created                 : 03.04.2009
//*****************************************************************************
//* Last base level: BL0001
//*****************************************************************************

using System.Windows;
using GalaSoft.Utilities.Attributes;
using System.Windows.Interactivity;


namespace GalaSoft.Utilities.Wpf.Zoom
{
  [ClassInfo(typeof(ZoomBehavior))]
  public static class ZoomExtension
  {
    #region Static Members

    #region Static Public Methods

    /// <summary>
    /// This extension method adds a method to any FrameworkElement allowing
    /// to toggle the ZoomBehavior on and off. Simply call (for example)
    /// LayoutRoot.ToggleZoom(). If the FrameworkElement doesn't include
    /// a ZoomBehavior, this method has no effect at all.
    /// </summary>
    public static void ToggleZoom(this FrameworkElement element)
    {
      ZoomBehavior behavior = element.GetZoomBehavior();
      if (behavior != null)
      {
        behavior.ToggleZoom();
      }
    }

    /// <summary>
    /// This extension method adds a method to any FrameworkElement allowing to
    /// get an attached ZoomBehavior, if one is available on this FrameworkElement.
    /// If no ZoomBehavior is found, the method returns null.
    /// </summary>
    public static ZoomBehavior GetZoomBehavior(this FrameworkElement element)
    {
      ZoomBehavior behavior = null;
      var behaviors = Interaction.GetBehaviors(element);

      if (behaviors != null)
      {
        foreach (var b in behaviors)
        {
          if (b is ZoomBehavior)
          {
            behavior = b as ZoomBehavior;
            break;
          }
        }
      }

      return behavior;
    }

    #endregion

    #endregion
  }
}
