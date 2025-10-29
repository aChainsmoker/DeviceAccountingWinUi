using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeviceAccountingWinUi.Views
{
    public sealed partial class DepartmentDevicesView : Page
    {
        private DepartmentDevicesViewModel _viewModel;

        public DepartmentDevicesView()
        {
            this.InitializeComponent();
            _viewModel = App.Services.GetRequiredService<DepartmentDevicesViewModel>();
            this.DataContext = _viewModel;
        }

        private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape && Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(DepartmentIdBox.Text, out int deptId))
            {
                await _viewModel.LoadDepartmentDevicesAsync(deptId);
            }
        }

        // Обработчик события кнопки "Назад" из BasicDataLayout
        private void MainLayout_BackButtonClicked(object sender, EventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}