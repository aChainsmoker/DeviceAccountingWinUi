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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeviceAccountingWinUi.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
    }
}
