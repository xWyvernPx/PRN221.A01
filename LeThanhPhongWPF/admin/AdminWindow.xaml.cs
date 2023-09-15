﻿using Business.Service.Interface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeThanhPhongWPF.admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly ICustomerService customerService;
        private readonly ICarService carService;
        public ObservableCollection<CarInformation> Cars  { get; set; }
    public AdminWindow(ICustomerService _customerServivce, ICarService carService)
        {
            InitializeComponent();
            this.customerService = _customerServivce;
            var list = this.customerService.GetCustomers();
            customerDatagrid.ItemsSource = list;
            this.carService = carService;
            DataContext = this;
            Cars = new(carService.GetAll());
            carDatagrid.ItemsSource = Cars;
        }
        
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
