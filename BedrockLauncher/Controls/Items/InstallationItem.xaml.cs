﻿
using BedrockLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BedrockLauncher.Methods;
using BedrockLauncher.Pages.Preview;
using BL_Core.Pages.Common;
using BL_Core.Classes;

namespace BedrockLauncher.Controls.Items
{
    /// <summary>
    /// Interaction logic for InstallationItem.xaml
    /// </summary>
    public partial class InstallationItem : UserControl
    {
        public InstallationItem()
        {
            InitializeComponent();
        }

        public Visibility ButtonPanelVisibility
        {
            get
            {
                return ButtonPanel != null ? ButtonPanel.Visibility : Visibility.Collapsed;
            }
            set
            {
                if (ButtonPanel != null) ButtonPanel.Visibility = value;
            }
        }

        public static readonly DependencyProperty ButtonPanelVisibilityProperty = DependencyProperty.Register("ButtonPanelVisibility", typeof(Visibility), typeof(InstallationItem), new PropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(ChangePanelVisibility)));

        private static void ChangePanelVisibility(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as InstallationItem).ButtonPanelVisibility = (Visibility)e.NewValue;
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var installation = button.DataContext as MCInstallation;
            ConfigManager.Default.GameManager.OpenFolder(installation);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var installation = button.DataContext as MCInstallation;
            ConfigManager.Default.GameManager.Play(installation);
        }

        private async void DeleteInstallationButton_Click(object sender, RoutedEventArgs e)
        {
            var title = this.FindResource("Dialog_DeleteItem_Title") as string;
            var content = this.FindResource("Dialog_DeleteItem_Text") as string;
            var item = this.FindResource("Dialog_Item_Installation_Text") as string;

            MenuItem button = sender as MenuItem;
            var installation = button.DataContext as MCInstallation;
            var result = await DialogPrompt.ShowDialog_YesNo(title, content, item, installation.DisplayName);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                ConfigManager.Default.DeleteInstallation(installation);
                ConfigManager.Default.OnConfigStateChanged(sender, Events.ConfigStateArgs.Empty);
            }
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var installation = button.DataContext as MCInstallation;
            (this.Tag as Pages.Play.InstallationsScreen).InstallationsList.SelectedItem = installation;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            button.ContextMenu.DataContext = installation;
            button.ContextMenu.IsOpen = true;
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            (this.Tag as Pages.Play.InstallationsScreen).InstallationsList.SelectedItem = null;
        }

        private void EditInstallationButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem button = sender as MenuItem;
            var installation = button.DataContext as MCInstallation;
            int index = ConfigManager.Default.CurrentInstallations.IndexOf(installation);
            ViewModels.LauncherModel.Default.SetOverlayFrame(new EditInstallationScreen(index, installation));
        }

        private void DuplicateInstallationButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem button = sender as MenuItem;
            var installation = button.DataContext as MCInstallation;
            ConfigManager.Default.DuplicateInstallation(installation);
            ConfigManager.Default.OnConfigStateChanged(sender, Events.ConfigStateArgs.Empty);
        }
    }
}
