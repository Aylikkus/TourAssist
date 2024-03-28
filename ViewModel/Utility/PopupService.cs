using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    }
}
