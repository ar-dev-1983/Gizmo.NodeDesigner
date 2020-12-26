using Gizmo.NodeBase;
using System;

namespace Gizmo.NodeFramework
{
    public class Module : EntityViewModel, IDisposable
    {
        public delegate void ModuleEventHandler(Module module);
        public event ModuleEventHandler OnModuleSelected;
        public event ModuleEventHandler OnModuleUnselected;

        #region Private Properties
        private string name = string.Empty;
        private string description = string.Empty;
        private Guid id = Guid.NewGuid();
        private ModuleType type = ModuleType.Undefined;
        private bool isSelected = false;
        #endregion

        #region Public Properties
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

        public ModuleType Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                if (value != ModuleType.Independent || value != ModuleType.Undefined || value != ModuleType.Unique)
                {
                    Name = GetName(value);
                    Description = GetDescription(value);
                    Id = GetId(value);
                }
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
                if (isSelected)
                {
                    OnModuleSelected?.Invoke(this);
                }
                else
                {
                    OnModuleUnselected?.Invoke(this);
                }
            }
        }
        #endregion

        #region Constructors
        public Module() { }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        private static string GetName(ModuleType moduleType) => moduleType switch
        {
            ModuleType.Init => "Init",
            ModuleType.Main => "Main",
            ModuleType.Exit => "Exit",
            _ => string.Empty
        };

        private static string GetDescription(ModuleType moduleType) => moduleType switch
        {
            ModuleType.Init => "Init sequence module",
            ModuleType.Main => "Main sequence module",
            ModuleType.Exit => "Exit sequence module",
            _ => string.Empty
        };

        private static Guid GetId(ModuleType moduleType) => moduleType switch
        {
            ModuleType.Init => Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ModuleType.Main => Guid.Parse("00000000-0000-0000-0000-000000000002"),
            ModuleType.Exit => Guid.Parse("00000000-0000-0000-0000-000000000003"),
            ModuleType.Undefined => Guid.Empty,
            ModuleType.Independent => Guid.NewGuid(),
            ModuleType.Unique => Guid.NewGuid(),
            _ => throw new NotImplementedException()
        };
        #endregion

        #region Public Methods
        public static ModuleType GetModuleType(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000001"))
            {
                return ModuleType.Init;
            }
            else if (id == Guid.Parse("00000000-0000-0000-0000-000000000002"))
            {
                return ModuleType.Main;
            }
            else if (id == Guid.Parse("00000000-0000-0000-0000-000000000003"))
            {
                return ModuleType.Exit;
            }
            else if (id == Guid.Empty)
            {
                return ModuleType.Undefined;
            }
            else
            {
                return ModuleType.Independent;
            }
        }
        public void SetState(bool state, bool fireEvent = false)
        {
            if (fireEvent)
                IsSelected = state;
            else
                isSelected = state;
        }
        #endregion    
    }
}
