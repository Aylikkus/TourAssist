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
    public class AdminViewModel : INotifyPropertyChanged
    {
        private Country selectedCountry;

        public TourismDbContext DbContext { get; private set; }

        public Country SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }

        public ObservableCollection<Country> Countries { get; private set; }

        private RelayCommand? save;
        public RelayCommand Save
        {
            get
            {
                return save ??= new RelayCommand(obj =>
                {
                    try
                    {
                        DbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        PopupService.ShowMessage("Произошла ошибка при изменении элементов." +
                            "Убедитесь, что все поля проставлены в правильном формате." +
                            "Ошибка: " + ex.Message);
                    }
                });
            }
        }

        private RelayCommand? addCountry;
        public RelayCommand AddCountry
        {
            get
            {
                return addCountry ??= new RelayCommand(obj =>
                {
                    DbContext.Countries.Add(SelectedCountry);
                    fetchDb();
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void fetchDb()
        {
            Countries = new ObservableCollection<Country>(DbContext.Countries.ToList());
        }

        public AdminViewModel() 
        {
            DbContext = new TourismDbContext();
            SelectedCountry = new Country();
            fetchDb();
        }

        ~AdminViewModel()
        {
            DbContext.Dispose();
        }
    }
}
