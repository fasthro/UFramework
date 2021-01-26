using System;

namespace Lockstep
{
    public struct LSVector2 : IEquatable<LSVector2>
    {
        #region base

        public Fix64 x;
        public Fix64 y;

        #endregion

        #region base const

        public static readonly LSVector2 zero = new LSVector2(0, 0);
        public static readonly LSVector2 one = new LSVector2(1, 1);
        public static readonly LSVector2 up = new LSVector2(0, 1);
        public static readonly LSVector2 down = new LSVector2(0, -1);
        public static readonly LSVector2 left = new LSVector2(-1, 0);
        public static readonly LSVector2 right = new LSVector2(1, 0);

        #endregion

        public LSVector2(int x, int y)
        {
            this.x = (Fix64) x;
            this.y = (Fix64) y;
        }

        public LSVector2(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        #region Dot

        public static Fix64 Dot(LSVector2 value1, LSVector2 value2)
        {
            return value1.x * value2.x + value1.y * value2.y;
        }

        public static void Dot(ref LSVector2 value1, ref LSVector2 value2, out Fix64 result)
        {
            result = value1.x * value2.x + value1.y * value2.y;
        }

        #endregion


        #region Distance

        public static Fix64 Distance(LSVector2 value1, LSVector2 value2)
        {
            DistanceSquared(ref value1, ref value2, out var result);
            return Fix64.Sqrt(result);
        }


        public static void Distance(ref LSVector2 value1, ref LSVector2 value2, out Fix64 result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = Fix64.Sqrt(result);
        }

        public static Fix64 DistanceSquared(LSVector2 value1, LSVector2 value2)
        {
            DistanceSquared(ref value1, ref value2, out var result);
            return result;
        }

        public static void DistanceSquared(ref LSVector2 value1, ref LSVector2 value2, out Fix64 result)
        {
            result = (value1.x - value2.x) * (value1.x - value2.x) + (value1.y - value2.y) * (value1.y - value2.y);
        }

        #endregion


        #region Normalize

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static LSVector2 Normalize(LSVector2 value)
        {
            Normalize(ref value, out value);
            return value;
        }

        public LSVector2 normalized
        {
            get
            {
                LSVector2.Normalize(ref this, out var result);
                return result;
            }
        }

        public static void Normalize(ref LSVector2 value, out LSVector2 result)
        {
            var zeroVector = new LSVector2(0, 0);
            DistanceSquared(ref value, ref zeroVector, out var factor);
            factor = (Fix64) 1 / Fix64.Sqrt(factor);
            result.x = value.x * factor;
            result.y = value.y * factor;
        }

        #endregion


        #region operator

        public static LSVector2 operator -(LSVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static bool operator ==(LSVector2 value1, LSVector2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(LSVector2 value1, LSVector2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static LSVector2 operator +(LSVector2 value1, LSVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static LSVector2 operator -(LSVector2 value1, LSVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static Fix64 operator *(LSVector2 value1, LSVector2 value2)
        {
            return LSVector2.Dot(value1, value2);
        }


        public static LSVector2 operator *(LSVector2 value, Fix64 scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static LSVector2 operator *(Fix64 scaleFactor, LSVector2 value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static LSVector2 operator /(LSVector2 value1, LSVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static LSVector2 operator /(LSVector2 value1, Fix64 divider)
        {
            var factor = (Fix64) 1 / divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        #endregion


        public override bool Equals(object obj)
        {
            return obj is LSVector2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public bool Equals(LSVector2 other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override string ToString()
        {
            return $"[{x},{y}]";
        }
    }
}