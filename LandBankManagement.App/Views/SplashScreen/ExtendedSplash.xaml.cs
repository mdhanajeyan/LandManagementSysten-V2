﻿using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.ApplicationModel.Activation;
using LandBankManagement.Services;
using LandBankManagement.ViewModels;
using System.Threading.Tasks;
using Windows.System;
using LandBankManagement.Models;

namespace LandBankManagement.Views.SplashScreen
{

    public sealed partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private Windows.ApplicationModel.Activation.SplashScreen splashScreen; // Variable to hold the splash screen object.
        private Frame rootFrame;
        private IActivatedEventArgs activatedEventArgs;

        public ExtendedSplash(IActivatedEventArgs e, bool loadState)
        {
            this.InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            splashScreen = e.SplashScreen;
            this.activatedEventArgs = e;

            if (splashScreen != null)
            {
                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splashScreen.ImageLocation;
            }

            Resize();
            rootFrame = new Frame();
            LoadDataAsync(this.activatedEventArgs);
        }

        private async void LoadDataAsync(IActivatedEventArgs e)
        {
            var activationInfo = ActivationService.GetActivationInfo(e);

            await Startup.ConfigureAsync();

            var shellArgs = new ShellArgs
            {
                ViewModel = activationInfo.EntryViewModel,
                Parameter = activationInfo.EntryArgs,
                UserInfo = await TryGetUserInfoAsync(e as IActivatedEventArgsWithUser)
            };


            rootFrame.Navigate(typeof(LoginView), shellArgs);



            Window.Current.Content = rootFrame;

            Window.Current.Activate();

        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        private void Resize()
        {
            if (this.splashScreen == null) return;

            // The splash image's not always perfectly centered. Therefore we need to set our image's position 
            // to match the original one to obtain a clean transition between both splash screens.

            this.splashImage.Height = this.splashScreen.ImageLocation.Height;
            this.splashImage.Width = this.splashScreen.ImageLocation.Width;

            this.splashImage.SetValue(Canvas.TopProperty, this.splashScreen.ImageLocation.Top);
            this.splashImage.SetValue(Canvas.LeftProperty, this.splashScreen.ImageLocation.Left);

            this.progressRing.SetValue(Canvas.TopProperty, this.splashScreen.ImageLocation.Top + this.splashScreen.ImageLocation.Height + 50);
            this.progressRing.SetValue(Canvas.LeftProperty, this.splashScreen.ImageLocation.Left + this.splashScreen.ImageLocation.Width / 2 - this.progressRing.Width / 2);
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splashScreen != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splashScreen.ImageLocation;
                Resize();
            }
        }

        private async Task<UserInfoModel> TryGetUserInfoAsync(IActivatedEventArgsWithUser argsWithUser)
        {
            if (argsWithUser != null)
            {
                var user = argsWithUser.User;
                var userInfo = new UserInfoModel
                {
                    UserName = await user.GetPropertyAsync(KnownUserProperties.AccountName) as String,
                    loginName = await user.GetPropertyAsync(KnownUserProperties.FirstName) as String

                };
                if (!userInfo.IsEmpty)
                {
                    return userInfo;
                }
            }
            return null;
        }
    }
}
