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
        public int CurrentSubImageIndex { get; protected set; }
        protected Bitmap currentSubImage { get; set; }

        public ImageCollection(string path, int tileSize, int width, int height, int offsetX, int offsetY)
        {
            this.Filename = path;
            this.CroppedImageSize = new Size(width, height);

            BuildImageCollection(tileSize, offsetX, offsetY, -1);
        }

        public void BuildImageCollection(int tileSize, int offsetX, int offsetY, int subImageIndex)
        {
            this.ImageOffset = new Point(offsetX, offsetY);
            this.TileSize = tileSize;

            //Bitmap image = Program.LoadImage(path);
            //int imageWidth = image.Width;
            //int imageHeight = image.Height;

            //int croppedWidth = TileSize * ((imageWidth - offsetX) / TileSize);
            //int croppedHeight = TileSize * ((imageHeight - offsetY) / TileSize);

            //if (croppedWidth != imageWidth || croppedHeight != imageHeight)
            //{
            //    Rectangle rect = new Rectangle(offsetX, offsetY, croppedWidth, croppedHeight);
            //    Bitmap croppedImage = image.Clone(rect, PixelFormat.Format24bppRgb);
            //    image = croppedImage;
            //}

            int width = CroppedImageSize.Width;
            int height = CroppedImageSize.Height;

            int widthDivisor = 1;
            int heightDivisor = 1;

            while (width / widthDivisor > 256)
            {
                widthDivisor *= 2;
                //int remainder = (imageWidth / widthDivisor) % tileSize;
            }

            while (height / heightDivisor > 256)
            {
                heightDivisor *= 2;
                //int remainder = (imageHeight / heightDivisor) % tileSize;
            }

            int subImageWidth = width / widthDivisor;
            int subImageHeight = height / heightDivisor;

            this.SubImageSize = new Size(subImageWidth, subImageHeight);
            this.TotalSubImages = widthDivisor * heightDivisor;

            //this.currentSubImage = null;
            this.CurrentSubImageIndex = subImageIndex;

            Bitmap image = GetImage();
            SetSubImageByRefFromImage(ref image, 0);
            image.Dispose();
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
            //int height = CroppedImageSize.Height;

            int subImageWidth = SubImageSize.Width;
            int subImageHeight = SubImageSize.Height;

            int subImagesWide = width / subImageWidth;

            //int totalTilesWide = width / TileSize;
            //int totalTilesHigh = height / TileSize;
            int subImageTilesWide = subImageWidth / TileSize;
            int subImageTilesHigh = subImageHeight / TileSize;

            int tileIndex = subImageTileIndex
                + subImageTileIndex / subImageTilesWide * subImageTilesWide * (subImagesWide - 1)
                + (subImageIndex % subImagesWide) * subImageTilesWide
                + subImageIndex / subImagesWide * subImageTilesWide * subImagesWide * subImageTilesHigh;
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
            if (subImageIndex == CurrentSubImageIndex)
            {
                return currentSubImage;
            }

            Bitmap croppedImage = GetImage();
            Bitmap subImage = GetSubImageFromImageByRef(ref croppedImage, subImageIndex);
            croppedImage.Dispose();

            return subImage;
        }

        public Bitmap GetSubImageFromImageByRef(ref Bitmap image, int subImageIndex)
        {
            int width = image.Width;
            int height = image.Height;

            int subWidth = SubImageSize.Width;
            int subHeight = SubImageSize.Height;

            Bitmap subImage;
            if (width == subWidth)
            {
                if (height == subHeight)
                {
                    subImage = new Bitmap(width, height);
                    using (Graphics graphics = Graphics.FromImage(subImage))
                    {
                        graphics.DrawImage(image, 0, 0);
                    }

                    return subImage;
                }
            }

            int subImagesWide = width / subWidth;

            int x = (subImageIndex % subImagesWide) * subWidth;
            int y = (subImageIndex / subImagesWide) * subHeight;
            Rectangle rect = new Rectangle(x, y, subWidth, subHeight);
            //Bitmap subImage = image.Clone(rect, PixelFormat.Format24bppRgb);
            subImage = new Bitmap(subWidth, subHeight);
            using (Graphics graphics = Graphics.FromImage(subImage))
            {
                graphics.DrawImage(image, 0, 0, rect, GraphicsUnit.Pixel);
            }

            return subImage;
        }

        public void SetSubImageByRefFromImage(ref Bitmap image, int subImageIndex)
        {
            if (CurrentSubImageIndex == subImageIndex)
            {
                return;
            }

            Bitmap subImage = GetSubImageFromImageByRef(ref image, subImageIndex);

            this.currentSubImage = subImage;
            this.CurrentSubImageIndex = subImageIndex;
        }

        public void SetSubImageByRef(ref Bitmap subImage, int subImageIndex)
        {
            currentSubImage = subImage;
            CurrentSubImageIndex = subImageIndex;
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
            Bitmap tileImage = new Bitmap(TileSize, TileSize);
            using (Graphics graphics = Graphics.FromImage(tileImage))
            {
                graphics.DrawImage(subImage, 0, 0, rect, GraphicsUnit.Pixel);
            }

            //Bitmap tileImage = subImage.Clone(rect, subImage.PixelFormat);
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
