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
 */

using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Utils.Collections;
using KaVE.FeedbackProcessor.Activities.Model;
using KaVE.FeedbackProcessor.Cleanup.Processors;

namespace KaVE.FeedbackProcessor.Activities
{
    internal class InIDEActivityDetector : BaseProcessor
    {
        public InIDEActivityDetector()
        {
            RegisterFor<WindowEvent>(ProcessIDEFocusEvents);
        }

        private IKaVESet<IDEEvent> ProcessIDEFocusEvents(WindowEvent @event)
        {
            if (@event.Window.Type.Equals("vsWindowTypeMainWindow"))
            {
                var phase = @event.Action == WindowEvent.WindowAction.Activate ? ActivityPhase.Start : ActivityPhase.End;
                return AnswerActivity(@event, Activity.InIDE, phase);
            }
            return AnswerDrop();
        }

        private IKaVESet<IDEEvent> AnswerActivity(IDEEvent @event, Activity activity, ActivityPhase phase)
        {
            var activityEvent = new ActivityEvent
            {
                Activity = activity,
                Phase = phase,
                IDESessionUUID = @event.IDESessionUUID,
                KaVEVersion = @event.KaVEVersion,
                TriggeredAt = @event.TriggeredAt,
                TriggeredBy = @event.TriggeredBy,
                Duration = @event.Duration,
                ActiveWindow = @event.ActiveWindow
            };
            return AnswerReplace(activityEvent);
        }
    }
}