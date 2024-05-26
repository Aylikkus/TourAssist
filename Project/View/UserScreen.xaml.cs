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
using TourAssist.Model;
using TourAssist.ViewModel;

namespace TourAssist.View
{
    /// <summary>
    /// Логика взаимодействия для UserScreen.xaml
    /// </summary>
    public partial class UserScreen : Window
    {
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();
        public TourSearchViewModel TourSearchViewModel { get; set; } = new TourSearchViewModel();
        public PlacesSearchViewModel PlacesSearchViewModel { get; set; } = new PlacesSearchViewModel();
        public HistoryViewModel HistoryViewModel { get; set; } = new HistoryViewModel();

        public UserScreen()
        {
            InitializeComponent();

            UserViewModel.WindowClose += new EventHandler(CloseWindow);
            DataContext = this;
        }

        private void CloseWindow(object? sender, EventArgs e)
        {
            Close();
        }

        private void openTourSearch_Click(object sender, RoutedEventArgs e)
        {
            tourSearchGrid.Visibility = Visibility.Visible;
            placesSearchGrid.Visibility = Visibility.Collapsed;
            historyhGrid.Visibility = Visibility.Collapsed;
        }

        private void openPlacesSearch_Click(object sender, RoutedEventArgs e)
        {
            tourSearchGrid.Visibility = Visibility.Collapsed;
            placesSearchGrid.Visibility = Visibility.Visible;
            historyhGrid.Visibility = Visibility.Collapsed;
        }

        private void openHistory_Click(object sender, RoutedEventArgs e)
        {
            tourSearchGrid.Visibility = Visibility.Collapsed;
            placesSearchGrid.Visibility = Visibility.Collapsed;
            historyhGrid.Visibility = Visibility.Visible;
        }

        private void countriesRb_Checked(object sender, RoutedEventArgs e)
        {
            countriesListBox.Visibility = Visibility.Visible;
            regionsListBox.Visibility = Visibility.Collapsed;
            citiesListBox.Visibility = Visibility.Collapsed;
        }

        private void regionsRb_Checked(object sender, RoutedEventArgs e)
        {
            countriesListBox.Visibility = Visibility.Collapsed;
            regionsListBox.Visibility = Visibility.Visible;
            citiesListBox.Visibility = Visibility.Collapsed;
        }

        private void citiesRb_Checked(object sender, RoutedEventArgs e)
        {
            countriesListBox.Visibility = Visibility.Collapsed;
            regionsListBox.Visibility = Visibility.Collapsed;
            citiesListBox.Visibility = Visibility.Visible;
        }
    }
}
