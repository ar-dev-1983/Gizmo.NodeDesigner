using System;

namespace Gizmo.GraphicFramework.Interfaces
{
    public interface IShape
    {
        Guid Id { get; }
        Guid ParentId { get; set; }
        bool IsSelected { set; get; }
        bool IsGroup { set; get; }
    }
}
