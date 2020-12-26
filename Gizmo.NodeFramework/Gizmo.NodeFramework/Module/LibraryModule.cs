using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Gizmo.NodeFramework
{
    public class LibraryModule : Module
    {
        #region Handlers
        private readonly NotifyCollectionChangedEventHandler NodesChangedhandler;
        private readonly NotifyCollectionChangedEventHandler LinksChangedhandler;
        #endregion

        #region Private Properties
        private ObservableCollection<Node> nodes = new ObservableCollection<Node>();
        private ObservableCollection<Link> links = new ObservableCollection<Link>();
        #endregion

        #region Public Properties
        public ObservableCollection<Node> Nodes
        {
            get => nodes;
            set
            {
                if (nodes == value) return;
                nodes = value;
            }
        }
        public ObservableCollection<Link> Links
        {
            get => links;
            set
            {
                if (links == value) return;
                links = value;
            }
        }
        #endregion

        #region Constructors
        public LibraryModule() : base()
        {
            Type = ModuleType.Independent;

            NodesChangedhandler = new NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
            LinksChangedhandler = new NotifyCollectionChangedEventHandler(Links_CollectionChanged);

            Nodes.CollectionChanged += NodesChangedhandler;
            Links.CollectionChanged += LinksChangedhandler;
        }

        public new void Dispose()
        {
            Nodes.CollectionChanged -= NodesChangedhandler;
            Links.CollectionChanged -= LinksChangedhandler;

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Event Handlers
        private void Nodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged("Nodes");
            }
        }

        private void Links_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged("Links");
            }
        }
        #endregion
    }
}
