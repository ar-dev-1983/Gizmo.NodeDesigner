using Gizmo.NodeFramework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeFrameworkUI
{
    public enum SettingsType
    {
        TextValue,
        IntegerValue,
        DecimalValue,
        BooleanValue
    }

    public class NodeSettingsItem : Control
    {
        static NodeSettingsItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeSettingsItem), new FrameworkPropertyMetadata(typeof(NodeSettingsItem)));
        }

        public NodeSettingsItem() : base()
        {
            DefaultStyleKey = typeof(NodeSettingsItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public SettingsType SettingsType
        {
            get => (SettingsType)GetValue(SettingsTypeProperty);
            set => SetValue(SettingsTypeProperty, value);
        }
        public static readonly DependencyProperty SettingsTypeProperty = DependencyProperty.Register("SettingsType", typeof(SettingsType), typeof(NodeSettingsItem), new FrameworkPropertyMetadata(SettingsType.TextValue));
    }

    public class NodeSettings : ItemsControl
    {
        static NodeSettings()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeSettings), new FrameworkPropertyMetadata(typeof(NodeSettings)));
        }

        public NodeSettings() : base()
        {
            DefaultStyleKey = typeof(NodeSettings);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is NodeSettingsItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NodeSettingsItem();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        public Node SelectedNode
        {
            get => (Node)GetValue(SelectedNodeProperty);
            set => SetValue(SelectedNodeProperty, value);
        }

        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(Node), typeof(NodeSettings), new FrameworkPropertyMetadata(null, SelectedNodeChanges));

        private static void SelectedNodeChanges(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                NodeSettings nodeSettings = o as NodeSettings;
                if (nodeSettings.SelectedNode != null)
                {
                    nodeSettings.UpdateSettings();
                }
                else
                    return;
            }
        }

        private void UpdateSettings()
        {

        }
    }
}
