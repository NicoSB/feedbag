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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.ProjectModel;
using KaVE.Commons.Model.Events.VersionControlEvents;
using KaVE.Commons.Model.Names.VisualStudio;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;
using KaVE.JetBrains.Annotations;
using KaVE.VS.FeedbackGenerator.MessageBus;

namespace KaVE.VS.FeedbackGenerator.Generators.Git
{
    [SolutionComponent]
    internal class GitEventGenerator : EventGeneratorBase
    {
        public GitEventGenerator([NotNull] IRSEnv env, [NotNull] IMessageBus messageBus, [NotNull] IDateUtils dateUtils)
            : base(env, messageBus, dateUtils) {}

        public void OnGitHistoryFileChanged(object sender, GitHistoryFileChangedEventArgs args)
        {
            var eventContent = ReadGitActionsFrom(ReadLogContent(args.FullPath));
            Fire(eventContent, SolutionName.Get(args.Solution.Name));
        }

        private void Fire(IKaVEList<VersionControlAction> content, SolutionName solutionName)
        {
            var gitEvent = Create<VersionControlEvent>();
            gitEvent.Solution = solutionName;
            gitEvent.Content = content;
            FireNow(gitEvent);
        }

        [Pure]
        protected virtual string[] ReadLogContent(string fullPath)
        {
            return File.ReadAllLines(fullPath);
        }

        private static IKaVEList<VersionControlAction> ReadGitActionsFrom(IEnumerable<string> logContent)
        {
            var gitActions = Lists.NewList<VersionControlAction>();

            foreach (
                var gitAction in
                    logContent.Select(
                        logEntry =>
                            new VersionControlAction
                            {
                                ExecutedAt = ExtractExecutedAtFrom(logEntry),
                                ActionType = ExtractActionTypeFrom(logEntry)
                            }))
            {
                gitActions.Add(gitAction);
            }

            return gitActions;
        }

        private static VersionControlActionType ExtractActionTypeFrom([NotNull] string entry)
        {
            return new Regex("\t.*:").Match(entry).Value.TrimEnd(':').ToVersionControlActionType();
        }

        private static DateTime? ExtractExecutedAtFrom([NotNull] string entry)
        {
            // Unix timestamp is seconds since 1970-01-01T00:00:00Z
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStamp = entry.Split(' ')[4];
            dateTime = dateTime.AddSeconds(int.Parse(unixTimeStamp)).ToLocalTime();
            return dateTime;
        }
    }
}