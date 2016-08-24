using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Core
{
    public interface IScene
    {
        IScene RegisterAsset<T>() 
            where T : class, IAsset;

        IScene RegisterLoader<T1, T2>()
            where T1 : class, IAsset
            where T2 : class, ILoader;

        IScene RegisterAgent<T1, T2>()
            where T1 : class, IAsset
            where T2 : class, IAgent;

        List<IAgent> GetAgents();

        List<IAsset> GetAssets<T>() where T : class, IAsset;

        List<IAsset> QueryAssets<T>(RectangleF bounds) where T : class, IAsset;

        T LoadAsset<T>(IAsset parent, int x, int y, int z) where T : class, IAsset;

        void SpawnAgents<T>(T asset) where T : class, IAsset;
        
        void UnloadAgent(IAgent agent);

        void UnloadAsset(IAsset asset);
    }
}
