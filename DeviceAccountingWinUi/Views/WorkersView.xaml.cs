using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeviceAccountingWinUi.Views;


public sealed partial class WorkersView : Page
{
    private readonly WorkersViewModel _viewModel = App.Services.GetRequiredService<WorkersViewModel>();
    public WorkersView()
    {
        InitializeComponent();
        this.DataContext = _viewModel;
    }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Escape)
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
        if(AddRadio.IsChecked == true)
        {
            IdBox.IsEnabled = false;
            NameBox.IsEnabled = true;
            RoleIdBox.IsEnabled = true;
            DepartmentIdBox.IsEnabled = true;
        }
        else if (EditRadio.IsChecked == true)
        {
            IdBox.IsEnabled = true;
            NameBox.IsEnabled = true;
            RoleIdBox.IsEnabled = true;
            DepartmentIdBox.IsEnabled = true;
        }
        else if (DeleteRadio.IsChecked == true)
        {
            IdBox.IsEnabled = true;
            NameBox.IsEnabled = false;
            RoleIdBox.IsEnabled = false;
            DepartmentIdBox.IsEnabled = false;
        }
    }

    private async void ApplyEdit_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel == null) return;
        int id = 0;
        if(AddRadio.IsChecked != true)
            id = int.TryParse(IdBox.Text, out var l) ? l : throw new ArgumentException("couldnt parse id");
        string name = NameBox.Text;
        int? roleId = int.TryParse(RoleIdBox.Text, out var r) ? r : null;
        int? departmentId = int.TryParse(DepartmentIdBox.Text, out var d) ? d : null;

        try
        {
            if (AddRadio.IsChecked == true)
            {
                await _viewModel.AddWorkerAsync(new Worker
                {
                    Name = name,
                    RoleId = roleId,
                    DepartmentId = departmentId
                });
            }
            else if (EditRadio.IsChecked == true)
            {
                await _viewModel.UpdateWorkerAsync(new Worker
                {
                    Id = id,
                    Name = name,
                    RoleId = roleId,
                    DepartmentId = departmentId
                });
            }
            else if (DeleteRadio.IsChecked == true)
            {
                await _viewModel.DeleteWorkerAsync(id);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        // Очистка полей
        IdBox.Text = "";
        NameBox.Text = "";
        RoleIdBox.Text = "";
        DepartmentIdBox.Text = "";
    }

}
