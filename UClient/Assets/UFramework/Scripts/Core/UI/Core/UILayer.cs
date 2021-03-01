// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-29 13:22:48
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;

namespace UFramework.UI
{
    /// <summary>
    /// 面板层定义
    /// </summary>
    public enum Layer
    {
        SCNEN = 0,
        PANEL,
        MESSAGE_BOX,
        GUIDE,
        NOTIFICATION,
        NETWORK,
        LOADER,
        TOP,
    }

    /// <summary>
    /// 面板层分组
    /// </summary>
    public class LayerGroup
    {
        // 层级间隔
        const int LAYER_INTERVAL = 1000000;

        public Layer layer { get; private set; }

        private HashSet<UIPanel> _panels;
        private int _layerIndex;

        public LayerGroup(Layer layer)
        {
            this.layer = layer;
            this._layerIndex = (int) layer + LAYER_INTERVAL;
            this._panels = new HashSet<UIPanel>();
        }

        public int Register(UIPanel panel)
        {
            if (!_panels.Contains(panel))
            {
                _panels.Add(panel);
            }

            Update();
            return _layerIndex + _panels.Count;
        }

        public void Remove(UIPanel panel)
        {
            if (_panels.Contains(panel))
            {
                _panels.Remove(panel);
            }
        }

        public void Update()
        {
            var index = 0;
            foreach (var panel in _panels)
            {
                index++;
                panel.UpdateSortOrder(_layerIndex + index);
            }
        }

        public void Release()
        {
            _panels.Clear();
        }
    }

    /// <summary>
    /// 面板层代理管理者
    /// </summary>
    public static class LayerAgents
    {
        static readonly Dictionary<Layer, LayerGroup> layerDict = new Dictionary<Layer, LayerGroup>();

        public static int Register(UIPanel panel)
        {
            if (!layerDict.TryGetValue(panel.layer, out var @group))
            {
                group = new LayerGroup(panel.layer);
                layerDict.Add(panel.layer, group);
            }

            return group.Register(panel);
        }

        public static void Remove(UIPanel panel)
        {
            if (layerDict.TryGetValue(panel.layer, out var @group))
            {
                group.Remove(panel);
            }
        }

        public static void Update()
        {
            foreach (var item in layerDict)
            {
                item.Value.Update();
            }
        }

        public static void Release()
        {
            foreach (var item in layerDict)
            {
                item.Value.Release();
            }
        }
    }
}