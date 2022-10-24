using System.Windows.Media;
using System.Windows.Media.Imaging;
using IPLab1.Common;
using IPLab1.Models;

namespace IPLab1.ViewModels;

public class MainWindowViewModel : NotifyPropertyChanged
{
    public ImageSource? Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged(nameof(Image));
        }
    }
    
    public RelayCommand OpenFileCommand { get; }
    public RelayCommand SaveFileCommand { get; }
    
    public RelayCommand GrayscaleFilterCommand { get; }
    public RelayCommand AverageFilterCommand { get; }
    public RelayCommand AutoContrastFilterCommand { get; }
    public RelayCommand BlackNoiseCommand { get; }
    public RelayCommand ExponentialNoise { get; }

    public MainWindowViewModel()
    {
        var fileManager = new FileManager();
        
        var grayscaleFilter = new GrayscaleFilter();
        var averageFilter = new AverageFilter(3);
        var autoContrastFilter = new AutoContrastFilter();

        var blackNoise = new BlackNoise(4242);
        var expNoise = new ExponentialNoise(.05);
        
        OpenFileCommand = new RelayCommand(() => Image = fileManager.Open());
        SaveFileCommand = new RelayCommand(() => fileManager.Save(Image as BitmapImage));
        
        GrayscaleFilterCommand = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            Image = ConvertService.WritableBitmapToBitmapImage(grayscaleFilter.ApplyFilter((BitmapImage)Image));
        });
        
        AverageFilterCommand = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            Image = ConvertService.WritableBitmapToBitmapImage(averageFilter.ApplyFilter((BitmapImage) Image));
        });

        AutoContrastFilterCommand = new RelayCommand((() =>
        {
            if (Image is null)
            {
                return;
            }
            Image = ConvertService.WritableBitmapToBitmapImage(autoContrastFilter.ApplyFilter((BitmapImage) Image));
        }));

        BlackNoiseCommand = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            Image = ConvertService.WritableBitmapToBitmapImage(blackNoise.ApplyFilter((BitmapImage) Image));
        });

        ExponentialNoise = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            Image = ConvertService.WritableBitmapToBitmapImage(expNoise.ApplyFilter((BitmapImage) Image));
        });
    }

    private ImageSource? _image;
}
