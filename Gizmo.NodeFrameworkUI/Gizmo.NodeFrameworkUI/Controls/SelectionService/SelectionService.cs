using System;
using System.Collections.Generic;
using System.Linq;

namespace Gizmo.NodeFrameworkUI
{
    public class SelectionService
    {
        private readonly EngineWrapper Designer;

        private List<EntityWrapper> currentSelection;
        public List<EntityWrapper> CurrentSelection
        {
            get
            {
                if (currentSelection == null)
                    currentSelection = new List<EntityWrapper>();

                return currentSelection;
            }
        }

        public SelectionService(EngineWrapper designer)
        {
            Designer = designer;
        }

        public void SelectItem(EntityWrapper item)
        {
            ClearSelection();
            AddToSelection(item);
        }

        public void AddToSelection(EntityWrapper item)
        {
            if (item is EntityWrapper)
            {
                List<EntityWrapper> groupItems = GetGroupMembers(item as EntityWrapper);

                foreach (EntityWrapper groupItem in groupItems)
                {
                    groupItem.IsSelected = true;
                    CurrentSelection.Add(groupItem);
                }
            }
            else
            {
                item.IsSelected = true;
                CurrentSelection.Add(item);
            }
        }

        public void RemoveFromSelection(EntityWrapper item)
        {
            if (item is EntityWrapper)
            {
                List<EntityWrapper> groupItems = GetGroupMembers(item as EntityWrapper);

                foreach (EntityWrapper groupItem in groupItems)
                {
                    groupItem.IsSelected = false;
                    CurrentSelection.Remove(groupItem);
                }
            }
            else
            {
                item.IsSelected = false;
                CurrentSelection.Remove(item);
            }
        }

        public void ClearSelection()
        {
            CurrentSelection.ForEach(item => item.IsSelected = false);
            CurrentSelection.Clear();
        }

        public void SelectAll()
        {
            ClearSelection();
            CurrentSelection.AddRange(Designer.Items.OfType<EntityWrapper>());
            CurrentSelection.ForEach(item => item.IsSelected = true);
        }

        public List<EntityWrapper> GetGroupMembers(EntityWrapper item)
        {
            IEnumerable<EntityWrapper> list = Designer.Items.OfType<EntityWrapper>();
            EntityWrapper rootItem = GetRoot(list, item);
            return GetGroupMembers(list, rootItem);
        }

        public EntityWrapper GetGroupRoot(EntityWrapper item)
        {
            IEnumerable<EntityWrapper> list = Designer.Items.OfType<EntityWrapper>();
            return GetRoot(list, item);
        }

        private EntityWrapper GetRoot(IEnumerable<EntityWrapper> list, EntityWrapper node)
        {
            if (node == null || node.GroupId == Guid.Empty && !node.IsInGroup)
            {
                return node;
            }
            else
            {
                foreach (EntityWrapper item in list)
                {
                    if (item.Id == node.GroupId)
                    {
                        return GetRoot(list, item);
                    }
                }
                return null;
            }
        }

        private List<EntityWrapper> GetGroupMembers(IEnumerable<EntityWrapper> list, EntityWrapper parent)
        {
            List<EntityWrapper> groupMembers = new List<EntityWrapper>
            {
                parent
            };

            var children = list.Where(node => node.IsInGroup && node.GroupId == parent.Id);
            if (children != null)
            {
                foreach (EntityWrapper child in children)
                {
                    groupMembers.AddRange(GetGroupMembers(list, child));
                }
            }
            return groupMembers;
        }
    }
}
