using FuzzySharp;
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
        private Transport? selectedTransport;

        private ObservableCollection<Country> countries;
        private ObservableCollection<Region> regions;
        private ObservableCollection<City> cities;
        private ObservableCollection<Peculiarity> peculiarities;
        private ObservableCollection<Transport> transports;

        private string query;

        private ObservableCollection<T> filterBy<T>(ObservableCollection<T> values, Func<T, string> param)
        {
            var filtered = new ObservableCollection<T>();

            foreach (var value in values)
            {
                if (Fuzz.Ratio(Query.ToLowerInvariant(), param(value).ToLowerInvariant()) > 50)
                    filtered.Add(value);
            }

            return filtered;
        }

        public ObservableCollection<Country> Countries
        {
            get 
            {
                if (Query != null && Query != "")
                {
                    return filterBy(countries, (c) => c.FullName);
                }

                return countries; 
            }
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
            get 
            {
                if (Query != null && Query != "")
                {
                    return filterBy(regions, (r) => r.FullName);
                }

                return regions; 
            }
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
            get 
            {
                if (Query != null && Query != "")
                {
                    return filterBy(cities, (c) => c.FullName);
                }

                return cities; 
            }
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
            get 
            {
                if (Query != null && Query != "")
                {
                    return filterBy(peculiarities, (p) => p.Description);
                }

                return peculiarities; 
            }
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

        public ObservableCollection<Transport> Transports
        {
            get 
            {
                if (Query != null && Query != "")
                {
                    return filterBy(transports, (t) => t.Name);
                }

                return transports; 
            }
            private set
            {
                transports = value;
                OnPropertyChanged(nameof(Transports));
            }
        }

        public Transport? SelectedTransport
        {
            get { return selectedTransport; }
            set
            {
                selectedTransport = value;
                OnPropertyChanged(nameof(SelectedTransport));
            }
        }

        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));

                OnPropertyChanged(nameof(Cities));
                OnPropertyChanged(nameof(Regions));
                OnPropertyChanged(nameof(Countries));
                OnPropertyChanged(nameof(Peculiarities));
                OnPropertyChanged(nameof(Transports));
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

        public void FetchTransports()
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                Transports = new ObservableCollection<Transport>(dbContext.Transports.ToList());
            }
        }

        public void FetchPeculiaritiesCountry(Country country)
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var joins = new ObservableCollection<PecularitiesCountry>(dbContext.PecularitiesCountries
                    .Where(pc => pc.CountryIso31661 == country.Iso31661));
                Peculiarities = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                    .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
            }
        }

        public void FetchPeculiaritiesRegion(Region region)
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var joins = new ObservableCollection<PecularitiesRegion>(dbContext.PecularitiesRegions
                    .Where(pc => pc.RegionIdRegion == region.IdRegion));
                Peculiarities = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                    .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
            }
        }

        public void FetchPeculiaritiesCity(City city)
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var joins = new ObservableCollection<PecularitiesCity>(dbContext.PecularitiesCities
                    .Where(pc => pc.CityIdCity == city.IdCity));
                Peculiarities = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                    .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
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
            transports = new ObservableCollection<Transport>();
            query = "";
        }
    }
}
