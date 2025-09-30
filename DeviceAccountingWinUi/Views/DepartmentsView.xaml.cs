using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;


namespace DeviceAccountingWinUi.Views;

public sealed partial class DepartmentsView : Page
{
    private readonly DepartmentsViewModel _viewModel = App.Services.GetRequiredService<DepartmentsViewModel>();
    public DepartmentsView()
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
            // В режиме добавления Id отключен (автоопределяется)
            IdBox.IsEnabled = false;
            FullNameBox.IsEnabled = true;
            ShortNameBox.IsEnabled = true;
        }
        else if (EditRadio.IsChecked == true)
        {
            IdBox.IsEnabled = true;
            FullNameBox.IsEnabled = true;
            ShortNameBox.IsEnabled = true;
        }
        else if (DeleteRadio.IsChecked == true)
        {
            // В режиме удаления только Id активен
            IdBox.IsEnabled = true;
            FullNameBox.IsEnabled = false;
            ShortNameBox.IsEnabled = false;
        }
    }


    private async void ApplyEdit_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel == null) return;

        int id = 0;
        if (AddRadio.IsChecked != true)
            id = int.TryParse(IdBox.Text, out var l) ? l : throw new ArgumentException("couldnt parse id");
        string fullName = FullNameBox.Text;
        string shortName = ShortNameBox.Text;

        try
        {
            if (AddRadio.IsChecked == true)
            {
                await _viewModel.AddDepartmentAsync(new Department
                {
                    FullName = fullName,
                    ShortName = shortName
                });
            }
            else if (EditRadio.IsChecked == true)
            {
                await _viewModel.UpdateDepartmentAsync(new Department
                {
                    Id = id,
                    FullName = fullName,
                    ShortName = shortName
                });
            }
            else if (DeleteRadio.IsChecked == true)
            {
                await _viewModel.DeleteDepartmentAsync(id);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        // Очистка полей
        IdBox.Text = "";
        FullNameBox.Text = "";
        ShortNameBox.Text = "";
    }

}
