using System;

namespace Lockstep
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [Serializable]
    public struct LSVector4
    {
        private static FP ZeroEpsilonSq = LSMath.Epsilon;
        internal static LSVector4 InternalZero;

        /// <summary>The X component of the vector.</summary>
        public FP x;

        /// <summary>The Y component of the vector.</summary>
        public FP y;

        /// <summary>The Z component of the vector.</summary>
        public FP z;

        /// <summary>The W component of the vector.</summary>
        public FP w;

        #region Static readonly variables

        /// <summary>
        /// A vector with components (0,0,0,0);
        /// </summary>
        public static readonly LSVector4 zero;

        /// <summary>
        /// A vector with components (1,1,1,1);
        /// </summary>
        public static readonly LSVector4 one;

        /// <summary>
        /// A vector with components 
        /// (FP.MinValue,FP.MinValue,FP.MinValue);
        /// </summary>
        public static readonly LSVector4 MinValue;

        /// <summary>
        /// A vector with components 
        /// (FP.MaxValue,FP.MaxValue,FP.MaxValue);
        /// </summary>
        public static readonly LSVector4 MaxValue;

        #endregion

        #region Private static constructor

        static LSVector4()
        {
            one = new LSVector4(1, 1, 1, 1);
            zero = new LSVector4(0, 0, 0, 0);
            MinValue = new LSVector4(FP.MinValue);
            MaxValue = new LSVector4(FP.MaxValue);
            InternalZero = zero;
        }

        #endregion

        public static LSVector4 Abs(LSVector4 other)
        {
            return new LSVector4(FP.Abs(other.x), FP.Abs(other.y), FP.Abs(other.z), FP.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public FP sqrMagnitude
        {
            get { return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z) + (this.w * this.w)); }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public FP magnitude
        {
            get
            {
                FP num = sqrMagnitude;
                return FP.Sqrt(num);
            }
        }

        public static LSVector4 ClampMagnitude(LSVector4 vector, FP maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public LSVector4 normalized
        {
            get
            {
                LSVector4 result = new LSVector4(this.x, this.y, this.z, this.w);
                result.Normalize();

                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        public LSVector4(int x, int y, int z, int w)
        {
            this.x = (FP) x;
            this.y = (FP) y;
            this.z = (FP) z;
            this.w = (FP) w;
        }

        public LSVector4(FP x, FP y, FP z, FP w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(LSVector4 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
            this.w = w * other.w;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        public void Set(FP x, FP y, FP z, FP w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public LSVector4(FP xyzw)
        {
            this.x = xyzw;
            this.y = xyzw;
            this.z = xyzw;
            this.w = xyzw;
        }

        public static LSVector4 Lerp(LSVector4 from, LSVector4 to, FP percent)
        {
            return from + (to - from) * percent;
        }

        /// <summary>
        /// Builds a string from the JVector.
        /// </summary>
        /// <returns>A string containing all three components.</returns>

        #region public override string ToString()

        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1}, {2:f1}, {3:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat(), w.AsFloat());
        }

        #endregion

        /// <summary>
        /// Tests if an object is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if they are euqal, otherwise false.</returns>

        #region public override bool Equals(object obj)

        public override bool Equals(object obj)
        {
            if (!(obj is LSVector4)) return false;
            LSVector4 other = (LSVector4) obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z) && (w == other.w));
        }

        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static LSVector4 Scale(LSVector4 vecA, LSVector4 vecB)
        {
            LSVector4 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;
            result.w = vecA.w * vecB.w;

            return result;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>

        #region public static bool operator ==(JVector value1, JVector value2)

        public static bool operator ==(LSVector4 value1, LSVector4 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z) && (value1.w == value2.w));
        }

        #endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>

        #region public static bool operator !=(JVector value1, JVector value2)

        public static bool operator !=(LSVector4 value1, LSVector4 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y) && (value1.z == value2.z))
            {
                return (value1.w != value2.w);
            }

            return true;
        }

        #endregion

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>

        #region public static JVector Min(JVector value1, JVector value2)

        public static LSVector4 Min(LSVector4 value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref LSVector4 value1, ref LSVector4 value2, out LSVector4 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
            result.w = (value1.w < value2.w) ? value1.w : value2.w;
        }

        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>

        #region public static JVector Max(JVector value1, JVector value2)

        public static LSVector4 Max(LSVector4 value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Max(ref value1, ref value2, out result);
            return result;
        }

        public static FP Distance(LSVector4 v1, LSVector4 v2)
        {
            return FP.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z) + (v1.w - v2.w) * (v1.w - v2.w));
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref LSVector4 value1, ref LSVector4 value2, out LSVector4 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
            result.w = (value1.w > value2.w) ? value1.w : value2.w;
        }

        #endregion

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>

        #region public void MakeZero()

        public void MakeZero()
        {
            x = FP.Zero;
            y = FP.Zero;
            z = FP.Zero;
            w = FP.Zero;
        }

        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>

        #region public bool IsZero()

        public bool IsZero()
        {
            return (this.sqrMagnitude == FP.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }

        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>

        #region public static JVector Transform(JVector position, JMatrix matrix)

        public static LSVector4 Transform(LSVector4 position, LSMatrix4x4 matrix)
        {
            LSVector4 result;
            LSVector4.Transform(ref position, ref matrix, out result);
            return result;
        }

        public static LSVector4 Transform(LSVector3 position, LSMatrix4x4 matrix)
        {
            LSVector4 result;
            LSVector4.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref LSVector3 vector, ref LSMatrix4x4 matrix, out LSVector4 result)
        {
            result.x = vector.x * matrix.M11 + vector.y * matrix.M12 + vector.z * matrix.M13 + matrix.M14;
            result.y = vector.x * matrix.M21 + vector.y * matrix.M22 + vector.z * matrix.M23 + matrix.M24;
            result.z = vector.x * matrix.M31 + vector.y * matrix.M32 + vector.z * matrix.M33 + matrix.M34;
            result.w = vector.x * matrix.M41 + vector.y * matrix.M42 + vector.z * matrix.M43 + matrix.M44;
        }

        public static void Transform(ref LSVector4 vector, ref LSMatrix4x4 matrix, out LSVector4 result)
        {
            result.x = vector.x * matrix.M11 + vector.y * matrix.M12 + vector.z * matrix.M13 + vector.w * matrix.M14;
            result.y = vector.x * matrix.M21 + vector.y * matrix.M22 + vector.z * matrix.M23 + vector.w * matrix.M24;
            result.z = vector.x * matrix.M31 + vector.y * matrix.M32 + vector.z * matrix.M33 + vector.w * matrix.M34;
            result.w = vector.x * matrix.M41 + vector.y * matrix.M42 + vector.z * matrix.M43 + vector.w * matrix.M44;
        }

        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>

        #region public static FP Dot(JVector vector1, JVector vector2)

        public static FP Dot(LSVector4 vector1, LSVector4 vector2)
        {
            return LSVector4.Dot(ref vector1, ref vector2);
        }


        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static FP Dot(ref LSVector4 vector1, ref LSVector4 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z) + (vector1.w * vector2.w);
        }

        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>

        #region public static void Add(JVector value1, JVector value2)

        public static LSVector4 Add(LSVector4 value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref LSVector4 value1, ref LSVector4 value2, out LSVector4 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
            result.z = value1.z + value2.z;
            result.w = value1.w + value2.w;
        }

        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static LSVector4 Divide(LSVector4 value1, FP scaleFactor)
        {
            LSVector4 result;
            LSVector4.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref LSVector4 value1, FP scaleFactor, out LSVector4 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
            result.w = value1.w / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>

        #region public static JVector Subtract(JVector value1, JVector value2)

        public static LSVector4 Subtract(LSVector4 value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref LSVector4 value1, ref LSVector4 value2, out LSVector4 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
            result.z = value1.z - value2.z;
            result.w = value1.w - value2.w;
        }

        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>

        #region public override int GetHashCode()

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>

        #region public static JVector Negate(JVector value)

        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
            this.w = -this.w;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static LSVector4 Negate(LSVector4 value)
        {
            LSVector4 result;
            LSVector4.Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref LSVector4 value, out LSVector4 result)
        {
            result.x = -value.x;
            result.y = -value.y;
            result.z = -value.z;
            result.w = -value.w;
        }

        #endregion

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>

        #region public static JVector Normalize(JVector value)

        public static LSVector4 Normalize(LSVector4 value)
        {
            LSVector4 result;
            LSVector4.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            FP num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z) + (this.w * this.w);
            FP num = FP.One / FP.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
            this.w *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref LSVector4 value, out LSVector4 result)
        {
            FP num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z) + (value.w * value.w);
            FP num = FP.One / FP.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
            result.w = value.w * num;
        }

        #endregion

        #region public static void Swap(ref JVector vector1, ref JVector vector2)

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref LSVector4 vector1, ref LSVector4 vector2)
        {
            FP temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;

            temp = vector1.w;
            vector1.w = vector2.w;
            vector2.w = temp;
        }

        #endregion

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>

        #region public static JVector Multiply(JVector value1, FP scaleFactor)

        public static LSVector4 Multiply(LSVector4 value1, FP scaleFactor)
        {
            LSVector4 result;
            LSVector4.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref LSVector4 value1, FP scaleFactor, out LSVector4 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
            result.w = value1.w * scaleFactor;
        }

        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>

        #region public static FP operator *(JVector value1, JVector value2)

        public static FP operator *(LSVector4 value1, LSVector4 value2)
        {
            return LSVector4.Dot(ref value1, ref value2);
        }

        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>

        #region public static JVector operator *(JVector value1, FP value2)

        public static LSVector4 operator *(LSVector4 value1, FP value2)
        {
            LSVector4 result;
            LSVector4.Multiply(ref value1, value2, out result);
            return result;
        }

        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>

        #region public static JVector operator *(FP value1, JVector value2)

        public static LSVector4 operator *(FP value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Multiply(ref value2, value1, out result);
            return result;
        }

        #endregion

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>

        #region public static JVector operator -(JVector value1, JVector value2)

        public static LSVector4 operator -(LSVector4 value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Subtract(ref value1, ref value2, out result);
            return result;
        }

        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>

        #region public static JVector operator +(JVector value1, JVector value2)

        public static LSVector4 operator +(LSVector4 value1, LSVector4 value2)
        {
            LSVector4 result;
            LSVector4.Add(ref value1, ref value2, out result);
            return result;
        }

        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static LSVector4 operator /(LSVector4 value1, FP value2)
        {
            LSVector4 result;
            LSVector4.Divide(ref value1, value2, out result);
            return result;
        }

        public LSVector2 ToTSVector2()
        {
            return new LSVector2(this.x, this.y);
        }

        public LSVector3 ToTSVector()
        {
            return new LSVector3(this.x, this.y, this.z);
        }
    }
}