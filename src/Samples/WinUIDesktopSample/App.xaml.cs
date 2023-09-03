using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.Web.Http;

namespace WinUIDesktopSample
{
    [GeneratedComInterface()]
    [Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface IDataTransferManagerInterop
    {
        IntPtr GetForWindow(IntPtr appWindow, IntPtr riid);
        void ShowShareUIForWindow(IntPtr appWindow);
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public App()
        {
        }

        Window myWindow;
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var value = DependencyProperty.UnsetValue;
            var button = new Button
            {
                Content = "Click me to load MainPage",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            button.Click += Button_Click;
            var window = new Microsoft.UI.Xaml.Window
            {
                Content = button
            };

            window.Activate();

            myWindow = window;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var interop = DataTransferManager.As<IDataTransferManagerInterop>();
            Guid id = new Guid(0xa5caee9b, 0x8708, 0x49d1, 0x8d, 0x36, 0x67, 0xd2, 0x5a, 0x8d, 0xa0, 0x0c);
            IntPtr result;
            byte[] array = id.ToByteArray();
            unsafe
            {
                fixed (void* ptr = array)
                {
                    result = interop.GetForWindow(WinRT.Interop.WindowNative.GetWindowHandle(myWindow), new nint(ptr));
                    dataTransferManager = WinRT.MarshalInterface<DataTransferManager>.FromAbi(result);
                   
                }
            }
           
            myWindow.Content = new MainPage();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            interop.ShowShareUIForWindow(WinRT.Interop.WindowNative.GetWindowHandle(myWindow));
        }

        DataTransferManager dataTransferManager;

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            WinRT.ComWrappersSupport.InitializeComWrappers();

            Microsoft.UI.Xaml.Application.Start((e) => new App());
        }
    }
}
