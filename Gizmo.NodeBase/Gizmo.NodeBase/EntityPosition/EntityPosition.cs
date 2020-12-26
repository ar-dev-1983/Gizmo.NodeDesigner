using System;

namespace Gizmo.NodeBase
{
    public class EntityPosition : EntityViewModel, IDisposable
    {
        private double x = 0;
        private double y = 0;

        public double X
        {
            get => x;
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get => y;
            set
            {
                if (y == value) return;
                y = value;
                OnPropertyChanged();
            }
        }

        public EntityPosition()
        {
            X = 0;
            Y = 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public EntityPosition(double _x, double _y, bool initialize = false)
        {
            if (initialize)
            {
                X = _x;
                Y = _y;
            }
            else
            {
                x = _x;
                y = _y;
            }
        }
        public override bool Equals(object obj) => obj is EntityPosition position && X == position.X && Y == position.Y;
#if NET5_0
        public override int GetHashCode() => HashCode.Combine(X, Y);
#else
        public override int GetHashCode() => Tuple.Create(X, Y).GetHashCode();
#endif

    }
}
