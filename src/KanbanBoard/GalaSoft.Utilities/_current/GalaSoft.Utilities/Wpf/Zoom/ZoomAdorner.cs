//*****************************************************************************
//* Class: ZoomAdorner
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GalaSoft.Utilities.Attributes;

namespace GalaSoft.Utilities.Wpf.Zoom
{
    /// <summary>
    /// Adorner used to display a magnifying glass on the Adorner layer.
    /// </summary>
    [ClassInfo(typeof(ZoomBehavior))]
    public class ZoomAdorner : Adorner
    {
        #region Constructors

        /// <summary>
        /// Constructs a ZoomAdorner. Note that even if the element is a child
        /// down the Logical Tree, the Adorner will be attached to the 
        /// root element (the Window's content).
        /// </summary>
        public ZoomAdorner(FrameworkElement element)
            : base(element)
        {
            _element = element;
            _element.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(element_MouseMove), true);

            _brush = new VisualBrush(_element);
            _brush.ViewboxUnits = BrushMappingMode.Absolute;

            var children = new VisualCollection(this);
            _canvas = new Canvas();
            children.Add(_canvas);

            CreateGlass();
            SetGlass();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);
            adornerLayer.Add(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// This property sets or gets the width of the control displaying the
        /// magnified view.
        /// </summary>
        public static readonly DependencyProperty MagnifierWidthProperty =
            DependencyProperty.Register("ZoomGlassRadius", typeof(double), typeof(ZoomAdorner), new UIPropertyMetadata(200.0,
              (s, e) =>
              {
                  var adorner = s as ZoomAdorner;
                  adorner._glass.Width = (double)e.NewValue;
                  adorner.SetGlass();
              }));

        /// <summary>
        /// This property sets or gets the height of the control displaying the
        /// magnified view.
        /// </summary>
        public static readonly DependencyProperty MagnifierHeightProperty =
            DependencyProperty.Register("MagnifierHeight", typeof(double), typeof(ZoomAdorner), new UIPropertyMetadata(200.0,
              (s, e) =>
              {
                  var adorner = s as ZoomAdorner;
                  adorner._glass.Height = (double)e.NewValue;
                  adorner.SetGlass();
              }));

        /// <summary>
        /// This property sets or gets the factor of magnification. The value can be comprised
        /// between 1.0 and Double.MaxValue. There is no validation of the value, but a value
        /// smaller than 1.0 has no effect.
        /// </summary>
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(ZoomAdorner), new UIPropertyMetadata(2.0,
              (s, e) =>
              {
                  (s as ZoomAdorner).SetGlass();
              }));

        /// <summary>
        /// This property sets or gets the Template of the control showing the magnified view. If set to null, an
        /// ellipse will be used.
        /// </summary>
        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(ZoomAdorner), new UIPropertyMetadata(null,
              (s, e) =>
              {
                  (s as ZoomAdorner).CreateGlass();
              }));

        /// <summary>
        /// This property sets or gets the distance in pixels between the mouse (cross cursor) and the control
        /// displaying the magnified view.
        /// </summary>
        public static readonly DependencyProperty DistanceFromMouseProperty =
            DependencyProperty.Register("DistanceFromMouse", typeof(double), typeof(ZoomAdorner), new UIPropertyMetadata(5.0,
              (s, e) =>
              {
                  (s as ZoomAdorner).SetGlass();
              }));

        private VisualBrush _brush = null;
        private Canvas _canvas = null;
        private FrameworkElement _element = null;
        private FrameworkElement _glass = null;

        protected override int VisualChildrenCount { get { return 1; } }
        protected override Visual GetVisualChild(int index) { return _canvas; }

        private Size Boundary { get; set; }
        private double RectangleWidth { get { return MagnifierWidth / ZoomFactor; } }
        private double RectangleHeight { get { return MagnifierHeight / ZoomFactor; } }

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
        /// This property sets or gets the height of the control displaying the
        /// magnified view. This is a dependency property.
        /// </summary>
        public double MagnifierHeight
        {
            get { return (double)GetValue(MagnifierHeightProperty); }
            set { SetValue(MagnifierHeightProperty, value); }
        }

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
        /// This property sets or gets the Template of the control showing the magnified view. If set to null, an
        /// ellipse will be used. This is a dependency property.
        /// </summary>
        public ControlTemplate Template
        {
            get { return (ControlTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }

        /// <summary>
        /// This property sets or gets the distance in pixels between the mouse (cross cursor) and the control
        /// displaying the magnified view. This is a dependency property.
        /// </summary>
        public double DistanceFromMouse
        {
            get { return (double)GetValue(DistanceFromMouseProperty); }
            set { SetValue(DistanceFromMouseProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes the Adorner from the AdornerLayer. This method should be called
        /// before the Adorner is disposed, to avoid memory leaks.
        /// </summary>
        public void Detach()
        {
            _element.RemoveHandler(FrameworkElement.MouseMoveEvent, new MouseEventHandler(element_MouseMove));
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_element);
            adornerLayer.Remove(this);
        }

        #endregion

        #region Internal Methods

        private void element_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SetGlass();
        }

        private void CreateGlass()
        {
            if (_glass != null)
            {
                _canvas.Children.Remove(_glass);
            }

            if (Template == null)
            {
                _glass = MakeEllipse();
            }
            else
            {
                _glass = MakeControl();
            }

            _canvas.Children.Add(_glass);
            SetGlass();
        }

        private FrameworkElement MakeControl()
        {
            var control = new Control();
            control.Width = MagnifierWidth;
            control.Height = MagnifierHeight;
            control.Background = _brush;
            control.Template = Template;
            return control;
        }

        private FrameworkElement MakeEllipse()
        {
            var grid = new Grid();
            grid.Width = MagnifierWidth;
            grid.Height = MagnifierHeight;

            var background = new Ellipse();
            background.Fill = Brushes.Silver;
            grid.Children.Add(background);

            var ellipse = new Ellipse();
            ellipse.Fill = _brush;
            ellipse.Stroke = Brushes.Black;
            grid.Children.Add(ellipse);

            return grid;
        }

        private void SetGlass()
        {
            Point currentMousePosition = Mouse.GetPosition(this.AdornedElement);
            Point ellipsePoint = new Point();
            double d = DistanceFromMouse;
            double w = MagnifierWidth;
            double h = MagnifierHeight;
            double rw = RectangleWidth;
            double rh = RectangleHeight;

            // Determine whether the magnifying glass should be shown to the
            // the left or right of the mouse pointer.
            if (_element.ActualWidth - currentMousePosition.X > w + d)
            {
                ellipsePoint.X = currentMousePosition.X + d;
            }
            else
            {
                ellipsePoint.X = currentMousePosition.X - d - w;
            }

            // Determine whether the magnifying glass should be shown 
            // above or below the mouse pointer.
            if (_element.ActualHeight - currentMousePosition.Y > h + d)
            {
                ellipsePoint.Y = currentMousePosition.Y + d;
            }
            else
            {
                ellipsePoint.Y = currentMousePosition.Y - d - h;
            }

            // Update the visual brush's Viewbox to magnify a 20 by 20 rectangle,
            // centered on the current mouse position.
            _brush.Viewbox = new Rect(currentMousePosition.X - (rw / 2),
              currentMousePosition.Y - (rh / 2),
              rw,
              rh);

            Canvas.SetLeft(_glass, ellipsePoint.X);
            Canvas.SetTop(_glass, ellipsePoint.Y);

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);
            adornedElementRect.Inflate(Boundary);
            _canvas.Arrange(adornedElementRect);
            return finalSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _canvas.Measure(constraint);
            return _canvas.DesiredSize;
        }

        #endregion
    }
}
