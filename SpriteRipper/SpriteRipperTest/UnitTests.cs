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
            int tileSize = 16;
            int offsetX = 0;
            int offsetY = 0;

            //Bitmap croppedImage = Program.LoadImage(@"C:\Users\nicB\Documents\tileTest.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\tileTest.png", tileSize, offsetX, offsetY);

            //int x = 0;
            //int y = 0;
            int index = 0;

            //Bitmap image1 = Program.GetTileImage(x, y, TileSize);
            Bitmap image1 = Program.GetTileImage(index);
            Tile tile1 = new Tile(image1, bitsPerColour, tileSize, index);
            Console.WriteLine("Tile 1: " + tile1.ToString());

            //x = 16;
            index = 1;
            //Bitmap image2 = Program.GetTileImage(x, y, TileSize);
            Bitmap image2 = Program.GetTileImage(index);
            Tile tile2 = new Tile(image2, bitsPerColour, tileSize, 1);
            Console.WriteLine("Tile 2: " + tile2.ToString());

            // Execution
            Tuple<float, float> results = tile1.GetMatches(tile2);
            float patternMatch = results.Item1;
            float colourMatch = results.Item2;

            bool identical = tile1.IdenticalTo(tile2);

            // Assertion
            float expectedPatternMatch = 223 / 255f;
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
            int tileSize = 16;
            int offsetX = 0;
            int offsetY = 0;

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\tileTest.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\tileTest.png", tileSize, offsetX, offsetY);

            //int x = 0;
            //int y = 0;
            int index = 0;

            //Bitmap image1 = Program.GetTileImage(x, y, TileSize);
            Bitmap image1 = Program.GetTileImage(index);
            Tile master = new Tile(image1, bitsPerColour, tileSize, index);
            //Console.WriteLine("Tile 1: " + tileToDraw.ToString());

            Tile identicalTile = new Tile(image1, bitsPerColour, tileSize, index);

            //x = 16;
            index = 1;
            //Bitmap image2 = Program.GetTileImage(x, y, TileSize);
            Bitmap image2 = Program.GetTileImage(index);
            Tile similarTile = new Tile(image2, bitsPerColour, tileSize, index);
            //Console.WriteLine("Tile 2: " + similarTile.ToString());

            //x = 32;
            index = 2;
            //Bitmap image3 = Program.GetTileImage(x, y, TileSize);
            Bitmap image3 = Program.GetTileImage(index);
            Tile otherSimilarTile = new Tile(image3, bitsPerColour, tileSize, index);
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

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\test2.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\test2.png", tileSize, offsetX, offsetY);
            Bitmap image = Program.Images.GetImage();

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            //Program.LoadAllTilesByRef(BitsPerColour, TileSize);
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

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\test2.png");
            Bitmap image = Program.LoadCroppedImage(@"C:\Users\nicB\Documents\test2.png", tileSize, offsetX, offsetY);

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

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\test2a.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\test2a.png", tileSize, offsetX, offsetY);
            Bitmap image = Program.Images.GetImage();

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            //Program.LoadAllTilesByRef(BitsPerColour, TileSize);
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

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\test.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\test.png", tileSize, offsetX, offsetY);
            Bitmap image = Program.Images.GetImage();

            PixelFormat format = image.PixelFormat;

            // Execution
            Program.LoadAllTiles(image, bitsPerColour, tileSize);
            //Program.LoadAllTilesByRef(BitsPerColour, TileSize, offsetX, offsetY);
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

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\test.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\test.png", tileSize, offsetX, offsetY);
            Bitmap image = Program.Images.GetImage();

            long sortTimes8BitSum = 0;
            long sortTimes4BitSum = 0;
            int runs = 4;

            // Execution

            // 8Bit sorting
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            //Program.LoadAllTilesByRef(BitsPerColour, TileSize);
            for (int i = 0; i < runs; i++)
            {
                Program.SortTiles(patternThreshold, colourThreshold);
                long sortTime = Program.SortTime;
                sortTimes8BitSum += sortTime;
            }

            long averageSortTime8Bit = sortTimes8BitSum / runs;

            // 4Bit sorting
            bitsPerColour = 4;
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            //Program.LoadAllTilesByRef(BitsPerColour, TileSize);
            for (int i = 0; i < runs; i++)
            {
                Program.SortTiles(patternThreshold, colourThreshold);
                long sortTime = Program.SortTime;
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

            //Bitmap image = Program.LoadImage(@"C:\Users\nicB\Documents\JurassicPark-IslaNublar.png");
            Program.LoadImage(@"C:\Users\nicB\Documents\JurassicPark-IslaNublar.png", tileSize, offsetX, offsetY);

            //PixelFormat format = loadedImage.PixelFormat;
            PixelFormat format = PixelFormat.Format24bppRgb;

            int totalSubImages = Program.Images.TotalSubImages;
            for (int subImageIndex = 0; subImageIndex < totalSubImages; subImageIndex++)
            {
                Program.LoadSubImage(bitsPerColour, tileSize, subImageIndex);

                // Execution
                //Program.LoadAllTiles(croppedImage, BitsPerColour, TileSize, offsetX, offsetY);
                //Program.LoadAllTilesByRef(BitsPerColour, TileSize);
                Program.SortTiles(patternThreshold, colourThreshold);
            }

            Bitmap groupedTileset = Program.GetGroupedTileset(format, tileSize, zoom);

            string filename = @"C:\Users\nicB\Documents\jurassicParkTileset2.png";
            ImageFormat fileFormat = Program.GetImageFormat(filename);
            groupedTileset.Save(filename, fileFormat);
        }

        [TestMethod]
        public void SubDivisorTest()
        {
            int width = 4080;
            int height = 4048;
            int tileSize = 16;

            Size subImageSize = Program.GetSubImageSize(width, height, tileSize);

            int subWidth = subImageSize.Width;
            int expectedSubWidth = 272;
            Assert.IsTrue(subWidth == expectedSubWidth, string.Format("SubWidth: Expected {0}, Actual {1}", expectedSubWidth, subWidth));

            int subHeight = subImageSize.Height;
            int expectedSubHeight = 176;
            Assert.IsTrue(subHeight == expectedSubHeight, string.Format("SubWidth: Expected {0}, Actual {1}", expectedSubHeight, subHeight));
        }

        [TestMethod]
        public void TileIndex()
        {
            int width = 96;
            int height = 96;
            int tileSize = 16;

            int offsetX = 0;
            int offsetY = 0;

            ImageCollection imageGroup = new ImageCollection("", tileSize, width, height, offsetX, offsetY);

            int tileIndex, subImageIndex, tileSubImageIndex;

            tileIndex = 0;
            subImageIndex = imageGroup.GetSubImageIndexFromTileIndex(tileIndex);
            Assert.IsTrue(subImageIndex == 0);

            tileSubImageIndex = imageGroup.GetSubImageTileIndexFromTileIndex(tileIndex);
            Assert.IsTrue(tileSubImageIndex == 0);

            tileIndex = 3;
            subImageIndex = imageGroup.GetSubImageIndexFromTileIndex(tileIndex);
            Assert.IsTrue(subImageIndex == 1);

            tileSubImageIndex = imageGroup.GetSubImageTileIndexFromTileIndex(tileIndex);
            Assert.IsTrue(tileSubImageIndex == 0);

            tileIndex = 8;
            subImageIndex = imageGroup.GetSubImageIndexFromTileIndex(tileIndex);
            Assert.IsTrue(subImageIndex == 0);

            tileSubImageIndex = imageGroup.GetSubImageTileIndexFromTileIndex(tileIndex);
            Assert.IsTrue(tileSubImageIndex == 5);

            tileIndex = 21;
            subImageIndex = imageGroup.GetSubImageIndexFromTileIndex(tileIndex);
            Assert.IsTrue(subImageIndex == 3);

            tileSubImageIndex = imageGroup.GetSubImageTileIndexFromTileIndex(tileIndex);
            Assert.IsTrue(tileSubImageIndex == 3);
        }
    }
}
