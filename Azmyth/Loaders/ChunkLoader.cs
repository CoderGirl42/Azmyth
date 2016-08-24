using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Core;

namespace Azmyth
{
    public class ChunkLoader : ILoader
    {
        public IAsset LoadAsset(IScene scene, IAsset parent, int x, int y, int z, object parameters) {
            TerrainChunk chunk = new TerrainChunk(scene, parent, x, y, z, parameters);
            System.Drawing.RectangleF Bounds = new System.Drawing.RectangleF(x, y, 10, 10);
            
            chunk.Bounds = Bounds;

            int totalTiles = (int)Bounds.Width * (int)Bounds.Height;

            //Loop through each tile
            for (int index = 0; index < totalTiles; index++) {
                //Convert index value into x and y coordinates.
                int cellX = (int)(index / Bounds.Height) + (int)Bounds.X;
                int cellY = (int)(index % Bounds.Height) + (int)Bounds.Y;

                //Load tile.
                TerrainTile tile = scene.LoadAsset<TerrainTile>(chunk, cellX, cellY, 0);

                //Insert new tile into chunk QuadTree
                //m_tiles.Insert(tile);
            }

            return chunk;
        }
    }
}
