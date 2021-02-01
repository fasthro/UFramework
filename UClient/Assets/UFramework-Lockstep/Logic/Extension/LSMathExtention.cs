// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-01-05 11:41:25
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace Lockstep.Logic
{
    public static class LSMathExtention
    {
        public static LSVector3 ToLSVector3(this Vector3 vector)
        {
            return new LSVector3(vector.x, vector.y, vector.z);
        }

        public static LSVector2 ToLSVector2(this Vector3 vector)
        {
            return new LSVector2(vector.x, vector.y);
        }

        public static LSVector3 ToLSVector3(this Vector2 vector)
        {
            return new LSVector3(vector.x, vector.y, 0);
        }

        public static LSVector2 ToLSVector2(this Vector2 vector)
        {
            return new LSVector2(vector.x, vector.y);
        }

        public static Vector3 Abs(this Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        public static LSQuaternion ToLSQuaternion(this Quaternion rot)
        {
            return new LSQuaternion(rot.x, rot.y, rot.z, rot.w);
        }

        public static Quaternion ToQuaternion(this LSQuaternion rot)
        {
            return new Quaternion((float) rot.x, (float) rot.y, (float) rot.z, (float) rot.w);
        }

        public static LSMatrix ToLSMatrix(this Quaternion rot)
        {
            return LSMatrix.CreateFromQuaternion(rot.ToLSQuaternion());
        }

        public static Vector3 ToVector3(this LSVector3 jVector)
        {
            return new Vector3((float) jVector.x, (float) jVector.y, (float) jVector.z);
        }

        public static Vector3 ToVector3(this LSVector2 jVector)
        {
            return new Vector3((float) jVector.x, (float) jVector.y, 0);
        }

        public static void Set(this LSVector3 jVector, LSVector3 otherVector)
        {
            jVector.Set(otherVector.x, otherVector.y, otherVector.z);
        }

        public static Quaternion ToQuaternion(this LSMatrix jMatrix)
        {
            return LSQuaternion.CreateFromMatrix(jMatrix).ToQuaternion();
        }
    }
}