/*
 * @Author: fasthro
 * @Date: 2021-01-05 11:41:25
 * @Description: 
 */
using V3 = System.Numerics.Vector3;
using QR = System.Numerics.Quaternion;

using UV3 = UnityEngine.Vector3;
using UQR = UnityEngine.Quaternion;

namespace Lockstep.Logic
{
    public static class Vector3Extention
    {
        public static UV3 ToUnity(this V3 v3)
        {
            return new UV3(v3.X, v3.Y, v3.Z);
        }

        public static V3 ToCS(this UV3 v3)
        {
            return new V3(v3.x, v3.y, v3.z);
        }
    }

    public static class QuaternionExtention
    {
        public static UQR ToUnity(this QR qr)
        {
            return new UQR(qr.X, qr.Y, qr.Z, qr.W);
        }

        public static QR ToCS(this UQR qr)
        {
            return new QR(qr.x, qr.y, qr.z, qr.w);
        }
    }
    
}