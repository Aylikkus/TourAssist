using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.Model;
using TourAssist.Model.Scaffold;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class TourSearchViewModel : INotifyPropertyChanged
    {
        private City? fromCity;
        public City? FromCity
        {
            get
            {
                return fromCity;
            }
            set
            {
                fromCity = value;
                OnPropertyChanged(nameof(FromCity));
            }
        }

        private Country? toCountry;
        public Country? ToCountry
        {
            get
            {
                return toCountry;
            }
            set
            {
                toCountry = value;
                OnPropertyChanged(nameof(ToCountry));

                ToRegion = null;
                ToCity = null;
            }
        }

        private Region? toRegion;
        public Region? ToRegion
        {
            get
            {
                return toRegion;
            }
            set
            {
                toRegion = value;
                OnPropertyChanged(nameof(ToRegion));

                if (toRegion != null)
                {
                    using (TourismDbContext dbContext = new TourismDbContext())
                    {
                        toCountry = dbContext.Countries.Where((c) =>
                            c.Iso31661 == toRegion.CountryIso31661).First();
                        OnPropertyChanged(nameof(ToCountry));
                    }
                }

                ToCity = null;
            }
        }

        private City? toCity;
        public City? ToCity
        {
            get
            {
                return toCity;
            }
            set
            {
                toCity = value;
                OnPropertyChanged(nameof(ToCity));

                if (toCity != null)
                {
                    using (TourismDbContext dbContext = new TourismDbContext())
                    {
                        toRegion = dbContext.Regions.Where((r) =>
                            r.IdRegion == toCity.RegionIdRegion).First();
                        OnPropertyChanged(nameof(ToRegion));

                        if (toRegion != null)
                        {
                            toCountry = dbContext.Countries.Where((c) =>
                                c.Iso31661 == toRegion.CountryIso31661).First();
                            OnPropertyChanged(nameof(ToCountry));
                        }
                    }
                }
            }
        }

        private int peopleCount;

        public int PeopleCount
        {
            get
            {
                return peopleCount;
            }
            set
            {
                peopleCount = value;
                OnPropertyChanged(nameof(PeopleCount));
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        private decimal priceLessThanEqual;

        public decimal PriceLessThanEqual
        {
            get
            {
                return priceLessThanEqual;
            }
            set
            {
                priceLessThanEqual = value;
                OnPropertyChanged(nameof(PriceLessThanEqual));
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        private DateOnly departureDate;

        public DateOnly DepartureDate
        {
            get
            {
                return departureDate;
            }
            set
            {
                departureDate = value;
                OnPropertyChanged(nameof(DepartureDate));
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        private ObservableCollection<RouteCitiesView> searchResults;

        public ObservableCollection<RouteCitiesView> SearchResults
        {
            get
            {
                return new ObservableCollection<RouteCitiesView>(searchResults.Where((r) => 
                    r.TransportCapacity >= peopleCount &&
                    r.Price <= priceLessThanEqual && 
                    DateOnly.FromDateTime(r.Departure) == DepartureDate));
            }
            set
            {
                searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        private RelayCommand? search;
        public RelayCommand Search
        {
            get
            {
                return search ??= new RelayCommand(obj =>
                {
                    SearchResults = new ObservableCollection<RouteCitiesView>(
                        Interpreter.FindRoutes(FromCity, ToCountry, ToRegion, ToCity));
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public TourSearchViewModel()
        {
            searchResults = new ObservableCollection<RouteCitiesView>();
        }
    }
}
