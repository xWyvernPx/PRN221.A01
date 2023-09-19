using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeThanhPhongWPF.Extensions
{
    public static class Utils
    {
        public static MessageBoxResult ConfirmationBox(string msg)
        {
            return MessageBox.Show(msg, "Confirm", MessageBoxButton.YesNo);
        }
        public static MessageBoxResult ErrorAlert(string msg)
        {
            return MessageBox.Show(msg, "Error", MessageBoxButton.OK,MessageBoxImage.None);
        }
    }
}
