using System;

namespace Gizmo.NodeBase
{
    public interface IGroupable
    {
        Guid Id { get; }
        Guid GroupId { get; set; }
        bool IsInGroup { get; set; }
    }
}
