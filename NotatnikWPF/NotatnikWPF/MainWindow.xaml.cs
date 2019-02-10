using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Ribbon;

namespace NotatnikWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        private string fileSource = null;

        private int startSoughtString;

        private bool isTextChanged = false;


        public MainWindow()
        {
            Settings.ReadLanguage();
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = Properties.Resources.ChooseFileDialog;
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = Properties.Resources.DialogBoxesFiltr;
            openFileDialog.FilterIndex = 1;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = Properties.Resources.SaveFileDialog;
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = openFileDialog.Filter;
            saveFileDialog.FilterIndex = 1;
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileSource))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(fileSource);
                openFileDialog.FileName = Path.GetFileName(fileSource);
            }

            bool? result = openFileDialog.ShowDialog();

            if(result.HasValue && result.Value)
            {
                fileSource = openFileDialog.FileName;
                textBox.Text = File.ReadAllText(fileSource);
                statusBarText.Text = Path.GetFileName(fileSource);
                isTextChanged = false;
                statusBar.Background = Brushes.Green;
            }
        }

        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileSource))
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(fileSource);
                saveFileDialog.FileName = Path.GetFileName(fileSource);
            }

            bool? result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                fileSource = saveFileDialog.FileName;
                File.WriteAllText(fileSource, textBox.Text);
                statusBarText.Text = Path.GetFileName(fileSource);
                isTextChanged = false;
                statusBar.Background = Brushes.Green;
                statusBarTextChanged.Text = Properties.Resources.TextSaved;
            }
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileSource))
            {
                File.WriteAllText(fileSource, textBox.Text);
                isTextChanged = false;
                statusBar.Background = Brushes.Green;
                statusBarTextChanged.Text = Properties.Resources.TextSaved;
            }
            else MenuItem_SaveAs_Click(sender, e);
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            isTextChanged = true;
            statusBar.Background = Brushes.Red;
            statusBarTextChanged.Text = Properties.Resources.TextNotSaved;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool cancel;
            AskForSavingFile(sender, out cancel);
            e.Cancel = cancel;
        }

        private void AskForSavingFile(object sender, out bool cancel)
        {
            cancel = false;
            if (isTextChanged)
            {
                MessageBoxResult result = MessageBox.Show("Czy zapisać zmiany w edytowanym dokumencie?",
                    this.Title,
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question,
                    MessageBoxResult.Cancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_Save_Click(sender, null);
                        break;
                    case MessageBoxResult.No: break;
                    case MessageBoxResult.Cancel:
                    default:
                        cancel = true;
                        break;
                }
            }
        }

        private void MenuItem_NewFile_Click(object sender, RoutedEventArgs e)
        {
            bool cancel;
            AskForSavingFile(sender, out cancel);
            if (!cancel) textBox.Text = "";
            statusBarText.Text = "[Brak pliku]";
        }

        private void MenuItem_Undo_Click(object sender, RoutedEventArgs e)
        {
            textBox.Undo();
        }

        private void MenuItem_Redo_Click(object sender, RoutedEventArgs e)
        {
            textBox.Redo();
        }

        private void MenuItem_Cut_Click(object sender, RoutedEventArgs e)
        {
            textBox.Cut();
        }

        private void MenuItem_Copy_Click(object sender, RoutedEventArgs e)
        {
            textBox.Copy();
        }

        private void MenuItem_Paste_Click(object sender, RoutedEventArgs e)
        {
            textBox.Paste();
        }

        private void MenuItem_DeleteText_Click(object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = "";
        }

        private void MenuItem_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
        }

        private void MenuItem_DataTime_Click(object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = System.DateTime.Now.ToString();
        }

        private void MenuItem_WrapRows_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = null;
            if (sender is MenuItem) isChecked = (sender as MenuItem).IsChecked;
            if (sender is RibbonCheckBox) isChecked = (sender as RibbonCheckBox).IsChecked;
            if (isChecked.HasValue)
                textBox.TextWrapping = isChecked.Value ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }

        private static void ChangeLanguage(string lang)
        {
            Settings.SaveLanguage(lang);
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }

        
        private void MenuItem_ToolBar_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = null;
            if (sender is MenuItem) isChecked = (sender as MenuItem).IsChecked;
            if (sender is RibbonCheckBox) isChecked = (sender as RibbonCheckBox).IsChecked;
            if (isChecked.HasValue)
                toolBar.Visibility = isChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_StatusBar_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = null;
            if (sender is MenuItem) isChecked = (sender as MenuItem).IsChecked;
            if (sender is RibbonCheckBox) isChecked = (sender as RibbonCheckBox).IsChecked;
            if( isChecked.HasValue )
                statusBar.Visibility = isChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_Print_Click(object sender, RoutedEventArgs e)
        {
            Printing.PrintText(textBox.Text);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5: MenuItem_DataTime_Click(sender, null);
                    break;
            }
            if((e.KeyboardDevice.Modifiers) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.N: MenuItem_NewFile_Click(sender, null);
                        break;
                    case Key.O:
                        MenuItem_Open_Click(sender, null);
                        break;
                    case Key.S:
                        MenuItem_Save_Click(sender, null);
                        break;
                    case Key.P:
                        MenuItem_Print_Click(sender, null);
                        break;
                }
            }
        }

        private void MenuItem_SwitchLangToPL_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("pl");
        }

        private void MenuItem_SwitchLangToEN_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("en");
        }

        private void MenuItem_OpenFindMenu_Click(object sender, RoutedEventArgs e)
        {
            FindMenu.IsOpen = true;
        }

        private void Button_CloseFindMenu_Click(object sender, RoutedEventArgs e)
        {
            FindMenu.IsOpen = false;
            FindingResult.Text = null;
        }

        private void Button_Find_Click(object sender, RoutedEventArgs e)
        {
            try {
                startSoughtString = textBox.Text.IndexOf(ToFind.Text);
                textBox.Focus();
                textBox.Select(startSoughtString, ToFind.Text.Length);
                FindingResult.Text = null;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                startSoughtString = -1;
                FindingResult.Text = Properties.Resources.NotFound;
            }
        }

        private void ToFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ToFind.Text))
            {
                ToFindButton.IsEnabled = true;
                FindNextButton.IsEnabled = true;
            }
            else
            {
                ToFindButton.IsEnabled = false;
                ToChangeButton.IsEnabled = false;
                FindNextButton.IsEnabled = false;
            }
        }

        private void ToChange_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ToFind.Text) && !string.IsNullOrWhiteSpace(ToChange.Text))
            {
                ToChangeButton.IsEnabled = true;
            }
            else ToChangeButton.IsEnabled = false;
        }

        private void Button_FindNext_Click(object sender, RoutedEventArgs e)
        {
            try {
                startSoughtString = textBox.Text.IndexOf(ToFind.Text, startSoughtString + ToFind.Text.Length);
                textBox.Focus();
                textBox.Select(startSoughtString, ToFind.Text.Length);
                FindingResult.Text = null;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                startSoughtString = -1;
                FindingResult.Text = Properties.Resources.NotFoundMore;
            }
        }

        private void ToChangeButton_Click(object sender, RoutedEventArgs e)
        {
            textBox.Focus();
            textBox.SelectedText = ToChange.Text;
            FindingResult.Text = null;
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox.SelectedText))
            {
                ToChangeButton.IsEnabled = true;
            }
            else ToChangeButton.IsEnabled = false;
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            CanDo.IsEnabled = textBox.CanRedo;
            UnDo.IsEnabled = textBox.CanUndo;
            Paste.IsEnabled = Clipboard.ContainsText();
            Copy.IsEnabled = !string.IsNullOrWhiteSpace(textBox.SelectedText);
            Cut.IsEnabled = !string.IsNullOrWhiteSpace(textBox.SelectedText);
        }

        private void MenuItem_Font_Click(object sender, RoutedEventArgs e)
        {
            Fonts font = Fonts.ExtractFont(textBox);
            if(Fonts.ChooseFont(ref font)) font.ChangeTo(textBox);
        }
    }
}
