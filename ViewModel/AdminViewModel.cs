﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
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
                Cities = new ObservableCollection<City>(dbContext.Cities.ToList());
                Peculiarities = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList());
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
                    OnPropertyChanged(nameof(SelectedCountryPeculiarities));
                }

                return selectedCountry;
            }
            set
            {
                selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                OnPropertyChanged(nameof(SelectedCountryPeculiarities));
            }
        }

        public string SelectedCountryPeculiarities
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    var joins = new ObservableCollection<PecularitiesCountry>(dbContext.PecularitiesCountries
                        .Where(pc => pc.CountryIso31661 == SelectedCountry.Iso31661));
                    var pecs = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                        .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
                    StringBuilder sb = new StringBuilder();
                    foreach (var p in pecs)
                    {
                        sb.Append(p.Description + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    return sb.ToString();
                }
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
                    doContextAction(dbContext => dbContext.Countries.Add(
                        new Country()
                        {
                            Iso31661 = SelectedCountry.Iso31661,
                            FullName = SelectedCountry.FullName
                        }));
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

        private RelayCommand? addCountryPeculiarity;
        public RelayCommand AddCountryPeculiarity
        {
            get
            {
                return addCountryPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? peculiarity = PopupService.SelectPeculiarity();

                    if (peculiarity == null) return;

                    doContextAction(dbContext => dbContext.PecularitiesCountries.Add(
                        new PecularitiesCountry()
                        {
                            CountryIso31661 = SelectedCountry.Iso31661,
                            PeculiarityIdPeculiarity = peculiarity.IdPeculiarity
                        }));
                    OnPropertyChanged(nameof(SelectedCountryPeculiarities));
                });
            }
        }

        private RelayCommand? removeCountryPeculiarity;
        public RelayCommand RemoveCountryPeculiarity
        {
            get
            {
                return removeCountryPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? peculiarity = PopupService.SelectPeculiarityFor(SelectedCountry);

                    if (peculiarity == null) return;

                    doContextAction(dbContext => 
                    {
                        var join = new ObservableCollection<PecularitiesCountry>(dbContext.PecularitiesCountries
                            .Where(pc => pc.PeculiarityIdPeculiarity == peculiarity.IdPeculiarity &&
                                pc.CountryIso31661 == SelectedCountry.Iso31661)).FirstOrDefault();

                        if (join != null)
                            dbContext.Remove(join);
                    });
                    OnPropertyChanged(nameof(SelectedCountryPeculiarities));
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
                    OnPropertyChanged(nameof(SelectedRegionCountry));
                    OnPropertyChanged(nameof(SelectedRegionPeculiarities));
                }

                return selectedRegion;
            }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));
                OnPropertyChanged(nameof(SelectedRegionCountry));
                OnPropertyChanged(nameof(SelectedRegionPeculiarities));
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

        public string SelectedRegionPeculiarities
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    var joins = new ObservableCollection<PecularitiesRegion>(dbContext.PecularitiesRegions
                        .Where(pc => pc.RegionIdRegion == SelectedRegion.IdRegion));
                    var pecs = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                        .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
                    StringBuilder sb = new StringBuilder();
                    foreach (var p in pecs)
                    {
                        sb.Append(p.Description + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    return sb.ToString();
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

        private RelayCommand? addRegionPeculiarity;
        public RelayCommand AddRegionPeculiarity
        {
            get
            {
                return addRegionPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? peculiarity = PopupService.SelectPeculiarity();

                    if (peculiarity == null) return;

                    doContextAction(dbContext => dbContext.PecularitiesRegions.Add(
                        new PecularitiesRegion()
                        {
                            RegionIdRegion = SelectedRegion.IdRegion,
                            PeculiarityIdPeculiarity = peculiarity.IdPeculiarity
                        }));
                    OnPropertyChanged(nameof(SelectedRegionPeculiarities));
                });
            }
        }

        private RelayCommand? removeRegionPeculiarity;
        public RelayCommand RemoveRegionPeculiarity
        {
            get
            {
                return removeRegionPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? peculiarity = PopupService.SelectPeculiarityFor(SelectedRegion);

                    if (peculiarity == null) return;

                    doContextAction(dbContext =>
                    {
                        var join = new ObservableCollection<PecularitiesRegion>(dbContext.PecularitiesRegions
                            .Where(pc => pc.PeculiarityIdPeculiarity == peculiarity.IdPeculiarity &&
                                pc.RegionIdRegion == SelectedRegion.IdRegion)).FirstOrDefault();

                        if (join != null)
                            dbContext.Remove(join);
                    });
                    OnPropertyChanged(nameof(SelectedRegionPeculiarities));
                });
            }
        }

        #endregion

        #region Города

        private City selectedCity;

        public City SelectedCity
        {
            get
            {
                if (selectedCity == null)
                {
                    selectedCity = new City();
                    OnPropertyChanged(nameof(SelectedCity));
                    OnPropertyChanged(nameof(SelectedCityCountry));
                    OnPropertyChanged(nameof(SelectedCityRegion));
                    OnPropertyChanged(nameof(SelectedCityPeculiarities));
                }

                return selectedCity;
            }
            set
            {
                selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
                OnPropertyChanged(nameof(SelectedCityCountry));
                OnPropertyChanged(nameof(SelectedCityRegion));
                OnPropertyChanged(nameof(SelectedCityPeculiarities));
            }
        }

        public string? SelectedCityCountry
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    Region? region = dbContext.Regions.FirstOrDefault(
                        r => r.IdRegion == SelectedCity.RegionIdRegion);

                    if (region == null) return null;

                    return dbContext.Countries
                        .FirstOrDefault(c => c.Iso31661 == region.CountryIso31661)
                        ?.FullName;
                }
            }
        }

        public string? SelectedCityRegion
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    return dbContext.Regions
                        .FirstOrDefault(r => r.IdRegion == SelectedCity.RegionIdRegion)
                        ?.FullName;
                }
            }
        }

        public string SelectedCityPeculiarities
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    var joins = new ObservableCollection<PecularitiesCity>(dbContext.PecularitiesCities
                        .Where(pc => pc.CityIdCity == SelectedCity.IdCity));
                    var pecs = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                        .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
                    StringBuilder sb = new StringBuilder();
                    foreach (var p in pecs)
                    {
                        sb.Append(p.Description + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    return sb.ToString();
                }
            }
        }

        private ObservableCollection<City> cities;

        public ObservableCollection<City> Cities
        {
            get { return cities; }
            private set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private RelayCommand? addCity;
        public RelayCommand AddCity
        {
            get
            {
                return addCity ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Cities.Add(
                        new City
                        {
                            FullName = SelectedCity.FullName,
                            RegionIdRegion = SelectedCity.RegionIdRegion
                        }));
                });
            }
        }

        private RelayCommand? updateCity;
        public RelayCommand UpdateCity
        {
            get
            {
                return updateCity ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Cities.Update(SelectedCity));
                });
            }
        }

        private RelayCommand? removeCity;
        public RelayCommand RemoveCity
        {
            get
            {
                return removeCity ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Cities.Remove(SelectedCity));
                });
            }
        }

        private RelayCommand? selectRegion;
        public RelayCommand SelectRegion
        {
            get
            {
                return selectRegion ??= new RelayCommand(obj =>
                {
                    Region? region = PopupService.SelectRegion();
                    if (region != null)
                    {
                        SelectedCity.RegionIdRegion = region.IdRegion;
                        OnPropertyChanged(nameof(SelectedCityCountry));
                        OnPropertyChanged(nameof(SelectedCityRegion));
                    }
                });
            }
        }

        private RelayCommand? addCityPeculiarity;
        public RelayCommand AddCityPeculiarity
        {
            get
            {
                return addCityPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? peculiarity = PopupService.SelectPeculiarity();

                    if (peculiarity == null) return;

                    doContextAction(dbContext => dbContext.PecularitiesCities.Add(
                        new PecularitiesCity()
                        {
                            CityIdCity = SelectedCity.IdCity,
                            PeculiarityIdPeculiarity = peculiarity.IdPeculiarity
                        }));
                    OnPropertyChanged(nameof(SelectedCityPeculiarities));
                });
            }
        }

        private RelayCommand? removeCityPeculiarity;
        public RelayCommand RemoveCityPeculiarity
        {
            get
            {
                return removeCityPeculiarity ??= new RelayCommand(obj =>
                {
                    Peculiarity? peculiarity = PopupService.SelectPeculiarityFor(SelectedCity);

                    if (peculiarity == null) return;

                    doContextAction(dbContext =>
                    {
                        var join = new ObservableCollection<PecularitiesCity>(dbContext.PecularitiesCities
                            .Where(pc => pc.PeculiarityIdPeculiarity == peculiarity.IdPeculiarity &&
                                pc.CityIdCity == SelectedCity.IdCity)).FirstOrDefault();

                        if (join != null)
                            dbContext.Remove(join);
                    });
                    OnPropertyChanged(nameof(SelectedCityPeculiarities));
                });
            }
        }

        #endregion

        #region Особенности

        private Peculiarity selectedPeculiarity;

        public Peculiarity SelectedPeculiarity
        {
            get
            {
                if (selectedPeculiarity == null)
                {
                    selectedPeculiarity = new Peculiarity();
                    OnPropertyChanged(nameof(SelectedPeculiarity));
                }

                return selectedPeculiarity;
            }
            set
            {
                selectedPeculiarity = value;
                OnPropertyChanged(nameof(SelectedPeculiarity));
            }
        }

        private ObservableCollection<Peculiarity> peculiarities;

        public ObservableCollection<Peculiarity> Peculiarities
        {
            get { return peculiarities; }
            private set
            {
                peculiarities = value;
                OnPropertyChanged(nameof(Peculiarities));
            }
        }

        private RelayCommand? addPeculiarity;
        public RelayCommand AddPeculiarity
        {
            get
            {
                return addPeculiarity ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Peculiarities.Add(
                        new Peculiarity
                        {
                            Description = SelectedPeculiarity.Description
                        }));
                });
            }
        }

        private RelayCommand? updatePeculiarity;
        public RelayCommand UpdatePeculiarity
        {
            get
            {
                return updatePeculiarity ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Peculiarities.Update(SelectedPeculiarity));
                });
            }
        }

        private RelayCommand? removePeculiarity;
        public RelayCommand RemovePeculiarity
        {
            get
            {
                return removePeculiarity ??= new RelayCommand(obj =>
                {
                    doContextAction(dbContext => dbContext.Peculiarities.Remove(SelectedPeculiarity));
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
            fetchDb();
        }
    }
}
