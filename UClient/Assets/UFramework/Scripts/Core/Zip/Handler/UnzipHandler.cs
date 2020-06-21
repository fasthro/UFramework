/*
 * @Author: fasthro
 * @Date: 2020-06-21 20:22:27
 * @Description: unzip handler
 */

using ICSharpCode.SharpZipLib.Zip;

namespace UFramework.Zip
{
    public abstract class UnzipHandler
    {
        /// <summary>
        /// 解压单个文件或文件夹前执行的回调
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>如果返回true，则压缩文件或文件夹，反之则不压缩文件或文件夹</returns>
        public virtual bool OnPreUnzip(ZipEntry entry)
        {
            return true;
        }

        /// <summary>
        /// 解压单个文件或文件夹后执行的回调
        /// </summary>
        /// <param name="entry"></param>
        public virtual void OnPostUnzip(ZipEntry entry) { }

        /// <summary>
        /// 解压执行完毕后的回调
        /// </summary>
        /// <param name="result">true表示解压成功，false表示解压失败</param>
        public virtual void OnFinished(bool result) { }
    }
}