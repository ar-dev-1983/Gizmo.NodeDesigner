using Gizmo.NodeFramework;

namespace Gizmo.NodeDesigner
{
    public interface ISerialization
    {
        Project OpenProject(string filename);
        void SaveProject(string filename, Project item);

        AppSettings OpenSettings(string filename);
        void SaveSettings(string filename, AppSettings settings);
    }
}
