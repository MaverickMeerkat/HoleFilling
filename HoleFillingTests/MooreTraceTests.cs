using System;
using System.Collections.Generic;
using HoleFilling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HoleFillingTests
{
    [TestClass]
    public class MooreTraceTests
    {
        private HoleHandler ArrangeHoleHandler(float[,] t)
        {
            ImageMatrix img = new ImageMatrix(t);
            var weightFunc = new DefaultWeightFunction(new Dictionary<string, object>
            {
                ["z"] = 5,
                ["e"] = 0.0001
            });
            return new HoleHandler(img, weightFunc);
        }

        [TestMethod, Timeout(5000)]
        public void BasicSanityTest()
        {
            // arrange
            var t = new float[,]
            {
                { 0.1F, 0.2F, 0.3F },
                { 0.1F, -1F, 0.3F },
                { 0.1F, 0.2F, 0.3F }
            };
            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 8);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 16);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 18);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 27);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 30);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 5);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 5);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 5);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 5);
        }

        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 7);
        }


        [TestMethod, Timeout(5000)]
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

            var hole_handler = ArrangeHoleHandler(t);

            // act        
            var bound = hole_handler.FindBoundary(new MooreTrace());

            // assert
            Assert.AreEqual(bound.Count, 7);
        }
    }
}
