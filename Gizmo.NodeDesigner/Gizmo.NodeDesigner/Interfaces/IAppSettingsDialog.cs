namespace Gizmo.NodeDesigner
{
    public interface IAppSettingsDialog
    {
        AppSettings Settings { set; get; }
        bool EditSettingsDialog(AppSettings settings);
    }
}
