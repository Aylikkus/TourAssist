using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourAssist.Model.Scaffold;
using TourAssist.View;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private void fetchDb()
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                Countries = new ObservableCollection<Country>(dbContext.Countries.ToList());
                Regions = new ObservableCollection<Region>(dbContext.Regions.ToList());
            }
        }

        private void doContextAction(Action<TourismDbContext> action)
        {
            try
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    action(dbContext);
                    dbContext.SaveChanges();
                }

                fetchDb();
            }
            catch (Exception ex)
            {
                PopupService.ShowMessage("Произошла ошибка при изменении элементов." +
                    "Убедитесь, что все поля проставлены в правильном формате." +
                    "Ошибка: " + ex.Message + "\n" + ex.InnerException?.Message);
            }
        }

        #region Страны
        private Country selectedCountry;

        public Country SelectedCountry
        {
            get
            {
                if (selectedCountry == null)
                {
                    selectedCountry = new Country();
                    OnPropertyChanged(nameof(SelectedCountry));
                }

                return selectedCountry;
            }
            set
            {
                selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }

        private ObservableCollection<Country> countries;

        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            private set
            {
                countries = value;
                OnPropertyChanged(nameof(Countries));
            }
        }

        private RelayCommand? addCountry;
        public RelayCommand AddCountry
        {
            get
            {
                return addCountry ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Countries.Add(SelectedCountry));
                });
            }
        }

        private RelayCommand? updateCountry;
        public RelayCommand UpdateCountry
        {
            get
            {
                return updateCountry ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Countries.Update(SelectedCountry));
                });
            }
        }

        private RelayCommand? removeCountry;
        public RelayCommand RemoveCountry
        {
            get
            {
                return removeCountry ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Countries.Remove(SelectedCountry));
                });
            }
        }
        #endregion

        #region Регионы

        private Region selectedRegion;

        public Region SelectedRegion
        {
            get
            {
                if (selectedRegion == null)
                {
                    selectedRegion = new Region();
                    OnPropertyChanged(nameof(SelectedRegion));
                }

                return selectedRegion;
            }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));
                OnPropertyChanged(nameof(SelectedRegionCountry));
            }
        }

        public string? SelectedRegionCountry
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    return dbContext.Countries
                        .FirstOrDefault(c => c.Iso31661 == SelectedRegion.CountryIso31661)
                        ?.FullName;
                }
            }
        }

        private ObservableCollection<Region> regions;

        public ObservableCollection<Region> Regions
        {
            get { return regions; }
            private set
            {
                regions = value;
                OnPropertyChanged(nameof(Regions));
            }
        }

        private RelayCommand? addRegion;
        public RelayCommand AddRegion
        {
            get
            {
                return addRegion ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Regions.Add(
                        new Region { 
                            FullName=SelectedRegion.FullName, 
                            CountryIso31661=SelectedRegion.CountryIso31661}));
                });
            }
        }

        private RelayCommand? updateRegion;
        public RelayCommand UpdateRegion
        {
            get
            {
                return updateRegion ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Regions.Update(SelectedRegion));
                });
            }
        }

        private RelayCommand? removeRegion;
        public RelayCommand RemoveRegion
        {
            get
            {
                return removeRegion ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Regions.Remove(SelectedRegion));
                });
            }
        }

        private RelayCommand? selectCountry;
        public RelayCommand SelectCountry
        {
            get
            {
                return selectCountry ??= new RelayCommand(obj =>
                {
                    Country? country = PopupService.SelectCountry();
                    if (country != null)
                    {
                        SelectedRegion.CountryIso31661 = country.Iso31661;
                        OnPropertyChanged(nameof(SelectedRegionCountry));
                    }
                });
            }
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public AdminViewModel() 
        {
            selectedCountry = new Country();
            countries = new ObservableCollection<Country>();
            fetchDb();
        }
    }
}
