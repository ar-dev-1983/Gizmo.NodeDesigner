using Gizmo.NodeBase;
using System;

namespace Gizmo.NodeFramework
{
    public class Variable : EntityViewModel, IDisposable
    {
        #region Event Handlers
        public delegate void VariableEventHandler(Variable variable);
        public event VariableEventHandler OnValueChange;
        public event VariableEventHandler OnConnected;
        public event VariableEventHandler OnDisonnected;
        public event VariableEventHandler OnVariableInitialized;
        #endregion

        #region Private Properties
        private int index = -1;
        private bool isConnected = false;
        private Guid id = Guid.NewGuid();
        private Guid? parentId = null;
        private Guid? linkId = null;
        private string name = string.Empty;
        private object val = null;
        private object previousValue = null;
        private object defaultValue = null;
        private bool showName = false;
        private bool showValue = false;
        private bool isEditable;
        private VariableType variableType = VariableType.Unset;
        private Type dataType = null;
        #endregion

        #region Public Properties
        public int Index
        {
            get => index;
            set
            {
                if (index == value) return;
                index = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected == value) return;
                isConnected = value;
                if (value)
                    OnConnected?.Invoke(this);
                else
                    OnDisonnected?.Invoke(this);
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

        public Guid? ParentId
        {
            get => parentId;
            set
            {
                if (parentId == value) return;
                parentId = value;
                OnPropertyChanged();
            }
        }

        public Guid? LinkId
        {
            get => linkId;
            set
            {
                if (linkId == value) return;
                linkId = value;
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

        public object Value
        {
            get { return val; }
            set
            {
                PreviousValue = val;
                val = value;
                OnValueChange?.Invoke(this);
                OnPropertyChanged();
            }
        }

        public object PreviousValue
        {
            get { return previousValue; }
            set
            {
                previousValue = value;
                OnPropertyChanged();
            }
        }

        public object DefaultValue
        {
            get { return defaultValue; }
            set
            {
                defaultValue = value;
                OnPropertyChanged();
            }
        }

        public bool ShowName
        {
            get => showName;
            set
            {
                if (showName == value) return;
                showName = value;
                OnPropertyChanged();
            }
        }
        public bool ShowValue
        {
            get => showValue;
            set
            {
                if (showValue == value) return;
                showValue = value;
                OnPropertyChanged();
            }
        }
        public bool IsEditable
        {
            get => isEditable;
            set
            {
                if (isEditable == value) return;
                isEditable = value;

                if (isEditable)
                    ShowValue = false;

                OnPropertyChanged();
            }
        }

        public VariableType VariableType
        {
            get => variableType;
            set
            {
                if (variableType == value) return;
                variableType = value;
                OnPropertyChanged();
            }
        }

        public Type DataType
        {
            get
            {
                if (dataType == null)
                {
                    if (Value != null)
                        dataType = Value.GetType();
                    else if (DefaultValue != null)
                        dataType = DefaultValue.GetType();
                    else
                        dataType = typeof(object);
                }
                return dataType;
            }
            set
            {
                if (dataType == value) return;
                dataType = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public Variable()
        {

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public Methods

        public void Initialize(object value, bool raiseEvent = false)
        {
            PreviousValue = null;
            Value = value;

            if (raiseEvent)
                OnVariableInitialized?.Invoke(this);
        }

        public void Initialize(bool raiseEvent = false)
        {
            Value = DefaultValue;
            PreviousValue = DefaultValue;

            if (raiseEvent)
                OnVariableInitialized?.Invoke(this);
        }

        public void InitializeInternally(object value, bool raiseEvent = false)
        {
            PreviousValue = null;
            val = value;

            if (raiseEvent)
                OnVariableInitialized?.Invoke(this);
        }
        public void SetValueInternally(object value)
        {
            val = value;
        }
        #endregion
    }
}
