using Gizmo.NodeBase;
using System;

namespace Gizmo.NodeFramework
{
    public class Link : EntityViewModel, IEntity, IDisposable
    {
        #region Private Properties
        private Guid id = Guid.NewGuid();
        private Guid groupId = Guid.Empty;
        private Guid moduleId = Guid.Empty;
        private bool isSelected = false;
        private bool isInGroup = false;
        private Guid inputId = Guid.Empty;
        private Guid outputId = Guid.Empty;
        public EntityTypeEnum EntityType { get => EntityTypeEnum.Link; }
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

        public Guid GroupId
        {
            get => groupId;
            set
            {
                if (groupId == value) return;
                groupId = value;
                OnPropertyChanged();
            }
        }

        public Guid ModuleId
        {
            get => moduleId;
            set
            {
                if (moduleId == value) return;
                moduleId = value;
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

        public bool IsInGroup
        {
            get => isInGroup;
            set
            {
                if (isInGroup == value) return;
                isInGroup = value;
                OnPropertyChanged();
            }
        }

        public Guid InputId
        {
            get => inputId;
            set
            {
                if (inputId == value) return;
                inputId = value;
                OnPropertyChanged();
            }
        }

        public Guid OutputId
        {
            get => outputId;
            set
            {
                if (outputId == value) return;
                outputId = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public Link() { }

        public Link(Guid moduleID)
        {
            ModuleId = moduleID;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Link(Guid inputID, Guid outputID, Guid moduleID)
        {
            ModuleId = moduleID;
            InputId = inputID;
            OutputId = outputID;
        }

        public Link(Guid id, Guid inputID, Guid outputID, Guid moduleID)
        {
            Id = id;
            ModuleId = moduleID;
            InputId = inputID;
            OutputId = outputID;
        }
        #endregion
    }
}
