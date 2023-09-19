﻿using Business.Service.Impl;
using Business.Service.Interface;
using Domain.Models;
using LeThanhPhongWPF.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LeThanhPhongWPF.common
{
    /// <summary>
    /// Interaction logic for CustomerManipulateWindow.xaml
    /// </summary>
    public partial class CustomerManipulateWindow : Window
    {
        public Customer CustomerInformation { get; set; }
        private bool isUpdate;
        private readonly ICustomerService customerService;
        public Action SaveSuccessCallback { get; set; }
        public CustomerManipulateWindow(ICustomerService customerService)
        {

            InitializeComponent();
            this.customerService = customerService;
            Loaded += (sender, e) =>
            {
                isUpdate = CustomerInformation is not null;
               
                if (CustomerInformation is not null)
                {
                    this.Title = "Update";
                    txtEmail.Text = CustomerInformation.Email;
                    txtName.Text = CustomerInformation.CustomerName;
                    txtPhone.Text = CustomerInformation.Telephone;
                    dob.SelectedDate = CustomerInformation.CustomerBirthday;
                    txtPassword.Password = CustomerInformation.Password;
                    //TODO set information
                }
                else
                {
                    this.Title = "New";

                }
            };

        }
        private List<string> getListAdminEmail()
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true).Build();
            string adminEmail = config.GetSection("Admin:Email").Value;
            return new List<string>()
            {
                adminEmail
            };
        }
        private void CreateNewCustomer(Customer customer)
        {
            var preExistUSer = customerService.GetCustomerByEmail(customer.Email);
            if (preExistUSer is not null)
            {
                Utils.ErrorAlert("This email has been used");
                return;
            }
            var id = customerService.getNextId();
            customerService.Create(customer);
        }
        private void UpdateCustomer(Customer customer)
        {
            var preExistUSer = customerService.GetCustomerByEmail(customer.Email);
            if (preExistUSer is null)
            {
                Utils.ErrorAlert("This user is not existed");
                return;
            }
            customerService.Update(customer);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = txtName.Text;
                var phone = txtPhone.Text;
                var email = txtEmail.Text;
                var birth = dob.SelectedDate;
                var password = txtPassword.Password;
                var repassword = txtRepassword.Password;
                const int ACTIVE_STATE = 1;
                name.CheckNotEmpty("Name");
                phone.CheckTelephone("Phone number");
                email.CheckEmail("Email");
                //TODO check valid date
                password.CheckPassword("Password");

                if (!password.Equals(repassword))
                {
                    Utils.ErrorAlert("Repassword must be same as password");
                    return;
                }

                var adminEmails = getListAdminEmail();
                if (adminEmails.Contains(email))
                {
                    Utils.ErrorAlert("This email can not be used");
                    return;
                }
                Customer customer = new()
                {
                    CustomerName = name,
                    CustomerBirthday = birth,
                    Email = email,
                    Password = password,
                    Telephone = phone,
                    CustomerStatus = ACTIVE_STATE
                };
                if (isUpdate)
                {
                    customer.CustomerId = CustomerInformation.CustomerId;
                    UpdateCustomer(customer);
                    CustomerInformation.CustomerName = customer.CustomerName;
                }
                else
                {
                    CreateNewCustomer(customer);
                }
                SaveSuccessCallback?.Invoke();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Utils.ErrorAlert(ex.Message);
            }
        }
    }
}
