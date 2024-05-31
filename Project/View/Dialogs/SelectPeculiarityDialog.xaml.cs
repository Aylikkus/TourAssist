using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TourAssist.Model.Scaffold;
using TourAssist.ViewModel;

namespace TourAssist.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SelectEntityDialog.xaml
    /// </summary>
    public partial class SelectPeculiarityDialog : Window, IDialogView<Peculiarity>
    {
        SelectEntityViewModel viewModel = new SelectEntityViewModel();
        public SelectPeculiarityDialog()
        {
            InitializeComponent();
            viewModel.Fetch(false, false, false, true);
            DataContext = viewModel;
        }

        public SelectPeculiarityDialog(Country country)
        {
            InitializeComponent();
            viewModel.FetchPeculiaritiesCountry(country);
            DataContext = viewModel;
        }

        public SelectPeculiarityDialog(Region region)
        {
            InitializeComponent();
            viewModel.FetchPeculiaritiesRegion(region);
            DataContext = viewModel;
        }

        public SelectPeculiarityDialog(City city)
        {
            InitializeComponent();
            viewModel.FetchPeculiaritiesCity(city);
            DataContext = viewModel;
        }

        public Peculiarity? Selected { get => viewModel.SelectedPeculiarity; }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            okButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
