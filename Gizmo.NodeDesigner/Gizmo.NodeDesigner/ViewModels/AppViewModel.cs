using Gizmo.NodeBase;
using Gizmo.NodeFramework;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace Gizmo.NodeDesigner
{
    public class AppViewModel : BaseViewModel, IDisposable
    {
        private Project currentProject;
        readonly IDialog dialogService;
        readonly IProjectItemDialog projectItemService;
        readonly ISerialization serializationService;
        readonly IAppSettingsDialog appSettingsService;

        private ProjectItem root;
        private string appPath;
        private AppSettings settings = new AppSettings();

        public Project CurrentProject
        {
            get => currentProject;
            set
            {
                if (currentProject == value) return;
                currentProject = value;
                OnPropertyChanged();
            }
        }
        public ProjectItem Root
        {
            get => root;
            set
            {
                if (root == value) return;
                root = value;
                OnPropertyChanged();
            }
        }
        public AppSettings Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
                OnPropertyChanged();
            }
        }

        public ProjectItem SelectedProjectItem => Root.SelectedItem;

        public AppViewModel(IDialog defaultDialogService, IProjectItemDialog projectService, ISerialization jsonService, IAppSettingsDialog appSettingsDialog)
        {
            appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gizmo\\NodeDesigner");
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            var SettingsFile = Path.Combine(appPath, "Settings.dat");
            dialogService = defaultDialogService;
            projectItemService = projectService;
            serializationService = jsonService;
            appSettingsService = appSettingsDialog;

            if (new FileInfo(SettingsFile).Exists)
            {
                Settings = serializationService.OpenSettings(SettingsFile);
            }
            else
            {
                serializationService.SaveSettings(SettingsFile, Settings);
            }

            CurrentProject = new Project();
            CurrentProject = serializationService.OpenProject(Path.Combine(appPath, "test.prj"));

            CurrentProject.ProjectEngine.Initialise();

            CurrentProject.ProjectEngine.Modules.CollectionChanged += Modules_CollectionChanged;

            //var dc01 = new DecimalNode() { ModuleId = CurrentProject.ProjectEngine.SelectedModule.Id, Position = new EntityPosition(10, 10, true) };
            //var dc02 = new DecimalNode() { ModuleId = CurrentProject.ProjectEngine.SelectedModule.Id, Position = new EntityPosition(10, 30, true) };
            //var sub = new MathSubNode() { ModuleId = CurrentProject.ProjectEngine.SelectedModule.Id, Position = new EntityPosition(200, 10, true) };
            //dc01.Initialize();
            //dc01.Outputs[0].Value = 22.0m;
            //dc02.Initialize();
            //dc02.Outputs[0].Value = 12.0m;
            //sub.Initialize();
            //CurrentProject.ProjectEngine.AddNode(dc01);
            //CurrentProject.ProjectEngine.AddNode(dc02);
            //CurrentProject.ProjectEngine.AddNode(sub);
            //CurrentProject.ProjectEngine.AddLink(dc01.Outputs[0].Id, sub.Inputs[0].Id);
            //CurrentProject.ProjectEngine.AddLink(dc02.Outputs[0].Id, sub.Inputs[1].Id);
            //serializationService.SaveProject(Path.Combine(appPath, "test.prj"), CurrentProject);

            Root = new ProjectItem() { Children = { ProjectItem.CreateRoot() } };

            foreach (var node in CurrentProject.ProjectEngine.Modules)
            {
                AddModule(node, Root.GetItemByType(ProjectItemType.Modules));
            }

            foreach (var node in CurrentProject.Library)
            {
                AddLibraryModule(node, Root.GetItemByType(ProjectItemType.Library));
            }

            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }

        }

        public void saveProject()
        {
            serializationService.SaveProject(Path.Combine(appPath, "test.prj"), CurrentProject);
        }

        private void Modules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Module node in e.NewItems)
                {
                    AddModule(node, Root.GetItemByType(ProjectItemType.Modules));
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Module node in e.OldItems)
                {
                    RemoveModule(node, Root.GetItemByType(ProjectItemType.Modules));
                }
            }
        }

        internal void SetModuleSelected(Guid id)
        {
            foreach (var module in CurrentProject.ProjectEngine.Modules)
            {
                if (module.Id == id)
                {
                    module.SetState(true, true);
                }
            }
        }

        private void OnModuleUnselected(Module module)
        {
            foreach (var node in Root.GetItemsByType(ProjectItemType.Module))
            {
                if (node.Id == module.Id)
                {
                    node.SetDotState(false);
                }
            }
        }

        private void OnModuleSelected(Module module)
        {
            foreach (var node in Root.GetItemsByType(ProjectItemType.Module))
            {
                if (node.Id == module.Id)
                {
                    node.SetDotState(true);
                }
            }
        }

        private void SelectedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedItem"))
            {
                OnNamedPropertyChanged("SelectedProjectItem");
            }
        }

        public void Dispose()
        {
            CurrentProject.ProjectEngine.Modules.CollectionChanged -= Modules_CollectionChanged;

            GC.SuppressFinalize(this);
        }

        private void AddModule(Module module, ProjectItem parent)
        {
            module.OnModuleSelected += OnModuleSelected;
            module.OnModuleUnselected += OnModuleUnselected;
            var newItem = new ProjectItem() { Id = module.Id, Name = module.Name, Description = module.Description, ParentId = parent.Id, ParentType = ProjectItemType.Modules, Type = ProjectItemType.Module };
            newItem.SetDotState(module.IsSelected);
            parent.Children.Add(newItem);
        }
        private void RemoveModule(Module module, ProjectItem parent)
        {
            module.OnModuleSelected -= OnModuleSelected;
            module.OnModuleUnselected -= OnModuleUnselected;
            parent.Children.Remove(Root.GetItemById(module.Id));
        }

        private void AddLibraryModule(Module module, ProjectItem parent)
        {
            parent.Children.Add(new ProjectItem() { Name = module.Name, Description = module.Description, ParentId = parent.Id, ParentType = ProjectItemType.Library, Type = ProjectItemType.LibraryModule });
        }
    }
}
