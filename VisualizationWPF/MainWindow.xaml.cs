using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using VisualizationWPF.Algorithms;
using NAudio.Wave;

namespace VisualizationWPF
{
    public partial class MainWindow : Window
    {
        public const int DelayTime = 32;
        public const int BarCount = 70;

        public int[] data;
        public List<Rectangle> bars = new();

        // 🔊 NAudio fields
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            // 🔊 Initialize NAudio
            audioFile = new AudioFileReader("click.wav");
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateData();
            DrawBars();

            // 🔁 SWITCH ALGORITHM HERE
            //ISortAlgorithm algorithm = new QuickSort(this);
            //ISortAlgorithm algorithm = new MergeSort(this);
            ISortAlgorithm algorithm = new HeapSort(this);

            await algorithm.Sort(data);
        }

        void GenerateData()
        {
            data = Enumerable.Range(1, BarCount).ToArray();
            Shuffle(data);
        }

        void Shuffle(int[] array)
        {
            Random rnd = new();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        void DrawBars()
        {
            BarCanvas.Children.Clear();
            bars.Clear();

            double width = BarCanvas.ActualWidth / BarCount;

            for (int i = 0; i < BarCount; i++)
            {
                Rectangle rect = new Rectangle
                {
                    Width = width - 1,
                    Height = data[i] * 8,
                    Fill = Brushes.Lime
                };

                Canvas.SetLeft(rect, i * width);
                Canvas.SetBottom(rect, 0);

                bars.Add(rect);
                BarCanvas.Children.Add(rect);
            }
        }

        // ===== UI HELPERS FOR ALGORITHMS =====

        public async Task Delay() => await Task.Delay(DelayTime);

        public void Swap(int i, int j)
        {
            bars[i].Height = data[i] * 8;
            bars[j].Height = data[j] * 8;
            PlaySound();
        }

        public void UpdateBar(int i)
        {
            bars[i].Height = data[i] * 8;
            PlaySound();
        }

        public void Highlight(int i, int j)
        {
            bars[i].Fill = Brushes.White;
            bars[j].Fill = Brushes.White;
        }

        public void ResetColor(int i, int j)
        {
            bars[i].Fill = Brushes.Lime;
            bars[j].Fill = Brushes.Lime;
        }

        public void SetColor(int i, Brush color)
        {
            bars[i].Fill = color;
        }

        // 🔊 Low-latency sound playback
        void PlaySound()
        {
            if (audioFile == null || outputDevice == null)
                return;

            audioFile.Position = 0;   // rewind instantly
            outputDevice.Play();
        }

        // Clean up audio on close
        protected override void OnClosed(EventArgs e)
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
            base.OnClosed(e);
        }
    }
}
