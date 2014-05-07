using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Extensions.Wpf.Behaviors
{
    internal class ScrollViewerNoArrowsBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            base.OnAttached();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollBar1("PART_VerticalScrollBar");
            ScrollBar1("PART_HorizontalScrollBar");
        }

        private void ScrollBar1(string partVerticalscrollbar)
        {
            var scrollBar1 = AssociatedObject.Template.FindName(partVerticalscrollbar, AssociatedObject) as ScrollBar;
            if (scrollBar1 != null)
            {
                var b = scrollBar1.Orientation == Orientation.Vertical;
                var partLineleftbutton = b ? "PART_LineUpButton" : "PART_LineLeftButton";
                var partLinerightbutton = b ? "PART_LineDownButton" : "PART_LineRightButton";
                var rb1 = scrollBar1.Template.FindName(partLineleftbutton, scrollBar1) as RepeatButton;
                if (rb1 != null) rb1.Visibility = Visibility.Collapsed;
                var rb2 = scrollBar1.Template.FindName(partLinerightbutton, scrollBar1) as RepeatButton;
                if (rb2 != null) rb2.Visibility = Visibility.Collapsed;
                var g = scrollBar1.Template.FindName("Bg", scrollBar1) as Grid;
                if (g != null)
                {
                    if (b)
                    {
                        g.RowDefinitions[0].Height = g.RowDefinitions[2].Height = GridLength.Auto;
                    }

                    else
                    {
                        g.ColumnDefinitions[0].Width = g.ColumnDefinitions[2].Width = GridLength.Auto;
                    }
                }
            }
        }
    }
}