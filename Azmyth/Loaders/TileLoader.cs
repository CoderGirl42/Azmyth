using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Core;

namespace Azmyth
{
    public class TileLoader : ILoader
    {
        public IAsset LoadAsset(IScene scene, IAsset parent, int x, int y, int z, object parameters) {
            return new TerrainTile(scene, parent, x, y, z, parameters); 
        }
    }
}
