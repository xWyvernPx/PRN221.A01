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
        private readonly ICarService carService;
        public CarInformation carInformation;
        private bool isUpdate;
        public Action SaveSuccessCallback { get; set; }
        List<string> fuelTypes = new List<string>
            {
                "Gasoline",
                "Electricity"
            };
        public CarManipulateWindow(IManufacturerService manufacturerService, ISupplierService supplierService, ICarService carService)
        {
            InitializeComponent();
            this.manufacturerService = manufacturerService;
            this.supplierService = supplierService;
            manufacturers = manufacturerService.GetAll().ToList();
            suppliers = supplierService.GetAll().ToList();
            this.manufacturerCombobox.ItemsSource = manufacturers;
            this.supplierCombobox.ItemsSource = suppliers;
            fuelType.ItemsSource = fuelTypes;
            this.carService = carService;
            Loaded += (e,args) =>
            {
                isUpdate = carInformation is not null;
                if(carInformation is not null)
                {
                    carName.Text = carInformation.CarName;
                    carNoDoors.Text = carInformation.NumberOfDoors.ToString();
                    seatCapacity.Text = carInformation.SeatingCapacity.ToString();
                    fuelType.SelectedItem = carInformation.FuelType;
                    manufacturerCombobox.SelectedItem = manufacturers.Find(manu => manu.ManufacturerId == carInformation.ManufacturerId);
                    supplierCombobox.SelectedItem = suppliers.Find(manu => manu.SupplierId == carInformation.SupplierId);
                    carYear.Text = carInformation.Year.ToString();
                    carDesc.Text = carInformation.CarDescription;
                    pricePerDay.Text = carInformation.CarRentingPricePerDay.ToString();
                }
            };
        }

        private void CreateNewCar(CarInformation car)
        {
            carService.Create(car);
        }
        private void UpdateCar(CarInformation car)
        {
            carService.Update(car);
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
                manufacturerCombobox.SelectedItem.CheckObjectNotEmpty("Manufacturer");
                supplierCombobox.SelectedItem.CheckObjectNotEmpty("Supplier");
                carYear.Text.CheckYear("Published year");
                carDesc.Text.CheckNotEmpty("Description");
                pricePerDay.Text.CheckFloat("pricePerDay");
                var supplier = (Supplier)supplierCombobox.SelectedItem;
                var manufacturer = (Manufacturer)manufacturerCombobox.SelectedItem;
                const byte ACTIVE_STATUS = 1;
                CarInformation car = new()
                {
                    CarName = carName.Text,
                    CarDescription = carDesc.Text,
                    NumberOfDoors = int.Parse(carNoDoors.Text),
                    SeatingCapacity = int.Parse(seatCapacity.Text),
                    FuelType = fuelType.SelectedItem.ToString(),
                    Year = int.Parse(carYear.Text),
                    ManufacturerId = manufacturer.ManufacturerId,
                    SupplierId = supplier.SupplierId,
                    CarStatus = ACTIVE_STATUS,
                    CarRentingPricePerDay = decimal.Parse(pricePerDay.Text)
                };
                if (isUpdate) {
                    car.CarId = carInformation.CarId;
                    UpdateCar(car);
                }
                else
                {
                    CreateNewCar(car);
                }
                this.DialogResult = true;
                SaveSuccessCallback?.Invoke();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
    }

}
