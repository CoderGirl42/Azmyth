using System;
using System.Collections.Generic;
using System.Drawing;
using Azmyth.Core;

namespace Azmyth
{
    public class TerrainTile : IAsset
    {
        private IScene m_scene = null;
        private IAsset m_parent = null;
        private TerrainTypes m_terrain;

        private float m_height = 0;

        public char MapChar { get; set; }
        public ConsoleColor BackColor { get; set; }
        public ConsoleColor ForeColor { get; set; }

        public event EventHandler BoundsChanged;

        public IScene Scene {
            get {
                return m_scene;
            }
        }

        public IAsset Parent {
            get {
                return m_parent;
            }
        }

        public TerrainTile(IScene world, IAsset parent, int x, int y, int z, object parameters)
        {
            m_scene = world;
            m_parent = parent;

            Bounds = new RectangleF(x, y, 1, 1);

            MapChar = ' ';
        }

        public float Height {
            get {
                return m_height;
            }
            set {
                m_height = value;
            }
        }

        public TerrainTypes Terrain {
            get {
                return m_terrain;
            }
            set {
                m_terrain = value;
            }
        }

        public RectangleF Bounds { get; private set; }

        public bool Disposed { get; set; }

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
