using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Core;

namespace Azmyth
{
    public class TerrainChunk : IAsset
    {
        private IScene m_world = null;
        private IAsset m_parent = null;

        public bool Disposed { get; private set; }
        public RectangleF Bounds { get; set; }

        public event EventHandler BoundsChanged;

        public IScene Scene {
            get {
                return m_world;
            }
        }

        public IAsset Parent  {
            get {
                return m_parent;
            }
        }

        public TerrainChunk(IScene scene, IAsset parent, int x, int y, int z, object parameters) {
            m_world = scene;
            m_parent = parent;
        }

        public void Update(TimeSpan elapsed) {
            if (Parent.Disposed) {
                Dispose();
            }
        }

        public void Dispose() {
            if (!Disposed) {
                Disposed = true;
            }
        }
    }
}
