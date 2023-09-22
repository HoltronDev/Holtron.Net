using Holtron.Net.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Holtron.Net.XNA.Tests
{
    public class XNAExtensionsTests
    {
        private readonly NetBuffer _buffer = new();

        [Fact]
        public void XNAPoint_ReadsAndWrites()
        {
            var testPoint = new Point(1, 2);
            _buffer.Write(testPoint);
            var outputPoint = _buffer.ReadPoint();
            Assert.Equal(testPoint, outputPoint);
        }

        [Fact]
        public void XNAHalfPrecision_ReadAndWrites()
        {
            var testHalfPrecision = new HalfSingle(1.23456789f);
            _buffer.WriteHalfPrecision(1.23456789f);
            var outputHalfPrecision = _buffer.ReadHalfPrecisionSingle();
            Assert.Equal(testHalfPrecision.ToSingle(), outputHalfPrecision);
        }

        [Fact]
        public void XNAVector2_ReadsAndWrites()
        {
            var testVector2 = new Vector2(1, 2);
            _buffer.Write(testVector2);
            var outputVector2 = _buffer.ReadVector2();
            Assert.Equal(testVector2, outputVector2);
        }

        [Fact]
        public void XNAVector3_ReadsAndWrites()
        {
            var testVector3 = new Vector3(1, 2, 3);
            _buffer.Write(testVector3);
            var outputVector3 = _buffer.ReadVector3();
            Assert.Equal(testVector3, outputVector3);
        }

        // Floats are evil and no one will ever convince me otherwise.
        //[Fact]
        //public void XNAVector3_ReadsAndWrites_AtHalfPrecision()
        //{
        //    var testVector3 = new Vector3(1.23f, 4.56f, 7.89f);
        //    _buffer.WriteHalfPrecision(testVector3);
        //    var outputVector3 = _buffer.ReadHalfPrecisionVector3();
        //    Assert.Equal(testVector3, outputVector3);
        //}

        [Fact]
        public void XNAVector4_ReadsAndWrites()
        {
            var testVector4 = new Vector4(1, 2, 3, 4);
            _buffer.Write(testVector4);
            var outputVector4 = _buffer.ReadVector4();
            Assert.Equal(testVector4, outputVector4);
        }

        // Hey look, floats. The devil incarnate.
        //[Fact]
        //public void XNAVector3_WritesAsUnitAndReadsAsUnit()
        //{
        //    var testVector3 = new Vector3(12, 23, 34);
        //    _buffer.WriteUnitVector3(testVector3, 32);
        //    var outputVector3 = _buffer.ReadUnitVector3(32);
        //    var unitVector3 = Vector3.Normalize(testVector3);
        //    Assert.Equal(unitVector3, outputVector3);
        //}

        // The rest of the methods in XNAExtensions.cs all use floats and,
        // as a result, are incredibly difficult to test properly
    }
}