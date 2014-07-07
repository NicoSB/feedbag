﻿/*
 * Copyright 2014 Technische Universität Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Contributors:
 *    - Uli Fahrer
 *    - Sven Amann
 */

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JetBrains.ActionManagement;
using KaVE.VsFeedbackGenerator.Interactivity;
using KaVE.VsFeedbackGenerator.TrayNotification;
using KaVE.VsFeedbackGenerator.Utils;

namespace KaVE.VsFeedbackGenerator.SessionManager.Presentation
{
    /// <summary>
    ///     Interaktionslogik für SessionManagerControl.xaml
    /// </summary>
    public partial class SessionManagerControl
    {
        private readonly FeedbackViewModel _feedbackViewModel;
        private readonly IActionManager _actionManager;
        private readonly ISettingsStore _settingsStore;

        public SessionManagerControl(FeedbackViewModel feedbackViewModel,
            IActionManager actionManager,
            ISettingsStore settingsStore)
        {
            _feedbackViewModel = feedbackViewModel;
            _actionManager = actionManager;
            _settingsStore = settingsStore;
            DataContext = feedbackViewModel;

            _feedbackViewModel.ConfirmationRequest.Raised += new ConfirmationRequestHandler(this).Handle;
            _feedbackViewModel.NotificationRequest.Raised += (sender, args) => ShowNotification(args.Notification);
            _feedbackViewModel.UploadOptionsRequest.Raised += UploadOptionsRequestOnRaised;

            InitializeComponent();
        }

        private static void ShowNotification(Notification notification)
        {
            MessageBox.Show(notification.Message, notification.Caption, MessageBoxButton.OK);
        }

        private void UploadOptionsRequestOnRaised(object sender,
            InteractionRequestedEventArgs<UploadWizard.UploadOptions> args)
        {
            var uploadWizard = new UploadWizard(_actionManager, _settingsStore);
            uploadWizard.ShowDialog();
            args.Notification.Type = uploadWizard.ResultType;
            args.Callback();
        }

        private void FeedbackWindowControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        ///     Makes the overflow dropdown button invisible.
        /// </summary>
        private void ToolBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;
            if (toolBar == null)
            {
                return;
            }
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Unfortunately, SelectedItems is not a bidirectional control property, hence, we cannot bind it to a
        ///     property of out view model to access the set of selected items. Therefore, we react on selection changes
        ///     here and update our model property manually.
        /// </summary>
        private void SessionListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _feedbackViewModel.SelectedSessions = SessionListView.SelectedItems.Cast<SessionViewModel>();
        }

        /// <summary>
        ///     Unfortunately, SelectedItems is not a bidirectional control property, hence, we cannot bind it to a
        ///     property of out view model to access the set of selected items. Therefore, we react on selection changes
        ///     here and update our model property manually.
        /// </summary>
        private void EventListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_feedbackViewModel.SingleSelectedSession != null)
            {
                _feedbackViewModel.SingleSelectedSession.SelectedEvents =
                    EventListView.SelectedItems.Cast<EventViewModel>();
            }
        }

        private void SortableListViewColumnHeaderClicked(object sender, RoutedEventArgs e)
        {
            ((SortableListView) sender).GridViewColumnHeaderClicked(e.OriginalSource as GridViewColumnHeader);
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshControl();
        }

        private void VisitUploadPageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var settingsStore = Registry.GetComponent<ISettingsStore>();
            var export = settingsStore.GetSettings<ExportSettings>();

            var idx = export.UploadUrl.LastIndexOf('/');
            var url = export.UploadUrl.Substring(0, idx);

            System.Diagnostics.Process.Start(url);
        }

        private void VisitHomepageButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                _settingsStore.GetSettings<ExportSettings>().WebAccessPraefix + "http://kave.cc");
        }

        private void OpenOptionPage_OnClick(object sender, RoutedEventArgs e)
        {
            _actionManager.ExecuteActionGuarded("ShowOptions", "AgentAction");
        }

        private void RefreshControl()
        {
            _feedbackViewModel.Refresh();
        }
    }
}