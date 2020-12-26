using Gizmo.NodeBase;
using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{

    public class EngineWrapperGenerator
    {
        private static readonly DependencyProperty EntityProperty = DependencyProperty.RegisterAttached("Entity", typeof(object), typeof(EngineWrapperGenerator), new FrameworkPropertyMetadata(null));

        private readonly Dictionary<IEntity, DependencyObject> ContainersDictionary = new Dictionary<IEntity, DependencyObject>();


        public event EventHandler<ContainerPreparingEventArgs> ContainerPreparing;

        public EngineWrapperGenerator()
        {

        }

        public DependencyObject CreateContainer(IEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.EntityType == EntityTypeEnum.Link)
            {
                var container = new LinkWrapper();
                PrepareContainer(container, item);
                var doContainer = container;
                ContainersDictionary.Add(item, doContainer);
                return doContainer;
            }
            else if (item.EntityType == EntityTypeEnum.Node)
            {
                var container = new NodeWrapper();
                PrepareContainer(container, item);
                var doContainer = container;
                ContainersDictionary.Add(item, doContainer);
                return doContainer;
            }
            else
                throw new ArgumentNullException(nameof(item));
        }

        private void PrepareContainer(DependencyObject container, IEntity item)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (item.EntityType == EntityTypeEnum.Node && container is NodeWrapper)
            {
                (container as NodeWrapper).SetValue(NodeWrapper.NodeStyleProperty, (item as Node).NodeStyle);
                (container as NodeWrapper).SetValue(NodeWrapper.ShowHeaderProperty, (item as Node).UseHeader);
                (container as NodeWrapper).SetValue(NodeWrapper.NodeNameProperty, (item as Node).Name);

                (container as NodeWrapper).SetValue(NodeWrapper.IdProperty, (item as Node).Id);

                var WidthBinding = new Binding() { Path = new PropertyPath("Width"), Source = (item as Node).Size, Mode = BindingMode.TwoWay };
                (container as NodeWrapper).SetBinding(NodeWrapper.WidthProperty, WidthBinding);

                var HeightBinding = new Binding() { Path = new PropertyPath("Height"), Source = (item as Node).Size, Mode = BindingMode.TwoWay };
                (container as NodeWrapper).SetBinding(NodeWrapper.HeightProperty, HeightBinding);

                (container as NodeWrapper).SetValue(NodeWrapper.MinWidthProperty, (item as Node).Size.Width);
                (container as NodeWrapper).SetValue(NodeWrapper.MinHeightProperty, (item as Node).Size.Height);


                var GroupIdBinding = new Binding() { Path = new PropertyPath("GroupId"), Source = (item as Node), Mode = BindingMode.TwoWay };
                (container as NodeWrapper).SetBinding(NodeWrapper.GroupIdProperty, GroupIdBinding);

                var IsInGroupBinding = new Binding() { Path = new PropertyPath("IsInGroup"), Source = (item as Node), Mode = BindingMode.TwoWay };
                (container as NodeWrapper).SetBinding(NodeWrapper.IsInGroupProperty, IsInGroupBinding);

                var IsSelectedBinding = new Binding() { Path = new PropertyPath("IsSelected"), Source = (item as Node), Mode = BindingMode.TwoWay };
                (container as NodeWrapper).SetBinding(NodeWrapper.IsSelectedProperty, IsSelectedBinding);

                if ((item as Node).UseIcon && (item as Node).Icon != null)
                {
                    (container as NodeWrapper).SetValue(NodeWrapper.IconProperty,(item as Node).Icon != null && (item as Node).Icon.GetType() == typeof(string) ? Geometry.Parse((item as Node).Icon.ToString()) : null);
                }
                Canvas.SetLeft((container as NodeWrapper), (item as Node).Position.X);
                Canvas.SetTop((container as NodeWrapper), (item as Node).Position.Y);
            }
            else if (item.EntityType == EntityTypeEnum.Link && container is LinkWrapper)
            {
                var GroupIdBinding = new Binding() { Path = new PropertyPath("GroupId"), Source = (item as Link), Mode = BindingMode.TwoWay };
                (container as LinkWrapper).SetBinding(LinkWrapper.GroupIdProperty, GroupIdBinding);

                var IsInGroupBinding = new Binding() { Path = new PropertyPath("IsInGroup"), Source = (item as Link), Mode = BindingMode.TwoWay };
                (container as LinkWrapper).SetBinding(LinkWrapper.IsInGroupProperty, IsInGroupBinding);

                var IsSelectedBinding = new Binding() { Path = new PropertyPath("IsSelected"), Source = (item as Link), Mode = BindingMode.TwoWay };
                (container as LinkWrapper).SetBinding(LinkWrapper.IsSelectedProperty, IsSelectedBinding);
            }

            container.SetValue(EntityProperty, item);
            container.SetValue(FrameworkElement.DataContextProperty, item);
            OnContainerPreparing(new ContainerPreparingEventArgs(container, item));
        }

        public void Recycle(DependencyObject container)
        {
            Recycle(container, true);
        }

        private void Recycle(DependencyObject container, bool remove)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var item = ItemFromContainer(container);

            if (item == null)
                throw new InvalidOperationException("Item not exist!");

            if (remove)
                ContainersDictionary.Remove(item);

        }

        public void RecycleAll()
        {
            ContainersDictionary.Clear();
        }


        public DependencyObject ContainerFromItem(IEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            DependencyObject container;

            if (ContainersDictionary.TryGetValue(item, out container))
            {
                return container;
            }
            return null;
        }

        public IEntity ItemFromContainer(DependencyObject container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var localValue = container.ReadLocalValue(EntityProperty);

            if (localValue == DependencyProperty.UnsetValue)
                localValue = null;
            return (IEntity)localValue;
        }

        protected virtual void OnContainerPreparing(ContainerPreparingEventArgs e)
        {
            ContainerPreparing?.Invoke(this, e);
        }
    }
}
