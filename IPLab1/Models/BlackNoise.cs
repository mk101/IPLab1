using System;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class BlackNoise : Filter
{
    public BlackNoise(int count)
    {
        _count = count;
        _random = new Random();
    }

    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        if (_count > 0)
        {
            _count--;
            return new Color(0, 0, 0, 255);
        }

        return Colors![x * source.PixelWidth + y];
    }

    protected override void Execute(BitmapImage image, int width, int height, int stride, byte[] pixels)
    {
        while (_count != 0)
        {
            int x = _random.Next(width);
            int y = _random.Next(height);

            var color = CalculatePixelColor(image, x, y);
            var index = y * stride + 4 * x;
            pixels[index] = color.R;
            pixels[index + 1] = color.G;
            pixels[index + 2] = color.B;
        }
    }

    private int _count;
    private Random _random;
}
