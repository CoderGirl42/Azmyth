using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Core;

namespace Azmyth
{
    public class ChunkAgent : IAgent
    {
        private IScene m_world = null;
        private TerrainChunk m_chunk = null;

        public int x, y;

        public IAsset Asset
        {
            get { return m_chunk; }
        }

        public bool Disposed { get; set; }

        public void Init(IScene world, IAsset asset) {
            m_world = world;
            m_chunk = asset as TerrainChunk;
            x = (int)m_chunk.Bounds.X;
            y = (int)m_chunk.Bounds.Y;
        }

        public void Update() {
            if (Disposed) {
                return;
            }

            if (Asset.Disposed) {
                Disposed = true;
                return;
            }

            TerrainTile tile = m_world.QueryAssets<TerrainTile>(new System.Drawing.RectangleF(x, y, 1, 1))[0] as TerrainTile;

            if (tile.Bounds.X == m_chunk.Bounds.Left && tile.Bounds.Y == m_chunk.Bounds.Top) {
                tile.MapChar = '╔';
                tile.BackColor = ConsoleColor.Gray;
            } else if (tile.Bounds.X == m_chunk.Bounds.Left && tile.Bounds.Y == m_chunk.Bounds.Bottom - 1) {
                tile.MapChar = '╚';
                tile.BackColor = ConsoleColor.Gray;
            } else if (tile.Bounds.X == m_chunk.Bounds.Right - 1 && tile.Bounds.Y == m_chunk.Bounds.Top) {
                tile.MapChar = '╗';
                tile.BackColor = ConsoleColor.Gray;
            } else if (tile.Bounds.X == m_chunk.Bounds.Right - 1 && tile.Bounds.Y == m_chunk.Bounds.Bottom - 1) {
                tile.MapChar = '╝';
                tile.BackColor = ConsoleColor.Gray;
            } else if (tile.Bounds.X == m_chunk.Bounds.Left || tile.Bounds.X == m_chunk.Bounds.Right - 1) {
                tile.MapChar = '║';
                tile.BackColor = ConsoleColor.Gray;
            } else if (tile.Bounds.Y == m_chunk.Bounds.Top || tile.Bounds.Y == m_chunk.Bounds.Bottom - 1) {
                tile.MapChar = '═';
                tile.BackColor = ConsoleColor.Gray;
            } else {
                tile.MapChar = '.';
            }

            if (x < m_chunk.Bounds.Right - 1) {
                x++;
            } else {
                x = (int)m_chunk.Bounds.X;

                if (y < m_chunk.Bounds.Bottom - 1) {
                    y++;
                } else {
                    y = (int)m_chunk.Bounds.Y;
                    Disposed = true;
                }
            }
        }
    }
}
