using System.IO;
using System.Windows.Media.Imaging;
using IPLab1.Common;
using Microsoft.Win32;

namespace IPLab1.Models;

public class FileManager
{
    public const string Filter = "png files (*.png)|*.png";

    public BitmapImage? Open()
    {
        var ofd = new OpenFileDialog
        {
            Filter = Filter
        };

        bool? result = ofd.ShowDialog();
        if (result == false)
        {
            return null;
        }

        if (Path.GetExtension(ofd.FileName) == ".png")
        {
            _fileService = new PngFileService();
        }
        else
        {
            throw new FileFormatException("Unknown file format");
        }

        return _fileService.Open(ofd.FileName);
    }

    public void Save(BitmapImage? bitmap)
    {
        if (bitmap is null)
        {
            return;
        }
        if (_fileService is null)
        {
            return;
        }
        
        var sfd = new SaveFileDialog()
        {
            Filter = Filter
        };

        bool? result = sfd.ShowDialog();
        if (result == false)
        {
            return;
        }
        
        _fileService.Save(bitmap, sfd.FileName);
    }
    
    private IFileService? _fileService;
}
