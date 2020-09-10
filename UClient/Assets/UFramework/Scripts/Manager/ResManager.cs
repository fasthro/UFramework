/*
 * @Author: fasthro
 * @Date: 2020-09-09 10:18:30
 * @Description: 资源 Manager（AssetsBundle/Resources）
 */

namespace UFramework
{
    public class ResManager : BaseManager
    {
        protected override void OnInitialize()
        {

        }

        #region api

        public T LoadAsset<T>(string path)
        {
            return default(T);
        }

        protected override void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnLateUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region assetsbundle

        #endregion

        #region resources

        #endregion
    }
}