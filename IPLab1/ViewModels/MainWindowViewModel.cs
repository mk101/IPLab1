using System.Windows;
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
    public RelayCommand ExponentialNoiseCommand { get; }
    
    public RelayCommand AlfaTrimmedMeanCommand { get; }
    public RelayCommand GaussMethodCommand { get; }
    
    public RelayCommand SetFirstImageCommand { get; }
    public RelayCommand SetSecondImageCommand { get; }
    public RelayCommand PSNRCommand { get; }
    public RelayCommand SSIMCommand { get; }

    public MainWindowViewModel()
    {
        var fileManager = new FileManager();
        
        var grayscaleFilter = new GrayscaleFilter();
        var averageFilter = new AverageFilter(3);
        var autoContrastFilter = new AutoContrastFilter();

        var blackNoise = new BlackNoise(4242);
        var expNoise = new ExponentialNoise(.05);

        var alfa = new AlfaTrimmedMeanFilter(3, 3);
        var gauss = new GaussMethod(0.5, 1);

        var ie = new ImageEvaluation(1);
        
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

        ExponentialNoiseCommand = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            Image = ConvertService.WritableBitmapToBitmapImage(expNoise.ApplyFilter((BitmapImage) Image));
        });

        AlfaTrimmedMeanCommand = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            
            Image = ConvertService.WritableBitmapToBitmapImage(alfa.ApplyFilter((BitmapImage) Image));
        });

        GaussMethodCommand = new RelayCommand(() =>
        {
            if (Image is null)
            {
                return;
            }
            
            Image = ConvertService.WritableBitmapToBitmapImage(gauss.ApplyFilter((BitmapImage) Image));
        });

        SetFirstImageCommand = new RelayCommand(() =>
        {
            BitmapImage? image = fileManager.Open();
            if (image is null)
            {
                return;
            }

            ie.SetImage1(image);
        });
        
        SetSecondImageCommand = new RelayCommand(() =>
        {
            BitmapImage? image = fileManager.Open();
            if (image is null)
            {
                return;
            }

            ie.SetImage2(image);
        });

        PSNRCommand = new RelayCommand(() =>
        {
            MessageBox.Show($"PSNR is {ie.PSNR()}", "PSNR");
        });
        
        SSIMCommand = new RelayCommand(() =>
        {
            MessageBox.Show($"SSIM is {ie.SSIM()}", "SSIM");
        });
    }

    private ImageSource? _image;
}
