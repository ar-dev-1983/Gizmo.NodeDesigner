using Gizmo.NodeFrameworkUI;

namespace Gizmo.NodeDesigner
{
    public class AppSettings : BaseViewModel
    {
        private bool loadLastFile;
        private string lastFile;
        private UINodeThemeEnum theme;

        public bool LoadLastFile
        {
            get => loadLastFile;
            set
            {
                if (loadLastFile == value) return;
                loadLastFile = value;
                OnPropertyChanged();
            }
        }

        public string LastFile
        {
            get => lastFile;
            set
            {
                if (lastFile == value) return;
                lastFile = value;
                OnPropertyChanged();
            }
        }
        public UINodeThemeEnum Theme
        {
            get => theme;
            set
            {
                if (theme == value) return;
                theme = value;
                OnPropertyChanged();
            }
        }

        public AppSettings()
        {
            theme = UINodeThemeEnum.Dark;
            loadLastFile = false;
            lastFile = string.Empty;
        }
    }
}
