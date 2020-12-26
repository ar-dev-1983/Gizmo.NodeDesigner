using Gizmo.NodeBase;
using System;
using System.Windows;

namespace Gizmo.NodeFrameworkUI
{
    public class ContainerPreparingEventArgs : EventArgs
    {
        public DependencyObject Container { get; }
        public IEntity DataContext { get; }

        public ContainerPreparingEventArgs(DependencyObject container, IEntity dataContext)
        {
            Container = container;
            DataContext = dataContext;
        }
    }
}
