using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Prakticheskaya3
{
    public partial class MainWindow : Window
    {
        public bool Proverka;
        public DirectoryInfo direct;
        public List<string> musicPath = new List<string>();
        public int musicIndex = 0;
        public DispatcherTimer timer = new DispatcherTimer();
        public bool repeat = true;
        public bool random = true;
        public Random rnd = new Random();
        public int sohran;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void pomoh()
        {
            try
            {
                Media.Source = new Uri(musicPath[musicIndex]);
                Media.Play();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
                Proverka = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Кетчуп");
                throw;
            }
        }

        private void kiki()
        {
            if (AudioSlider.Value == Media.NaturalDuration.TimeSpan.Ticks)
            {
                try
                {
                    timer.Stop();

                    if (repeat == false)
                    {
                        prodolj();
                    }
                    if (random == false)
                    {
                        musicIndex = rnd.Next(0, musicPath.Count - 1);
                        prodolj();
                    }
                    else
                    {
                        if (musicIndex == musicPath.Count)
                        {
                            musicIndex = 0;
                            prodolj();
                        }
                        else if (musicIndex < musicPath.Count)
                        {
                            musicIndex += 1;
                            prodolj();
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("ошибка в кики");
                    throw;
                }
            }
        }

        private void prodolj()
        {
            try
            {
                if (musicIndex == musicPath.Count) 
                {
                    musicIndex = 0;
                }
                AudioSlider.Value = 0;
                timer.Start();
                Media.Source = new Uri(musicPath[musicIndex]);
                Media.Play();
                Proverka = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Кетчуп");
                throw;
            }
        }


        void timer_Tick(object sender, EventArgs e)
        {
            if (Media.Source != null)
            {
                AudioSlider.Value = Media.Position.Ticks;
                TimeStart.Content = String.Format("{0} / {1}", Media.Position.ToString(@"mm\:ss"), Media.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                kiki();
            }
            else
            {
                TimeStart.Content = "No file selected...";
            }
        }

        private void AudioSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Media.Position = new TimeSpan(Convert.ToInt64(AudioSlider.Value));
        }

        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            AudioSlider.Maximum = Media.NaturalDuration.TimeSpan.Ticks;
        }

        private void ViborPapki_Click(object sender, RoutedEventArgs e)
        {
            if (Spisok.Items.Count != 0)
            {
                Spisok.Items.Clear();
            }
            
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };            
            var result = dialog.ShowDialog();
            dialog.RestoreDirectory= true;

            if(result == CommonFileDialogResult.Ok)
            {
                string FileName = dialog.FileName;
                var Folder = Directory.GetFiles(dialog.FileName, "*.mp3");

                direct = new DirectoryInfo(dialog.FileName);
                var files = direct.GetFiles("*.mp3");
                foreach(var file in files)
                {
                    Spisok.Items.Add(file.Name);
                    musicPath.Add(file.FullName);
                }
                pomoh();
            }
        }

        private void Spisok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.Stop();
            Media.Stop();
            int i = Spisok.SelectedIndex;
            musicIndex = i;
            prodolj();
        }

        private void ButtonStart_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var i in musicPath)
            {
                if (Proverka)
                {
                    Media.Pause();
                    Proverka = false;
                    return;
                }
                else
                {
                    Media.Play();
                    Proverka = true;
                    return;
                }
            }
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            if (musicPath.Count != 0)
            {
                if (random == false)
                {
                    musicIndex = rnd.Next(0, musicPath.Count - 1);
                    prodolj();
                }
                else if (musicIndex > 0)
                {
                    timer.Stop();
                    Media.Stop();
                    musicIndex -= 1;
                    prodolj();
                }
                else if (musicIndex == 0)
                {
                    timer.Stop();
                    musicIndex = musicPath.Count - 1;
                    Media.Stop();
                    prodolj();
                }
            }
        }

        private void ButtonVpered_Click(object sender, RoutedEventArgs e)
        {
            if (random == false)
            {
                musicIndex = rnd.Next(0, musicPath.Count - 1);
                prodolj();
            }
            else if (musicPath.Count != 0)
            {
                timer.Stop();
                Media.Stop();
                musicIndex += 1;
                prodolj();
            }
        }

        private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            Window2 window2 = new Window2();
            if (repeat)
            {
                window1.Show();
                repeat = false;
            }
            else
            {
                window2.Show();
                repeat = true;
            }
        }

        private void ButtonRandom_Click(object sender, RoutedEventArgs e)
        {
            if (random)
            {
                MessageBox.Show("Вы включили режим с рандомным порядком");
                random = false;
                musicIndex = rnd.Next(0, musicPath.Count-1);
            }
            else
            {
                MessageBox.Show("Вы выключили режим с рандомным порядком");
                random = true;
            }
        }
    }
}
