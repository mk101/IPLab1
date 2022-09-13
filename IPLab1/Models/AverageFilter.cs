using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class AverageFilter : Filter
{
    public AverageFilter(int size)
    {
        _maskSize = size;
        _mask = new int[_maskSize, _maskSize];
        for (int i = 0; i < _maskSize; i++)
        {
            for (int j = 0; j < _maskSize; j++)
            {
                _mask[i, j] = 1;
            }
        }
    }

    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        float red = 0;
        float green = 0;
        float blue = 0;

        int k = 0;
        for (int i = -_maskSize/2; i <= _maskSize/2; i++) {
            for (int j = -_maskSize/2; j <= _maskSize/2; j++)
            {
                var c = Colors![
                    Clamp(x + i, 0, source.PixelWidth - 1) * source.PixelWidth + Clamp(y + j, 0, source.PixelHeight - 1)];

                red += c.R;
                green += c.G;
                blue += c.B;
                k++;
            }
        }

        var color = new Color(
            (byte) Clamp((int) (red / k), 0, 255),
            (byte) Clamp((int) (green / k), 0, 255),
            (byte) Clamp((int) (blue / k), 0, 255),
            255
        );

        return color;
    }

    private int[,] _mask;
    private readonly int _maskSize;
}
