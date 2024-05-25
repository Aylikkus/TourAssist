using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.Model.Scaffold;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        string query;
        bool searchCountry, searchRegion, searchCity;
        ObservableCollection<Country> searchResultCountries;
        ObservableCollection<Region> searchResultRegions;
        ObservableCollection<City> searchResultCities;

        public string Query 
        { 
            get
            {
                return query;
            }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
            }
        }

        public bool SearchCountry
        {
            get
            {
                return searchCountry;
            }
            set
            {
                searchCountry = value;
                OnPropertyChanged(nameof(SearchCountry));
            }
        }

        public bool SearchRegion
        {
            get
            {
                return searchRegion;
            }
            set
            {
                searchRegion = value;
                OnPropertyChanged(nameof(SearchRegion));
            }
        }

        public bool SearchCity
        {
            get
            {
                return searchCity;
            }
            set
            {
                searchCity = value;
                OnPropertyChanged(nameof(SearchCity));
            }
        }

        public ObservableCollection<Country> SearchResultCountries
        {
            get
            {
                return searchResultCountries;
            }
            set
            {
                searchResultCountries = value;
                OnPropertyChanged(nameof(SearchResultCountries));
            }
        }

        public ObservableCollection<Region> SearchResultRegions
        {
            get
            {
                return searchResultRegions;
            }
            set
            {
                searchResultRegions = value;
                OnPropertyChanged(nameof(SearchResultRegions));
            }
        }

        public ObservableCollection<City> SearchResultCities
        {
            get
            {
                return searchResultCities;
            }
            set
            {
                searchResultCities = value;
                OnPropertyChanged(nameof(SearchResultCities));
            }
        }

        private RelayCommand? search;
        public RelayCommand Search
        {
            get
            {
                return search ??= new RelayCommand(obj =>
                {
                    if (SearchCountry)
                    {

                    }
                    if (SearchRegion)
                    {

                    }
                    if (SearchCity)
                    {

                    }
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public HistoryViewModel()
        {
            query = "";
        }
    }
}
