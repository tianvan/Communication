using System.IO;
using System.Windows;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace CommunicationSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MemoryStream _sourceStream = new MemoryStream();
        private MemoryStream _targetStream;

        private long _size;
        private const int PackageSize = 1;
        private SampleData _sampleData;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Serialize()
        {
            var writer = new BsonWriter(_sourceStream);
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, new SampleData());
            _size = _sourceStream.Length;
            _targetStream = new MemoryStream();

        }

        public void Transmit()
        {
            for (var i = 0; i < _size; i += PackageSize)
            {
                _sourceStream.Position = i;
                _targetStream.Position = i;

                var buffer = new byte[PackageSize];
                _sourceStream.Read(buffer, 0, PackageSize);
                _targetStream.Write(buffer, 0, PackageSize);
            }
        }

        public void Deserialize()
        {
            _targetStream.Position = 0;
            var reader = new BsonReader(_targetStream);
            var deserializer = new JsonSerializer();
            _sampleData = deserializer.Deserialize<SampleData>(reader);
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Serialize();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Deserialize();
            TextBlock.Text = _sampleData.Title;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) => Transmit();
    }
}
