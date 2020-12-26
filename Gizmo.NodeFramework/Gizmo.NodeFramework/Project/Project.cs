using Gizmo.NodeBase;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Gizmo.NodeFramework
{
    public class Project : EntityViewModel, IDisposable
    {
        private const string TagsCollectionName = "Tags";
        private const string LibraryCollectionName = "Library";
        #region Handlers
        private readonly NotifyCollectionChangedEventHandler LibraryChangedhandler;
        private readonly NotifyCollectionChangedEventHandler TagsChangedhandler;
        #endregion

        #region Private Properties
        private DateTime projectChangedTime = new DateTime();
        private string name = string.Empty;
        private string description = string.Empty;
        private Guid projectId = Guid.NewGuid();
        private Engine projectEngine;

        private ObservableCollection<Module> library = new ObservableCollection<Module>();
        private ObservableCollection<TagDefinition> tags = new ObservableCollection<TagDefinition>();
        #endregion

        #region Public Properties
        public DateTime ProjectChangedTime
        {
            get => projectChangedTime;
            set
            {
                if (projectChangedTime == value) return;
                projectChangedTime = value;
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

        public Guid ProjectId
        {
            get => projectId;
            set
            {
                if (projectId == value) return;
                projectId = value;
                OnPropertyChanged();
            }
        }

        public Engine ProjectEngine
        {
            get => projectEngine;
            set
            {
                if (projectEngine == value) return;
                projectEngine = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Module> Library
        {
            get => library;
            set
            {
                if (library == value) return;
                library = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TagDefinition> Tags
        {
            get => tags;
            set
            {
                if (tags == value) return;
                tags = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public Project()
        {
            ProjectEngine = new Engine();

            LibraryChangedhandler = new NotifyCollectionChangedEventHandler(Library_CollectionChanged);
            TagsChangedhandler = new NotifyCollectionChangedEventHandler(Tags_CollectionChanged);

            Library.CollectionChanged += LibraryChangedhandler;
            Tags.CollectionChanged += TagsChangedhandler;
        }

        public void Dispose()
        {
            Library.CollectionChanged -= LibraryChangedhandler;
            Tags.CollectionChanged -= TagsChangedhandler;
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Event Handlers
        private void Library_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(LibraryCollectionName);
            }
        }

        private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(TagsCollectionName);
            }
        }
        #endregion
    }
}
