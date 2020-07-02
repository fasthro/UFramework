/*
 * @Author: fasthro
 * @Date: 2020-06-21 20:18:34
 * @Description: zip handler
 */

using ICSharpCode.SharpZipLib.Zip;

namespace UFramework.Tools
{
    public abstract class ZipHandler
    {
        /// <summary>
        /// 压缩单个文件或文件夹前执行的回调
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>如果返回true，则压缩文件或文件夹，反之则不压缩文件或文件夹</returns>
        public virtual bool OnPreZip(ZipEntry entry)
        {
            return true;
        }

        /// <summary>
        /// 压缩单个文件或文件夹后执行的回调
        /// </summary>
        /// <param name="entry"></param>
        public virtual void OnPostZip(ZipEntry entry) { }

        /// <summary>
        /// 压缩执行完毕后的回调
        /// </summary>
        /// <param name="result">true表示压缩成功，false表示压缩失败</param>
        public virtual void OnFinished(bool result) { }
    }
}