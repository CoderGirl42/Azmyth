using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Core
{
    public interface ILoader
    {
        IAsset LoadAsset(IScene world, IAsset parent, int x, int y, int z, object paramters = null);
    }
}
