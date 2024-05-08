using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TourAssist.View.Dialogs
{
    public interface IDialogView<T> where T : class
    {
        T? Selected { get; }
    }
}
