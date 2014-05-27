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
 *    - Sven Amann
 */

using KaVE.Model.Events;

namespace KaVE.VsFeedbackGenerator.SessionManager.Anonymize
{
    internal class IDEEventAnonymizer
    {
        public void AnonymizeSessionUUID(IDEEvent ideEvent)
        {
            ideEvent.IDESessionUUID = null;
        }

        public virtual void AnonymizeStartTimes(IDEEvent ideEvent)
        {
            ideEvent.TriggeredAt = null;
        }

        public virtual void AnonymizeDurations(IDEEvent ideEvent)
        {
            ideEvent.Duration = null;
        }

        public virtual void AnonymizeCodeNames(IDEEvent ideEvent)
        {
            ideEvent.ActiveDocument = ideEvent.ActiveDocument.ToAnonymousName();
            ideEvent.ActiveWindow = ideEvent.ActiveWindow.ToAnonymousName();
        }
    }
}