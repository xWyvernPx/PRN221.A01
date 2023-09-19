using Business.Service.Impl;
using Business.Service.Interface;
using Domain.Models;
using Infra.Implement;
using Infra.Interface;
using LeThanhPhongWPF.admin;
using LeThanhPhongWPF.common;
using LeThanhPhongWPF.customer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Schema;

namespace LeThanhPhongWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;
        private IConfiguration _configuration;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();

            Console.WriteLine( _configuration.GetConnectionString("DefaultConnection"));
            //var mainWindow = serviceProvider.GetService<MainWindow>();
            //mainWindow.Tag = serviceProvider;
            //mainWindow?.Show();
            var adminWindow = serviceProvider.GetService<AdminWindow>();
            adminWindow.Tag = serviceProvider;
            adminWindow.Show();
            //var customerWindow = serviceProvider.GetService<CustomerWindow>();
            //customerWindow.Show();
            //var carMani = serviceProvider.GetService<CarManipulateWindow>();
            //carMani.Show();

        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
            services.AddScoped(typeof(ICarService), typeof(CarService));
            services.AddScoped(typeof(IRentingTransactionService), typeof(RentingTransactionService));
            services.AddDbContext<FucarRentingManagementContext>();
            services.AddScoped(typeof(IManufacturerService), typeof(ManufacturerService));
            services.AddScoped(typeof(ISupplierService), typeof(SupplierService));

            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


            services.AddTransient<MainWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<CustomerWindow>();
            services.AddTransient<CarManipulateWindow>();
            services.AddTransient<CustomerManipulateWindow>();


        }
    }
}
