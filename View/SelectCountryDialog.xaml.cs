﻿using System;
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

namespace TourAssist.View
{
    /// <summary>
    /// Логика взаимодействия для SelectEntityDialog.xaml
    /// </summary>
    public partial class SelectCountryDialog : Window
    {
        SelectEntityViewModel viewModel = new SelectEntityViewModel();
        public SelectCountryDialog()
        {
            InitializeComponent();
            viewModel.Fetch(true, false, false, false);
            DataContext = viewModel;
        }

        public Country? Selected { get => viewModel.SelectedCountry; }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
