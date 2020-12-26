namespace Gizmo.NodeDesigner
{
    public interface IDialog
    {
        void ShowMessage(string message);
        bool QueryYesNoAnswer(string message);
        string FilePath { get; set; }
        bool OpenFileDialog();
        bool OpenFileDialog(string type);
        bool SaveFileDialog();
        bool SaveFileDialog(string file, string type);
    }
}
