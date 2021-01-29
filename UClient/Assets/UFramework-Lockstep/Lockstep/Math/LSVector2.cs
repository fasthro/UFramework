using System;

namespace Lockstep
{
    [Serializable]
    public struct LSVector2 : IEquatable<LSVector2>
    {
        #region Private Fields

        private static LSVector2 zeroVector = new LSVector2(0, 0);
        private static LSVector2 oneVector = new LSVector2(1, 1);

        private static LSVector2 rightVector = new LSVector2(1, 0);
        private static LSVector2 leftVector = new LSVector2(-1, 0);

        private static LSVector2 upVector = new LSVector2(0, 1);
        private static LSVector2 downVector = new LSVector2(0, -1);

        #endregion Private Fields

        #region Public Fields

        public FP x;
        public FP y;

        #endregion Public Fields

        #region Properties

        public static LSVector2 zero
        {
            get { return zeroVector; }
        }

        public static LSVector2 one
        {
            get { return oneVector; }
        }

        public static LSVector2 right
        {
            get { return rightVector; }
        }

        public static LSVector2 left
        {
            get { return leftVector; }
        }

        public static LSVector2 up
        {
            get { return upVector; }
        }

        public static LSVector2 down
        {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public LSVector2(FP x, FP y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public LSVector2(FP value)
        {
            x = value;
            y = value;
        }

        public void Set(FP x, FP y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref LSVector2 vector, ref LSVector2 normal, out LSVector2 result)
        {
            FP dot = Dot(vector, normal);
            result.x = vector.x - ((2f * dot) * normal.x);
            result.y = vector.y - ((2f * dot) * normal.y);
        }

        public static LSVector2 Reflect(LSVector2 vector, LSVector2 normal)
        {
            LSVector2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static LSVector2 Add(LSVector2 value1, LSVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref LSVector2 value1, ref LSVector2 value2, out LSVector2 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static LSVector2 Barycentric(LSVector2 value1, LSVector2 value2, LSVector2 value3, FP amount1, FP amount2)
        {
            return new LSVector2(
                LSMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                LSMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref LSVector2 value1, ref LSVector2 value2, ref LSVector2 value3, FP amount1,
            FP amount2, out LSVector2 result)
        {
            result = new LSVector2(
                LSMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                LSMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static LSVector2 CatmullRom(LSVector2 value1, LSVector2 value2, LSVector2 value3, LSVector2 value4, FP amount)
        {
            return new LSVector2(
                LSMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                LSMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref LSVector2 value1, ref LSVector2 value2, ref LSVector2 value3, ref LSVector2 value4,
            FP amount, out LSVector2 result)
        {
            result = new LSVector2(
                LSMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                LSMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static LSVector2 Clamp(LSVector2 value1, LSVector2 min, LSVector2 max)
        {
            return new LSVector2(
                LSMath.Clamp(value1.x, min.x, max.x),
                LSMath.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref LSVector2 value1, ref LSVector2 min, ref LSVector2 max, out LSVector2 result)
        {
            result = new LSVector2(
                LSMath.Clamp(value1.x, min.x, max.x),
                LSMath.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns FP precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="LSVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="LSVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static FP Distance(LSVector2 value1, LSVector2 value2)
        {
            FP result;
            DistanceSquared(ref value1, ref value2, out result);
            return (FP) FP.Sqrt(result);
        }


        public static void Distance(ref LSVector2 value1, ref LSVector2 value2, out FP result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (FP) FP.Sqrt(result);
        }

        public static FP DistanceSquared(LSVector2 value1, LSVector2 value2)
        {
            FP result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref LSVector2 value1, ref LSVector2 value2, out FP result)
        {
            result = (value1.x - value2.x) * (value1.x - value2.x) + (value1.y - value2.y) * (value1.y - value2.y);
        }

        /// <summary>
        /// Devide first vector with the secund vector
        /// </summary>
        /// <param name="value1">
        /// A <see cref="LSVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="LSVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="LSVector2"/>
        /// </returns>
        public static LSVector2 Divide(LSVector2 value1, LSVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static void Divide(ref LSVector2 value1, ref LSVector2 value2, out LSVector2 result)
        {
            result.x = value1.x / value2.x;
            result.y = value1.y / value2.y;
        }

        public static LSVector2 Divide(LSVector2 value1, FP divider)
        {
            FP factor = 1 / divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static void Divide(ref LSVector2 value1, FP divider, out LSVector2 result)
        {
            FP factor = 1 / divider;
            result.x = value1.x * factor;
            result.y = value1.y * factor;
        }

        public static FP Dot(LSVector2 value1, LSVector2 value2)
        {
            return value1.x * value2.x + value1.y * value2.y;
        }

        public static void Dot(ref LSVector2 value1, ref LSVector2 value2, out FP result)
        {
            result = value1.x * value2.x + value1.y * value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is LSVector2) ? this == ((LSVector2) obj) : false;
        }

        public bool Equals(LSVector2 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int) (x + y);
        }

        public static LSVector2 Hermite(LSVector2 value1, LSVector2 tangent1, LSVector2 value2, LSVector2 tangent2, FP amount)
        {
            LSVector2 result = new LSVector2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref LSVector2 value1, ref LSVector2 tangent1, ref LSVector2 value2, ref LSVector2 tangent2,
            FP amount, out LSVector2 result)
        {
            result.x = LSMath.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = LSMath.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        public FP magnitude
        {
            get
            {
                FP result;
                DistanceSquared(ref this, ref zeroVector, out result);
                return FP.Sqrt(result);
            }
        }

        public static LSVector2 ClampMagnitude(LSVector2 vector, FP maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        public FP LengthSquared()
        {
            FP result;
            DistanceSquared(ref this, ref zeroVector, out result);
            return result;
        }

        public static LSVector2 Lerp(LSVector2 value1, LSVector2 value2, FP amount)
        {
            amount = LSMath.Clamp(amount, 0, 1);

            return new LSVector2(
                LSMath.Lerp(value1.x, value2.x, amount),
                LSMath.Lerp(value1.y, value2.y, amount));
        }

        public static LSVector2 LerpUnclamped(LSVector2 value1, LSVector2 value2, FP amount)
        {
            return new LSVector2(
                LSMath.Lerp(value1.x, value2.x, amount),
                LSMath.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref LSVector2 value1, ref LSVector2 value2, FP amount, out LSVector2 result)
        {
            result = new LSVector2(
                LSMath.Lerp(value1.x, value2.x, amount),
                LSMath.Lerp(value1.y, value2.y, amount));
        }

        public static LSVector2 Max(LSVector2 value1, LSVector2 value2)
        {
            return new LSVector2(
                LSMath.Max(value1.x, value2.x),
                LSMath.Max(value1.y, value2.y));
        }

        public static void Max(ref LSVector2 value1, ref LSVector2 value2, out LSVector2 result)
        {
            result.x = LSMath.Max(value1.x, value2.x);
            result.y = LSMath.Max(value1.y, value2.y);
        }

        public static LSVector2 Min(LSVector2 value1, LSVector2 value2)
        {
            return new LSVector2(
                LSMath.Min(value1.x, value2.x),
                LSMath.Min(value1.y, value2.y));
        }

        public static void Min(ref LSVector2 value1, ref LSVector2 value2, out LSVector2 result)
        {
            result.x = LSMath.Min(value1.x, value2.x);
            result.y = LSMath.Min(value1.y, value2.y);
        }

        public void Scale(LSVector2 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
        }

        public static LSVector2 Scale(LSVector2 value1, LSVector2 value2)
        {
            LSVector2 result;
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static LSVector2 Multiply(LSVector2 value1, LSVector2 value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static LSVector2 Multiply(LSVector2 value1, FP scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref LSVector2 value1, FP scaleFactor, out LSVector2 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
        }

        public static void Multiply(ref LSVector2 value1, ref LSVector2 value2, out LSVector2 result)
        {
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;
        }

        public static LSVector2 Negate(LSVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref LSVector2 value, out LSVector2 result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

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
                LSVector2 result;
                LSVector2.Normalize(ref this, out result);

                return result;
            }
        }

        public static void Normalize(ref LSVector2 value, out LSVector2 result)
        {
            FP factor;
            DistanceSquared(ref value, ref zeroVector, out factor);
            factor = 1f / (FP) FP.Sqrt(factor);
            result.x = value.x * factor;
            result.y = value.y * factor;
        }

        public static LSVector2 SmoothStep(LSVector2 value1, LSVector2 value2, FP amount)
        {
            return new LSVector2(
                LSMath.SmoothStep(value1.x, value2.x, amount),
                LSMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref LSVector2 value1, ref LSVector2 value2, FP amount, out LSVector2 result)
        {
            result = new LSVector2(
                LSMath.SmoothStep(value1.x, value2.x, amount),
                LSMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static LSVector2 Subtract(LSVector2 value1, LSVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref LSVector2 value1, ref LSVector2 value2, out LSVector2 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static FP Angle(LSVector2 a, LSVector2 b)
        {
            return FP.Acos(a.normalized * b.normalized) * FP.Rad2Deg;
        }

        public LSVector3 ToTSVector()
        {
            return new LSVector3(this.x, this.y, 0);
        }

        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

        #region Operators

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


        public static FP operator *(LSVector2 value1, LSVector2 value2)
        {
            return LSVector2.Dot(value1, value2);
        }


        public static LSVector2 operator *(LSVector2 value, FP scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static LSVector2 operator *(FP scaleFactor, LSVector2 value)
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


        public static LSVector2 operator /(LSVector2 value1, FP divider)
        {
            FP factor = 1 / divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        #endregion Operators
    }
}