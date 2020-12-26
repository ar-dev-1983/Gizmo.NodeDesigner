using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Gizmo.NodeDesigner
{
    public class ProjectItem : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;
        private readonly NotifyCollectionChangedEventHandler collectionChangedhandler;

        #region Private Properties
        private Guid id = Guid.NewGuid();
        private Guid parentId = Guid.NewGuid();
        private ProjectItemType type = ProjectItemType.None;
        private ProjectItemType parentType = ProjectItemType.None;
        private string name = string.Empty;
        private string description = string.Empty;
        private ObservableCollection<ProjectItem> children = new ObservableCollection<ProjectItem>();
        private bool isSelected = false;
        private bool isExpanded = false; 
        private bool showSelectionDot = false;
        #endregion

        #region Public Properties
        public Guid Id
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged();
            }
        }

        public Guid ParentId
        {
            get => parentId;
            set
            {
                if (parentId == value) return;
                parentId = value;
                OnPropertyChanged();
            }
        }

        public ProjectItemType Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged();
            }
        }

        public ProjectItemType ParentType
        {
            get => parentType;
            set
            {
                if (parentType == value) return;
                parentType = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (description == value) return;
                description = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProjectItem> Children
        {
            get => children;
            set
            {
                if (children == value) return;
                children = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value) return;
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded == value) return;
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        public bool ShowSelectionDot
        {
            get => showSelectionDot;
            private set
            {
                if (showSelectionDot == value) return;
                showSelectionDot = value;
                OnPropertyChanged();
            }
        }

        public ProjectItem SelectedItem
        {
            get
            {
                return Traverse(this, node => node.Children).FirstOrDefault(m => m.IsSelected);
            }
        }
        #endregion

        public ProjectItem()
        {
            propertyChangedHandler = new PropertyChangedEventHandler(Item_PropertyChanged);
            collectionChangedhandler = new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
            Children.CollectionChanged += collectionChangedhandler;
        }

        public void Initialise()
        {
            SubscribePropertyChanged(this);
        }

        public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>();
            stack.Push(item);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

        private void SubscribePropertyChanged(ProjectItem item)
        {
            item.PropertyChanged += propertyChangedHandler;
            item.Children.CollectionChanged += collectionChangedhandler;
            foreach (var subitem in item.Children)
            {
                SubscribePropertyChanged(subitem);
            }
        }

        private void UnsubscribePropertyChanged(ProjectItem item)
        {
            foreach (var subitem in item.Children)
            {
                UnsubscribePropertyChanged(subitem);
            }
            item.Children.CollectionChanged -= collectionChangedhandler;
            item.PropertyChanged -= propertyChangedHandler;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ProjectItem item in e.OldItems)
                {
                    UnsubscribePropertyChanged(item);
                }
            }

            if (e.NewItems != null)
            {
                foreach (ProjectItem item in e.NewItems)
                {
                    SubscribePropertyChanged(item);
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSelected"))
            {
                OnNamedPropertyChanged("SelectedItem");
            }
        }

        public void Dispose()
        {
            UnsubscribePropertyChanged(this);
            Children.CollectionChanged -= collectionChangedhandler;
        }

        public static ProjectItem CreateRoot()
        {
            var result = new ProjectItem()
            {
                Type = ProjectItemType.Root,
                Description = "",
                Name = "Project",
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                ParentId = new Guid()
            };
            var engine = CreateEngine();
            engine.children.Add(CreateModules());
            result.Children.Add(engine);
            result.Children.Add(CreateLibrary());
            result.Children.Add(CreateTags());
            result.Children.Add(CreateGraphics());
            return result;
        }

        private static ProjectItem CreateEngine() => new ProjectItem()
        {
            Type = ProjectItemType.Engine,
            Description = "",
            Name = "Engine",
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            ParentId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        };

        private static ProjectItem CreateModules() => new ProjectItem()
        {
            Type = ProjectItemType.Modules,
            Description = "",
            Name = "Modules",
            Id = Guid.Parse("50000000-0000-0000-0000-000000000005"),
            ParentId = Guid.Parse("20000000-0000-0000-0000-000000000002")
        };

        private static ProjectItem CreateLibrary() => new ProjectItem()
        {
            Type = ProjectItemType.Library,
            Description = "",
            Name = "Module Library",
            Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            ParentId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        };

        private static ProjectItem CreateTags() => new ProjectItem()
        {
            Type = ProjectItemType.TagDefinitions,
            Description = "",
            Name = "Tag Definitions",
            Id = Guid.Parse("40000000-0000-0000-0000-000000000004"),
            ParentId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        };

        private static ProjectItem CreateGraphics() => new ProjectItem()
        {
            Type = ProjectItemType.Displays,
            Description = "",
            Name = "Displays",
            Id = Guid.Parse("50000000-0000-0000-0000-000000000005"),
            ParentId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        };

        internal ProjectItem GetItemByType(ProjectItemType itemType)
        {
            return Traverse(this, node => node.Children).Where(x=>x.Type== itemType).FirstOrDefault();
        }
        internal ProjectItem GetItemById(Guid id)
        {
            return Traverse(this, node => node.Children).Where(x => x.id == id).FirstOrDefault();
        }
        internal IEnumerable<ProjectItem> GetItemsByType(ProjectItemType itemType)
        {
            return Traverse(this, node => node.Children).Where(x => x.Type == itemType);
        }

        public void SetDotState(bool state)
        {
            ShowSelectionDot = state;
        }
    }
}
