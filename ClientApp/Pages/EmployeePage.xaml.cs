﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : DockPanel, IPage
    {
        public EmployeeDirectory.Models.Employee AssociatedEmployee { get; }
        public IEnumerable<EmployeeDirectory.Models.Phone> Phones {
            get;
            private set;
        }

        public EmployeePage(EmployeeDirectory.Models.Employee employee)
        {
            InitializeComponent();

            AssociatedEmployee = employee;

            nameText.Text = $"{employee.FirstName} {employee.SecondName ?? ""} {employee.MiddleName ?? ""}".Replace("  ", " ");
            loginText.Text = employee.Login;
            if (employee.BirthDay != null) birthdayText.Text = employee.BirthDay?.GetDateTimeFormats('D').First();

            Phones = UpdatePhones();
            RedrawPhones();
        }

        private void changeEmployee_Click(object sender, RoutedEventArgs e)
        {
            Elements.Tab tab = MainWindow.Current.FindTab((Elements.Tab tab) =>
            {
                ChangeEmployeePage page = tab.AssociatedPage as ChangeEmployeePage;
                if (page == null)
                    return false;
                if (page.AssociatedEmployee.Id == AssociatedEmployee.Id)
                    return true;
                return false;
            });

            if (tab != null)
            {
                MainWindow.Current.ActiveTab = tab;
            }
            else
            {
                ChangeEmployeePage page = new ChangeEmployeePage(AssociatedEmployee);
                string tabName = $"{Properties.Resources.changeEmployeeTab} {AssociatedEmployee.Login}";
                tab = new Elements.Tab(MainWindow.Current.GiveFreeTabName(tabName), true, page);

                MainWindow.Current.AddTab(page, tab);
            }

        }

        private void addPhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            AddPhonePage page = new AddPhonePage(AssociatedEmployee.Id);
            string tabName = $"{AssociatedEmployee.Login} - {Properties.Resources.newPhoneTab}";
            Elements.Tab tab = new Elements.Tab(MainWindow.Current.GiveFreeTabName(tabName), true, page);

            MainWindow.Current.AddTab(page, tab);
        }

        private void deleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Windows.ConfirmDialog dialog = new Windows.ConfirmDialog(Properties.Resources.deleteEmployeeConfirmMessage);
            if (dialog.ShowDialog() == true)
            {
                EmployeeDirectory.Infrastructure.ResultCode resultCode;
                MainWindow.Current.DataAccessor.RemoveUser(AssociatedEmployee.Id, out resultCode);

                // Add resultCode handler!

                MainWindow.Current.CloseTab(this);
            }
        }

        public IEnumerable<EmployeeDirectory.Models.Phone> UpdatePhones()
        {
            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            return MainWindow.Current.DataAccessor.GetPhonesById(AssociatedEmployee.Id, out resultCode);

            // Add resultCode handler!
        }

        public void RedrawPhones()
        {
            phonesList.Items.Clear();

            foreach (var phone in Phones)
            {
                Elements.PhoneField phoneField = new Elements.PhoneField(phone);
                phonesList.Items.Add(phoneField);
            }

            if (Phones.Any())
            {
                phonesListText.Text = Properties.Resources.employeePhonesList;
                phonesListText.Visibility = Visibility.Visible;
                phonesList.Visibility = Visibility.Visible;

            }
            else
            {
                phonesListText.Text = Properties.Resources.emptyEmployeePhonesList;
                phonesListText.Visibility = Visibility.Collapsed;
                phonesList.Visibility = Visibility.Collapsed;
            }
        }

        public void Update()
        {
            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            EmployeeDirectory.Models.Employee employee = MainWindow.Current.DataAccessor.GetEmployeeById(AssociatedEmployee.Id, out resultCode);

            // Add resultCode handler!

            nameText.Text = $"{employee.FirstName} {employee.SecondName ?? ""} {employee.MiddleName ?? ""}".Replace("  ", " ");
            loginText.Text = employee.Login;
            if (employee.BirthDay != null) birthdayText.Text = employee.BirthDay?.GetDateTimeFormats('D').First();

            Phones = UpdatePhones();
            RedrawPhones();
            
        }
    }
}
