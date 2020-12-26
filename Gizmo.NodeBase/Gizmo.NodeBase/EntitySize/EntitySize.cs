using System;

namespace Gizmo.NodeBase
{
    public class EntitySize : EntityViewModel, IDisposable
    {
        private double width = 0;
        private double height = 0;

        public double Width
        {
            get => width;
            set
            {
                if (width == value) return;
                width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => height;
            set
            {
                if (height == value) return;
                height = value;
                OnPropertyChanged();
            }
        }

        public EntitySize()
        {
            Width = 0;
            Height = 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public EntitySize(double _width, double _height, bool initialize = false)
        {
            if (initialize)
            {
                Width = _width;
                Height = _height;
            }
            else
            {
                width = _width;
                height = _height;
            }
        }

        public override bool Equals(object obj) => obj is EntitySize size && Width == size.Width && Height == size.Height;
#if NET5_0
        public override int GetHashCode() => HashCode.Combine(Width, Height);
#else
        public override int GetHashCode() => Tuple.Create(Width, Height).GetHashCode();
#endif
    }
}
