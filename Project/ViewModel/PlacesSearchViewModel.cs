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
using TourAssist.Model;

namespace TourAssist.ViewModel
{
    public class PlacesSearchViewModel : INotifyPropertyChanged
    {
        private string query;

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

        private ObservableCollection<Country> searchResultCountries;
        private ObservableCollection<Region> searchResultRegions;
        private ObservableCollection<City> searchResultCities;

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

        private bool searchCountries;
        private bool searchRegions;
        private bool searchCities;

        public bool SearchCountries
        {
            get
            {
                return searchCountries;
            }
            set
            {
                searchCountries = value;
                OnPropertyChanged(nameof(SearchCountries));
            }
        }

        public bool SearchRegions
        {
            get
            {
                return searchRegions;
            }
            set
            {
                searchRegions = value;
                OnPropertyChanged(nameof(SearchRegions));
            }
        }

        public bool SearchCities
        {
            get
            {
                return searchCities;
            }
            set
            {
                searchCities = value;
                OnPropertyChanged(nameof(SearchCities));
            }
        }

        private RelayCommand? search;

        public RelayCommand Search
        {
            get
            {
                return search ??= new RelayCommand(obj =>
                {
                    Interpreter interpreter = new Interpreter(Query);

                    if (SearchCountries)
                    {
                        SearchResultCountries = new ObservableCollection<Country>(
                            interpreter.SearchCountries());
                    }

                    if (SearchRegions)
                    {
                        SearchResultRegions = new ObservableCollection<Region>(
                            interpreter.SearchRegions());
                    }

                    if (SearchCities)
                    {
                        SearchResultCities = new ObservableCollection<City>(
                            interpreter.SearchCities());
                    }
                });
            }
        }

        private RelayCommand? addPeculiarity;

        public RelayCommand AddPeculiarity
        {
            get
            {
                return addPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? result = PopupService.SelectPeculiarity();

                    if (result != null)
                    {
                        Query += Query == "" ? result.Description : " и " + result.Description;
                    }
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public PlacesSearchViewModel()
        {
            query = "";

            searchResultCountries = new ObservableCollection<Country>();
            searchResultRegions = new ObservableCollection<Region>();
            searchResultCities = new ObservableCollection<City>();
        }
    }
}
