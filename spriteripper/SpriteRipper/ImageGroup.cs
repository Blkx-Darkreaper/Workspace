using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpriteRipper
{
    public class ImageCollection
    {
        public string Filename { get; set; }
        public Point ImageOffset { get; protected set; }
        public Size CroppedImageSize { get; protected set; }
        public int TileSize { get; protected set; }
        public Size SubImageSize { get; protected set; }
        public int TotalSubImages { get; protected set; }
        protected int currentSubImageIndex { get; set; }
        protected Bitmap currentSubImage { get; set; }

        public ImageCollection(string path, int tileSize, int width, int height, int offsetX, int offsetY)
        {
            this.Filename = path;
            this.ImageOffset = new Point(offsetX, offsetY);
            this.TileSize = tileSize;

            //Bitmap image = Program.LoadImage(path);
            //int width = image.Width;
            //int height = image.Height;

            //int croppedWidth = TileSize * ((width - offsetX) / TileSize);
            //int croppedHeight = TileSize * ((height - offsetY) / TileSize);

            //if (croppedWidth != width || croppedHeight != height)
            //{
            //    Rectangle rect = new Rectangle(offsetX, offsetY, croppedWidth, croppedHeight);
            //    Bitmap croppedImage = image.Clone(rect, PixelFormat.Format24bppRgb);
            //    image = croppedImage;
            //}

            //width = image.Width;
            //height = image.Height;

            this.CroppedImageSize = new Size(width, height);

            this.SubImageSize = Program.GetSubImageSize(width, height, tileSize);

            int subWidth = SubImageSize.Width;
            int subHeight = SubImageSize.Height;

            int subImagesWide = width / subWidth;
            this.TotalSubImages = subImagesWide * (height / subHeight);

            currentSubImage = null;
            currentSubImageIndex = -1;
        }

        public int GetSubImageIndexFromTileIndex(int tileIndex)
        {
            int subImageWidth = SubImageSize.Width;
            int subImageHeight = SubImageSize.Height;

            int totalTilesWide = CroppedImageSize.Width / TileSize;
            int subImageTilesWide = subImageWidth / TileSize;
            int subImageTilesHigh = subImageHeight / TileSize;

            int subImageIndex = tileIndex / (totalTilesWide * subImageTilesHigh) + (tileIndex / totalTilesWide) / subImageTilesHigh + (tileIndex % totalTilesWide) / subImageTilesWide;
            return subImageIndex;
        }

        public int GetSubImageTileIndexFromTileIndex(int tileIndex)
        {
            int subImageWidth = SubImageSize.Width;
            int subImageHeight = SubImageSize.Height;

            int totalTilesWide = CroppedImageSize.Width / TileSize;
            int subImageTilesWide = subImageWidth / TileSize;
            int subImageTilesHigh = subImageHeight / TileSize;

            int subImageTileIndex = (tileIndex % totalTilesWide) % subImageTilesWide + subImageTilesWide * ((tileIndex % (totalTilesWide * subImageTilesHigh)) / totalTilesWide);
            return subImageTileIndex;
        }

        public int GetTileIndex(int subImageIndex, int subImageTileIndex)
        {
            int width = CroppedImageSize.Width;

            int subImageWidth = SubImageSize.Width;

            int subImagesWide = width / subImageWidth;

            int totalTilesWide = width / TileSize;
            int subImageTilesWide = subImageWidth / TileSize;

            //int tileIndex = subImageIndex * subImageTilesWide + subImageTileIndex + totalTilesWide * (subImageIndex / subImagesWide) + subImageTilesWide * (subImageTileIndex / subImagesWide);
            int tileIndex = subImageIndex * subImageTilesWide + subImageTileIndex + totalTilesWide * (subImageIndex / subImagesWide);
            return tileIndex;
        }

        public Bitmap GetImage()
        {
            int offsetX = ImageOffset.X;
            int offsetY = ImageOffset.Y;
            Bitmap croppedImage = Program.LoadCroppedImage(Filename, TileSize, offsetX, offsetY);
            return croppedImage;
        }

        public Bitmap LoadSubImage(int subImageIndex)
        {
            Bitmap subImage = GetSubImage(subImageIndex);

            SetSubImageByRef(ref subImage, subImageIndex);

            return subImage;
        }

        public Bitmap GetSubImage(int subImageIndex)
        {
            if (subImageIndex == currentSubImageIndex)
            {
                return currentSubImage;
            }

            Bitmap croppedImage = GetImage();
            return GetSubImageFromImageByRef(ref croppedImage, subImageIndex);
        }

        public Bitmap GetSubImageFromImageByRef(ref Bitmap image, int subImageIndex)
        {
            int width = image.Width;
            int height = image.Height;

            int subWidth = SubImageSize.Width;
            int subHeight = SubImageSize.Height;

            if (width == subWidth)
            {
                if (height == subHeight)
                {
                    return image;
                }
            }

            int subImagesWide = width / subWidth;

            int x = (subImageIndex % subImagesWide) * subWidth;
            int y = (subImageIndex / subImagesWide) * subHeight;
            Rectangle rect = new Rectangle(x, y, subWidth, subHeight);
            Bitmap subImage = image.Clone(rect, PixelFormat.Format24bppRgb);

            return subImage;
        }

        public void SetSubImageByRefFromImage(ref Bitmap image, int subImageIndex)
        {
            if (currentSubImageIndex == subImageIndex)
            {
                return;
            }

            Bitmap subImage = GetSubImageFromImageByRef(ref image, subImageIndex);

            currentSubImage = subImage;
            currentSubImageIndex = subImageIndex;
        }

        public void SetSubImageByRef(ref Bitmap subImage, int subImageIndex)
        {
            currentSubImage = subImage;
            currentSubImageIndex = subImageIndex;
        }

        public int GetSubImageTileCount(int subImageIndex)
        {
            int subImageWidth = SubImageSize.Width;
            int subImageHeight = SubImageSize.Height;

            int subImageTilesWide = subImageWidth / TileSize;
            int subImageTilesHigh = subImageHeight / TileSize;

            int totalSubImageTiles = subImageTilesWide * subImageTilesHigh;
            return totalSubImageTiles;
        }

        public Bitmap GetTileImage(int tileIndex)
        {
            int subImageIndex = GetSubImageIndexFromTileIndex(tileIndex);

            int tileSubImageIndex = GetSubImageTileIndexFromTileIndex(tileIndex);

            Bitmap tileImage = GetTileImage(subImageIndex, tileSubImageIndex);
            return tileImage;
        }

        public Bitmap GetTileImage(int subImageIndex, int tileSubImageIndex)
        {
            Bitmap subImage = GetSubImage(subImageIndex);

            int subImageWidth = SubImageSize.Width;

            int tilesWide = subImageWidth / TileSize;

            int x = (tileSubImageIndex % tilesWide) * TileSize;
            int y = (tileSubImageIndex / tilesWide) * TileSize;

            Rectangle rect = new Rectangle(x, y, TileSize, TileSize);
            Bitmap tileImage = subImage.Clone(rect, subImage.PixelFormat);
            return tileImage;
        }

        public PixelFormat GetPixelFormat()
        {
            if (currentSubImage == null)
            {
                throw new NullReferenceException();
            }

            PixelFormat format = currentSubImage.PixelFormat;
            return format;
        }
    }
}
