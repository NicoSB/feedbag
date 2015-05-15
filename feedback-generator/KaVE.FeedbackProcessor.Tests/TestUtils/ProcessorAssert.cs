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
 *    - Markus Zimmermann
 *    - Sven Amann
 */

using KaVE.Commons.Model.Events;
using KaVE.Commons.Utils.Collections;
using KaVE.FeedbackProcessor.Cleanup;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.TestUtils
{
    internal static class ProcessorAssert
    {
        public static void DoesNotFilter(IIDEEventProcessor<IKaVESet<IDEEvent>> uut, IDEEvent @event)
        {
            var processedEvent = uut.Process(@event);

            CollectionAssert.AreEqual(new KaVEHashSet<IDEEvent> {@event}, processedEvent);
        }

        public static void Drops(IIDEEventProcessor<IKaVESet<IDEEvent>> uut, IDEEvent @event)
        {
            var answer = uut.Process(@event);

            CollectionAssert.IsEmpty(answer);
        }
    }
}