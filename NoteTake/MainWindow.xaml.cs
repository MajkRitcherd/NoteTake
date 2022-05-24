using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NoteTake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath = "";
        
        public MainWindow()
        {
            InitializeComponent();
        }


        private void borderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                DragMove();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            if(e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void minimizeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void maximizeButtonClick(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.BorderThickness = new Thickness(7);
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.BorderThickness = new Thickness(0);
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void closeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void newClick(object sender, RoutedEventArgs e)
        {
            if(editorText.Text.Length != 0)
            {
                if (MessageBox.Show("Do you want leave without saving?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    editorText.Clear();
                else
                {
                    if(filePath == "")
                        saveAsClick(sender, e);
                }
            }
        }

        private void openClick(object sender, RoutedEventArgs e)
        {
            if (editorText.Text.Length != 0)
            {
                if (MessageBox.Show("Do you want to leave without saving?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    openFile();
                }
                else
                    return;
            }
            else
                openFile();
        }

        private void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            { Filter = "Text files (*.txt)|*.txt",
              Title = "Open text file"};

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    filePath = openFileDialog.FileName;
                    string text = File.ReadAllText(openFileDialog.FileName);
                    editorText.Text = text;
                }
                else
                    return;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void saveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string textEditor = editorText.Text;
                if(textEditor != "")
                {
                    if (filePath != "" && textEditor != "")
                        File.WriteAllText(filePath, textEditor);
                    else
                        saveAsClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox .Show("Error: " + ex.Message);
            }
        }

        private void saveAsClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            if(saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, editorText.Text);
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void fontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (fontSizeSlider.Value <= 10.0)
                editorText.FontSize = 10.0;
            else
                editorText.FontSize = e.NewValue;
        }
    }
}
