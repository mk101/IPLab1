using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class AlfaTrimmedMeanFilter : Filter
{
    public AlfaTrimmedMeanFilter(int hd, int size)
    {
        _hd = hd;
        _size = size;
    }

    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        List<Color> maskPixels = new List<Color>();
        for (int i = -_size/2; i <= _size/2; i++)
        {
            for (int j = -_size/2; j <= _size/2; j++)
            {
                maskPixels.Add(Colors![(x+i) * source.PixelWidth + (y+j)]);
            }
        }
        
        maskPixels.Sort((c1,c2) => c1.I.CompareTo(c2.I));
        maskPixels = maskPixels.Skip(_hd).SkipLast(_hd).ToList();

        double intensity = maskPixels.Sum(c => c.I);
        intensity /= maskPixels.Count;

        return new Color(
            (byte)intensity,
            (byte)intensity,
            (byte)intensity,
            255
        );
    }

    protected override void Execute(BitmapImage image, int width, int height, int stride, byte[] pixels)
    {
        for (int i = _size/2; i < width - _size/2; i++)
        {
            for (int j = _size/2; j < height - _size/2; j++)
            {
                var color = CalculatePixelColor(image, i, j);
                var index = j * stride + 4 * i;
                pixels[index] = color.R;
                pixels[index + 1] = color.G;
                pixels[index + 2] = color.B;
            }
        }
    }

    private readonly int _hd;
    private readonly int _size;
}
