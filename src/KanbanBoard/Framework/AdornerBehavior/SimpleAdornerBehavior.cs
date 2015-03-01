using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Framework
{
    public class SimpleAdornerBehavior : Behavior<FrameworkElement>
    {
        #region Dependency Properties

        public static readonly DependencyProperty AdornerTypeProperty =
            DependencyProperty.Register("AdornerType", typeof(Type), typeof(SimpleAdornerBehavior));
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(SimpleAdornerBehavior), new UIPropertyMetadata(false,
              (s, e) =>
              {
                  if (e.NewValue == e.OldValue)
                  {
                      return;
                  }

                  var behavior = s as SimpleAdornerBehavior;

                  if ((bool)e.NewValue)
                  {
                      behavior.AttachAdorner(behavior.AttachedElement);
                  }
                  else
                  {
                      Mouse.SetCursor(null);
                      behavior.DetachAdorner(behavior.AttachedElement);
                  }
              }));

        public Type AdornerType
        {
            get { return (Type)GetValue(AdornerTypeProperty); }
            set { SetValue(AdornerTypeProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        #endregion

        #region Internal Methods

        private void DetachAdorner(FrameworkElement element)
        {
            Adorner.Detach();
            Adorner = null;
        }

        private void AttachAdorner(FrameworkElement element)
        {
            var root = (element as Window).Content as FrameworkElement;
            Adorner = Activator.CreateInstance(AdornerType, root) as IAdorner;

            BindAdornerProperties();
            Adorner.Initialize();
        }

        protected override void OnAttached()
        {
            AttachedElement = this.AssociatedObject;
        }

        protected virtual void BindAdornerProperties() { }

        protected void BindProperty(DependencyProperty behaviorProperty, DependencyProperty adornerProperty)
        {
            var binding = new Binding();
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            binding.Path = new PropertyPath(behaviorProperty);
            BindingOperations.SetBinding((DependencyObject)Adorner, adornerProperty, binding);
        }

        #endregion

        #region Private Fields

        protected FrameworkElement AttachedElement { get; set; }
        protected IAdorner Adorner { get; set; }

        #endregion
    }
}
