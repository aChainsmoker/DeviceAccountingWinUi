using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace DeviceAccountingWinUi.Views
{
    public sealed partial class DevicesView : Page
    {
        private DevicesViewModel _viewModel = App.Services.GetRequiredService<DevicesViewModel>();

        public DevicesView()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = EditPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void ModeRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (DeleteRadio.IsChecked == true)
            {
                // В режиме удаления оставляем активным только InventoryId
                InventoryIdBox.IsEnabled = true;
                NameBox.IsEnabled = false;
                ModelBox.IsEnabled = false;
                DateOfObtainingBox.IsEnabled = false;
                CostBox.IsEnabled = false;
                WorkerIdBox.IsEnabled = false;
            }
            else
            {
                // В режиме добавления или изменения включаем все поля
                InventoryIdBox.IsEnabled = true;
                NameBox.IsEnabled = true;
                ModelBox.IsEnabled = true;
                DateOfObtainingBox.IsEnabled = true;
                CostBox.IsEnabled = true;
                WorkerIdBox.IsEnabled = true;
            }
        }

        private async void ApplyEdit_Click(object sender, RoutedEventArgs e)
        {
            var inventoryId = InventoryIdBox.Text;
            var name = NameBox.Text;
            var model = ModelBox.Text;
            var date = DateOfObtainingBox.Date;
            var costText = CostBox.Text;
            var workerIdText = WorkerIdBox.Text;

            if (!decimal.TryParse(costText, out var cost) && DeleteRadio.IsChecked == false)
            {
                // Можно вывести сообщение об ошибке
                return;
            }

            int? workerId = null;
            if (int.TryParse(workerIdText, out var wid))
                workerId = wid;

            var device = new Device
            {
                InventoryId = inventoryId,
                Name = name,
                Model = model,
                Cost = cost
            };

            try
            {
                if (AddRadio.IsChecked == true)
                {
                    await _viewModel.AddDeviceAsync(device, DateOnly.FromDateTime(date.Date), workerId);
                }
                else if (EditRadio.IsChecked == true)
                {
                    await _viewModel.UpdateDeviceAsync(device, DateOnly.FromDateTime(date.Date), workerId);
                }
                else if (DeleteRadio.IsChecked == true)
                {
                    await _viewModel.DeleteDeviceAsync(inventoryId);
                }
            }
            catch (Exception ex)
            {
                // Лог или сообщение пользователю
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            // Очистка полей после операции
            InventoryIdBox.Text = "";
            NameBox.Text = "";
            ModelBox.Text = "";
            CostBox.Text = "";
            WorkerIdBox.Text = "";
        }

        // Обработчик события из BasicDataLayout
        private void BackButtonClicked(object sender, EventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}