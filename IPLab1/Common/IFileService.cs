using System.Windows.Media.Imaging;

namespace IPLab1.Common;

public interface IFileService
{
    BitmapImage? Open(string fileName);
    void Save(BitmapImage image, string fileName);
}
