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

        private void doContextAction(Action action)
        {
            try
            {
                using (TourismDbContext dbContext = new TourismDbContext()) 
                {
                    action();
                    dbContext.SaveChanges();

                    // Обновление списков
                    Countries = new ObservableCollection<Country>(dbContext.Countries.ToList());
                }
            }
            catch (Exception ex)
            {
                PopupService.ShowMessage("Произошла ошибка при изменении элементов." +
                    "Убедитесь, что все поля проставлены в правильном формате." +
                    "Ошибка: " + ex.Message);
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
                    
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public AdminViewModel() 
        {
            SelectedCountry = new Country();
        }
    }
}
