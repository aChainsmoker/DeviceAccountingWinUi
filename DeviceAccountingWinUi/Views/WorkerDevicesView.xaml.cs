using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeviceAccountingWinUi.Views
{
    public sealed partial class WorkerDevicesView : Page
    {
        private WorkerDevicesViewModel _viewModel;

        public WorkerDevicesView()
        {
            this.InitializeComponent();
            _viewModel = App.Services.GetRequiredService<WorkerDevicesViewModel>();
            this.DataContext = _viewModel;
        }

        private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape && Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(WorkerIdBox.Text, out int workerId))
            {
                await _viewModel.LoadWorkerDevicesAsync(workerId);
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