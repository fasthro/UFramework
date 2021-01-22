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

        #region subtract

        public static LSVector3 Subtract(LSVector3 value1, LSVector3 value2)
        {
            LSVector3.Subtract(ref value1, ref value2, out var result);
            return result;
        }

        public static void Subtract(ref LSVector3 value1, ref LSVector3 value2, out LSVector3 result)
        {
            var num0 = value1.x - value2.x;
            var num1 = value1.y - value2.y;
            var num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        #endregion


        #region multiply

        public static LSVector3 Multiply(LSVector3 value1, Fix64 scaleFactor)
        {
            LSVector3.Multiply(ref value1, scaleFactor, out var result);
            return result;
        }

        public static void Multiply(ref LSVector3 value1, Fix64 scaleFactor, out LSVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }

        #endregion

        #region divide

        public static LSVector3 Divide(LSVector3 value1, Fix64 scaleFactor)
        {
            LSVector3.Divide(ref value1, scaleFactor, out var result);
            return result;
        }

        public static void Divide(ref LSVector3 value1, Fix64 scaleFactor, out LSVector3 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        #endregion

        #region cross

        public static LSVector3 Cross(LSVector3 vector1, LSVector3 vector2)
        {
            LSVector3.Cross(ref vector1, ref vector2, out var result);
            return result;
        }

        public static void Cross(ref LSVector3 vector1, ref LSVector3 vector2, out LSVector3 result)
        {
            var num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            var num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            var num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }

        #endregion

        #region dot

        public static Fix64 Dot(LSVector3 vector1, LSVector3 vector2)
        {
            return LSVector3.Dot(ref vector1, ref vector2);
        }


        public static Fix64 Dot(ref LSVector3 vector1, ref LSVector3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
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
            var num2 = Magnitude(value);
            if (num2 == Fix64.Zero)
            {
                result.x = Fix64.Zero;
                result.y = Fix64.Zero;
                result.z = Fix64.Zero;
            }
            else
            {
                var num = Fix64.One / Fix64.Sqrt(num2);
                result.x = value.x * num;
                result.y = value.y * num;
                result.z = value.z * num;
            }
        }

        public void Normalize()
        {
            var num2 = Magnitude(this);
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

        public static LSVector3 operator -(LSVector3 value1, LSVector3 value2)
        {
            LSVector3.Add(ref value1, ref value2, out var result);
            return result;
        }

        public static Fix64 operator *(LSVector3 value1, LSVector3 value2)
        {
            return LSVector3.Dot(ref value1, ref value2);
        }

        public static LSVector3 operator *(LSVector3 value1, Fix64 value2)
        {
            LSVector3.Multiply(ref value1, value2, out var result);
            return result;
        }

        public static LSVector3 operator *(Fix64 value1, LSVector3 value2)
        {
            LSVector3.Multiply(ref value2, value1, out var result);
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

        public override string ToString()
        {
            return $"[{x},{y},{z}]";
        }
    }
}