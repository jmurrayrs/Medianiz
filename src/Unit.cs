using System;

namespace Mediator
{
    /// <summary>
    /// Represents a unit of work or a void return type.
    /// This class is used in scenarios where a method does not return a value,
    /// but you still want to use it in a generic context.
    /// It is a singleton type, meaning there is only one instance of this class.
    /// It implements IEquatable<Unit> and IComparable<Unit> to allow for comparisons
    /// and equality checks, although all instances are considered equal.
    /// </summary>
    public sealed class Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        public static readonly Unit Value = new();


        private Unit() { }

        public int CompareTo(Unit other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return 0; // Since Unit is a singleton, all instances are considered equal.
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (obj is Unit otherUnit)
            {
                return CompareTo(otherUnit);
            }
            throw new ArgumentException("Object is not a Unit", nameof(obj));
        }

        public bool Equals(Unit other)
        {
            if (other == null) return false;
            return ReferenceEquals(this, other); // Since Unit is a singleton, we check reference equality.
        }
    }
}