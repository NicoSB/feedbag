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
 *    - Sven Amann
 *    - Mattis Manfred Kämmerer
 *    - Markus Zimmermann
 */

using System;
using System.Timers;
using EnvDTE;
using JetBrains.Application;
using JetBrains.Application.Components;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Utils;
using KaVE.VS.FeedbackGenerator.Generators.VisualStudio.EditEventGenerators.EventContext;
using KaVE.VS.FeedbackGenerator.MessageBus;

namespace KaVE.VS.FeedbackGenerator.Generators.VisualStudio.EditEventGenerators
{
    [ShellComponent(ProgramConfigurations.VS_ADDIN)]
    internal class EditEventGenerator : EventGeneratorBase, IDisposable
    {
        // TODO evaluate good threshold value
        private const int InactivityPeriodToCompleteEditAction = 2000;

        private readonly IContextProvider _contextProvider;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly TextEditorEvents _textEditorEvents;
        private EditEvent _currentEditEvent;

        private readonly Timer _eventSendingTimer = new Timer(InactivityPeriodToCompleteEditAction);
        private readonly object _eventLock = new object();
        private TextPoint _currentStartPoint;

        public EditEventGenerator(IRSEnv env,
            IMessageBus messageBus,
            IDateUtils dateUtils,
            IContextProvider contextProvider)
            : base(env, messageBus, dateUtils)
        {
            _contextProvider = contextProvider;
            _textEditorEvents = DTE.Events.TextEditorEvents;
            _textEditorEvents.LineChanged += TextEditorEvents_LineChanged;
            _eventSendingTimer.Elapsed += (source, e) => FireCurrentEditEvent();
        }

        private void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            _eventSendingTimer.Stop();
            lock (_eventLock)
            {
                _currentEditEvent = _currentEditEvent ?? Create<EditEvent>();
                _currentEditEvent.NumberOfChanges += 1;
                // TODO subtract whitespaces from change size
                _currentEditEvent.SizeOfChanges += endPoint.AbsoluteCharOffset - startPoint.AbsoluteCharOffset;
                _currentStartPoint = startPoint;
            }
            _eventSendingTimer.Start();
        }

        private void FireCurrentEditEvent()
        {
            _eventSendingTimer.Stop();
            lock (_eventLock)
            {
                _currentEditEvent.Context2 = _contextProvider.GetCurrentContext(_currentStartPoint);
                FireNow(_currentEditEvent);
                _currentEditEvent = null;
            }
        }

        public void Dispose()
        {
            _eventSendingTimer.Close();
        }
    }
}