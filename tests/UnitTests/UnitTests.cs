using Medianiz.Tests.UnitTests.Shared;
using Mediator;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Medianiz.Tests.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void Value_Should_BeSingletonInstance()
        {
            // Arrange & Act
            var unit1 = Unit.Value;
            var unit2 = Unit.Value;

            // Assert
            Assert.Same(unit1, unit2);
        }

        [Fact]
        public void Constructor_Should_BePrivate()
        {
            // Arrange
            var unitType = typeof(Unit);

            // Act
            var constructors = unitType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.Empty(constructors);
        }

        [Fact]
        public void CompareTo_WithOtherUnit_Should_ReturnZero()
        {
            // Arrange
            var unit1 = Unit.Value;
            var unit2 = Unit.Value;

            // Act
            var result = unit1.CompareTo(unit2);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareTo_WithNullUnit_Should_ThrowArgumentNullException()
        {
            // Arrange
            var unit = Unit.Value;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => unit.CompareTo(null));
        }

        [Fact]
        public void CompareTo_WithObject_Should_ReturnZeroWhenUnit()
        {
            // Arrange
            var unit1 = Unit.Value;
            object unit2 = Unit.Value;

            // Act
            var result = unit1.CompareTo(unit2);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareTo_WithNullObject_Should_ThrowArgumentNullException()
        {
            // Arrange
            var unit = Unit.Value;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => unit.CompareTo((object)null!));
        }

        [Fact]
        public void CompareTo_WithNonUnitObject_Should_ThrowArgumentException()
        {
            // Arrange
            var unit = Unit.Value;
            var obj = new object();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => unit.CompareTo(obj));
        }

        [Fact]
        public void Equals_WithOtherUnit_Should_ReturnTrue()
        {
            // Arrange
            var unit1 = Unit.Value;
            var unit2 = Unit.Value;

            // Act
            var result = unit1.Equals(unit2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithNullUnit_Should_ReturnFalse()
        {
            // Arrange
            var unit = Unit.Value;

            // Act
            var result = unit.Equals(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_WithDifferentReference_Should_ReturnFalse()
        {
            // Arrange
            var unit1 = Unit.Value;
            object unit2 = new object(); // Not a Unit instance

            // Act
            var result = unit1.Equals(unit2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_Should_ReturnConsistentValue()
        {
            var unit = Unit.Value;

            // Act
            var hashCode1 = unit.GetHashCode();
            var hashCode2 = unit.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
            Assert.Equal(0, hashCode1);
        }

        [Fact]
        public void EqualityOperator_Should_ReturnTrueForSameInstance()
        {
            // Arrange
            var unit1 = Unit.Value;
            var unit2 = Unit.Value;

            // Act & Assert
            Assert.True(unit1 == unit2);
        }

        [Fact]
        public void InequalityOperator_Should_ReturnFalseForSameInstance()
        {
            // Arrange
            var unit1 = Unit.Value;
            var unit2 = Unit.Value;

            // Act & Assert
            Assert.False(unit1 != unit2);
        }
    }
}

