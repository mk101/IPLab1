using System.IO;
using System.Windows.Media.Imaging;

namespace IPLab1.Common;

public static class ConvertService
{
    public static BitmapImage WritableBitmapToBitmapImage(WriteableBitmap writeableBitmap)
    {
        var image = new BitmapImage();
        using var stream = new MemoryStream();
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
        encoder.Save(stream);
            
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();

        return image;
    }
}
