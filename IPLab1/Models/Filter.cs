using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public abstract class Filter
{
    public WriteableBitmap ApplyFilter(BitmapImage image)
    {
        var result = new WriteableBitmap(image);
        int width = image.PixelWidth;
        int height = image.PixelHeight;
        int stride = width * 4;

        var pixels = new byte[stride * height];
        image.CopyPixels(pixels, stride, 0);

        Colors = new List<Color>(GetColors(image));
        
        Execute(image, width, height, stride, pixels);
        
        result.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
        return result;
    }

    protected virtual void Execute(BitmapImage image, int width, int height, int stride, byte[] pixels)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var color = CalculatePixelColor(image, i, j);
                var index = j * stride + 4 * i;
                pixels[index] = color.R;
                pixels[index + 1] = color.G;
                pixels[index + 2] = color.B;
            }
        }
    }

    protected static int Clamp(int value, int min, int max) => Math.Min(Math.Max(value, min), max);

    protected abstract Color CalculatePixelColor(BitmapImage source, int x, int y);
    protected List<Color>? Colors;

    private IEnumerable<Color> GetColors(BitmapImage image)
    {
        int width = image.PixelWidth;
        int height = image.PixelHeight;
        int stride = width * 4;
        var pixels = new byte[stride * height];
        image.CopyPixels(pixels, stride, 0);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var index = j * stride + 4 * i;
                yield return new Color(pixels[index], pixels[index + 1], pixels[index + 2], pixels[index + 3]);
            }
        }
    }
}
