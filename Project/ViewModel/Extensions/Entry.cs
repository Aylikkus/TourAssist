using MySqlConnector.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.ViewModel.Utility;

namespace TourAssist.Model.Scaffold
{
    public partial class Entry : INotifyPropertyChanged
    {
        private void updateRating()
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                dbContext.Update(this);
                dbContext.SaveChanges();
            }

            OnPropertyChanged(nameof(IsOneStar));
            OnPropertyChanged(nameof(IsTwoStar));
            OnPropertyChanged(nameof(IsThreeStar));
            OnPropertyChanged(nameof(IsFourStar));
            OnPropertyChanged(nameof(IsFiveStar));
        }

        private void setRating(int rating, bool toggle)
        {
            bool isTheSameValue = toggle == (Rating >= rating);
            if (isTheSameValue)
                return;

            if (toggle)
            {
                Rating = rating;
            }
            else
            {
                Rating = rating - 1;
            }

            updateRating();
        }

        [NotMapped]
        public bool IsOneStar
        {
            get
            {
                return Rating >= 1;
            }
            set
            {
                setRating(1, value);
            }
        }

        [NotMapped]
        public bool IsTwoStar
        {
            get
            {
                return Rating >= 2;
            }
            set
            {
                setRating(2, value);
            }
        }

        [NotMapped]
        public bool IsThreeStar
        {
            get
            {
                return Rating >= 3;
            }
            set
            {
                setRating(3, value);
            }
        }

        [NotMapped]
        public bool IsFourStar
        {
            get
            {
                return Rating >= 4;
            }
            set
            {
                setRating(4, value);
            }
        }

        [NotMapped]
        public bool IsFiveStar
        {
            get
            {
                return Rating >= 5;
            }
            set
            {
                setRating(5, value);
            }
        }

        [NotMapped]
        public RouteCitiesView RouteCitiesView
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    return dbContext.RouteCitiesViews.Where((r) => r.IdRoute == RouteIdRoute).First();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
