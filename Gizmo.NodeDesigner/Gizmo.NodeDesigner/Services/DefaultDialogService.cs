using Microsoft.Win32;
using System.Windows;

namespace Gizmo.NodeDesigner
{
    public class DefaultDialogService : IDialog
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            switch (openFileDialog.ShowDialog())
            {
                case true:
                    FilePath = openFileDialog.FileName;
                    return true;
                default:
                    return false;
            }
        }
        public bool OpenFileDialog(string type)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = type };
            switch (openFileDialog.ShowDialog())
            {
                case true:
                    FilePath = openFileDialog.FileName;
                    return true;
                default:
                    return false;
            }
        }
        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            switch (saveFileDialog.ShowDialog())
            {
                case true:
                    FilePath = saveFileDialog.FileName;
                    return true;
                default:
                    return false;
            }
        }
        public bool SaveFileDialog(string file, string type)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = file, Filter = type };
            switch (saveFileDialog.ShowDialog())
            {
                case true:
                    FilePath = saveFileDialog.FileName;
                    return true;
                default:
                    return false;
            }
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool QueryYesNoAnswer(string message)
        {
            return MessageBox.Show(message, "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
