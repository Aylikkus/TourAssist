using Microsoft.EntityFrameworkCore;
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
    public class SelectEntityViewModel : INotifyPropertyChanged
    {
        private Country? selectedCountry;
        private Region? selectedRegion;
        private City? selectedCity;
        private Peculiarity? selectedPeculiarity;

        private ObservableCollection<Country> countries;
        private ObservableCollection<Region> regions;
        private ObservableCollection<City> cities;
        private ObservableCollection<Peculiarity> peculiarities;

        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            private set
            {
                countries = value;
                OnPropertyChanged(nameof(Countries));
            }
        }

        public Country? SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }

        public ObservableCollection<Region> Regions
        {
            get { return regions; }
            private set
            {
                regions = value;
                OnPropertyChanged(nameof(Regions));
            }
        }

        public Region? SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));
            }
        }

        public ObservableCollection<City> Cities
        {
            get { return cities; }
            private set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        public City? SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        public ObservableCollection<Peculiarity> Peculiarities
        {
            get { return peculiarities; }
            private set
            {
                peculiarities = value;
                OnPropertyChanged(nameof(Peculiarities));
            }
        }

        public Peculiarity? SelectedPeculiarity
        {
            get { return selectedPeculiarity; }
            set
            {
                selectedPeculiarity = value;
                OnPropertyChanged(nameof(SelectedPeculiarity));
            }
        }

        public void Fetch(bool countries, bool regions, bool cities, bool peculiarities)
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                if (countries)
                    Countries = new ObservableCollection<Country>(dbContext.Countries.ToList());

                if (regions)
                    Regions = new ObservableCollection<Region>(dbContext.Regions.ToList());

                if (cities)
                    Cities = new ObservableCollection<City>(dbContext.Cities.ToList());

                if (peculiarities)
                    Peculiarities = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public SelectEntityViewModel()
        {
            countries = new ObservableCollection<Country>();
            regions = new ObservableCollection<Region>();
            cities = new ObservableCollection<City>();
            peculiarities = new ObservableCollection<Peculiarity>();
        }
    }
}
