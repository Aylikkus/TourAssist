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
using TourAssist.Model.Scaffold;
using TourAssist.ViewModel;

namespace TourAssist.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SelectEntityDialog.xaml
    /// </summary>
    public partial class SelectRegionDialog : Window, IDialogView<Region>
    {
        SelectEntityViewModel viewModel = new SelectEntityViewModel();
        public SelectRegionDialog()
        {
            InitializeComponent();
            viewModel.Fetch(false, true, false, false);
            DataContext = viewModel;
        }

        public Region? Selected { get => viewModel.SelectedRegion; }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
