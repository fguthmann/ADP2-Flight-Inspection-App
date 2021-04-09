using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using ex1.ViewModel;
namespace ex1.View
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }
        private FlightInfoViewModel flightInfoVM;
        public string pathFileNormalFlight;
        public string fileNameNormalFlight;
        public string pathFileExceptionFile;
        public string fileNameExceptionFile;


        // Upload our normal flight file
        private void NormalFLight_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //Download file
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                pathFileNormalFlight = System.IO.Path.GetFullPath(files[0]);
                fileNameNormalFlight = System.IO.Path.GetFileName(files[0]);

                // Show that file ha been download correctly
                FileNameLabel.Content = "Normal flight file downloaded";

            }
        }


        private void RegisterAlgo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InternCircleAlgo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            flightInfoVM.VM_close();
            this.Close();

        }


        //Start the flight and close our menu window
        private void RunFlight_Click(object sender, RoutedEventArgs e)
        {
            MainWindow runFlight = new MainWindow();
            runFlight.Show();
            this.Close();
        }


        // Open window to drop file of new algorithm
        private void AddAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            AddNewAlgorithm addNewAlgorithm = new AddNewAlgorithm();
            addNewAlgorithm.Show();
        }

        // Upload our exception flight file
        private void exceptionFLight_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //Download file
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                pathFileExceptionFile = System.IO.Path.GetFullPath(files[0]);
                fileNameExceptionFile = System.IO.Path.GetFileName(files[0]);

                // Show that file ha been download correctly
                FileNameLabel.Content = "Exception flight file downloaded";

            }
        }
    }
}
