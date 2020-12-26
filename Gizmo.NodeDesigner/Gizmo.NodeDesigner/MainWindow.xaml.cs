using Gizmo.NodeFramework;
using Gizmo.NodeFrameworkUI;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Gizmo.NodeDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppViewModel appvm;

        private static List<string> GetNodes()
        {
            var a = AppDomain.CurrentDomain.GetAssemblies();
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(Node).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.AssemblyQualifiedName).ToList();
        }

        public MainWindow()
        {
            InitializeComponent();
            appvm = new AppViewModel(new DefaultDialogService(), new ProjectItemService(), new SerializationService(), new AppSettingsDialogService());
            DataContext = appvm;
            UINodeThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
            FillNodeToolBox();
        }

        private void FillNodeToolBox()
        {
            foreach (var node in GetNodes())
            {
                var NodeType = Type.GetType(node);
                var NodeInstance = Activator.CreateInstance(NodeType) as Node;
                if (NodeInstance.VisbileInToolbox)
                {
                    NodeInstance.Initialize();
                    var ToolBoxCreated = false;
                    foreach (var tool in wpToolbox.Children)
                    {
                        if (tool is NodeToolbox)
                        {
                            if ((tool as NodeToolbox).Category == NodeInstance.NodeCategory)
                            {
                                ToolBoxCreated = true;
                                (tool as NodeToolbox).Items.Add(new NodeToolboxItem()
                                {
                                    NodeName = NodeInstance.NodeName,
                                    NodeType = NodeType,
                                    Content = NodeInstance.Icon != null && NodeInstance.Icon.GetType() == typeof(string) ? Geometry.Parse(NodeInstance.Icon.ToString()) : null
                                });
                            }
                        }
                    }

                    if (!ToolBoxCreated)
                    {
                        var newTool = new NodeToolbox() { Category = NodeInstance.NodeCategory };
                        wpToolbox.Children.Add(newTool);
                        newTool.Items.Add(new NodeToolboxItem()
                        {
                            NodeName = NodeInstance.NodeName,
                            NodeType = NodeType,
                            Content = NodeInstance.Icon != null && NodeInstance.Icon.GetType() == typeof(string) ? Geometry.Parse(NodeInstance.Icon.ToString()) : null
                        });
                    }
                }
                NodeInstance.Dispose();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Theme Changing Events

        private void MiDark_Click(object sender, RoutedEventArgs e)
        {
            appvm.Settings.Theme = UINodeThemeEnum.Dark;
            UINodeThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
        }
        private void MiLight_Click(object sender, RoutedEventArgs e)
        {
            appvm.Settings.Theme = UINodeThemeEnum.Light;
            UINodeThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
        }
        #endregion

        private void ProjectTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender != null)
            {
                if (appvm.SelectedProjectItem.Type == ProjectItemType.Module)
                {
                    appvm.SetModuleSelected(appvm.SelectedProjectItem.Id);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            appvm.saveProject();
        }
    }
}
