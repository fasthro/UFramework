using System;

namespace Lockstep
{
    public struct LSVector3 : IEquatable<LSVector3>
    {
        public Fix64 x;
        public Fix64 y;
        public Fix64 z;

        public static readonly LSVector3 zero = new LSVector3(0, 0, 0);
        public static readonly LSVector3 one = new LSVector3(1, 1, 1);
        public static readonly LSVector3 up = new LSVector3(0, 1, 0);
        public static readonly LSVector3 down = new LSVector3(0, -1, 0);
        public static readonly LSVector3 left = new LSVector3(-1, 0, 0);
        public static readonly LSVector3 right = new LSVector3(1, 0, 0);
        public static readonly LSVector3 forward = new LSVector3(0, 0, 1);
        public static readonly LSVector3 back = new LSVector3(0, 0, -1);

        public LSVector3(int x, int y, int z)
        {
            this.x = (Fix64) x;
            this.y = (Fix64) y;
            this.z = (Fix64) z;
        }

        public LSVector3(Fix64 x, Fix64 y, Fix64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region add

        public static LSVector3 Add(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        public static void Add(ref LSVector3 value1, ref LSVector3 value2, out LSVector3 result)
        {
            var num0 = value1.x + value2.x;
            var num1 = value1.y + value2.y;
            var num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        #endregion

        #region normalize

        public LSVector3 normalized => Normalize(this);

        public static LSVector3 Normalize(LSVector3 value)
        {
            LSVector3.Normalize(ref value, out var result);
            return result;
        }


        public static void Normalize(ref LSVector3 value, out LSVector3 result)
        {
            var num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            var num = Fix64.One / Fix64.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }

        public void Normalize()
        {
            var num2 = ((x * x) + (y * y)) + (z * z);
            var num = Fix64.One / Fix64.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        #endregion


        public static Fix64 Magnitude(LSVector3 vector) => Fix64.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);

        public static LSVector3 operator +(LSVector3 value1, LSVector3 value2)
        {
            LSVector3.Add(ref value1, ref value2, out var result);
            return result;
        }

        public bool Equals(LSVector3 other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is LSVector3 other && Equals(other);
        }

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;
    }
}