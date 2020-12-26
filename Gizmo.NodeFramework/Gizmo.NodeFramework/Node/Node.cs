using Gizmo.NodeBase;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Gizmo.NodeFramework
{
    public abstract class Node : EntityViewModel, IObject, IDisposable
    {
        private const string SettingsCollectionName = "Settings";
        private const string ConstantsCollectionName = "Constants";
        private const string OutputsCollectionName = "Outputs";
        private const string InputsCollectionName = "Inputs";
        #region Event Handlers
        private readonly NotifyCollectionChangedEventHandler InputsChangedhandler;
        private readonly NotifyCollectionChangedEventHandler OutputsChangedhandler;
        private readonly NotifyCollectionChangedEventHandler ConstantsChangedhandler;
        private readonly NotifyCollectionChangedEventHandler SettingsChangedhandler;

        public delegate void NodeEventHandler(Node node);

        public event NodeEventHandler OnAddedToEngine;
        public event NodeEventHandler OnRemovedFromEngine;

        public event NodeEventHandler OnInputAdded;
        public event NodeEventHandler OnInputRemoved;

        public event NodeEventHandler OnOutputAdded;
        public event NodeEventHandler OnOutputRemoved;

        public event NodeEventHandler OnSettingAdded;
        public event NodeEventHandler OnSettingRemoved;

        public event NodeEventHandler OnConstantAdded;
        public event NodeEventHandler OnConstantRemoved;
        #endregion

        #region Private Properties
        private string name = string.Empty;
        private string nodeName = string.Empty;
        private string nodeDescription = string.Empty;
        private string nodeCategory = "Unspecified";
        private Guid id = Guid.NewGuid();
        private Guid groupId = Guid.Empty;
        private Guid moduleId = Guid.Empty;
        private bool isSelected = false;
        private bool isInGroup = false;
        private EntityPosition position = new EntityPosition();
        private EntitySize size = new EntitySize();
        private bool hasChildren = false;
        private bool addInputsAllowed = false;
        private bool addOutputsAllowed = false;
        private bool useInputs = false;
        private bool useOutputs = false;
        private bool useSettings = false;
        private bool useConstants = false;

        private bool useIcon = false;
        private bool useHeader = false;
        private bool visbileInToolbox = true;

        private object icon = null;
        private NodeStyleEnum nodeStyle = NodeStyleEnum.Default;

        private ObservableCollection<Variable> inputs = new ObservableCollection<Variable>();
        private ObservableCollection<Variable> outputs = new ObservableCollection<Variable>();
        private ObservableCollection<Variable> settings = new ObservableCollection<Variable>();
        private ObservableCollection<Variable> constants = new ObservableCollection<Variable>();

        protected Engine NodeEngine;

        #endregion

        #region Public Properties
        public EntityTypeEnum EntityType { get => EntityTypeEnum.Node; }

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

        public string NodeName
        {
            get => nodeName;
            set
            {
                if (nodeName == value) return;
                nodeName = value;
                OnPropertyChanged();
            }
        }

        public string NodeDescription
        {
            get => nodeDescription;
            set
            {
                if (nodeDescription == value) return;
                nodeDescription = value;
                OnPropertyChanged();
            }
        }

        public string NodeCategory
        {
            get => nodeCategory;
            set
            {
                if (nodeCategory == value) return;
                nodeCategory = value;
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

        public EntityPosition Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                OnPropertyChanged();
            }
        }

        public EntitySize Size
        {
            get => size;
            set
            {
                if (size == value) return;
                size = value;
                OnPropertyChanged();
            }
        }

        public bool HasChildren
        {
            get => hasChildren;
            set
            {
                if (hasChildren == value) return;
                hasChildren = value;
                OnPropertyChanged();
            }
        }

        public bool AddInputsAllowed
        {
            get => addInputsAllowed;
            set
            {
                if (addInputsAllowed == value) return;
                addInputsAllowed = value;
                OnPropertyChanged();
            }
        }

        public bool AddOutputsAllowed
        {
            get => addOutputsAllowed;
            set
            {
                if (addOutputsAllowed == value) return;
                addOutputsAllowed = value;
                OnPropertyChanged();
            }
        }

        public bool UseInputs
        {
            get => useInputs;
            set
            {
                if (useInputs == value) return;
                useInputs = value;
                OnPropertyChanged();
            }
        }

        public bool UseOutputs
        {
            get => useOutputs;
            set
            {
                if (useOutputs == value) return;
                useOutputs = value;
                OnPropertyChanged();
            }
        }

        public bool UseSettings
        {
            get => useSettings;
            set
            {
                if (useSettings == value) return;
                useSettings = value;
                OnPropertyChanged();
            }
        }

        public bool UseConstants
        {
            get => useConstants;
            set
            {
                if (useConstants == value) return;
                useConstants = value;
                OnPropertyChanged();
            }
        }

        public bool UseIcon
        {
            get => useIcon;
            set
            {
                if (useIcon == value) return;
                useIcon = value;
                OnPropertyChanged();
            }
        }

        public bool UseHeader
        {
            get => useHeader;
            set
            {
                if (useHeader == value) return;
                useHeader = value;
                OnPropertyChanged();
            }
        }

        public bool VisbileInToolbox
        {
            get => visbileInToolbox;
            set
            {
                if (visbileInToolbox == value) return;
                visbileInToolbox = value;
                OnPropertyChanged();
            }
        }

        public object Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                UseIcon = value != null;
                icon = value;
                OnPropertyChanged();
            }
        }

        public NodeStyleEnum NodeStyle
        {
            get => nodeStyle;
            set
            {
                if (nodeStyle == value) return;
                nodeStyle = value;
                Size = NodeSizeHelper.GetDefaultSize(value);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Variable> Inputs
        {
            get => inputs;
            set
            {
                if (inputs == value) return;
                inputs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Variable> Outputs
        {
            get => outputs;
            set
            {
                if (outputs == value) return;
                outputs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Variable> Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Variable> Constants
        {
            get => constants;
            set
            {
                if (constants == value) return;
                constants = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public Node()
        {
            InputsChangedhandler = new NotifyCollectionChangedEventHandler(Inputs_CollectionChanged);
            OutputsChangedhandler = new NotifyCollectionChangedEventHandler(Outputs_CollectionChanged);
            ConstantsChangedhandler = new NotifyCollectionChangedEventHandler(Constants_CollectionChanged);
            SettingsChangedhandler = new NotifyCollectionChangedEventHandler(Settings_CollectionChanged);

            Inputs.CollectionChanged += InputsChangedhandler;
            Outputs.CollectionChanged += OutputsChangedhandler;
            Constants.CollectionChanged += ConstantsChangedhandler;
            Settings.CollectionChanged += SettingsChangedhandler;
        }

        public void Dispose()
        {
            Inputs.CollectionChanged -= InputsChangedhandler;
            Outputs.CollectionChanged -= OutputsChangedhandler;
            Constants.CollectionChanged -= ConstantsChangedhandler;
            Settings.CollectionChanged -= SettingsChangedhandler;

            GC.SuppressFinalize(this);
        }

        private void Inputs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(InputsCollectionName);
            }
        }

        private void Outputs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(OutputsCollectionName);
            }
        }

        private void Constants_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(ConstantsCollectionName);
            }
        }

        private void Settings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(SettingsCollectionName);
            }
        }
        #endregion

        #region Private Methods
        private static void AddVariable<T>(ObservableCollection<T> List, T Variable)
        {
            var index = List.IndexOf(Variable);
            if (index >= 0)
            {
                List.Insert(index, Variable);
            }
            else
            {
                List.Add(Variable);
            }
        }
        private static void RemoveVariable<T>(ObservableCollection<T> List, T Variable)
        {
            var index = List.IndexOf(Variable);
            if (index >= 0)
            {
                List.RemoveAt(index);
            }
            else
            {
                List.Remove(Variable);
            }
        }
        #endregion

        #region Public Methods

        #region Get Varable Methods
        public Variable GetInput(Guid id)
        {
            return Inputs.FirstOrDefault(x => x.Id == id);
        }

        public Variable GetInputs(string name)
        {
            return Inputs.FirstOrDefault(x => x.Name == name);
        }

        public Variable GetOutput(Guid id)
        {
            return Outputs.FirstOrDefault(x => x.Id == id);
        }

        public Variable GetOutput(string name)
        {
            return Outputs.FirstOrDefault(x => x.Name == name);
        }

        public Variable GetSetting(Guid id)
        {
            return Settings.FirstOrDefault(x => x.Id == id);
        }

        public Variable GetSetting(string name)
        {
            return Settings.FirstOrDefault(x => x.Name == name);
        }

        public Variable GetConstant(Guid id)
        {
            return Constants.FirstOrDefault(x => x.Id == id);
        }

        public Variable GetConstant(string name)
        {
            return Constants.FirstOrDefault(x => x.Name == name);
        }
        #endregion

        #region Variable Methods
        public virtual void AddVariable(Variable variable)
        {
            if (variable.VariableType == VariableType.Unset)
                return;

            switch (variable.VariableType)
            {
                case VariableType.Input:
                    AddVariable(Inputs, variable);
                    OnInputAdded?.Invoke(this);
                    break;
                case VariableType.Output:
                    AddVariable(Outputs, variable);
                    OnOutputAdded?.Invoke(this);
                    break;
                case VariableType.Constant:
                    AddVariable(Constants, variable);
                    OnConstantAdded?.Invoke(this);
                    break;
                case VariableType.Setting:
                    AddVariable(Settings, variable);
                    OnSettingAdded?.Invoke(this);
                    break;
            }

            if (NodeEngine != null)
            {
                variable.Initialize();
                switch (variable.VariableType)
                {
                    case VariableType.Input:
                        variable.OnValueChange += NodeEngine.OnInputChanges;
                        break;
                    case VariableType.Output:
                        variable.OnValueChange += NodeEngine.OnOutputChanges;
                        break;
                    case VariableType.Constant:
                        variable.OnValueChange += NodeEngine.OnConstantChanges;
                        break;
                    case VariableType.Setting:
                        variable.OnValueChange += NodeEngine.OnSettingChanges;
                        break;
                }
            }
        }

        public virtual void RemoveVariable(Variable variable)
        {
            if (variable.VariableType == VariableType.Unset)
                return;

            switch (variable.VariableType)
            {
                case VariableType.Input:
                    RemoveVariable(Inputs, variable);
                    OnInputRemoved?.Invoke(this);
                    break;
                case VariableType.Output:
                    RemoveVariable(Outputs, variable);
                    OnOutputRemoved?.Invoke(this);
                    break;
                case VariableType.Constant:
                    RemoveVariable(Constants, variable);
                    OnConstantRemoved?.Invoke(this);
                    break;
                case VariableType.Setting:
                    RemoveVariable(Settings, variable);
                    OnSettingRemoved?.Invoke(this);
                    break;
            }

            switch (variable.VariableType)
            {
                case VariableType.Input:
                    variable.OnValueChange -= NodeEngine.OnInputChanges;
                    break;
                case VariableType.Output:
                    variable.OnValueChange -= NodeEngine.OnOutputChanges;
                    break;
                case VariableType.Constant:
                    variable.OnValueChange -= NodeEngine.OnConstantChanges;
                    break;
                case VariableType.Setting:
                    variable.OnValueChange -= NodeEngine.OnSettingChanges;
                    break;
            }
        }
        #endregion

        #region Reset Variable Methods
        public bool ResetAllToDefaultValue()
        {
            foreach (var node in Inputs)
            {
                node.Initialize();
            }

            foreach (var node in Outputs)
            {
                node.Initialize();
            }

            foreach (var node in Constants)
            {
                node.Initialize();
            }

            foreach (var node in Settings)
            {
                node.Initialize();
            }
            return true;
        }

        public bool ResetInputsToDefaultValue()
        {
            foreach (var node in Inputs)
            {
                node.Initialize();
            }
            return true;
        }

        public bool ResetOutputsToDefaultValue()
        {
            foreach (var node in Outputs)
            {
                node.Initialize();
            }
            return true;
        }

        public bool ResetConstantsToDefaultValue()
        {
            foreach (var node in Constants)
            {
                node.Initialize();
            }
            return true;
        }

        public bool ResetSettingsToDefaultValue()
        {
            foreach (var node in Settings)
            {
                node.Initialize();
            }
            return true;
        }
        #endregion

        #region Engine related Methods
        public virtual void OnRemove()
        {
            foreach (var node in Inputs)
            {
                node.OnValueChange -= NodeEngine.OnInputChanges;
            }

            foreach (var node in Outputs)
            {
                node.OnValueChange -= NodeEngine.OnOutputChanges;
            }

            foreach (var node in Constants)
            {
                node.OnValueChange -= NodeEngine.OnConstantChanges;
            }

            foreach (var node in Settings)
            {
                node.OnValueChange -= NodeEngine.OnSettingChanges;
            }
            NodeEngine = null;
            OnRemovedFromEngine?.Invoke(this);
        }

        public virtual bool OnAdd(Engine engine)
        {
            NodeEngine = engine;
            if (Name == string.Empty)
                Name = NamesHelper.GenerateName(NodeEngine.Nodes.Select(x => x.name).ToList(), NodeName);

            foreach (var input in Inputs)
            {
                input.OnValueChange += NodeEngine.OnInputChanges;
            }

            foreach (var output in Outputs)
            {
                output.OnValueChange += NodeEngine.OnOutputChanges;
            }

            foreach (var constant in Constants)
            {
                constant.OnValueChange += NodeEngine.OnConstantChanges;
            }

            foreach (var internal_variable in Settings)
            {
                internal_variable.OnValueChange += NodeEngine.OnSettingChanges;
            }
            OnAddedToEngine?.Invoke(this);
            return true;
        }
        #endregion

        #endregion

        #region Abstract Methods
        public virtual void Loop() { }
        public virtual void OnUpdate() { }

        public virtual void Initialize() { }
        public virtual void EngineStarted() { }
        public virtual void EngineStopped() { }

        public virtual void OnInputChanges(Variable variable) { }
        public virtual void OnOutputChanges(Variable variable)
        {
            var sourceList = from link in NodeEngine?.GetLinksForOutput(variable) let input = NodeEngine?.GetVariable(link.InputId, VariableType.Input) where input != null select input;
            foreach (var input in sourceList)
            {
                input.Value = variable.Value;
            }
        }
        public virtual void OnConstantChanges(Variable variable) { }
        public virtual void OnSettingChanges(Variable variable) { }
        public virtual Variable GetReferenceVariable() => null;
        #endregion
    }
}
