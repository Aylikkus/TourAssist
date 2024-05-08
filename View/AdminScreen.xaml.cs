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
using TourAssist.ViewModel;
using TourAssist.ViewModel.Utility;

namespace TourAssist.View
{
    /// <summary>
    /// Логика взаимодействия для AdminScreen.xaml
    /// </summary>
    public partial class AdminScreen : Window
    {
        public AdminScreen()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }

        private void countriesBtn_Click(object sender, RoutedEventArgs e)
        {
            countriesGrid.Visibility = Visibility.Visible;
            regionsGrid.Visibility = Visibility.Collapsed;
            citiesGrid.Visibility = Visibility.Collapsed;
            peculiaritiesGrid.Visibility = Visibility.Collapsed;
        }

        private void regionsBtn_Click(object sender, RoutedEventArgs e)
        {
            countriesGrid.Visibility= Visibility.Collapsed;
            regionsGrid.Visibility= Visibility.Visible;
            citiesGrid.Visibility = Visibility.Collapsed;
            peculiaritiesGrid.Visibility = Visibility.Collapsed;
        }

        private void citiesBtn_Click(object sender, RoutedEventArgs e)
        {
            countriesGrid.Visibility = Visibility.Collapsed;
            regionsGrid.Visibility = Visibility.Collapsed;
            citiesGrid.Visibility = Visibility.Visible;
            peculiaritiesGrid.Visibility = Visibility.Collapsed;
        }

        private void peculiaritiesBtn_Click(object sender, RoutedEventArgs e)
        {
            countriesGrid.Visibility = Visibility.Collapsed;
            regionsGrid.Visibility = Visibility.Collapsed;
            citiesGrid.Visibility = Visibility.Collapsed;
            peculiaritiesGrid.Visibility = Visibility.Visible;
        }
    }
}
