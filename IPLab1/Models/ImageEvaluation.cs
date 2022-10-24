using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class ImageEvaluation
{
    public ImageEvaluation(int radius)
    {
        _radius = radius;
    }
    
    public void SetImage1(BitmapImage image)
    {
        _image1 = image;
        _image1Colors = new List<Color>(GetColors(image));
    }
    
    public void SetImage2(BitmapImage image)
    {
        _image2 = image;
        _image2Colors = new List<Color>(GetColors(image));
    }
    
    public int PSNR()
    {
        if (_image1 is null || _image2 is null)
        {
            throw new ArgumentNullException();
        }

        if (_image1.PixelWidth != _image2.PixelWidth || _image1.PixelHeight != _image2.PixelHeight)
        {
            throw new ArgumentException();
        }

        double mseSum = 0, mse;
        for (int i = 0; i < _image1.PixelWidth; i++)
        {
            for (int j = 0; j < _image1.PixelHeight; j++)
            {
                Color c1 = _image1Colors![i * _image1.PixelWidth + j];
                Color c2 = _image2Colors![i * _image2.PixelWidth + j];

                mseSum += Math.Pow(c1.I - c2.I, 2);
            }
        }
        mse = mseSum / (_image1.PixelHeight * _image1.PixelWidth);

        return (int) (20 * Math.Log10(255 / Math.Sqrt(mse)));
    }

    public double SSIM()
    {
        if (_image1 is null || _image2 is null)
        {
            throw new ArgumentNullException();
        }

        if (_image1.PixelWidth != _image2.PixelWidth || _image1.PixelHeight != _image2.PixelHeight)
        {
            throw new ArgumentException();
        }
        
        double result = 0;
        double L = Math.Pow(2, _image1.Format.BitsPerPixel) - 1;
        double c1 = (0.01 * L) * (0.01 * L), c2 = (0.03 * L)*(0.03 * L);
        double c3 = c2 / 2;

        for (int i = 0; i < _image1.PixelWidth; i++)
        {
            for (int j = 0; j < _image1.PixelHeight; j++)
            {
                var mX = PixelSampleMean(_image1, i, j);
                var mY = PixelSampleMean(_image2, i, j);

                var sigmaX = Variance(_image1, i, j, mX);
                var sigmaY = Variance(_image2, i, j, mY);

                var sigmaXY = Covariance(_image1, _image2, i, j, mX, mY);

                var l = (2 * mX * mY + c1) / (mX * mX + mY * mY + c1);
                var c = (2 * sigmaX * sigmaY + c2) / (sigmaX * sigmaX + sigmaY * sigmaY + c2);
                var s = (2 * sigmaXY + c3) / (sigmaX * sigmaY + c3);

                result += l * c * s;
            }
        }

        return result / (_image1.PixelWidth * _image1.PixelHeight);
    }

    private double PixelSampleMean(BitmapImage image, int x, int y)
    {
        int n = (_radius * 2 + 2) * (_radius * 2 + 2);
        double sum = 0;
        var colors = new List<Color>(GetColors(image));

        for (int i = -_radius; i <= _radius; i++)
        {
            for (int j = -_radius; j <= _radius; j++)
            {
                int nx = Clamp(x + i, 0, image.PixelWidth - 1);
                int ny = Clamp(y + j, 0, image.PixelHeight - 1);
                
                
                Color color = colors[nx * image.PixelWidth + ny];

                sum += color.I;
            }
        }

        return sum / n;
    }

    private double Variance(BitmapImage image, int x, int y, double k)
    {
        int n = (_radius * 2 + 1) * (_radius * 2 + 1);
        double sum = 0;
        var colors = new List<Color>(GetColors(image));
        
        for (int i = -_radius; i <= _radius; i++)
        {
            for (int j = -_radius; j <= _radius; j++)
            {
                int nx = Clamp(x + i, 0, image.PixelWidth - 1);
                int ny = Clamp(y + j, 0, image.PixelHeight - 1);
                
                
                Color color = colors[nx * image.PixelWidth + ny];

                sum += Math.Pow(color.I - k, 2);
            }
        }

        return Math.Sqrt(sum / n);
    }

    private double Covariance(BitmapImage image1, BitmapImage image2, int x, int y, double k1, double k2)
    {
        int n = (_radius * 2 + 1) * (_radius * 2 + 1);
        double sum = 0;
        var colors1 = new List<Color>(GetColors(image1));
        var colors2 = new List<Color>(GetColors(image2));
        
        for (int i = -_radius; i <= _radius; i++)
        {
            for (int j = -_radius; j <= _radius; j++)
            {
                int nx = Clamp(x + i, 0, image1.PixelWidth - 1);
                int ny = Clamp(y + j, 0, image1.PixelHeight - 1);
                
                
                
                Color color1 = colors1[nx * image1.PixelWidth + ny];
                Color color2 = colors2[nx * image2.PixelWidth + ny];

                sum += (color1.I - k1) * (color2.I - k2);
            }
        }

        return sum / n;
    }

    private readonly int _radius;

    private BitmapImage? _image1;
    private BitmapImage? _image2;

    private List<Color>? _image1Colors;
    private List<Color>? _image2Colors;

    private static IEnumerable<Color> GetColors(BitmapImage image)
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
    
    private static int Clamp(int value, int min, int max) => Math.Min(Math.Max(value, min), max);
}
