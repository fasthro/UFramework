using System.Collections.Generic;

namespace UFramework.UI
{
    /// <summary>
    /// 面板层定义
    /// </summary>
    public enum Layer
    {
        Scene = 0,
        Panel,
        MessageBox,
        Guide,
        Notification,
        Network,
        Loader,
        Top,
    }

    /// <summary>
    /// 面板层分组
    /// </summary>
    public class LayerGroup
    {
        // 层级间隔
        readonly static int LAYER_INTERVAL = 1000000;

        private HashSet<UIPanel> m_panels = new HashSet<UIPanel>();

        public Layer layer { get; private set; }
        private int m_layerIndex;

        public LayerGroup(Layer layer)
        {
            this.layer = layer;
            this.m_layerIndex = (int)layer + LAYER_INTERVAL;
        }

        public int Register(UIPanel panel)
        {
            if (!m_panels.Contains(panel))
            {
                m_panels.Add(panel);
            }
            Update();
            return m_layerIndex + m_panels.Count;
        }

        public void Remove(UIPanel panel)
        {
            if (m_panels.Contains(panel))
            {
                m_panels.Remove(panel);
            }
        }

        public void Update()
        {
            int index = 0;
            foreach (var panel in m_panels)
            {
                index++;
                panel.UpdateSortOrder(m_layerIndex + index);
            }
        }

        public void Release()
        {
            m_panels.Clear();
        }
    }

    /// <summary>
    /// 面板层代理管理者
    /// </summary>
    public static class LayerAgents
    {
        readonly static Dictionary<Layer, LayerGroup> layerDictionary = new Dictionary<Layer, LayerGroup>();

        public static int Register(UIPanel panel)
        {
            LayerGroup group;
            if (!layerDictionary.TryGetValue(panel.layer, out group))
            {
                group = new LayerGroup(panel.layer);
                layerDictionary.Add(panel.layer, group);
            }
            return group.Register(panel);
        }

        public static void Remove(UIPanel panel)
        {
            LayerGroup group;
            if (layerDictionary.TryGetValue(panel.layer, out group))
            {
                group.Remove(panel);
            }
        }

        public static void Update()
        {
            foreach (KeyValuePair<Layer, LayerGroup> item in layerDictionary)
            {
                item.Value.Update();
            }
        }

        public static void Release()
        {
            foreach (KeyValuePair<Layer, LayerGroup> item in layerDictionary)
            {
                item.Value.Release();
            }
        }
    }
}