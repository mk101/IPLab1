using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace IPLab1.Common;

public class PngFileService : IFileService
{
    public BitmapImage? Open(string fileName)
    {
        return new BitmapImage(new Uri(fileName));
    }

    public void Save(BitmapImage image, string fileName)
    {
        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(image));

        using var fileStream = new FileStream(fileName, FileMode.Create);
        encoder.Save(fileStream);
    }
}
