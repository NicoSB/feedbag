/*
 * Copyright 2014 Technische Universit�t Darmstadt
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
 */

using System;
using KaVE.Commons.Utils.Assertion;

namespace KaVE.FeedbackProcessor.Cleanup.Heuristics
{
    internal class ConcurrentEventHeuristic {

        public static DateTime GetValidEventTime(DateTime? eventTime)
        {
            if (!eventTime.HasValue)
                Asserts.Fail("Events should have a DateTime value in TriggeredAt");
            
            return eventTime.Value;
        }

        public static bool HaveSimiliarEventTime(DateTime currentEventTime, DateTime lastEventTime, TimeSpan eventTimeDifference)
        {
            var timeDifference = Math.Abs(currentEventTime.Ticks - lastEventTime.Ticks);
            return timeDifference <= eventTimeDifference.Ticks;
        }
    }
}