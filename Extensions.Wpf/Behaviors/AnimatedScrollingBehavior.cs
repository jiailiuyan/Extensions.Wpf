using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Extensions.Wpf.Behaviors
{
    public sealed class AnimatedScrollingBehavior : Behavior<ScrollViewer>
    {
        #region EasingFunction

        /// <summary>
        ///     EasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(AnimatedScrollingBehavior),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the EasingFunction property.  This dependency property
        ///     indicates easing function.
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue((DependencyProperty)EasingFunctionProperty, value); }
        }

        #endregion

        #region HorizontalOffset

        /// <summary>
        ///     HorizontalOffset Dependency Property
        /// </summary>
        private static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(AnimatedScrollingBehavior),
                new UIPropertyMetadata(0.0d,
                    OnHorizontalOffsetChanged));

        /// <summary>
        ///     Gets or sets the HorizontalOffset property.  This dependency property
        ///     indicates horizontal offset.
        /// </summary>
        private double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        ///     Handles changes to the HorizontalOffset property.
        /// </summary>
        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedScrollingBehavior)d).OnHorizontalOffsetChanged(e);
        }

        /// <summary>
        ///     Provides derived classes an opportunity to handle changes to the HorizontalOffset property.
        /// </summary>
        private void OnHorizontalOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            AssociatedObject.ScrollToHorizontalOffset((double)e.NewValue);
        }

        #endregion

        #region VerticalOffset

        /// <summary>
        ///     VerticalOffset Dependency Property
        /// </summary>
        private static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(double), typeof(AnimatedScrollingBehavior),
                new UIPropertyMetadata(0.0d,
                    OnVerticalOffsetChanged));

        /// <summary>
        ///     Gets or sets the VerticalOffset property.  This dependency property
        ///     indicates vertical offset.
        /// </summary>
        private double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        ///     Handles changes to the VerticalOffset property.
        /// </summary>
        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedScrollingBehavior)d).OnVerticalOffsetChanged(e);
        }

        /// <summary>
        ///     Provides derived classes an opportunity to handle changes to the VerticalOffset property.
        /// </summary>
        private void OnVerticalOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            AssociatedObject.ScrollToVerticalOffset((double)e.NewValue);
        }

        #endregion

        #region HorizontalScrollingSpeed

        /// <summary>
        /// HorizontalScrollingSpeed Dependency Property
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollingSpeedProperty =
            DependencyProperty.Register("HorizontalScrollingSpeed", typeof(double), typeof(AnimatedScrollingBehavior),
                new FrameworkPropertyMetadata((double)1.0d,
                 null,
                    new CoerceValueCallback(CoerceHorizontalScrollingSpeedValue)));

        /// <summary>
        /// Gets or sets the HorizontalScrollingSpeed property.  This dependency property 
        /// indicates horizontal scrolling speed as seconds.
        /// </summary>
        public double HorizontalScrollingSpeed
        {
            get { return (double)GetValue(HorizontalScrollingSpeedProperty); }
            set { SetValue(HorizontalScrollingSpeedProperty, value); }
        }


        /// <summary>
        /// Coerces the HorizontalScrollingSpeed value.
        /// </summary>
        private static object CoerceHorizontalScrollingSpeedValue(DependencyObject d, object value)
        {
            var speedValue = (double)value;
            return speedValue < 0.0d ? 0.0d : speedValue;
        }

        #endregion

        #region VerticalScrollingSpeed

        /// <summary>
        /// VerticalScrollingSpeed Dependency Property
        /// </summary>
        public static readonly DependencyProperty VerticalScrollingSpeedProperty =
            DependencyProperty.Register("VerticalScrollingSpeed", typeof(double), typeof(AnimatedScrollingBehavior),
                new FrameworkPropertyMetadata((double)0.0d,
                   null,
                    new CoerceValueCallback(CoerceVerticalScrollingSpeedValue)));

        /// <summary>
        /// Gets or sets the VerticalScrollingSpeed property.  This dependency property 
        /// indicates vertical scrolling speed as seconds.
        /// </summary>
        public double VerticalScrollingSpeed
        {
            get { return (double)GetValue(VerticalScrollingSpeedProperty); }
            set { SetValue(VerticalScrollingSpeedProperty, value); }
        }


        /// <summary>
        /// Coerces the VerticalScrollingSpeed value.
        /// </summary>
        private static object CoerceVerticalScrollingSpeedValue(DependencyObject d, object value)
        {
            var speedValue = (double)value;
            return speedValue < 0.0d ? 0.0d : speedValue;

        }

        #endregion



        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            base.OnAttached();
        }
        // todo : test whether to perform actions according object visiblity
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
            AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
            var buttonUp = AssociatedObject.Template.FindName("ButtonUp", AssociatedObject) as Button;
            if (buttonUp != null)
                buttonUp.Click += buttonUp_Click;
            //    buttonUp.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, ExecuteMoveUp));

            var buttonDown = AssociatedObject.Template.FindName("ButtonDown", AssociatedObject) as Button;
            if (buttonDown != null)
                buttonDown.Click += buttonDown_Click;
            //    buttonDown.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, ExecuteMoveDown));

            var buttonLeft = AssociatedObject.Template.FindName("ButtonLeft", AssociatedObject) as Button;
            if (buttonLeft != null)
                buttonLeft.Click += buttonLeft_Click;
            //    buttonLeft.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveLeft, ExecuteMoveLeft));

            var buttonRight = AssociatedObject.Template.FindName("ButtonRight", AssociatedObject) as Button;
            if (buttonRight != null)
                buttonRight.Click += buttonRight_Click;
            //    buttonRight.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveRight, ExecuteMoveRight));
        }

        void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //e.Handled = false;
            //var mouseWheelEvent = UIElement.MouseWheelEvent;
        }

        void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //   e.Handled = false;
        }

        void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // 48 : value from decompiled sources
            ScrollViewer parent = GetParent<ScrollViewer>(AssociatedObject);
            if (parent != null)
            {
                if (e.Delta > 0)
                {
                    parent.ScrollToVerticalOffset(parent.VerticalOffset - 48);
                }
                else
                {
                    parent.ScrollToVerticalOffset(parent.VerticalOffset + 48);
                }
            }
            e.Handled = true;
        }

        public static T GetParent<T>(DependencyObject scrollViewer) where T : DependencyObject
        {
            while (true)
            {
                DependencyObject dependencyObject = VisualTreeHelper.GetParent(scrollViewer);
                if (dependencyObject != null)
                {
                    var b = dependencyObject.GetType() == typeof(ScrollViewer);
                    if (b)
                    {
                        return dependencyObject as T;
                    }
                    scrollViewer = dependencyObject;
                    continue;
                }

                return null;
            }
        }

        void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            AnimatedMoveDown();
        }

        void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            AnimatedMoveUp();
        }

        void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            AnimatedMoveRight();
        }

        void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            AnimatedMoveLeft();
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                AnimatedMoveLeft();
            }
            else if (e.Key == Key.Right)
            {
                AnimatedMoveRight();
            }
            else if (e.Key == Key.Up)
            {
                AnimatedMoveUp();
            }
            else if (e.Key == Key.Down)
            {
                AnimatedMoveDown();
            }
            else
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }


        private void AnimatedMoveUp()
        {
            double offset = AssociatedObject.VerticalOffset - AssociatedObject.ViewportHeight;
            var animation = new DoubleAnimation(AssociatedObject.VerticalOffset, offset,
                new Duration(TimeSpan.FromSeconds(VerticalScrollingSpeed)));
            if (EasingFunction != null) animation.EasingFunction = EasingFunction;
            BeginAnimation(VerticalOffsetProperty, null);
            BeginAnimation(VerticalOffsetProperty, animation);
        }

        private void AnimatedMoveDown()
        {
            double offset = AssociatedObject.VerticalOffset + AssociatedObject.ViewportHeight;
            var animation = new DoubleAnimation(AssociatedObject.VerticalOffset, offset,
                new Duration(TimeSpan.FromSeconds(VerticalScrollingSpeed)));
            if (EasingFunction != null) animation.EasingFunction = EasingFunction;
            BeginAnimation(VerticalOffsetProperty, null);
            BeginAnimation(VerticalOffsetProperty, animation);
        }


        private void AnimatedMoveLeft()
        {
            double offset = AssociatedObject.HorizontalOffset - AssociatedObject.ViewportWidth;
            if (offset < 0)
                offset = 0;
            AnimateHorizontally(offset);
        }

        private void AnimatedMoveRight()
        {
            double offset = AssociatedObject.HorizontalOffset + AssociatedObject.ViewportWidth;
            if (offset > AssociatedObject.ScrollableWidth)
                offset = AssociatedObject.ScrollableWidth;
            AnimateHorizontally(offset);
        }

        private void AnimateHorizontally(double offset)
        {
            var animation = new DoubleAnimation(AssociatedObject.HorizontalOffset, offset,
                new Duration(TimeSpan.FromSeconds(HorizontalScrollingSpeed)));
            if (EasingFunction != null) animation.EasingFunction = EasingFunction;
            BeginAnimation(HorizontalOffsetProperty, null);
            BeginAnimation(HorizontalOffsetProperty, animation);
        }
    }
}