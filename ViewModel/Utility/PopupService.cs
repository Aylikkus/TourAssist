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

        internal static Region? SelectRegion()
        {
            return GetDialogResult(new SelectRegionDialog());
        }

        internal static Peculiarity? SelectPeculiarity()
        {
            return GetDialogResult(new SelectPeculiarityDialog());
        }

        internal static Peculiarity? SelectPeculiarityFor(Country country)
        {
            return GetDialogResult(new SelectPeculiarityDialog(country));
        }

        internal static Peculiarity? SelectPeculiarityFor(Region region)
        {
            return GetDialogResult(new SelectPeculiarityDialog(region));
        }

        internal static Peculiarity? SelectPeculiarityFor(City city)
        {
            return GetDialogResult(new SelectPeculiarityDialog(city));
        }

        internal static Transport? SelectTransport()
        {
            throw new NotImplementedException();
        }

        internal static City? SelectCity()
        {
            throw new NotImplementedException();
        }
    }
}
