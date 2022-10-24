using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class ExponentialNoise : Filter
{
    
    public ExponentialNoise(double a)
    {
        _a = a;
        _random = new Random();
    }
    
    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        Color sourceColor = Colors![x * source.PixelWidth + y];
        byte noise = _noise![source.PixelHeight * x + y];
        
        return new Color(
            (byte) Clamp(sourceColor.R + noise, 0, 255),
            (byte) Clamp(sourceColor.G + noise, 0, 255),
            (byte) Clamp(sourceColor.B + noise, 0, 255),
            255
        );
    }

    protected override void Execute(BitmapImage image, int width, int height, int stride, byte[] pixels)
    {
        _noise = new byte[image.PixelWidth * image.PixelHeight];
        var intensity = CalculateIntensity();

        CalculateNoise(intensity);

        base.Execute(image, width, height, stride, pixels);
    }

    private void CalculateNoise(double[] intensity)
    {
        int count = 0;
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < (int) intensity[i]; j++)
            {
                _noise[count + j] = (byte) i;
            }

            count += (int) intensity[i];
        }

        _noise = _noise.OrderBy(_ => _random.Next()).ToArray();
    }

    private double[] CalculateIntensity()
    {
        double[] intensity = new double[256];
        double sum = 0;

        for (int i = 0; i < 256; i++)
        {
            intensity[i] = NoiseFunction(i);
            sum += intensity[i];
        }

        for (int i = 0; i < 256; i++)
        {
            intensity[i] /= sum;
            intensity[i] *= _noise!.Length;
            intensity[i] = (int) intensity[i];
        }

        return intensity;
    }

    private double NoiseFunction(double z) => _a * Math.Exp(-_a * z);

    private readonly double _a;
    private byte[]? _noise;
    private Random _random;
}
