using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Extensions.Wpf.Behaviors
{
    public sealed class ScrollViewerHorizontalScrollingBehavior : Behavior<ScrollViewer>
    {
        #region ScrollingSpeed

        /// <summary>
        ///     ScrollingSpeed Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollingSpeedProperty =
            DependencyProperty.Register("ScrollingSpeed", typeof (int), typeof (ScrollViewerHorizontalScrollingBehavior),
                new UIPropertyMetadata(1,
                    OnScrollingSpeedChanged,
                    CoerceScrollingSpeedValue));

        /// <summary>
        ///     Gets or sets the ScrollingSpeed property.  This dependency property
        ///     indicates scrolling speed.
        /// </summary>
        public int ScrollingSpeed
        {
            get { return (int) GetValue(ScrollingSpeedProperty); }
            set { SetValue(ScrollingSpeedProperty, value); }
        }

        /// <summary>
        ///     Handles changes to the ScrollingSpeed property.
        /// </summary>
        private static void OnScrollingSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ScrollViewerHorizontalScrollingBehavior) d).OnScrollingSpeedChanged(e);
        }

        /// <summary>
        ///     Provides derived classes an opportunity to handle changes to the ScrollingSpeed property.
        /// </summary>
        private void OnScrollingSpeedChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        ///     Coerces the ScrollingSpeed value.
        /// </summary>
        private static object CoerceScrollingSpeedValue(DependencyObject d, object value)
        {
            var i = (int) value;
            return i < 1 ? 1 : i;
        }

        #endregion

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
            base.OnDetaching();
        }

        private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                for (int i = 0; i < ScrollingSpeed; i++)
                {
                    AssociatedObject.LineLeft();
                }
            }
            else if (e.Delta < 0)
            {
                for (int i = 0; i < ScrollingSpeed; i++)
                {
                    AssociatedObject.LineRight();
                }
            }
            e.Handled = true;
        }
    }
}