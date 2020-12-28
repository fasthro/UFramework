/*
 * @Author: fasthro
 * @Date: 2020-12-16 13:54:40
 * @Description:
 */

using UFramework.Core;
using UnityEngine;

namespace UFramework.Lockstep
{
    public class GameManager : BaseManager
    {
        public T CreateViewObject<T>(ViewData vd) where T : BaseView, new()
        {
            var request = Assets.LoadAsset(vd.artPath, typeof(GameObject));
            var gameObject = GameObject.Instantiate(request.asset) as GameObject;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.gameObject.SetActive(true);

            var view = new T();
            view.BindViewObject(gameObject.GetComponent<ViewObject>());
            return view;
        }
    }
}