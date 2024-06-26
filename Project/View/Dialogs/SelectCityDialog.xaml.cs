﻿using System;
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
    public partial class SelectCityDialog : Window, IDialogView<City>
    {
        SelectEntityViewModel viewModel = new SelectEntityViewModel();
        public SelectCityDialog()
        {
            InitializeComponent();
            viewModel.Fetch(false, false, true, false);
            DataContext = viewModel;
        }

        public City? Selected { get => viewModel.SelectedCity; }

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
