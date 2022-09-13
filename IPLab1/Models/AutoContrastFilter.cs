using System;
using System.Windows.Media.Imaging;
using IPLab1.Common;

namespace IPLab1.Models;

public class AutoContrastFilter : Filter
{
    public AutoContrastFilter()
    {
        _red = new byte[2]; // 0 - min, 1 - max
        _green = new byte[2];
        _blue = new byte[2];
    }

    protected override Color CalculatePixelColor(BitmapImage source, int x, int y)
    {
        var color = Colors![x * source.PixelWidth + y];
        return new Color(
            (byte) Clamp((color.R - _red[0]) * 255 / (_red[1] - _red[0]), 0, 255),
            (byte) Clamp((color.G - _green[0]) * 255 / (_green[1] - _green[0]), 0, 255),
            (byte) Clamp((color.B - _blue[0]) * 255 / (_blue[1] - _blue[0]), 0, 255),
            255
        );
    }

    protected override void Execute(BitmapImage image, int width, int height, int stride, byte[] pixels)
    {
        FindMinMax(image);
        base.Execute(image, width, height, stride, pixels);
    }

    private void FindMinMax(BitmapImage source)
    {
        _red[0] = _green[0] = _blue[0] = 255;
        _red[1] = _green[1] = _blue[1] = 0;

        foreach (var color in Colors!)
        {
            _red[0] = Math.Min(_red[0], color.R);
            _red[1] = Math.Max(_red[1], color.R);
            
            _green[0] = Math.Min(_green[0], color.G);
            _green[1] = Math.Max(_green[1], color.G);
            
            _blue[0] = Math.Min(_blue[0], color.B);
            _blue[1] = Math.Max(_blue[1], color.B);
        }
    }
    
    private byte[] _red;
    private byte[] _green;
    private byte[] _blue;
}
