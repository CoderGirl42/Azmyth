using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Azmyth.Core
{
    public interface IAsset : IDisposable
    {
        IScene Scene { get; }

        IAsset Parent { get; }

        bool Disposed { get; }

        RectangleF Bounds { get; }

        event EventHandler BoundsChanged;

        void Update(TimeSpan elapsed);
    }
}
