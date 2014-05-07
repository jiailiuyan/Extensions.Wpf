using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Extensions.Wpf.Behaviors
{
    public sealed class AnimatedExpanderBehavior : Behavior<Expander>
    {
        public Duration Duration { get; set; }
        private UIElement _expandSite;

        public AnimatedExpanderBehavior()
        {
            Duration=new Duration(TimeSpan.FromSeconds(0.5d));
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Collapsed += AssociatedObject_Collapsed;
            AssociatedObject.Expanded += AssociatedObject_Expanded;
            AssociatedObject.Loaded += (sender, args) =>
            {
                _expandSite = AssociatedObject.Template.FindName("ExpandSite", AssociatedObject) as UIElement;
                if (_expandSite == null)
                    throw new InvalidOperationException();
            };
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Collapsed -= AssociatedObject_Collapsed;
            AssociatedObject.Expanded -= AssociatedObject_Expanded;
        }

        private void AssociatedObject_Collapsed(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander == null)
                return;

            var name = expander.Content as FrameworkElement;
            if (name == null)
                return;

            _expandSite.Visibility = Visibility.Visible;
            var animation = new DoubleAnimation(name.ActualHeight, 0, Duration);
            animation.Completed += (o, args) =>
            {
                _expandSite.Visibility = Visibility.Collapsed;
                name.BeginAnimation(FrameworkElement.HeightProperty, null);
            };
            name.BeginAnimation(FrameworkElement.HeightProperty, animation);
        }

        private void AssociatedObject_Expanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander == null)
                return;

            var name = expander.Content as FrameworkElement;
            if (name == null)
                return;

            if (name.DesiredSize.Width <= 0 && name.DesiredSize.Height <= 0)
                name.Measure(new Size(9999, 9999));

            if (_expandSite != null) _expandSite.Visibility = Visibility.Visible;
            //  _expandSite = AssociatedObject.Template.FindName("ExpandSite", AssociatedObject) as UIElement;
            var animation = new DoubleAnimation(0, name.DesiredSize.Height, Duration);
            animation.Completed += (o, args) => name.BeginAnimation(FrameworkElement.HeightProperty, null);
            name.BeginAnimation(FrameworkElement.HeightProperty, animation);
        }
    }
}