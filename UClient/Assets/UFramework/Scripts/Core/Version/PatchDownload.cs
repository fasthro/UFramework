/*
 * @Author: fasthro
 * @Date: 2020-10-28 18:08:58
 * @Description: 
 */
using ICSharpCode.SharpZipLib.Zip;
using UFramework.Tools;

namespace UFramework.VersionControl
{
    public class PatchDownload : IUnzip
    {
        public void OnPostUnzip(ZipEntry entry)
        {
            throw new System.NotImplementedException();
        }

        public bool OnPreUnzip(ZipEntry entry)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnzipFinished(bool result)
        {
            throw new System.NotImplementedException();
        }
    }
}