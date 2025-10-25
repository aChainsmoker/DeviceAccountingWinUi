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

namespace DeviceAccountingWinUi.UserControls
{
    public sealed partial class BasicDataLayout : UserControl
    {
        public BasicDataLayout()
        {
            this.InitializeComponent();
        }

        // Содержимое центральной колонки
        public object CenterContent
        {
            get => GetValue(CenterContentProperty);
            set => SetValue(CenterContentProperty, value);
        }

        public static readonly DependencyProperty CenterContentProperty =
            DependencyProperty.Register(nameof(CenterContent), typeof(object), typeof(BasicDataLayout), new PropertyMetadata(null));

        // Содержимое правой колонки
        public object RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(BasicDataLayout), new PropertyMetadata(null));

        // Событие кнопки "Назад"
        public event EventHandler BackButtonClicked;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}