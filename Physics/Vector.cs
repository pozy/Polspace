﻿using System;

namespace Physics
{
    public readonly struct Vector : IEquatable<Vector>
    {
        public readonly double X;
        public readonly double Y;

        private Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector Zero { get; } = new(0, 0);

        public double GetLength() => Math.Sqrt(X * X + Y * Y);

        public static Vector New(double x, double y) => new(x, y);
        public static Vector NewRotated(double angle) => new(Math.Sin(angle), Math.Cos(angle));

        public static Vector operator -(Vector v) => new(-v.X, -v.Y);

        public static Vector operator -(Vector v1, Vector v2) => new(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector operator +(Vector v1, Vector v2) => new(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector operator *(Vector v, double x) => new(v.X * x, v.Y * x);

        public static Vector operator *(double x, Vector v) => new(v.X * x, v.Y * x);

        public static Vector operator /(Vector v, double x) => new(v.X / x, v.Y / x);

        public static double Dot(Vector v1, Vector v2) => v1.X * v2.X + v1.Y + v2.Y;

        public static double Cross(Vector v1, Vector v2) => v1.X * v2.Y - v2.X * v1.Y;

        public static bool operator ==(Vector v1, Vector v2) => v1.Equals(v2);

        public static bool operator !=(Vector v1, Vector v2) => !v1.Equals(v2);

        public override string ToString() => $"({X},{Y})";

        public override bool Equals(object? obj) => obj is Vector other && Equals(other);

        // ReSharper disable CompareOfFloatsByEqualityOperator
        public bool Equals(Vector other) => X == other.X && Y == other.Y;
        // ReSharper restore CompareOfFloatsByEqualityOperator

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}