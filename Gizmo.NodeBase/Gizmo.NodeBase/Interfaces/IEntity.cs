namespace Gizmo.NodeBase
{
    public interface IEntity : ISelectable, IGroupable
    {
        EntityTypeEnum EntityType { get; }
    }
}
