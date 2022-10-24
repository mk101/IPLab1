using System;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class GaussMethod : Filter
{
    public GaussMethod(double sigma, int radius)
    {
        _sigma = sigma;
        _radius = radius;
    }

    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        double sum = 0;
        for (int i = -_radius; i <= _radius; i++)
        {
            for (int j = -_radius; j <= _radius; j++)
            {
                int nx = Clamp(x + i, 0, source.PixelWidth - 1);
                int ny = Clamp(y + j, 0, source.PixelHeight - 1);
                Color color = Colors![nx * source.PixelWidth + ny];
                
                double gauss = 1 / (2 * Math.PI * Math.Pow(_sigma, 2)) * Math.Exp(-(Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * Math.Pow(_sigma, 2)));
                sum += gauss * color.I;
            }
        }

        return new Color(
            (byte) Clamp((int)sum, 0, 255),
            (byte) Clamp((int)sum, 0, 255),
            (byte) Clamp((int)sum, 0, 255),
            255
        );
    }

    private double _sigma;
    private int _radius;
}
