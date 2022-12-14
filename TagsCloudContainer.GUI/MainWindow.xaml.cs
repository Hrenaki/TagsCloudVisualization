using FluentResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.WordPreparers;
using TagsCloudContainer.Infrastructure.WordReaders;

namespace TagsCloudContainer.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISettingsProvider settingsProvider;
        private readonly IWordReader wordReader;
        private readonly IWordPreparer wordPreparer;
        private readonly ITagsCloudGenerator tagsCloudGenerator;
        private readonly WordPlateVisualizer wordPlateVisualizer;

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(OptionsViewModel), typeof(OptionsViewModel), typeof(MainWindow));
        public OptionsViewModel OptionsViewModel
        {
            get => (OptionsViewModel)GetValue(OptionsProperty);
            private set => SetValue(OptionsProperty, value);
        }

        public MainWindow(Options options,
                          ISettingsProvider settingsProvider,
                          IWordReader wordReader,
                          IWordPreparer wordPreparer,
                          ITagsCloudGenerator tagsCloudGenerator,
                          WordPlateVisualizer wordPlateVisualizer)
        {
            this.settingsProvider = settingsProvider;
            this.wordReader = wordReader;
            this.wordPreparer = wordPreparer;
            this.tagsCloudGenerator = tagsCloudGenerator;
            this.wordPlateVisualizer = wordPlateVisualizer;

            InitializeComponent();
            DataContext = this;

            OptionsViewModel = new OptionsViewModel();
            OptionsViewModel.CurrentOptions = options;
        }

        private async void GenerateTagsCloudButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = string.Empty;

            var result = await Task.Run(() =>
            {
                return wordReader.TryReadWords(settingsProvider.GetTextReaderSettings().Filename)
                                 .Bind(wds => wordPreparer.Prepare(wds))
                                 .Bind(GeneratePlates)
                                 .Bind(info => (Result<Bitmap>)wordPlateVisualizer.DrawPlates(info.Value.Plates, info.Value.PictureSize, info.Value.WordColorSettings));
            });

            result.OnSuccess(r => TagsCloudImage.Source = Imaging.CreateBitmapSourceFromHBitmap(r.Value!.GetHbitmap(),
                                                                                                IntPtr.Zero,
                                                                                                Int32Rect.Empty,
                                                                                                BitmapSizeOptions.FromEmptyOptions()))
                  .OnFail(r => HandleFailedResult(r));
        }

        private Result<dynamic> GeneratePlates(string[] wds)
        {
            var wordFrequencies = GetWordFrequencies(wds);
            var wordFontSettings = settingsProvider.GetWordFontSettings();
            wordFontSettings.FontSizeSettings.WordFrequencies = wordFrequencies;

            var wordColorSettings = settingsProvider.GetWordColorSettings();
            wordColorSettings.WordFrequencies = wordFrequencies;

            var words = wordFrequencies.Keys.ToArray();
            var outputImageSettings = settingsProvider.GetOutputImageSettings();
            var pictureSize = new System.Drawing.Size(outputImageSettings.Width, outputImageSettings.Height);

            var generatePlatesResult = tagsCloudGenerator.GeneratePlates(words,
                                                                         new PointF(pictureSize.Width / 2.0F, pictureSize.Height / 2.0F),
                                                                         wordFontSettings);

            return generatePlatesResult.ToResult(r => new { Plates = r, PictureSize = pictureSize, WordColorSettings = wordColorSettings });
        }

        private void HandleFailedResult<T>(Result<T> result)
        {
            ErrorTextBlock.Text = string.Join("\n", result.Reasons.Select(reason => reason.Message));
        }
        private void HandleFailedResult(Result result)
        {
            ErrorTextBlock.Text = string.Join("\n", result.Reasons.Select(reason => reason.Message));
        }

        private static Dictionary<string, int> GetWordFrequencies(IEnumerable<string> words)
        {
            var frequencies = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!frequencies.ContainsKey(word))
                    frequencies.Add(word, 0);
                frequencies[word]++;
            }
            return frequencies;
        }

        private void SaveTagsCloudButton_Click(object sender, RoutedEventArgs e)
        {
            Result.OkIf(TagsCloudImage.Source is BitmapSource, "Image is null")
                  .Bind(() => Result.Try(() =>
                  {
                      var jpgEncoder = new JpegBitmapEncoder();
                      jpgEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)TagsCloudImage.Source));
                      return jpgEncoder;
                  }, e => new Error("Can't make picture from bitmap").CausedBy(e)))
                  .Bind(jpgEncoder => Result.Try(() => new { Stream = File.Create(settingsProvider.GetOutputImageSettings().Filename), Encoder = jpgEncoder },
                                                  e => new Error("Can't create picture. Check path or folder permissions").CausedBy(e)))
                  .Bind(tools => Result.Try(() => tools.Encoder.Save(tools.Stream),
                                             e => new Error("Can't save picture").CausedBy(e)))
                  .OnSuccess(r => MessageBox.Show("Saved"))
                  .OnFail(r => HandleFailedResult(r));
        }
    }
}