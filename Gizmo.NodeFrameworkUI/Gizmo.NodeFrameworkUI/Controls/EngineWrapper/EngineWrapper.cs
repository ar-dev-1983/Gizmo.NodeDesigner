using Gizmo.NodeBase;
using Gizmo.NodeFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeFrameworkUI
{

    public class EngineWrapper : Control
    {
        #region Routed Events
        public static readonly RoutedEvent EngineChangedEvent;

        public event RoutedEventHandler EngineChanged
        {
            add
            {
                AddHandler(EngineChangedEvent, value);
            }
            remove
            {
                RemoveHandler(EngineChangedEvent, value);
            }
        }
        #endregion

        #region Internal
        internal enum InternalState
        {
            NotReady,
            ItemsCreated,
            Ready,
            IgnoreChanges
        }
        #endregion

        #region Private Properties
        private SelectionService selectionService;
        private InternalState state = InternalState.Ready;
        private readonly EngineWrapperGenerator Generator = new EngineWrapperGenerator();
        #endregion

        #region Constructors
        public EngineWrapper()
: base()
        {
            Items = new ObservableCollection<EntityWrapper>();
            DefaultStyleKey = typeof(EngineWrapper);
        }

        static EngineWrapper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EngineWrapper), new FrameworkPropertyMetadata(typeof(EngineWrapper)));
            EngineChangedEvent = EventManager.RegisterRoutedEvent("EngineChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EngineWrapper));
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion

        #region Public Properties
        public EngineWrapperGenerator ItemContainerGenerator => Generator;

        public ObservableCollection<EntityWrapper> Items { set; get; }

        public SelectionService SelectionService
        {
            get
            {
                if (selectionService == null)
                    selectionService = new SelectionService(this);

                return selectionService;
            }
        }
        public Engine Source
        {
            get => (Engine)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public bool ShowGrid
        {
            get => (bool)GetValue(ShowGridProperty);
            set => SetValue(ShowGridProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Engine), typeof(EngineWrapper), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register("ShowGrid", typeof(bool), typeof(EngineWrapper), new FrameworkPropertyMetadata(false));
        #endregion

        #region Property Callbacks
        private static void OnSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                EngineWrapper engineWrapper = o as EngineWrapper;
                if (engineWrapper.state == InternalState.IgnoreChanges)
                    return;
                else if (engineWrapper.state == InternalState.Ready)
                {
                    if (e.OldValue != null)
                        engineWrapper.Dismiss((Engine)e.OldValue);
                    if (e.NewValue != null)
                        engineWrapper.Update();
                }
            }
        }
        internal void Dismiss(Engine oldEngine)
        {
            oldEngine.OnSelectedModuleChanged -= OnSelectedModuleChanged;
            oldEngine.OnAddLink -= Source_OnAddLink;
            oldEngine.OnAddNode -= Source_OnAddNode;
            oldEngine.OnRemoveLink -= Source_OnRemoveLink;
            oldEngine.OnRemoveNode -= Source_OnRemoveNode;
            oldEngine.OnRemoveAllNodesAndLinks -= Source_OnRemoveAllNodesAndLinks;
        }

        internal void Update()
        {
            Source.OnSelectedModuleChanged += OnSelectedModuleChanged;
            Source.OnAddLink += Source_OnAddLink;
            Source.OnAddNode += Source_OnAddNode;
            Source.OnRemoveLink += Source_OnRemoveLink;
            Source.OnRemoveNode += Source_OnRemoveNode;
            Source.OnRemoveAllNodesAndLinks += Source_OnRemoveAllNodesAndLinks;
            AttachItems();

            RoutedEventArgs args = new RoutedEventArgs
            {
                RoutedEvent = EngineChangedEvent
            };
            RaiseEvent(args);
        }

        private void Source_OnRemoveAllNodesAndLinks()
        {
            Items.Clear();
        }

        private void Source_OnRemoveNode(Node node)
        {
            if (node.ModuleId == Source.SelectedModule.Id)
            {
                if (Items.Contains(Generator.ContainerFromItem(node) as NodeWrapper))
                {
                    Items.Remove(Generator.ContainerFromItem(node) as NodeWrapper);
                }
            }
        }

        private void Source_OnRemoveLink(Link link)
        {
            if (link.ModuleId == Source.SelectedModule.Id)
            {
                if (Items.Contains(Generator.ContainerFromItem(link) as LinkWrapper))
                {
                    Items.Remove(Generator.ContainerFromItem(link) as LinkWrapper);
                }
            }
        }

        private void Source_OnAddNode(Node node)
        {
            if (node.ModuleId == Source.SelectedModule.Id)
            {
                Items.Add(Generator.CreateContainer(node) as NodeWrapper);
            }
        }

        private void Source_OnAddLink(Link link)
        {
            if (link.ModuleId == Source.SelectedModule.Id)
            {
                Items.Add(Generator.CreateContainer(link) as LinkWrapper);
            }
        }

        private void AttachItems()
        {
            SelectionService.ClearSelection();

            if (Source.LastSelectedModule != null)
            {
                foreach (var item in Source.Nodes)
                {
                    if (item.ModuleId == Source.LastSelectedModule.Id)
                    {
                        if (Items.Contains(Generator.ContainerFromItem(item) as NodeWrapper))
                        {
                            Items.Remove(Generator.ContainerFromItem(item) as NodeWrapper);
                        }
                    }
                }
                foreach (var item in Source.Links)
                {
                    if (item.ModuleId == Source.LastSelectedModule.Id)
                    {
                        if (Items.Contains(Generator.ContainerFromItem(item) as LinkWrapper))
                        {
                            Items.Remove(Generator.ContainerFromItem(item) as LinkWrapper);
                        }
                    }
                }
                Generator.RecycleAll();
            }
            if (Source.SelectedModule != null)
            {
                foreach (var item in Source.Nodes)
                {
                    if (item.ModuleId == Source.SelectedModule.Id)
                    {
                        var newNode = Generator.CreateContainer(item) as NodeWrapper;
                        Items.Add(newNode);
                        if (item.IsSelected)
                        {
                            SelectionService.AddToSelection(newNode);
                        }
                    }
                }
                foreach (var item in Source.Links)
                {
                    if (item.ModuleId == Source.SelectedModule.Id)
                    {
                        var newLink = Generator.CreateContainer(item) as LinkWrapper;
                        Items.Add(newLink);
                        if (item.IsSelected)
                        {
                            SelectionService.AddToSelection(newLink);
                        }
                    }
                }
            }
        }

        private void OnSelectedModuleChanged()
        {
            AttachItems();
        }

        public void RemoveSelected()
        {
            EntityWrapper[] RemoveList = new EntityWrapper[SelectionService.CurrentSelection.Count];
            SelectionService.CurrentSelection.CopyTo(RemoveList);
            SelectionService.ClearSelection();

            foreach (var item in RemoveList)
            {
                RemoveContainer(item);
            }
        }

        public void RemoveContainer(DependencyObject container)
        {
            if (container != null)
            {
                if (container is LinkWrapper)
                {
                    Source.RemoveLink((container as LinkWrapper).DataContext as Link);
                }
                else if (container is NodeWrapper)
                {
                    Source.RemoveNode((container as NodeWrapper).DataContext as Node);
                }
            }
        }
        #endregion   
    }
}
