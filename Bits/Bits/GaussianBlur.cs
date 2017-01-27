using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public static class GaussianBlur
    {
        public static double[,] GetNormalizedSampleKernel(double deviation)
        {
            double[,] sampleKernel = GetSampleKernel(deviation);
            return NormalizeMatrix(sampleKernel);
        }

        public static double[,] GetSampleKernel(double deviation)
        {
            int size = (int)Math.Ceiling(deviation * 3) * 2 + 1;
            return GetSampleKernel(deviation, size);
        }

        public static double[,] GetSampleKernel(double deviation, int size)
        {
            double[,] kernel = new double[size, 1];
            double sum = 0;
            int half = size / 2;

            for (int i = 0; i < size; i++)
            {
                double value = 1 / (Math.Sqrt(2 * Math.PI) * deviation) * Math.Exp(-(i - half) * (i - half) / (2 * deviation * deviation));
                kernel[i, 0] = value;
                sum += value;
            }

            return kernel;
        }

        public static double[,] NormalizeMatrix(double[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            double[,] normalizedMatrix = new double[width, height];
            double sum = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    sum += matrix[x, y];
                }
            }

            if (sum != 0)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        normalizedMatrix[x, y] = matrix[x, y] / sum;
                    }
                }
            }

            return normalizedMatrix;
        }

        public static double[,] GaussianConvolution(double[,] matrix, double deviation)
        {
            double[,] kernel = GetNormalizedSampleKernel(deviation);

            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            double[,] horizontal = new double[width, height];
            double[,] vertical = new double[width, height];

            // X direction
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    horizontal[x, y] = ProcessPointHorizontally(matrix, x, y, kernel);
                }
            }

            // Y direction
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    vertical[x, y] = ProcessPointVertically(horizontal, x, y, kernel);
                }
            }

            return vertical;
        }

        private static double ProcessPointHorizontally(double[,] matrix, int x, int y, double[,] kernel)
        {
            return ProcessPoint(matrix, x, y, kernel, 0);
        }

        private static double ProcessPointVertically(double[,] matrix, int x, int y, double[,] kernel)
        {
            return ProcessPoint(matrix, x, y, kernel, 1);
        }

        private static double ProcessPoint(double[,] matrix, int x, int y, double[,] kernel, int direction)
        {
            double res = 0;

            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            int kernelWidth = kernel.GetLength(0);
            int halfKernelWidth = kernelWidth / 2;
            for (int i = 0; i < kernel.GetLength(0); i++)
            {
                int cox = direction == 0 ? x + i - halfKernelWidth : x;
                int coy = direction == 1 ? y + i - halfKernelWidth : y;

                if (cox >= 0 && cox < width && coy >= 0 && coy < height)
                {
                    res += matrix[cox, coy] * kernel[i, 0];
                }
            }

            return res;
        }

        private static Color Greyscale(Color colour)
        {
            int alpha = colour.A;
            int red = colour.R;
            int green = colour.G;
            int blue = colour.B;

            int greyscale = (int)(red * .3 + green * .59 + blue * 0.11);

            return Color.FromArgb(alpha, greyscale, greyscale, greyscale);
        }

        //public override Bitmap FilterProcessImage(double deviation, Bitmap image)
        //{
        //    int width = image.Width;
        //    int height = image.Height;

        //    Bitmap finalImage = new Bitmap(width, height);

        //    double[,] matrix = new double[width, height];

        //    for (int x = 0; x < width; x++)
        //    {
        //        for(int y = 0; y < height; y++) {
        //            Color pixel = image.GetPixel(x, y);

        //            Color greyscalePixel = Greyscale(pixel);

        //            matrix[x, y] = greyscalePixel.R;
        //        }
        //    }

        //    matrix = GaussianConvolution(matrix, deviation);

        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            double value = matrix[x, y];
        //            int greyscaleValue = (int)Math.Min(255, value);

        //            Color greyscale = Color.FromArgb(255, greyscaleValue, greyscaleValue, greyscaleValue);

        //            finalImage.SetPixel(x, y, greyscale);
        //        }
        //    }
        //}


        //public static void GaussianBlur(int[] source, int[] target, Size size, int radius)
        //{
        //    int width = size.Width;
        //    int height = size.Height;
        //    GaussianBlur(source, target, width, height, radius);
        //}

        //public static void GaussianBlur(int[] source, int[] target, int width, int height, int radius)
        //{
        //    int[] kernel = GetSampleKernel(radius, 3);

        //    int boxBlurRadius = (kernel[0] - 1) / 2;
        //    BoxBlur(source, target, width, height, boxBlurRadius);
        //    BoxBlur(source, target, width, height, boxBlurRadius);
        //    BoxBlur(source, target, width, height, boxBlurRadius);
        //}

        //public static void BoxBlur(int[] source, int[] target, int width, int height, int radius)
        //{
        //    for (var x = 0; x < source.Length; x++)
        //    {
        //        target[x] = source[x];
        //    }

        //    HorizontalBoxBlur(target, source, width, height, radius);   // Are target and source supposed to be switched?
        //    VerticalBoxBlur(source, target, width, height, radius);
        //}

        //public static void HorizontalBoxBlur(int[] source, int[] target, int width, int height, int radius)
        //{
        //    double iarr = 1 / (radius + radius + 1);

        //    for (int x = 0; x < height; x++)
        //    {
        //        int ti = x * width, li = ti, ri = ti + radius;
        //        int fv = source[ti], lv = source[ti + width - 1], val = (radius + 1) * fv;

        //        for (var j = 0; j < radius; j++)
        //        {
        //            val += source[ti + j];
        //        }

        //        for (var j = 0; j <= radius; j++)
        //        {
        //            val += source[ri++] - fv;
        //            target[ti++] = (int)Math.Round(val * iarr);
        //        }

        //        for (var j = radius + 1; j < width - radius; j++)
        //        {
        //            val += source[ri++] - source[li++];
        //            target[ti++] = (int)Math.Round(val * iarr);
        //        }

        //        for (var j = width - radius; j < width; j++)
        //        {
        //            val += lv - source[li++];
        //            target[ti++] = (int)Math.Round(val * iarr);
        //        }
        //    }
        //}

        //public static void VerticalBoxBlur(int[] source, int[] target, int width, int height, int radius)
        //{
        //    double iarr = 1 / (radius + radius + 1);

        //    for (var x = 0; x < width; x++)
        //    {
        //        int ti = x, li = ti, ri = ti + radius * width;
        //        int fv = source[ti], lv = source[ti + width * (height - 1)], val = (radius + 1) * fv;

        //        for (var j = 0; j < radius; j++)
        //        {
        //            val += source[ti + j * width];
        //        }

        //        for (var j = 0; j <= radius; j++)
        //        {
        //            val += source[ri] - fv;
        //            target[ti] = (int)Math.Round(val * iarr);
        //            ri += width;
        //            ti += width;
        //        }

        //        for (var j = radius + 1; j < height - radius; j++)
        //        {
        //            val += source[ri] - source[li];
        //            target[ti] = (int)Math.Round(val * iarr);
        //            li += width;
        //            ri += width;
        //            ti += width;
        //        }

        //        for (var j = height - radius; j < height; j++)
        //        {
        //            val += lv - source[li];
        //            target[ti] = (int)Math.Round(val * iarr);
        //            li += width; ti += width;
        //        }
        //    }
        //}

        //public static int[] GetSampleKernel(double deviation, int size)
        //{
        //    double idealWidth = Math.Sqrt((12 * deviation * deviation / size) + 1);   // Ideal averaging filter width
        //    int widthLower = (int)Math.Floor(idealWidth);
        //    if (widthLower % 2 == 0)
        //    {
        //        widthLower--;
        //    }

        //    int widthUpper = widthLower + 2;

        //    double idealM = (12 * deviation * deviation - size * widthLower * widthLower - 4 * size * widthLower - 3 * size) / (-4 * widthLower - 4);
        //    int m = (int)Math.Round(idealM);

        //    LinkedList<int> sizes = new LinkedList<int>();
        //    for (int x = 0; x < size; x++)
        //    {
        //        int greyscaleValue = x < m ? widthLower : widthUpper;
        //        sizes.AddLast(greyscaleValue);
        //    }

        //    return sizes.ToArray();
        //}
    }
}
