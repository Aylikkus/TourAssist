using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourAssist.Model.Scaffold;
using TourAssist.View;
using TourAssist.View.Dialogs;

namespace TourAssist.ViewModel.Utility
{
    public static class PopupService
    {
        public static void OpenWindow(Type ofType)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType() == ofType) return;
            }
            if (ofType.IsSubclassOf(typeof(Window)))
            {
                Window newWindow = (Window)Activator.CreateInstance(ofType);
                newWindow.Show();
            }
        }

        public static void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public static T? GetDialogResult<T>(IDialogView<T> dialog)
            where T : class
        {
            if (dialog is not Window) return null;

            bool? result = ((Window)dialog).ShowDialog();
            if (result.GetValueOrDefault())
            {
                return dialog.Selected;
            }
            else
            {
                return null;
            }
        }

        public static Country? SelectCountry()
        {
            return GetDialogResult(new SelectCountryDialog());
        }

        public static Region? SelectRegion()
        {
            return GetDialogResult(new SelectRegionDialog());
        }

        public static Peculiarity? SelectPeculiarity()
        {
            return GetDialogResult(new SelectPeculiarityDialog());
        }

        public static Peculiarity? SelectPeculiarityFor(Country country)
        {
            return GetDialogResult(new SelectPeculiarityDialog(country));
        }

        public static Peculiarity? SelectPeculiarityFor(Region region)
        {
            return GetDialogResult(new SelectPeculiarityDialog(region));
        }

        public static Peculiarity? SelectPeculiarityFor(City city)
        {
            return GetDialogResult(new SelectPeculiarityDialog(city));
        }

        public static City? SelectCity()
        {
            return GetDialogResult(new SelectCityDialog());
        }

        public static Transport? SelectTransport()
        {
            return GetDialogResult(new SelectTransportDialog());
        }

    }
}
