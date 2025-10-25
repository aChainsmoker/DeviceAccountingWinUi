using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DeviceAccountingWinUi.Views;

public sealed partial class DeviceTransfersView : Page
{
    private DeviceTransfersViewModel _viewModel = App.Services.GetRequiredService<DeviceTransfersViewModel>();

    public DeviceTransfersView()
    {
        InitializeComponent();
        this.DataContext = _viewModel;
    }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Escape)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
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
            IdBox.IsEnabled = false; // Id генерируется автоматически
            DeviceIdBox.IsEnabled = true;
            DateOfTransferBox.IsEnabled = true;
            FromWorkerIdBox.IsEnabled = true;
            ToWorkerIdBox.IsEnabled = true;
        }
        else if (EditRadio.IsChecked == true)
        {
            IdBox.IsEnabled = true;
            DeviceIdBox.IsEnabled = true;
            DateOfTransferBox.IsEnabled = true;
            FromWorkerIdBox.IsEnabled = true;
            ToWorkerIdBox.IsEnabled = true;
        }
        else if (DeleteRadio.IsChecked == true)
        {
            IdBox.IsEnabled = true;
            DeviceIdBox.IsEnabled = false;
            DateOfTransferBox.IsEnabled = false;
            FromWorkerIdBox.IsEnabled = false;
            ToWorkerIdBox.IsEnabled = false;
        }
    }

    private async void ApplyEdit_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel == null) return;

        int id = 0;
        if (AddRadio.IsChecked == false)
            id = int.TryParse(IdBox.Text, out var l) ? l : throw new ArgumentException("couldnt parse id");

        string deviceId = DeviceIdBox.Text;
        DateOnly date = DateOnly.FromDateTime(DateOfTransferBox.Date.Date);

        int fromWorker = 0;
        int toWorker = 0;
        if (DeleteRadio.IsChecked == false)
        {
            fromWorker = int.TryParse(FromWorkerIdBox.Text, out var l) ? l : throw new ArgumentException("couldnt parse id");
            toWorker = int.TryParse(ToWorkerIdBox.Text, out var d) ? d : throw new ArgumentException("couldnt parse id");
        }

        try
        {
            if (AddRadio.IsChecked == true)
            {
                await _viewModel.AddDeviceTransferAsync(new DeviceTransfer
                {
                    DeviceId = deviceId,
                    DateOfTransfer = date,
                    FromWorkerId = fromWorker,
                    ToWorkerId = toWorker
                });
            }
            else if (EditRadio.IsChecked == true)
            {
                await _viewModel.UpdateDeviceTransferAsync(new DeviceTransfer
                {
                    Id = id,
                    DeviceId = deviceId,
                    DateOfTransfer = date,
                    FromWorkerId = fromWorker,
                    ToWorkerId = toWorker
                });
            }
            else if (DeleteRadio.IsChecked == true)
            {
                await _viewModel.DeleteDeviceTransferAsync(id);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        // Очистка полей
        IdBox.Text = "";
        DeviceIdBox.Text = "";
        FromWorkerIdBox.Text = "";
        ToWorkerIdBox.Text = "";
    }

    // Обработчик события кнопки "Назад" из BasicDataLayout
    private void MainLayout_BackButtonClicked(object sender, EventArgs e)
    {
        if (Frame.CanGoBack)
            Frame.GoBack();
    }
}