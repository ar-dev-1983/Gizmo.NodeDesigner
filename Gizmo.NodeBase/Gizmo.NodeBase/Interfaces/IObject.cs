namespace Gizmo.NodeBase
{
    public interface IObject : IEntity
    {
        EntityPosition Position { get; set; }
        EntitySize Size { get; set; }
    }
}
