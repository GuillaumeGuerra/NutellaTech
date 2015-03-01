//*****************************************************************************
//* Class: ZoomBehavior
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.Utilities.Attributes;
using System.Windows.Interactivity;

namespace GalaSoft.Utilities.Wpf.Zoom
{
  /// <summary>
  /// This Behavior can be added to any FrameworkElement (typically, the root
  /// panel) and adds a magnifying glass to the Window containing the element.
  /// To toggle the magnifying glass on and off, use the ZoomBehavior.ToggleZoom() method.
  /// You can also call the extension method FrameworkElement.ToggleZoom(), which
  /// has the same effect.
  /// To set the zoom factor, use the ZoomFactor dependency property.
  /// To set the radius of the maginifying Ellipse, use the ZoomGlass Radius
  /// dependency property.
  /// Note that even if the element including this Behavior is further down
  /// the logical tree, the magnifying glass will be attached to the 
  /// root element (the Window's content).
  /// </summary>
  [ClassInfo(typeof(ZoomBehavior),
  VersionString = "V1.0.0",
  DateString = "200904042159",
  Description = "Adds a magnifying glass to any WPF window",
  UrlContacts = "http://www.galasoft.ch/contact_en.html",
  Email = "laurent@galasoft.ch")]
  public class ZoomBehavior : Behavior<FrameworkElement>
  {
    #region Dependency Properties

    #region MagnifierWidth

    /// <summary>
    /// This property sets or gets the width of the control displaying the
    /// magnified view. This is a dependency property.
    /// </summary>
    public double MagnifierWidth
    {
      get { return (double)GetValue(MagnifierWidthProperty); }
      set { SetValue(MagnifierWidthProperty, value); }
    }

    /// <summary>
    /// This property sets or gets the width of the control displaying the
    /// magnified view.
    /// </summary>
    public static readonly DependencyProperty MagnifierWidthProperty =
        DependencyProperty.Register("MagnifierWidth", typeof(double), typeof(ZoomBehavior), new UIPropertyMetadata(200.0));

    #endregion

    #region MagnifierHeight

    /// <summary>
    /// This property sets or gets the height of the control displaying the
    /// magnified view. This is a dependency property.
    /// </summary>
    public double MagnifierHeight
    {
      get { return (double)GetValue(MagnifierHeightProperty); }
      set { SetValue(MagnifierHeightProperty, value); }
    }

    /// <summary>
    /// This property sets or gets the height of the control displaying the
    /// magnified view.
    /// </summary>
    public static readonly DependencyProperty MagnifierHeightProperty =
        DependencyProperty.Register("MagnifierHeight", typeof(double), typeof(ZoomBehavior), new UIPropertyMetadata(200.0));

    #endregion

    #region ZoomFactor

    /// <summary>
    /// This property sets or gets the factor of magnification. The value can be comprised
    /// between 1.0 and Double.MaxValue. There is no validation of the value, but a value
    /// smaller than 1.0 has no effect. This is a dependency property.
    /// </summary>
    public double ZoomFactor
    {
      get { return (double)GetValue(ZoomFactorProperty); }
      set { SetValue(ZoomFactorProperty, value); }
    }

    /// <summary>
    /// This property sets or gets the factor of magnification. The value can be comprised
    /// between 1.0 and Double.MaxValue. There is no validation of the value, but a value
    /// smaller than 1.0 has no effect.
    /// </summary>
    public static readonly DependencyProperty ZoomFactorProperty =
        DependencyProperty.Register("ZoomFactor", typeof(double), typeof(ZoomBehavior), new UIPropertyMetadata(2.0,
          null,
          (d, o) =>
          {
            if ((double)o < 1.0)
            {
              return 1.0;
            }

            return o;
          }));

    #endregion

    #region Template

    /// <summary>
    /// This property sets or gets the Template of the control showing the magnified view. If set to null, an
    /// ellipse will be used. This is a dependency property.
    /// </summary>
    public ControlTemplate Template
    {
      get { return (ControlTemplate)GetValue(TemplateProperty); }
      set { SetValue(TemplateProperty, value); }
    }

    /// <summary>
    /// This property sets or gets the Template of the control showing the magnified view. If set to null, an
    /// ellipse will be used.
    /// </summary>
    public static readonly DependencyProperty TemplateProperty =
        DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(ZoomBehavior), new UIPropertyMetadata(null));

    #endregion

    #region DistanceFromMouse

    /// <summary>
    /// This property sets or gets the distance in pixels between the mouse (cross cursor) and the control
    /// displaying the magnified view. This is a dependency property.
    /// </summary>
    public double DistanceFromMouse
    {
      get { return (double)GetValue(DistanceFromMouseProperty); }
      set { SetValue(DistanceFromMouseProperty, value); }
    }

    /// <summary>
    /// This property sets or gets the distance in pixels between the mouse (cross cursor) and the control
    /// displaying the magnified view.
    /// </summary>
    public static readonly DependencyProperty DistanceFromMouseProperty =
        DependencyProperty.Register("DistanceFromMouse", typeof(double), typeof(ZoomBehavior), new UIPropertyMetadata(5.0));

    #endregion

    /// <summary>
    /// This property is used to toggle the magnifier on or off. This is a dependency property.
    /// </summary>
    public bool IsVisible
    {
      get { return (bool)GetValue(IsVisibleProperty); }
      set { SetValue(IsVisibleProperty, value); }
    }

    /// <summary>
    /// This property is used to toggle the magnifier on or off.
    /// </summary>
    public static readonly DependencyProperty IsVisibleProperty =
        DependencyProperty.Register("IsVisible", typeof(bool), typeof(ZoomBehavior), new UIPropertyMetadata(false,
          (s, e) =>
          {
            if (e.NewValue == e.OldValue)
            {
              return;
            }

            var behavior = s as ZoomBehavior;

            if ((bool)e.NewValue)
            {
              behavior.AttachAdorner(behavior._attachedElement);
            }
            else
            {
              Mouse.SetCursor(null);
              behavior.DetachAdorner(behavior._attachedElement);
            }
          }));

    #endregion

    #region Public Methods

    /// <summary>
    /// This method displays or hides the magnifying glass.
    /// </summary>
    public void ToggleZoom()
    {
      IsVisible = !IsVisible;
    }

    #endregion

    #region Internal Methods

    private void DetachAdorner(FrameworkElement element)
    {
      _adorner.Detach();
      _adorner = null;
    }

    private void AttachAdorner(FrameworkElement element)
    {
      var window = GetWindow(element);

      if (window == null)
      {
        return;
      }

      var root = window.Content as FrameworkElement;
      _adorner = new ZoomAdorner(root);

      var bindingWidth = new Binding();
      bindingWidth.Source = this;
      bindingWidth.Path = new PropertyPath(ZoomBehavior.MagnifierWidthProperty);
      BindingOperations.SetBinding(_adorner, ZoomAdorner.MagnifierWidthProperty, bindingWidth);

      var bindingHeight = new Binding();
      bindingHeight.Source = this;
      bindingHeight.Path = new PropertyPath(ZoomBehavior.MagnifierHeightProperty);
      BindingOperations.SetBinding(_adorner, ZoomAdorner.MagnifierHeightProperty, bindingHeight);

      var bindingFactor = new Binding();
      bindingFactor.Source = this;
      bindingFactor.Path = new PropertyPath(ZoomBehavior.ZoomFactorProperty);
      BindingOperations.SetBinding(_adorner, ZoomAdorner.ZoomFactorProperty, bindingFactor);

      var bindingTemplate = new Binding();
      bindingTemplate.Source = this;
      bindingTemplate.Path = new PropertyPath(ZoomBehavior.TemplateProperty);
      BindingOperations.SetBinding(_adorner, ZoomAdorner.TemplateProperty, bindingTemplate);

      var bindingDistance = new Binding();
      bindingDistance.Source = this;
      bindingDistance.Path = new PropertyPath(ZoomBehavior.DistanceFromMouseProperty);
      BindingOperations.SetBinding(_adorner, ZoomAdorner.DistanceFromMouseProperty, bindingDistance);
    }

    private Window GetWindow(FrameworkElement element)
    {
        object parent = element;
        while (!(parent is Window))
        {
            parent = (parent as FrameworkElement).Parent;
            if (parent == null)
            {
                break;
            }
        }

        return parent as Window;
    }

    protected override void OnAttached()
    {
      _attachedElement = this.AssociatedObject;
    }

    #endregion

    #region Private Fields

    private FrameworkElement _attachedElement;
    private ZoomAdorner _adorner;

    #endregion
  }
}
