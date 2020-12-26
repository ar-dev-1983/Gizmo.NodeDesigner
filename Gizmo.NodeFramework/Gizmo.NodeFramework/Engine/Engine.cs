using Gizmo.NodeBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Gizmo.NodeFramework
{
    public class Engine : EntityViewModel, IDisposable
    {
        private const string LinksCollectionName = "Links";
        private const string ModulesCollectionName = "Modules";
        private const string LogCollectionName = "Log";
        private const string SelectedModuleName = "SelectedModule";
        private const string NodesCollectionName = "Nodes";
        #region Event Handlers
        private NotifyCollectionChangedEventHandler LogChangedhandler;
        private NotifyCollectionChangedEventHandler ModulesChangedhandler;
        private NotifyCollectionChangedEventHandler NodesChangedhandler;
        private NotifyCollectionChangedEventHandler LinksChangedhandler;

        public delegate void NodeEventHandler(Node node);
        public delegate void VariableEventHandler(Variable variable);
        public delegate void LinkEventHandler(Link link);
        #endregion

        #region Private Properties
        private double updateInterval = 10; // ms
        private DateTime engineTime;
        private EngineMode mode = EngineMode.LoopUpdate;
        private EngineState state = EngineState.Stop;

        private ObservableCollection<LogMessage> log = new ObservableCollection<LogMessage>();
        private ObservableCollection<Module> modules = new ObservableCollection<Module>();
        private ObservableCollection<Node> nodes = new ObservableCollection<Node>();
        private ObservableCollection<Link> links = new ObservableCollection<Link>();

        private readonly ObservableCollection<Variable> detectedVariables = new ObservableCollection<Variable>();
        private readonly object NodesLock = new object();
        private readonly object LinksLock = new object();
        private readonly object ModulesLock = new object();

        private Module lastSelectedModule = null;
        #endregion

        #region Public Properties
        public double UpdateInterval
        {
            get => updateInterval;
            set
            {
                if (updateInterval == value) return;
                updateInterval = value;
                OnPropertyChanged();
            }
        }

        public DateTime EngineTime
        {
            get => engineTime;
            set
            {
                if (engineTime == value) return;
                engineTime = value;
                OnPropertyChanged();
            }
        }

        public EngineMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                OnPropertyChanged();
            }
        }

        public EngineState State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LogMessage> Log
        {
            get => log;
            set
            {
                if (log == value) return;
                log = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Module> Modules
        {
            get => modules;
            set
            {
                if (modules == value) return;
                modules = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Node> Nodes
        {
            get => nodes;
            set
            {
                if (nodes == value) return;
                nodes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Link> Links
        {
            get => links;
            set
            {
                if (links == value) return;
                links = value;
                OnPropertyChanged();
            }
        }
        public Module LastSelectedModule
        {
            get => lastSelectedModule;
            private set
            {
                if (lastSelectedModule == value) return;
                lastSelectedModule = value;
                OnPropertyChanged();
            }
        }
        public Module SelectedModule => Modules.Where(x => x.IsSelected).FirstOrDefault();
        #endregion

        #region Engine Actions
        public event Action OnSelectedModuleChanged;

        public event Action OnStart;
        public event Action OnStop;
        public event Action OnPause;
        public event Action OnStep;
        public event Action OnUpdateLoop;
        public event Action OnUpdateOnce;
        public event Action OnRemoveAllNodesAndLinks;
        #endregion

        #region Engine Event Handlers
        public event LinkEventHandler OnAddLink;
        public event LinkEventHandler OnRemoveLink;
        public event NodeEventHandler OnAddNode;
        public event NodeEventHandler OnRemoveNode;

        public event VariableEventHandler OnInputUpdated;
        public event VariableEventHandler OnOutputUpdated;
        public event VariableEventHandler OnConstantUpdated;
        public event VariableEventHandler OnInternalUpdated;
        #endregion

        #region Constructors and Management
        public Engine()
        {
            Mode = EngineMode.LoopUpdate;
            EngineTime = DateTime.Now;
            AddLogInformation($"Engine created.");
            UpdateNodes();
        }

        public Engine(EngineMode mode)
        {
            Mode = mode;
            EngineTime = DateTime.Now;
            AddLogInformation($"Engine created.");

            if (Mode == EngineMode.LoopUpdate)
                UpdateNodes();
        }

        public void Initialise()
        {
            if (modules is null)
            {
                Modules = new ObservableCollection<Module>();
            }

            if (Modules.Count == 0)
            {
                Modules.Add(new Module() { Type = ModuleType.Init });
                Modules.Add(new Module() { Type = ModuleType.Main });
                Modules.Add(new Module() { Type = ModuleType.Exit });

                foreach (var module in Modules)
                {
                    module.OnModuleSelected += Module_OnModuleSelected;
                    module.OnModuleUnselected += Module_OnModuleUnselected;
                }
                Modules[1].SetState(true, true);
            }
            else
            {
                foreach (var module in Modules)
                {
                    module.OnModuleSelected += Module_OnModuleSelected;
                    module.OnModuleUnselected += Module_OnModuleUnselected;
                }
            }

            LogChangedhandler = new NotifyCollectionChangedEventHandler(Log_CollectionChanged);
            ModulesChangedhandler = new NotifyCollectionChangedEventHandler(Modules_CollectionChanged);
            NodesChangedhandler = new NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
            LinksChangedhandler = new NotifyCollectionChangedEventHandler(Links_CollectionChanged);

            Log.CollectionChanged += LogChangedhandler;
            Modules.CollectionChanged += ModulesChangedhandler;
            Nodes.CollectionChanged += NodesChangedhandler;
            Links.CollectionChanged += LinksChangedhandler;

            AddLogInformation($"Engine initialized.");
        }

        private void Module_OnModuleUnselected(Module module)
        {
            if (module != null)
            {
                LastSelectedModule = module;
            }
        }

        private void Module_OnModuleSelected(Module module)
        {
            if (module != null)
            {
                foreach (var item in Modules)
                {
                    if (!Equals(item, module))
                        item.SetState(false, true);
                }
                OnNamedPropertyChanged(SelectedModuleName);
                OnSelectedModuleChanged?.Invoke();
            }
        }

        private void Log_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(LogCollectionName);
            }
        }

        private void Modules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(ModulesCollectionName);
                if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (e.NewItems != null)
                    {
                        foreach (Module module in e.NewItems)
                        {
                            module.OnModuleSelected += Module_OnModuleSelected;
                            module.OnModuleUnselected += Module_OnModuleUnselected;
                        }
                    }
                }
                if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (e.OldItems != null)
                    {
                        foreach (Module module in e.OldItems)
                        {
                            module.OnModuleSelected -= Module_OnModuleSelected;
                            module.OnModuleUnselected -= Module_OnModuleUnselected;
                        }
                    }
                }
            }
        }

        private void Nodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(NodesCollectionName);
            }
        }

        private void Links_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                OnNamedPropertyChanged(LinksCollectionName);
            }
        }

        public void AddLogError(string error)
        {
            Log.Add(new LogMessage(error, LogMessageType.Error));
        }

        public void AddLogWarning(string warning)
        {
            Log.Add(new LogMessage(warning, LogMessageType.Warning));
        }

        public void AddLogInformation(string information)
        {
            Log.Add(new LogMessage(information, LogMessageType.Information));
        }

        public void Dispose()
        {
            Log.CollectionChanged -= LogChangedhandler;
            foreach (Module module in Modules)
            {
                module.OnModuleSelected -= Module_OnModuleSelected;
                module.OnModuleUnselected -= Module_OnModuleUnselected;
            }
            Modules.CollectionChanged -= ModulesChangedhandler;
            Nodes.CollectionChanged -= NodesChangedhandler;
            Links.CollectionChanged -= LinksChangedhandler;

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Engine Control Methods
        public void Start()
        {
            if (State == EngineState.Start || State == EngineState.Play)
                return;

            OnStart?.Invoke();
            lock (NodesLock)
            {
                if (Nodes == null || !Nodes.Any())
                {
                    return;
                }


                foreach (var node in Nodes)
                {
                    node.EngineStarted();
                }
            }
            State = EngineState.Start;
            detectedVariables.Clear();
            UpdateNodesFromLinks();
            State = EngineState.Play;
            AddLogInformation("Engine started.");
        }

        public void Stop()
        {
            if (State == EngineState.Stop)
                return;

            OnStop?.Invoke();

            State = EngineState.Stop;
            detectedVariables.Clear();

            lock (NodesLock)
            {
                if (Nodes == null || !Nodes.Any())
                {
                    return;
                }

                foreach (var node in Nodes)
                {
                    node.EngineStopped();
                    node.ResetAllToDefaultValue();
                }
            }
            AddLogInformation("Engine stopped.");
        }

        public void Pause()
        {
            if (State == EngineState.Pause)
                return;

            OnPause?.Invoke();

            State = EngineState.Pause;
            detectedVariables.Clear();
            AddLogInformation("Engine paused.");
        }

        public void Step()
        {
            if (State != EngineState.Step)
                return;

            OnStep?.Invoke();

            State = EngineState.Step;
            detectedVariables.Clear();
            AddLogInformation("Engine single update");
        }

        private void UpdateNodesFromLinks()
        {
            if (Links == null)
                return;

            foreach (var link in Links)
            {
                if (State == EngineState.Stop || State == EngineState.Pause)
                    return;

                var input = GetVariable(link.InputId, VariableType.Input);
                var output = GetVariable(link.OutputId, VariableType.Output);
                input.Value = output.Value;

                var node = GetNodeByInput(input);

                node.OnInputChanges(input);
            }
        }

        public void OnInputChanges(Variable variable)
        {
            if (State != EngineState.Play)
                return;

            Node node = GetNodeByInputId(variable.Id);

            if (node == null)
                return;

            if (detectedVariables.Contains(variable))
            {
                try
                {
                    detectedVariables.Remove(variable);
                }
                catch { }
                return;
            }
            detectedVariables.Add(variable);

            OnInputUpdated?.Invoke(variable);

            node.OnInputChanges(variable);

            try
            {
                if (detectedVariables.Contains(variable))
                    detectedVariables.Remove(variable);
            }
            catch { }
        }

        public void OnOutputChanges(Variable variable)
        {
            if (State != EngineState.Play)
                return;

            Node node = GetNodeByOutput(variable);
            if (node == null)
                return;

            OnOutputUpdated?.Invoke(variable);

            node.OnOutputChanges(variable);
        }

        public void OnConstantChanges(Variable variable)
        {
            if (State != EngineState.Play)
                return;

            Node node = GetNodeByConstant(variable);
            if (node == null)
                return;

            OnConstantUpdated?.Invoke(variable);

            node.OnConstantChanges(variable);
        }

        public void OnSettingChanges(Variable variable)
        {
            if (State != EngineState.Play)
                return;

            Node node = GetNodeBySetting(variable);
            if (node == null)
                return;

            OnInternalUpdated?.Invoke(variable);

            node.OnSettingChanges(variable);
        }

        private void UpdateNodes()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(10);
                    if ((DateTime.Now - EngineTime).TotalMilliseconds < updateInterval)
                    {
                        continue;
                    }

                    EngineTime = DateTime.Now;

                    lock (NodesLock)
                    {
                        if (Nodes == null || !Nodes.Any())
                        {
                            continue;
                        }

                        try
                        {
                            foreach (var node in Nodes)
                            {
                                node.Loop();
                                if (State == EngineState.Stop || State == EngineState.Pause)
                                {
                                    break;
                                }
                            }

                            if (State == EngineState.Play)
                            {
                                OnUpdateLoop?.Invoke();
                            }
                        }
                        catch { }
                    }
                }
            });
        }

        public void UpdateOnce()
        {
            UpdateNodesOnce();
        }

        private void UpdateNodesOnce()
        {
            lock (NodesLock)
            {
                if (Nodes == null || !Nodes.Any())
                {
                    return;
                }

                try
                {
                    foreach (var node in Nodes)
                    {
                        node.Loop();
                        if (State == EngineState.Stop || State == EngineState.Pause)
                            return;
                    }

                    if (State == EngineState.Play)
                    {
                        OnUpdateOnce?.Invoke();
                    }
                }
                catch { }
            }

        }
        #endregion

        #region Nodes Management Methods
        public ObservableCollection<Node> GetNodes()
        {
            return Nodes;
        }

        public Node GetNode(Guid id)
        {
            lock (NodesLock)
                return Nodes.FirstOrDefault(x => x.Id == id);
        }

        public bool AddNode(Node node)
        {
            if (SelectedModule is null)
            {
                AddLogError($"Node \"{node.GetType().Name}\" creation error. Module not selected.");
                return false;
            }

            if (node.ModuleId != SelectedModule.Id && GetModuleNode(node.ModuleId) == null)
            {
                AddLogError($"Node \"{node.GetType().Name}\" creation error. Module does not exist or not selected.");
                return false;
            }

            bool checkNodeCanBeAdded = node.OnAdd(this);
            if (!checkNodeCanBeAdded)
            {
                AddLogError($"Node \"{node.GetType().Name}\" creation error. Operation aborted.");

                return false;
            }
            lock (NodesLock)
                Nodes.Add(node);

            AddLogInformation($"Node \"{node.GetType().Name}\" added.");

            OnAddNode?.Invoke(node);

            return true;
        }

        public int AddNodes(List<Node> nodes)
        {
            int count = 0;

            List<Node> addedNodes = new List<Node>();
            foreach (var node in from node in nodes let added = AddNode(node) where added select node)
            {
                addedNodes.Add(node);
                count++;
            }

            return count;
        }

        public void RemoveNode(Guid id)
        {
            Node RemovedNode = GetNode(id);

            if (RemovedNode == null)
            {
                return;
            }

            OnRemoveNode?.Invoke(RemovedNode);

            AddLogInformation($"Node \"{RemovedNode.GetType().Name}\" removed.");

            lock (LinksLock)
                Nodes.Remove(RemovedNode);
        }

        public void RemoveNode(Node node)
        {
            if (node == null)
            {
                return;
            }

            foreach (var input in node.Inputs)
            {
                if (input.IsConnected)
                {
                    var link = GetLinkForInput(input);
                    if (link != null)
                    {
                        RemoveLink(link);
                    }
                }
            }

            foreach (var output in node.Outputs)
            {
                if (output.IsConnected)
                {
                    var links = GetLinksForOutput(output);
                    if (links != null)
                    {
                        RemoveLinks(links);
                    }
                }
            }

            OnRemoveNode?.Invoke(node);

            AddLogInformation($"Node \"{node.GetType().Name}\" removed.");

            lock (LinksLock)
                Nodes.Remove(node);
        }
        #endregion

        #region Links Management Methods
        public ObservableCollection<Link> GetLinks()
        {
            return Links;
        }

        public Link AddLink(Guid outputId, Guid inputId)
        {
            var input = GetVariable(inputId, VariableType.Input);
            var output = GetVariable(outputId, VariableType.Output);

            if (input == null || output == null)
            {
                return null;
            }

            return AddLink(output, input);
        }

        public Link AddLink(Variable output, Variable input)
        {
            if (SelectedModule is null)
            {
                AddLogError("Link creation error. Module does not exist or not selected.");
                return null;
            }

            var inputNode = GetNodeByInput(input);
            var outputNode = GetNodeByOutput(output);

            if (inputNode == null || outputNode == null)
            {
                AddLogError("Link creation error. Input or Output node doesn't exist.");
                return null;
            }

            if (inputNode == outputNode)
            {
                AddLogError("Link creation error. Link sides belongs to same node.");
                return null;
            }

            if (inputNode.ModuleId != outputNode.ModuleId)
            {
                AddLogError("Link creation error. Input or Output node exist in different modules. Use instead a transfer nodes.");
                return null;
            }

            Link link = new Link(input.Id, output.Id, SelectedModule.Id);
            Link oldLink = GetLinkForInput(input);

            if (oldLink != null)
                RemoveLink(oldLink);

            lock (LinksLock)
                Links.Add(link);

            input.IsConnected = true;
            output.IsConnected = true;

            AddLogInformation($"New link from \"{outputNode.GetType().Name}\" to \"{inputNode.GetType().Name}\" added.");

            OnAddLink?.Invoke(link);

            if (State == EngineState.Play)
                input.Value = output.Value;

            return link;
        }

        public int AddLinks(List<Link> links)
        {
            var Links = (from link in links let newLink = AddLink(link.OutputId, link.InputId) where newLink != null select newLink).ToList();
            return Links.Count;
        }

        public void RemoveLink(Variable output, Variable input)
        {
            Link link = GetLink(output, input);

            if (link == null)
            {
                return;
            }

            OnRemoveLink?.Invoke(link);

            AddLogInformation($"New link from \"{output.Name}\" to \"{input.Name}\" added.");

            lock (LinksLock)
                Links.Remove(link);

            input.IsConnected = false;
            output.IsConnected = false;

            input.Value = null;

        }

        public void RemoveLink(Link link)
        {
            var output = GetVariable(link.OutputId, VariableType.Output);
            var input = GetVariable(link.InputId, VariableType.Input);

            if (output == null || input == null)
            {
                return;
            }
            AddLogInformation($"New link from \"{output.Name}\" to \"{input.Name}\" removed.");
            RemoveLink(output, input);
        }

        public void RemoveLinks(List<Link> links)
        {
            foreach (var link in links)
            {
                RemoveLink(link);
            }
        }
        #endregion

        #region Modules Management Methods
        public List<Node> GetModuleNodes(Guid modeleID, bool searchForModuleNodes)
        {
            if (searchForModuleNodes)
            {
                List<Node> nodesList;

                lock (NodesLock)
                    nodesList = nodes.Where(n => n.ModuleId == modeleID).ToList();

                List<ModuleNode> moduleNodes = nodesList.OfType<ModuleNode>().ToList();

                foreach (ModuleNode node in moduleNodes)
                {
                    nodesList.AddRange(GetModuleNodes(node.Id, true));
                }
                return nodesList;
            }
            else
            {
                lock (NodesLock)
                    return nodes.Where(n => n.ModuleId == modeleID).ToList();
            }
        }

        public List<Link> GetModuleLinks(Guid modeleID, bool searchForModuleNodes)
        {

            if (searchForModuleNodes)
            {
                List<Link> linksList;

                lock (LinksLock)
                    linksList = links.Where(n => n.ModuleId == modeleID).ToList();

                List<ModuleNode> moduleNodes = GetModuleNodes(modeleID, true).OfType<ModuleNode>().ToList();

                lock (LinksLock)
                {
                    foreach (ModuleNode node in moduleNodes)
                    {
                        linksList.AddRange(links.Where(n => n.ModuleId == node.Id).ToList());
                    }
                }
                return linksList;
            }
            else
            {
                lock (LinksLock)
                    return links.Where(n => n.ModuleId == modeleID).ToList();
            }
        }

        public ModuleNode GetModuleNode(Guid Id)
        {
            lock (NodesLock)
                return (ModuleNode)nodes.FirstOrDefault(n => n is ModuleNode && n.Id == Id);
        }

        public List<ModuleNode> GetModuleNodes()
        {
            lock (NodesLock)
                return nodes.Where(n => n is ModuleNode).Cast<ModuleNode>().ToList();
        }

        public Module ExtractSelectedToModule(string name, string description)
        {
            var result = new Module() { IsSelected = true, Name = name, Description = description, Type = ModuleType.Unique };

            return result;
        }

        public LibraryModule ExtractToLibrary(string name, string description)
        {
            var result = new LibraryModule() { Name = name, Description = description };

            return result;
        }
        #endregion

        #region Variable Management Methods
        public Variable GetVariable(Guid id, VariableType variableType)
        {
            if (variableType == VariableType.Input)
            {
                return GetInput(id);
            }
            else if (variableType == VariableType.Output)
            {
                return GetOutput(id);
            }
            else if (variableType == VariableType.Constant)
            {
                return GetConstant(id);
            }
            else if (variableType == VariableType.Setting)
            {
                return GetSetting(id);
            }
            else
                return null;
        }

        public Variable GetInput(Guid id)
        {
            lock (NodesLock)
                return nodes.SelectMany(node => node.Inputs).FirstOrDefault(item => item.Id == id);
        }

        public Variable GetOutput(Guid id)
        {
            lock (NodesLock)
                return nodes.SelectMany(node => node.Outputs).FirstOrDefault(item => item.Id == id);
        }

        public Variable GetConstant(Guid id)
        {
            lock (NodesLock)
                return nodes.SelectMany(node => node.Constants).FirstOrDefault(item => item.Id == id);
        }

        public Variable GetSetting(Guid id)
        {
            lock (NodesLock)
                return nodes.SelectMany(node => node.Settings).FirstOrDefault(item => item.Id == id);
        }
        #endregion

        #region Nodes And Links Search Methods
        public Link GetLink(Link link)
        {
            lock (LinksLock)
                return Links.FirstOrDefault(x => x.Id==link.Id);
        }

        public Link GetLink(Variable output, Variable input)
        {
            lock (LinksLock)
                return Links.FirstOrDefault(x => x.InputId == input.Id && x.OutputId == output.Id);
        }

        public Link GetLinkForInput(Variable input)
        {
            lock (LinksLock)
                return Links.FirstOrDefault(x => x.InputId == input.Id);
        }

        public List<Link> GetLinksForOutput(Variable output)
        {
            lock (LinksLock)
                return Links.Where(x => x.OutputId == output.Id).ToList();
        }

        public List<Link> GetLinksForNode(Node node)
        {
            var list = (from input in node.Inputs let link = GetLinkForInput(input) where link != null select link).ToList();
            foreach (var links in from output in node.Outputs let links = GetLinksForOutput(output) where links != null select links)
            {
                list.AddRange(links);
            }

            return list;
        }

        public Node GetNodeByInput(Variable variable)
        {
            lock (NodesLock)
                return Nodes.FirstOrDefault(node => node.Inputs.Contains(variable));
        }

        public Node GetNodeByOutput(Variable variable)
        {
            lock (NodesLock)
                return Nodes.FirstOrDefault(node => node.Outputs.Contains(variable));
        }

        public Node GetNodeByConstant(Variable variable)
        {
            lock (NodesLock)
                return Nodes.FirstOrDefault(node => node.Constants.Contains(variable));
        }

        public Node GetNodeBySetting(Variable variable)
        {
            lock (NodesLock)
                return Nodes.FirstOrDefault(node => node.Settings.Contains(variable));
        }

        public Node GetNodeByInputId(Guid id)
        {
            lock (NodesLock)
                return (from node in Nodes from input in node.Inputs where input.Id == id select node).FirstOrDefault();
        }

        public Node GetNodeByOutputId(Guid id)
        {
            lock (NodesLock)
                return (from node in Nodes from output in node.Outputs where output.Id == id select node).FirstOrDefault();
        }

        public Node GetNodeByConstantId(Guid id)
        {
            lock (NodesLock)
                return (from node in Nodes from constant in node.Constants where constant.Id == id select node).FirstOrDefault();
        }

        public Node GetNodeBySettingId(Guid id)
        {
            lock (NodesLock)
                return (from node in Nodes from setting in node.Settings where setting.Id == id select node).FirstOrDefault();
        }
        #endregion

        #region Cleanup Methods
        public void RemoveAll()
        {
            lock (NodesLock)
                Nodes = new ObservableCollection<Node>();
            lock (LinksLock)
                Links = new ObservableCollection<Link>();
            lock (ModulesLock)
            {
                Modules = new ObservableCollection<Module>
                {
                    new Module() { Type = ModuleType.Init },
                    new Module() { Type = ModuleType.Main },
                    new Module() { Type = ModuleType.Exit }
                };
            }
            OnRemoveAllNodesAndLinks?.Invoke();
            AddLogInformation("All links and nodes are deleted.");
        }
        #endregion

        #region Cloning Management
        //public void CloneNode(string id)
        //{
        //    Node oldNode = GetNode(id);
        //    if (oldNode is PanelNode)
        //    {
        //        string json = NodesEngineSerializer.SerializePanel(id, this);

        //        List<Node> newNodes;
        //        List<Link> newLinks;
        //        NodesEngineSerializer.DeserializePanel(json, out newNodes, out newLinks);

        //        newNodes[0].Position = new Position { X = oldNode.Position.X + 5, Y = oldNode.Position.Y + 20 };

        //        GenerateNewIds(ref newNodes, ref newLinks);

        //        AddNodes(newNodes);
        //        AddLinks(newLinks);

        //        newNodes[0].ResetInputs();
        //    }
        //    else
        //    {
        //        string json = NodesEngineSerializer.SerializeNode(oldNode);
        //        Node newNode = NodesEngineSerializer.DeserializeNode(json);

        //        GenerateNewIds(newNode);

        //        newNode.Position = new Position { X = oldNode.Position.X + 5, Y = oldNode.Position.Y + 20 };
        //        AddNode(newNode, true);
        //        newNode.ResetInputs();
        //    }
        //}

        public void GenerateNewIds(ref List<Node> nodesList, ref List<Link> linksList)
        {
            foreach (var node in nodesList)
            {
                //generate id`s for inputs
                foreach (var input in node.Inputs)
                {
                    Guid oldId = input.Id;
                    input.Id = Guid.NewGuid();

                    //update links
                    foreach (var link in linksList.Where(x => x.InputId == oldId))
                        link.InputId = input.Id;

                    //for panel update input node id
                    if (node is ModuleNode)
                        nodesList.FirstOrDefault(x => x.Id == oldId).Id = input.Id;
                }

                //generate id`s for outputs
                foreach (var output in node.Outputs)
                {
                    Guid oldId = output.Id;
                    output.Id = Guid.NewGuid();

                    //update links
                    foreach (var link in linksList.Where(x => x.OutputId == oldId))
                        link.OutputId = output.Id;

                    //for panel update output node id
                    if (node is ModuleNode)
                        nodesList.FirstOrDefault(x => x.Id == oldId).Id = output.Id;
                }


                if (node is ModuleNode)
                {
                    Guid oldId = node.Id;
                    node.Id = Guid.NewGuid();

                    foreach (var n in nodesList.Where(x => x.ModuleId == oldId))
                        n.ModuleId = node.Id;
                }
                else if (node is ModuleNodeInput || node is ModuleNodeOutput)
                {
                    //id already updated
                }
                else
                {
                    node.Id = Guid.NewGuid();
                }
            }
        }

        public void RenewId(Node node)
        {
            node.Id = Guid.NewGuid();

            foreach (var item in node.Inputs)
            {
                item.Id = Guid.NewGuid();
                item.ParentId = node.Id;
            }
            foreach (var item in node.Outputs)
            {
                item.Id = Guid.NewGuid();
                item.ParentId = node.Id;
            }
            foreach (var item in node.Constants)
            {
                item.Id = Guid.NewGuid();
                item.ParentId = node.Id;
            }
            foreach (var item in node.Settings)
            {
                item.Id = Guid.NewGuid();
                item.ParentId = node.Id;
            }
        }
        #endregion
    }
}
