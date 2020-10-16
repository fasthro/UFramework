/*
 * @Author: fasthro
 * @Date: 2020-07-23 01:06:06
 * @Description: Odin Menu Window Base
 */
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor
{
    public abstract class OdinMenuWindow : OdinMenuEditorWindow
    {
        /// <summary>
        /// 是否画搜索Bar
        /// </summary>
        protected bool drawSearchToolbar = true;

        protected OdinMenuItem menuItem;
        protected IPage page;
        protected IPageUpdate pageUpdate;
        protected bool isShowing;

        /// <summary>
        /// 自动保存描述时间
        /// </summary>
        readonly static float AUTO_SAVE_DESCREIBE_TIME = 1f;
        private float m_autoSaveDescribeTime = 0f;

        /// <summary>
        /// pages
        /// </summary>
        /// <typeparam name="IPage"></typeparam>
        /// <returns></returns>
        private List<IPage> m_pages = new List<IPage>();

        /// <summary>
        /// AddPage
        /// </summary>
        /// <param name="page"></param>
        protected void AddPage(IPage page)
        {
            m_pages.Add(page);
        }

        protected override void Initialize()
        {
            isShowing = true;
            m_autoSaveDescribeTime = Time.realtimeSinceStartup;
            OnInitialize();
        }

        /// <summary>
        /// OnInitialize
        /// </summary>
        protected abstract void OnInitialize();

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(false);

            if (drawSearchToolbar)
            {
                tree.DefaultMenuStyle.IconSize = 28.00f;
                tree.Config.DrawSearchToolbar = true;
            }

            for (int i = 0; i < m_pages.Count; i++)
            {
                var page = m_pages[i];
                tree.Add(page.menuName, page.GetInstance());
            }

            tree.Selection.SelectionChanged += SelectionChanged;
            EditorApplication.update += Update;
            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (this.MenuTree == null) return;
            var selection = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            if (selection != null)
            {
                SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
                {
                    var page = selection.Value as IPage;
                    if (page != null)
                    {
                        GUILayout.Label(page.menuName);
                    }
                    var pageBar = selection.Value as IPageBar;
                    if (pageBar != null)
                    {
                        pageBar.OnPageBarDraw();
                    }
                }
                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }

        private void Update()
        {
            if (!isShowing) return;
            // aotu save describe
            if (Time.realtimeSinceStartup - m_autoSaveDescribeTime >= AUTO_SAVE_DESCREIBE_TIME)
            {
                m_autoSaveDescribeTime = Time.realtimeSinceStartup;
                if (page != null)
                {
                    page.OnSaveDescribe();
                }
            }

            // page update
            if (pageUpdate != null)
            {
                pageUpdate.OnUpdate();
            }

            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        private void SelectionChanged(SelectionChangedType type)
        {
            if (type == SelectionChangedType.ItemAdded)
            {
                if (page != null)
                {
                    page.OnSaveDescribe();
                }
                menuItem = this.MenuTree.Selection.FirstOrDefault();
                page = menuItem.Value as IPage;
                pageUpdate = menuItem.Value as IPageUpdate;
                page.OnRenderBefore();
            }
        }

        protected override void OnDestroy()
        {
            isShowing = false;
            if (page != null)
            {
                page.OnSaveDescribe();
            }
            EditorApplication.update -= Update;
        }
    }
}