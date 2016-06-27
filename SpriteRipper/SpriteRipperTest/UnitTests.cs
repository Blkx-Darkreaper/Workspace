using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpriteRipper;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace SpriteRipperTest
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void CompareSimilarPatterns()
        {
            // Setup
            int bitsPerColour = 8;

            Bitmap image1 = new Bitmap(@"C:\Users\nicB\Documents\tile1.png");
            Tile tile1 = new Tile(image1, bitsPerColour);
            Console.WriteLine("Tile 1: " + tile1.ToString());

            Bitmap image2 = new Bitmap(@"C:\Users\nicB\Documents\tile1a.png");
            Tile tile2 = new Tile(image2, bitsPerColour);
            Console.WriteLine("Tile 2: " + tile2.ToString());

            // Execution
            Tuple<float, float> results = tile1.GetMatches(tile2);
            float patternMatch = results.Item1;
            float colourMatch = results.Item2;

            bool identical = tile1.IdenticalTo(tile2);

            // Assertion
            float expectedPatternMatch = 223/255f;
            Assert.IsTrue(patternMatch == expectedPatternMatch, string.Format("Expected {0}, Actual {1}", expectedPatternMatch, patternMatch));

            Assert.IsFalse(identical);
        }

        [TestMethod]
        public void CompareSimilarToMaster()
        {
            // Setup
            int bitsPerColour = 8;
            int accuracy = 2;
            int tileCount = 32;

            Bitmap image1 = new Bitmap(@"C:\Users\nicB\Documents\tile1.png");
            Tile master = new Tile(image1, bitsPerColour);
            //Console.WriteLine("Tile 1: " + tileToDraw.ToString());

            Tile identicalTile = new Tile(image1, bitsPerColour);

            Bitmap image2 = new Bitmap(@"C:\Users\nicB\Documents\tile1a.png");
            Tile similarTile = new Tile(image2, bitsPerColour);
            //Console.WriteLine("Tile 2: " + similarTile.ToString());

            Bitmap image3 = new Bitmap(@"C:\Users\nicB\Documents\tile1b.png");
            Tile otherSimilarTile = new Tile(image3, bitsPerColour);
            //Console.WriteLine("Tile 3: " + otherSimilarTile.ToString());

            Tuple<float, float> identicalResults = master.GetMatches(identicalTile);
            float patternMatch = identicalResults.Item1;
            float colourMatch = identicalResults.Item2;
            int identicalHashCode = Tile.GetHashcode(accuracy, patternMatch, colourMatch, tileCount);

            // Execution
            Tuple<float, float> similarResults = master.GetMatches(similarTile);
            patternMatch = similarResults.Item1;
            colourMatch = similarResults.Item2;
            int similarHashCode = Tile.GetHashcode(accuracy, patternMatch, colourMatch, tileCount);

            Tuple<float, float> otherSimilarResults = master.GetMatches(otherSimilarTile);
            patternMatch = similarResults.Item1;
            colourMatch = similarResults.Item2;
            int otherSimilarHashCode = Tile.GetHashcode(accuracy, patternMatch, colourMatch, tileCount);

            // Assertion
            Assert.IsTrue(identicalHashCode == 0, string.Format("Expected 0, Actual {0}", identicalHashCode));

            Assert.IsTrue(similarHashCode == otherSimilarHashCode, string.Format("Expected true, Actual {0} == {1}", similarHashCode, otherSimilarHashCode));
        }

        [TestMethod]
        public void LimitedTest()
        {
            // Setup
            int bitsPerColour = 8;
            int tileSize = 16;
            float patternThreshold = .5f;
            float colourThreshold = .5f;
            int offsetX = 0;
            int offsetY = 0;

            Bitmap image = new Bitmap(@"C:\Users\nicB\Documents\test2.png");

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            Program.SortTiles(patternThreshold, colourThreshold);

            // Assertion
            int expTileCount = 21;
            int expGroupCount = 2;
            int expGroup1Count = 7;
            int expGroup2Count = 1;

            int tileCount = Program.GetTileCount();
            Assert.IsTrue(tileCount == expTileCount, string.Format("Expected {0}, Actual {1}", expTileCount, tileCount));

            int groupCount = Program.GetGroupCount();
            Assert.IsTrue(groupCount == expGroupCount, string.Format("Groups: Expected {0}, Actual {1}", expGroupCount, groupCount));

            int group1Count = Program.GetCountOfGroup(0);
            int group2Count = Program.GetCountOfGroup(1);

            Assert.IsTrue(group1Count == expGroup1Count, string.Format("Group1 members: Expected {0}, Actual {1}", expGroup1Count, group1Count));
            Assert.IsTrue(group2Count == expGroup2Count, string.Format("Group2 members: Expected {0}, Actual {1}", expGroup2Count, group2Count));
        }

        [TestMethod]
        public void LimitedThreadedTest()
        {
            // Setup
            int bitsPerColour = 8;
            int tileSize = 16;
            float patternThreshold = .5f;
            float colourThreshold = .5f;
            int offsetX = 0;
            int offsetY = 0;

            Bitmap image = new Bitmap(@"C:\Users\nicB\Documents\test2.png");

            // Execution
            Program.LoadTilesThreaded(image, bitsPerColour, tileSize, offsetX, offsetY);
            Program.SortTiles(patternThreshold, colourThreshold);

            // Assertion
            int expTileCount = 21;
            int expGroupCount = 2;
            int expGroup1Count = 7;
            int expGroup2Count = 1;

            int tileCount = Program.GetTileCount();
            Assert.IsTrue(tileCount == expTileCount, string.Format("Expected {0}, Actual {1}", expTileCount, tileCount));

            int groupCount = Program.GetGroupCount();
            Assert.IsTrue(groupCount == expGroupCount, string.Format("Groups: Expected {0}, Actual {1}", expGroupCount, groupCount));

            int group1Count = Program.GetCountOfGroup(0);
            int group2Count = Program.GetCountOfGroup(1);

            Assert.IsTrue(group1Count == expGroup1Count, string.Format("Group1 members: Expected {0}, Actual {1}", expGroup1Count, group1Count));
            Assert.IsTrue(group2Count == expGroup2Count, string.Format("Group2 members: Expected {0}, Actual {1}", expGroup2Count, group2Count));
        }

        [TestMethod]
        public void LimitedTest2()
        {
            // Setup
            int bitsPerColour = 8;
            int tileSize = 16;
            float patternThreshold = .5f;
            float colourThreshold = .5f;
            int offsetX = 0;
            int offsetY = 0;

            Bitmap image = new Bitmap(@"C:\Users\nicB\Documents\test2a.png");

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            Program.SortTiles(patternThreshold, colourThreshold);

            // Assertion
            int expTileCount = 42;
            int expGroupCount = 2;
            int expGroup1Count = 7;
            int expGroup2Count = 1;

            int tileCount = Program.GetTileCount();
            Assert.IsTrue(tileCount == expTileCount, string.Format("Expected {0}, Actual {1}", expTileCount, tileCount));

            int groupCount = Program.GetGroupCount();
            Assert.IsTrue(groupCount == expGroupCount, string.Format("Groups: Expected {0}, Actual {1}", expGroupCount, groupCount));

            int group1Count = Program.GetCountOfGroup(0);
            int group2Count = Program.GetCountOfGroup(1);

            Assert.IsTrue(group1Count == expGroup1Count, string.Format("Group1 members: Expected {0}, Actual {1}", expGroup1Count, group1Count));
            Assert.IsTrue(group2Count == expGroup2Count, string.Format("Group2 members: Expected {0}, Actual {1}", expGroup2Count, group2Count));
        }

        [TestMethod]
        public void FullTest()
        {
            // Setup
            int bitsPerColour = 8;
            int tileSize = 16;
            float patternThreshold = .5f;
            float colourThreshold = .5f;
            int offsetX = 0;
            int offsetY = 0;
            int zoom = 1;

            int tilesWide = 5;

            Bitmap image = new Bitmap(@"C:\Users\nicB\Documents\test.png");

            PixelFormat format = image.PixelFormat;

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            Program.SortTiles(patternThreshold, colourThreshold);
            Bitmap groupedTileset = Program.GetGroupedTileset(format, tileSize, zoom);
            Bitmap ungroupedTileset = Program.GetTileset(format, tileSize, tilesWide, zoom);

            // Assertion
            int expectedWidth = tilesWide * tileSize * zoom + tilesWide - 1;
            //int expectedHeight;

            int ungroupedWidth = ungroupedTileset.Width;
            int ungroupedHeight = ungroupedTileset.Height;

            Assert.IsTrue(ungroupedWidth == expectedWidth, string.Format("Width: Expected {0}, Actual {1}", expectedWidth, ungroupedWidth));
            //Assert.IsTrue(ungroupedHeight == expectedHeight, string.Format("Width: Expected {0}, Actual {1}", expectedHeight, ungroupedHeight));
        }

        [TestMethod]
        public void TimingTest()
        {
            // Setup
            int bitsPerColour = 8;
            int tileSize = 16;
            float patternThreshold = .5f;
            float colourThreshold = .5f;
            int offsetX = 0;
            int offsetY = 0;

            Bitmap image = new Bitmap(@"C:\Users\nicB\Documents\test.png");

            long sortTimes8BitSum = 0;
            long sortTimes4BitSum = 0;
            int runs = 4;

            // Execution

            // 8Bit sorting
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            for (int i = 0; i < runs; i++)
            {
                Program.SortTiles(patternThreshold, colourThreshold);
                long sortTime = Program.sortTime;
                sortTimes8BitSum += sortTime;
            }

            long averageSortTime8Bit = sortTimes8BitSum / runs;

            // 4Bit sorting
            bitsPerColour = 4;
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            for (int i = 0; i < runs; i++)
            {
                Program.SortTiles(patternThreshold, colourThreshold);
                long sortTime = Program.sortTime;
                sortTimes4BitSum += sortTime;
            }

            long averageSortTime4Bit = sortTimes4BitSum / runs;

            // Assertion
            Assert.IsTrue(averageSortTime4Bit < averageSortTime8Bit, string.Format("Average sort time: 4Bit {0}ms, 8Bit {1}ms", averageSortTime4Bit, averageSortTime8Bit));
        }

        [TestMethod]
        public void StressTest()
        {
            // Setup
            int bitsPerColour = 8;
            int tileSize = 16;
            float patternThreshold = .5f;
            float colourThreshold = .5f;
            int offsetX = 0;
            int offsetY = 0;
            int zoom = 1;

            Bitmap image = new Bitmap(@"C:\Users\nicB\Documents\JurassicPark-IslaNublar.png");

            //PixelFormat format = image.PixelFormat;
            PixelFormat format = PixelFormat.Format24bppRgb;

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            Program.SortTiles(patternThreshold, colourThreshold);
            Bitmap groupedTileset = Program.GetGroupedTileset(format, tileSize, zoom);

            string filename = @"C:\Users\nicB\Documents\jurassicParkTileset2.png";
            ImageFormat fileFormat = Program.GetImageFormat(filename);
            groupedTileset.Save(filename, fileFormat);
        }
    }
}
