using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeFrameworkUI
{
    public class UINodeTheme
    {

    }

    public enum UINodeThemeEnum
    {
        Dark,
        Light
    }
    public static class UINodeThemeManager
    {
        private static ResourceDictionary _Dark;
        public static ResourceDictionary Dark
        {
            get
            {
                if (_Dark == null)
                {
                    _Dark = new ResourceDictionary
                    {
                        Source = new Uri("/Gizmo.NodeFrameworkUI;Component/Themes/Brushes.Dark.xaml", UriKind.Relative)
                    };
                }
                return _Dark;
            }
        }
        private static ResourceDictionary _Light;
        public static ResourceDictionary Light
        {
            get
            {
                if (_Light == null)
                {
                    _Light = new ResourceDictionary
                    {
                        Source = new Uri("/Gizmo.NodeFrameworkUI;Component/Themes/Brushes.Light.xaml", UriKind.Relative)
                    };
                }
                return _Light;
            }
        }
        public static void ApplyTheme(UINodeThemeEnum _theme)
        {
            if (Application.Current.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = Application.Current.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static void ApplyThemeToContentControl(ContentControl control, UINodeThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static void ApplyThemeToHeaderedContentControl(HeaderedContentControl control, UINodeThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static void ApplyThemeToItemsControl(ItemsControl control, UINodeThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static void ApplyThemeToHeaderedItemsControl(HeaderedItemsControl control, UINodeThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static void ApplyThemeToTabControl(TabControl control, UINodeThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static void ApplyThemeToWindow(Window control, UINodeThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(Dark);
                MergedDictionaries.Remove(Light);
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        MergedDictionaries.Add(Dark);
                        break;
                    case UINodeThemeEnum.Light:
                        MergedDictionaries.Add(Light);
                        break;
                }
            }
        }

        public static object GetResource(UINodeThemeEnum _theme, string _resourceName)
        {
            object result = null;

            try
            {
                switch (_theme)
                {
                    case UINodeThemeEnum.Dark:
                        result = Dark[(from node in Dark.Keys.OfType<ComponentResourceKey>() where node.ResourceId.ToString() == _resourceName select node).First()];
                        break;
                    case UINodeThemeEnum.Light:
                        result = Light[(from node in Light.Keys.OfType<ComponentResourceKey>() where node.ResourceId.ToString() == _resourceName select node).First()];
                        break;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }

}
