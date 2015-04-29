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
 *    - Sebastian Proksch
 */

using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Utils.Collections;
using KaVE.FeedbackProcessor.Activities;
using KaVE.FeedbackProcessor.Activities.Model;
using KaVE.FeedbackProcessor.Cleanup;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.Activities
{
    internal class CompletionEventProcessorTest : BaseEventProcessorTest
    {
        public override IIDEEventProcessor Sut
        {
            get { return new CompletionEventActivityProcessor(); }
        }

        [Test]
        public void ShouldAlwaysMapToEditing()
        {
            var @event = new CompletionEvent
            {
                Selections = Lists.NewList<IProposalSelection>(),
                ProposalCollection = new ProposalCollection(),
                Context2 = new Context(),
                Prefix = "",
                TerminatedState = TerminationState.Unknown
            };

            AssertMapsToActivities(@event, Activity.Editing);
        }
    }
}