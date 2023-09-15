using Business.Service.Impl;
using Business.Service.Interface;
using Domain.Models;
using LeThanhPhongWPF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeThanhPhongWPF.common
{
    /// <summary>
    /// Interaction logic for CarManipulateWindow.xaml
    /// </summary>
    public partial class CarManipulateWindow : Window
    {
        private List<Manufacturer> manufacturers;
        private List<Supplier> suppliers;
        private readonly IManufacturerService manufacturerService;
        private readonly ISupplierService supplierService;
        public Action SaveSuccessCallback;
        List<string> fuelTypes = new List<string>
            {
                "Gasoline",
                "Electricity"
            };
        public CarManipulateWindow(IManufacturerService manufacturerService, ISupplierService supplierService)
        {
            InitializeComponent();
            this.manufacturerService = manufacturerService;
            this.supplierService = supplierService;
            manufacturers = manufacturerService.GetAll().ToList();
            suppliers = supplierService.GetAll().ToList();
            this.manufacturerCombobox.ItemsSource = manufacturers;
            this.supplierCombobox.ItemsSource = suppliers;
            fuelType.ItemsSource = fuelTypes;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const int MIN_DOORS_OF_CAR = 1;
            const int MAX_DOORS_OF_CAR = 10;
            const int MIN_SEATS_OF_CAR = 1;
            const int MAX_SEATS_OF_CAR = 200;
            try
            {   

                carName.Text.CheckNotEmpty("Name of car");
                carNoDoors.Text.CheckIntInRange("Number doors of car", MIN_DOORS_OF_CAR, MAX_DOORS_OF_CAR);
                seatCapacity.Text.CheckIntInRange("Number of seats", MIN_SEATS_OF_CAR, MAX_SEATS_OF_CAR);
                fuelType.SelectedItem.CheckObjectNotEmpty("Fuel Type");
                manufacturerCombobox.CheckObjectNotEmpty("Manufacturer");
                supplierCombobox.CheckObjectNotEmpty("Supplier");
                carYear.Text.CheckYear("Published year");
                carDesc.Document.CheckObjectNotEmpty("Description");
                string carDescRaw = XamlWriter.Save(carDesc.Document);
                pricePerDay.Text.CheckFloat("pricePerDay");
                SaveSuccessCallback?.Invoke();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
    }

}
