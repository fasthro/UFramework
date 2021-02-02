﻿using System;

namespace Lockstep
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [Serializable]
    public struct LSVector3
    {
        private static FP ZeroEpsilonSq = LSMath.Epsilon;
        internal static LSVector3 InternalZero;
        internal static LSVector3 Arbitrary;

        /// <summary>The X component of the vector.</summary>
        public FP x;

        /// <summary>The Y component of the vector.</summary>
        public FP y;

        /// <summary>The Z component of the vector.</summary>
        public FP z;

        #region Static readonly variables

        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly LSVector3 zero;

        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly LSVector3 left;

        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly LSVector3 right;

        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly LSVector3 up;

        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly LSVector3 down;

        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly LSVector3 back;

        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly LSVector3 forward;

        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly LSVector3 one;

        /// <summary>
        /// A vector with components 
        /// (FP.MinValue,FP.MinValue,FP.MinValue);
        /// </summary>
        public static readonly LSVector3 MinValue;

        /// <summary>
        /// A vector with components 
        /// (FP.MaxValue,FP.MaxValue,FP.MaxValue);
        /// </summary>
        public static readonly LSVector3 MaxValue;

        #endregion

        #region Private static constructor

        static LSVector3()
        {
            one = new LSVector3(1, 1, 1);
            zero = new LSVector3(0, 0, 0);
            left = new LSVector3(-1, 0, 0);
            right = new LSVector3(1, 0, 0);
            up = new LSVector3(0, 1, 0);
            down = new LSVector3(0, -1, 0);
            back = new LSVector3(0, 0, -1);
            forward = new LSVector3(0, 0, 1);
            MinValue = new LSVector3(FP.MinValue);
            MaxValue = new LSVector3(FP.MaxValue);
            Arbitrary = new LSVector3(1, 1, 1);
            InternalZero = zero;
        }

        #endregion

        public static LSVector3 Abs(LSVector3 other)
        {
            return new LSVector3(FP.Abs(other.x), FP.Abs(other.y), FP.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public FP sqrMagnitude
        {
            get { return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z)); }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public FP magnitude
        {
            get
            {
                FP num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return FP.Sqrt(num);
            }
        }

        public static LSVector3 ClampMagnitude(LSVector3 vector, FP maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public LSVector3 normalized
        {
            get
            {
                LSVector3 result = new LSVector3(this.x, this.y, this.z);
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
        public LSVector3(int x, int y, int z)
        {
            this.x = (FP) x;
            this.y = (FP) y;
            this.z = (FP) z;
        }

        public LSVector3(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(LSVector3 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public void Set(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public LSVector3(FP xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

        public static LSVector3 Lerp(LSVector3 from, LSVector3 to, FP percent)
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
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
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
            if (!(obj is LSVector3)) return false;
            LSVector3 other = (LSVector3) obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }

        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static LSVector3 Scale(LSVector3 vecA, LSVector3 vecB)
        {
            LSVector3 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>

        #region public static bool operator ==(JVector value1, JVector value2)

        public static bool operator ==(LSVector3 value1, LSVector3 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }

        #endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>

        #region public static bool operator !=(JVector value1, JVector value2)

        public static bool operator !=(LSVector3 value1, LSVector3 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
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

        public static LSVector3 Min(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref LSVector3 value1, ref LSVector3 value2, out LSVector3 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }

        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>

        #region public static JVector Max(JVector value1, JVector value2)

        public static LSVector3 Max(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Max(ref value1, ref value2, out result);
            return result;
        }

        public static FP Distance(LSVector3 v1, LSVector3 v2)
        {
            return FP.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref LSVector3 value1, ref LSVector3 value2, out LSVector3 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
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

        public static LSVector3 Transform(LSVector3 position, LSMatrix matrix)
        {
            LSVector3 result;
            LSVector3.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref LSVector3 position, ref LSMatrix matrix, out LSVector3 result)
        {
            FP num0 = ((position.x * matrix.M11) + (position.y * matrix.M21)) + (position.z * matrix.M31);
            FP num1 = ((position.x * matrix.M12) + (position.y * matrix.M22)) + (position.z * matrix.M32);
            FP num2 = ((position.x * matrix.M13) + (position.y * matrix.M23)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Transforms a vector by the transposed of the given Matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransposedTransform(ref LSVector3 position, ref LSMatrix matrix, out LSVector3 result)
        {
            FP num0 = ((position.x * matrix.M11) + (position.y * matrix.M12)) + (position.z * matrix.M13);
            FP num1 = ((position.x * matrix.M21) + (position.y * matrix.M22)) + (position.z * matrix.M23);
            FP num2 = ((position.x * matrix.M31) + (position.y * matrix.M32)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>

        #region public static FP Dot(JVector vector1, JVector vector2)

        public static FP Dot(LSVector3 vector1, LSVector3 vector2)
        {
            return LSVector3.Dot(ref vector1, ref vector2);
        }


        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static FP Dot(ref LSVector3 vector1, ref LSVector3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }

        #endregion

        // Projects a vector onto another vector.
        public static LSVector3 Project(LSVector3 vector, LSVector3 onNormal)
        {
            FP sqrtMag = Dot(onNormal, onNormal);
            if (sqrtMag < LSMath.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrtMag;
        }

        // Projects a vector onto a plane defined by a normal orthogonal to the plane.
        public static LSVector3 ProjectOnPlane(LSVector3 vector, LSVector3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }


        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
        public static FP Angle(LSVector3 from, LSVector3 to)
        {
            return LSMath.Acos(LSMath.Clamp(Dot(from.normalized, to.normalized), -FP.ONE, FP.ONE)) * LSMath.Rad2Deg;
        }

        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        public static FP SignedAngle(LSVector3 from, LSVector3 to, LSVector3 axis)
        {
            LSVector3 fromNorm = from.normalized, toNorm = to.normalized;
            FP unsignedAngle = LSMath.Acos(LSMath.Clamp(Dot(fromNorm, toNorm), -FP.ONE, FP.ONE)) * LSMath.Rad2Deg;
            FP sign = LSMath.Sign(Dot(axis, Cross(fromNorm, toNorm)));
            return unsignedAngle * sign;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>

        #region public static void Add(JVector value1, JVector value2)

        public static LSVector3 Add(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref LSVector3 value1, ref LSVector3 value2, out LSVector3 result)
        {
            FP num0 = value1.x + value2.x;
            FP num1 = value1.y + value2.y;
            FP num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static LSVector3 Divide(LSVector3 value1, FP scaleFactor)
        {
            LSVector3 result;
            LSVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref LSVector3 value1, FP scaleFactor, out LSVector3 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>

        #region public static JVector Subtract(JVector value1, JVector value2)

        public static LSVector3 Subtract(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref LSVector3 value1, ref LSVector3 value2, out LSVector3 result)
        {
            FP num0 = value1.x - value2.x;
            FP num1 = value1.y - value2.y;
            FP num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        #endregion

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>

        #region public static JVector Cross(JVector vector1, JVector vector2)

        public static LSVector3 Cross(LSVector3 vector1, LSVector3 vector2)
        {
            LSVector3 result;
            LSVector3.Cross(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of both vectors.</param>
        public static void Cross(ref LSVector3 vector1, ref LSVector3 vector2, out LSVector3 result)
        {
            FP num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            FP num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            FP num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }

        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>

        #region public override int GetHashCode()

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
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
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static LSVector3 Negate(LSVector3 value)
        {
            LSVector3 result;
            LSVector3.Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref LSVector3 value, out LSVector3 result)
        {
            FP num0 = -value.x;
            FP num1 = -value.y;
            FP num2 = -value.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        #endregion

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>

        #region public static JVector Normalize(JVector value)

        public static LSVector3 Normalize(LSVector3 value)
        {
            LSVector3 result;
            LSVector3.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            FP num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            FP num = FP.One / FP.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref LSVector3 value, out LSVector3 result)
        {
            FP num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            FP num = FP.One / FP.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }

        #endregion

        #region public static void Swap(ref JVector vector1, ref JVector vector2)

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref LSVector3 vector1, ref LSVector3 vector2)
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
        }

        #endregion

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>

        #region public static JVector Multiply(JVector value1, FP scaleFactor)

        public static LSVector3 Multiply(LSVector3 value1, FP scaleFactor)
        {
            LSVector3 result;
            LSVector3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref LSVector3 value1, FP scaleFactor, out LSVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }

        #endregion

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>

        #region public static JVector operator %(JVector value1, JVector value2)

        public static LSVector3 operator %(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Cross(ref value1, ref value2, out result);
            return result;
        }

        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>

        #region public static FP operator *(JVector value1, JVector value2)

        public static FP operator *(LSVector3 value1, LSVector3 value2)
        {
            return LSVector3.Dot(ref value1, ref value2);
        }

        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>

        #region public static JVector operator *(JVector value1, FP value2)

        public static LSVector3 operator *(LSVector3 value1, FP value2)
        {
            LSVector3 result;
            LSVector3.Multiply(ref value1, value2, out result);
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

        public static LSVector3 operator *(FP value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Multiply(ref value2, value1, out result);
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

        public static LSVector3 operator -(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Subtract(ref value1, ref value2, out result);
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

        public static LSVector3 operator +(LSVector3 value1, LSVector3 value2)
        {
            LSVector3 result;
            LSVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static LSVector3 operator /(LSVector3 value1, FP value2)
        {
            LSVector3 result;
            LSVector3.Divide(ref value1, value2, out result);
            return result;
        }

        public LSVector2 ToTSVector2()
        {
            return new LSVector2(this.x, this.y);
        }

        public LSVector4 ToTSVector4()
        {
            return new LSVector4(this.x, this.y, this.z, FP.One);
        }
    }
}