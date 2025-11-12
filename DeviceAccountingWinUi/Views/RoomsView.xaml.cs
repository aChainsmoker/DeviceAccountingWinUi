using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeviceAccountingWinUi.Views
{
    public sealed partial class RoomsView : Page
    {
        private RoomsViewModel _viewModel = App.Services.GetRequiredService<RoomsViewModel>();

        public RoomsView()
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
                // В режиме удаления оставляем активным только Room Number
                NumberBox.IsEnabled = true;
                AreaBox.IsEnabled = false;
                DepartmentIdBox.IsEnabled = false;
            }
            else
            {
                // В режиме добавления или изменения включаем все поля
                NumberBox.IsEnabled = true;
                AreaBox.IsEnabled = true;
                DepartmentIdBox.IsEnabled = true;
            }
        }

        private async void ApplyEdit_Click(object sender, RoutedEventArgs e)
        {
            var number = NumberBox.Text;
            var areaText = AreaBox.Text;
            var departmentIdText = DepartmentIdBox.Text;

            if (!decimal.TryParse(areaText, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.CurrentCulture, out var area) && DeleteRadio.IsChecked == false)
            {
                // Можно вывести сообщение об ошибке
                return;
            }

            int? departmentId = null;
            if (int.TryParse(departmentIdText, out var did))
                departmentId = did;

            var room = new Room
            {
                Number = number,
                Area = area,
                DepartmentId = departmentId ?? 0
            };

            try
            {
                if (AddRadio.IsChecked == true)
                {
                    await _viewModel.AddRoomAsync(room);
                }
                else if (EditRadio.IsChecked == true)
                {
                    await _viewModel.UpdateRoomAsync(room);
                }
                else if (DeleteRadio.IsChecked == true)
                {
                    await _viewModel.DeleteRoomAsync(number);
                }
            }
            catch (Exception ex)
            {
                // Лог или сообщение пользователю
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            // Очистка полей после операции
            NumberBox.Text = "";
            AreaBox.Text = "";
            DepartmentIdBox.Text = "";
        }

        // Обработчик события из BasicDataLayout
        private void BackButtonClicked(object sender, EventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}