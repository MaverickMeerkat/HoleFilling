using System;
using System.Collections.Generic;
using HoleFilling;
using HoleFilling.DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HoleFillingTests
{
    [TestClass]
    public class MooreTraceTests
    {
        private HoleFinder ArrangeHoleHandler(float[,] t)
        {
            ImageMatrix img = new ImageMatrix(t);
            return new HoleFinder(img);
        }

        [TestMethod, Timeout(1000)]
        public void BasicSanityTest()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F },
                { 0.1F, -1F, 0.3F },
                { 0.1F, 0.2F, 0.3F }
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 8);
            Assert.AreEqual(hole.HolePixels.Count, 1);
        }

        [TestMethod, Timeout(1000)]
        public void BasicTestTest()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F},
                { 0.2F, 0.3F, -1F, 0.3F, 0.4F},
                { 0.3F, -1F, -1F, -1F, 0.5F},
                { 0.4F, 0.5F, -1F, 0.5F, 0.6F},
                { 0.5F, 0.6F, 0.6F, 0.6F, 0.7F }
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 16);
            Assert.AreEqual(hole.HolePixels.Count, 5);
        }

        [TestMethod, Timeout(1000)]
        public void DumbbellShapeTest()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, -1F, 0.3F, 0.4F, 0.5F},
                { 0.1F, -1F, -1F, -1F, -1F, 0.4F, 0.5F},
                { 0.1F, -1F, -1F, -1F, -1F, 0.2F, 0.5F},
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.2F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 18);
            Assert.AreEqual(hole.HolePixels.Count, 9);
        }

        [TestMethod, Timeout(1000)]
        public void Enclave1Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, -1F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, -1F, 0.2F, -1F, 0.4F, 0.5F},
                { 0.1F, -1F, 0.2F, 0.2F, 0.3F, -1F, 0.5F},
                { 0.1F, 0.2F, -1F, 0.2F, -1F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 27);
            Assert.AreEqual(hole.HolePixels.Count, 7);
        }

        [TestMethod, Timeout(1000)]
        public void Enclave2Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, -1F, 0.3F, 0.4F, 0.5F},
                { 0.1F, -1F, 0.3F, 0.2F, -1F, 0.4F, 0.5F},
                { 0.1F, -1F, 0.2F, 0.2F, 0.3F, -1F, 0.5F},
                { 0.1F, 0.2F, -1F, 0.2F, -1F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, -1F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 30);
            Assert.AreEqual(hole.HolePixels.Count, 8);
        }

        [TestMethod, Timeout(1000)]
        public void Edges1Test()
        {
            // arrange
            var t = new float[,]
            {
                { -1F, -1F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { -1F, -1F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 5);
            Assert.AreEqual(hole.HolePixels.Count, 4);
        }

        [TestMethod, Timeout(1000)]
        public void Edges2Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, -1F, -1F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, -1F, -1F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 5);
            Assert.AreEqual(hole.HolePixels.Count, 4);
        }

        [TestMethod, Timeout(1000)]
        public void Edges3Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { -1F, -1F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { -1F, -1F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 5);
            Assert.AreEqual(hole.HolePixels.Count, 4);
        }

        [TestMethod, Timeout(1000)]
        public void Edges4Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, -1F, -1F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, -1F, -1F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 5);
            Assert.AreEqual(hole.HolePixels.Count, 4);
        }

        [TestMethod, Timeout(1000)]
        public void Edges5Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F, 0.2F, -1F, -1F, -1F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, -1F, -1F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, -1F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 7);
            Assert.AreEqual(hole.HolePixels.Count, 6);
        }


        [TestMethod, Timeout(1000)]
        public void Edges6Test()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { 0.1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { -1F, 0.2F, 0.3F, 0.2F, 0.3F, 0.4F, 0.5F},
                { -1F, -1F, 0.2F, 0.2F, 0.3F, 0.4F, 0.5F},
                { -1F, -1F, -1F, 0.2F, 0.3F, 0.4F, 0.5F}
            };

            var holeFinder = ArrangeHoleHandler(t);

            // act        
            var hole = holeFinder.FindHole(new MooreTrace());

            // assert
            Assert.AreEqual(hole.Boundary.Count, 7);
            Assert.AreEqual(hole.HolePixels.Count, 6);
        }
    }
}
