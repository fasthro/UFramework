/*
 * @Author: fasthro
 * @Date: 2021-01-05 11:41:25
 * @Description: 
 */

using Lockstep;
using UnityEngine;

namespace Lockstep.Logic
{
    public static class LSVector3Extention
    {
        public static LSVector3 ToLSVector3(this Vector3 vec)
        {
            return new LSVector3((FP) vec.x, (FP) vec.y, (FP) vec.z);
        }

        public static Vector3 ToVector3(this LSVector3 vec)
        {
            return new Vector3((float) vec.x, (float) vec.y, (float) vec.z);
        }
    }
}