using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Extensions.Wpf.Behaviors
{
    public sealed class ListBoxScrollDownBehavior : Behavior<ListBox>
    {
        #region ScrollOnNewItem

        /// <summary>
        ///     ScrollOnNewItem Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollOnNewItemProperty =
            DependencyProperty.Register("ScrollOnNewItem", typeof (bool), typeof (ListBoxScrollDownBehavior),
                new FrameworkPropertyMetadata(true,
                    OnScrollOnNewItemChanged));

        /// <summary>
        ///     Gets or sets the ScrollOnNewItem property.  This dependency property
        ///     indicates ....
        /// </summary>
        public bool ScrollOnNewItem
        {
            get { return (bool) GetValue(ScrollOnNewItemProperty); }
            set { SetValue(ScrollOnNewItemProperty, value); }
        }

        /// <summary>
        ///     Handles changes to the ScrollOnNewItem property.
        /// </summary>
        private static void OnScrollOnNewItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ListBoxScrollDownBehavior) d).OnScrollOnNewItemChanged(e);
        }

        /// <summary>
        ///     Provides derived classes an opportunity to handle changes to the ScrollOnNewItem property.
        /// </summary>
        private void OnScrollOnNewItemChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            var notifyCollectionChanged = AssociatedObject.Items as INotifyCollectionChanged;
            notifyCollectionChanged.CollectionChanged += notifyCollectionChanged_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            var notifyCollectionChanged = AssociatedObject.Items as INotifyCollectionChanged;
            notifyCollectionChanged.CollectionChanged -= notifyCollectionChanged_CollectionChanged;
        }

        private void notifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool b = e.Action == NotifyCollectionChangedAction.Add;
            if (b)
            {
                AssociatedObject.ScrollIntoView(e.NewItems[0]);
                AssociatedObject.SelectedItem = e.NewItems[0];
            }
        }
    }
}