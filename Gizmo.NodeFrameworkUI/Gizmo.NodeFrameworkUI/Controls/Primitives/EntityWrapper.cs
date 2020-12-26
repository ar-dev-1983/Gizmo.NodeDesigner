using Gizmo.NodeBase;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeFrameworkUI
{
    public class EntityWrapper : Control, IEntity
    {
        public static RoutedEvent ItemSelectedEvent;
        public static RoutedEvent ItemUnSelectedEvent;

        public event RoutedEventHandler OnSelected
        {
            add { AddHandler(ItemSelectedEvent, value); }
            remove { RemoveHandler(ItemSelectedEvent, value); }
        }

        public event RoutedEventHandler OnUnselected
        {
            add { AddHandler(ItemUnSelectedEvent, value); }
            remove { RemoveHandler(ItemUnSelectedEvent, value); }
        }

        public EntityTypeEnum EntityType { get => EntityTypeEnum.Unknown; }

        static EntityWrapper()
        {
            ItemSelectedEvent = EventManager.RegisterRoutedEvent("OnSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EntityWrapper));
            ItemUnSelectedEvent = EventManager.RegisterRoutedEvent("OnUnselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EntityWrapper));
        }

        public EntityWrapper():base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        public bool IsInGroup
        {
            get => (bool)GetValue(IsInGroupProperty);
            set => SetValue(IsInGroupProperty, value);
        }
        public Guid Id
        {
            get => (Guid)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }
        public Guid GroupId
        {
            get => (Guid)GetValue(GroupIdProperty);
            set => SetValue(GroupIdProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(EntityWrapper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));
        public static readonly DependencyProperty IsInGroupProperty = DependencyProperty.Register("IsInGroup", typeof(bool), typeof(EntityWrapper), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(Guid), typeof(EntityWrapper), new FrameworkPropertyMetadata(Guid.Empty));
        public static readonly DependencyProperty GroupIdProperty = DependencyProperty.Register("GroupId", typeof(Guid), typeof(EntityWrapper), new FrameworkPropertyMetadata(Guid.Empty));
        
        private static void IsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            EntityWrapper item = (EntityWrapper)o;
            item.CheckIsSelected();
        }
        internal void CheckIsSelected()
        {
            if (IsSelected)
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = ItemSelectedEvent
                };
                RaiseEvent(args);
            }
            else
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = ItemUnSelectedEvent
                };
                RaiseEvent(args);
            }
        }
    }
}
