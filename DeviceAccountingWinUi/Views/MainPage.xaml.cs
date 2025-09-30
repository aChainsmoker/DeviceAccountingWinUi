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
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeviceAccountingWinUi.Views;

public sealed partial class MainPage : Page
{
    private MainWindowViewModel _viewModel = App.Services.GetRequiredService<MainWindowViewModel>();
    public MainPage()
    {
        InitializeComponent();
        this.DataContext = _viewModel;
    }


    private void btn1_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(WorkersView));
    }

    private void btn0_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(DevicesView));
    }

    private void btn2_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(DepartmentsView));
    }

    private void btn3_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(DeviceTransfersView));
    }
    private void btn4_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(DepartmentDevicesView));
    }
    private void btn5_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(DepartmentRoomsView));
    }
    private void btn6_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(WorkerDevicesView));
    }
}
