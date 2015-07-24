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
 */

using System.Windows;
using KaVE.RS.Commons;
using KaVEISettingsStore = KaVE.RS.Commons.Settings.ISettingsStore;


namespace KaVE.VS.FeedbackGenerator.UserControls.UploadWizard.UserProfileReminder
{
    public partial class UserProfileReminderDialog
    {
        private readonly ActionExecutor _actionExecutor;
        private readonly KaVEISettingsStore _settingsStore;

        public UserProfileReminderDialog(ActionExecutor actionExecutor, KaVEISettingsStore settingsStore)
        {
            _actionExecutor = actionExecutor;
            _settingsStore = settingsStore;
            InitializeComponent();
        }

        private void On_Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new UserProfileReminderWindow(_actionExecutor, _settingsStore).Show();
        }
    }
}
