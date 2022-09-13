using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class GrayscaleFilter : Filter
{
    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        Color sourceColor = Colors![x * source.PixelWidth + y];
        var intensity = 0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B;

        return new Color((byte) intensity, (byte) intensity, (byte) intensity, 255);
    }
}
