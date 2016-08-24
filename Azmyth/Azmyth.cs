using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Core;

namespace Azmyth
{
    public class Scene : IScene, IAsset
    {
        List<IAgent> m_agents = new List<IAgent>();
        List<Type> m_assetTypes = new List<Type>();

        Dictionary<Type, ILoader> m_assetLoaders = new Dictionary<Type, ILoader>();
        Dictionary<Type, List<Type>> m_agentTypes = new Dictionary<Type, List<Type>>();
        Dictionary<Type, QuadTree<IAsset>> m_assets = new Dictionary<Type, QuadTree<IAsset>>();

        public event EventHandler BoundsChanged;

        IScene IAsset.Scene {
            get {
                return this;
            }
        }

        public IAsset Parent { 
            get { 
                return null; 
            } 
        }

        public int AssetTypeCount {
            get { 
                return m_assetTypes.Count; 
            }
        }
        
        public int AgentTypeCount {
            get { 
                return m_agentTypes.Count; 
            }
        }

        public int LoaderCount {
            get { 
                return m_assetLoaders.Count; 
            }
        }

        public int QuadNodes {
            get {
                int count = 0;

                foreach (Type t in m_assetTypes) {
                    count += m_assets[t].GetQuadNodeCount();
                }

                return count;
            }
        }

        public int AssetsLoaded {
            get {
                int count = 0;

                foreach(Type t in m_assetTypes) {
                    count += m_assets[t].GetQuadObjectCount();
                }

                return count;
            }
        }

        public int AgentsLoaded {
            get { 
                return m_agents.Count; 
            }
        }

        public List<Type> GetAssetTypes() {
            return m_assetTypes.ToList();
        }

        public RectangleF Bounds { get; private set; }

        public bool Disposed { get;  private set; }

        public List<Type> GetAgentTypes(Type T) {
            List<Type> types = new List<Type>();

            if (m_agentTypes.ContainsKey(T)) {
                types = m_agentTypes[T].ToList();
            }

            return types;
        }

        public ILoader GetLoader<T>() where T : class, IAsset {
            ILoader loader = null;
            Type assetType = typeof(T);
            
            if (m_assetLoaders.ContainsKey(assetType)) {
                loader = m_assetLoaders[assetType];
            }

            return loader;
        }

        public List<ILoader> GetLoaders() {
            List<ILoader> loaders = new List<ILoader>();

            foreach (Type t in m_assetLoaders.Keys) {
                loaders.Add(m_assetLoaders[t]);
            }

            return loaders;
        }

        public IScene RegisterAsset<T>() where T: class, IAsset {
            Type assetType = typeof(T);

            if (m_assetTypes.Contains(assetType)) {
                throw new Exception(assetType.Name + " already registered.");
            }

            m_assetTypes.Add(assetType);
            m_assets.Add(assetType, new QuadTree<IAsset>(new Size(64, 64), 64));

            return this;
        }

        public IScene RegisterAgent<T1, T2>() where T1 : class, IAsset where T2: class, IAgent {
            Type assetType = typeof(T1);
            Type agentType = typeof(T2);

            if(!m_assetTypes.Contains(assetType)) {
                throw new Exception("Unable to register agent without asset registered.");
            }

            if(!m_agentTypes.ContainsKey(assetType)) {
                m_agentTypes.Add(assetType, new List<Type>());
            }

            if(!m_agentTypes[assetType].Contains(agentType)) {
                m_agentTypes[assetType].Add(agentType);
            }

            return this;
        }

        public IScene RegisterLoader<T1, T2>() where T1: class, IAsset where T2 : class, ILoader {
            Type assetType = typeof(T1);
            Type loaderType = typeof(T2);

            if (!m_assetTypes.Contains(assetType)) {
                throw new Exception("Unable to register loader without registered asset.");
            }

            if (!m_assetLoaders.ContainsKey(assetType)) {
                m_assetLoaders.Add(assetType, Activator.CreateInstance<T2>());
            }

            return this;
        }

        public T LoadAsset<T>(IAsset parent, int x, int y, int z) where T : class, IAsset {
            T asset = default(T);
            Type assetType = typeof(T);

            if (!m_assetLoaders.ContainsKey(assetType)) {
                throw new Exception(assetType.Name + " type has not been registered.");
            }

            asset = (T)m_assetLoaders[assetType].LoadAsset(this, parent, x, y, z, null);

            m_assets[assetType].Insert(asset);

            SpawnAgents(asset);

            return asset;
        }

        public List<IAsset> QueryAssets<T>(RectangleF bounds) where T : class, IAsset {
            Type assetType = typeof(T);
            List<IAsset> assets = null;

            if(!m_assets.ContainsKey(assetType)) {
                throw new Exception("Asset Type not registered.");
            }

            assets = m_assets[assetType].Query(bounds);

            return assets;
        }

        public List<IAgent> GetAgents() {
            List<IAgent> agents = new List<IAgent>();

            agents.AddRange(m_agents);

            return agents;
        }

        public List<IAgent> GetAgents<T>() where T : class, IAsset  {
            List<IAgent> agents = new List<IAgent>();

            agents.AddRange(m_agents.Where(x => x.Asset is T));

            return agents;
        }

        public void SpawnAgents<T>(T asset) where T : class, IAsset {
            Type assetType = typeof(T);
            IAgent agent = default(IAgent);

            if (m_agentTypes.ContainsKey(assetType)) {
                List<Type> agentTypes = m_agentTypes[assetType];

                foreach (Type t in agentTypes) {
                    agent = (IAgent)Activator.CreateInstance(t);
                    agent.Init(this, asset);
                    m_agents.Add(agent as IAgent);
                }
            }
        }

        public void UnloadAgent(IAgent agent) {
            if(m_agents.Contains(agent))
                m_agents.Remove(agent);
        }

        public void UnloadAsset(IAsset asset) {
            Type assetType = asset.GetType();

            if (m_assets.ContainsKey(assetType)) {
                m_assets[assetType].Remove(asset);
            }

            asset.Dispose();
        }

        public List<IAsset> GetAssets<T>() where T : class, IAsset {
            List<IAsset> assets = new List<IAsset>();

            foreach(QuadTree<IAsset>.QuadNode node in m_assets[typeof(T)].GetAllNodes()) {
                assets.AddRange(node.Objects);
            }

            return assets;
        }

        public void Update(TimeSpan elapsed)
        {

        }

        public void Dispose() {
            if(!Disposed) {
                Disposed = true;
            }
        }
    }
}
