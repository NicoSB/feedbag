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
 *    - Sebastian Proksch
 */

using System;
using JetBrains.Application.Settings;

namespace KaVE.VsFeedbackGenerator.SessionManager.Presentation
{
    [SettingsKey(typeof (FeedbackSettings), "Kave Feedback-Export Settings")]
    // WARNING: Do not change classname, as it is used to identify settings
    internal class ExportSettings
    {
        private const string DateTimeMinValue = "0001-01-01T00:00:00";

        [SettingsEntry(false, "KaVE FeedbackGeneration RemoveCodeNames")]
        public bool RemoveCodeNames;

        [SettingsEntry(false, "KaVE FeedbackGenerator RemoveDurations")]
        public bool RemoveDurations;

        [SettingsEntry(false, "KaVE FeedbackGenerator RemoveStartTimes")]
        public bool RemoveStartTimes;

        [SettingsEntry(false, "KaVE FeedbackGenerator RemoveSessionIDs")]
        public bool RemoveSessionIDs;

        [SettingsEntry("http://kave.st.informatik.tu-darmstadt.de:667/", "KaVE FeedbackGenerator UploadUrl")]
        public string UploadUrl;

        [SettingsEntry("", "KaVE FeedbackGenerator WebAccessPrefix")]
        public string WebAccessPrefix;

        /// <summary>
        /// Internal use only! Use LastReviewDate instead.
        /// </summary>
        [SettingsEntry(DateTimeMinValue, "Internal representation of the review date. Do not use this directly.")]
        public DateTime LastReviewDateInternal;

        /// <summary>
        /// The last-review date or null, if the session manager (review tool) is currently closed.
        /// </summary>
        public DateTime? LastReviewDate
        {
            get { return LastReviewDateInternal == default(DateTime) ? (DateTime?) null : LastReviewDateInternal; }
            set { LastReviewDateInternal = value.GetValueOrDefault(); }
        }
    }
}