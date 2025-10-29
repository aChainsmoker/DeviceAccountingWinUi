using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace DeviceAccountingWinUi.Views
{
    public sealed partial class RolesView : Page
    {
        private RolesViewModel _viewModel = App.Services.GetRequiredService<RolesViewModel>();

        public RolesView()
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
            if (AddRadio.IsChecked == true)
            {
                // В режиме добавления ID отключен (автоинкремент)
                IdBox.IsEnabled = false;
                NameBox.IsEnabled = true;
            }
            else if (EditRadio.IsChecked == true)
            {
                // В режиме редактирования ID включен для поиска
                IdBox.IsEnabled = true;
                NameBox.IsEnabled = true;
            }
            else if (DeleteRadio.IsChecked == true)
            {
                // В режиме удаления только ID активен
                IdBox.IsEnabled = true;
                NameBox.IsEnabled = false;
            }
        }

        private async void ApplyEdit_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)IdBox.Value;
            var name = NameBox.Text;

            var role = new Role
            {
                Id = id,
                Name = name
            };

            try
            {
                if (AddRadio.IsChecked == true)
                {
                    // Для добавления используем объект без ID (автоинкремент)
                    await _viewModel.AddRoleAsync(new Role { Name = name });
                }
                else if (EditRadio.IsChecked == true)
                {
                    await _viewModel.UpdateRoleAsync(role);
                }
                else if (DeleteRadio.IsChecked == true)
                {
                    await _viewModel.DeleteRoleAsync(id);
                }
            }
            catch (Exception ex)
            {
                // Лог или сообщение пользователю
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            // Очистка полей после операции
            IdBox.Value = 0;
            NameBox.Text = "";
        }

        // Обработчик события из BasicDataLayout
        private void BackButtonClicked(object sender, EventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}