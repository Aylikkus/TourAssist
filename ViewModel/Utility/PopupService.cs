using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourAssist.Model.Scaffold;
using TourAssist.View;

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

        public static Country? SelectCountry()
        {
            SelectCountryDialog dialog = new SelectCountryDialog();
            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                return dialog.Selected;
            }
            else
            {
                return null;
            }
        }
    }
}
