using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Core
{
    public interface IAgent //where T : class, IAsset
    {
        IAsset Asset { get;  }

        bool Disposed { get; set; }

        void Init(IScene world, IAsset asset);

        void Update();
    }
}
