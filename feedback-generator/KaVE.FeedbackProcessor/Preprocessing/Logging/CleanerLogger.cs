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

using System.Collections.Generic;

namespace KaVE.FeedbackProcessor.Preprocessing.Logging
{
    public interface ICleanerLogger
    {
        void ReadingZip(string zip);
        void ApplyingFilters();
        void ApplyingFilter(string name);
        void RemovingDuplicates();
        void OrderingEvents();
        void WritingEvents();
        void Finish(IDictionary<string, int> counts);
    }

    public class CleanerLogger : ICleanerLogger
    {
        private readonly IPrepocessingLogger _log;

        public CleanerLogger(IPrepocessingLogger log)
        {
            _log = log;
        }

        public void ReadingZip(string zip)
        {
            _log.Log();
            _log.Log("#### reading zip: {0}", zip);
        }

        public void ApplyingFilters()
        {
            _log.Log();
            _log.Log("applying filters:");
        }

        public void ApplyingFilter(string name)
        {
            _log.Log("\t- {0}", name);
        }

        public void RemovingDuplicates()
        {
            _log.Log();
            _log.Log("removing duplicates... ");
        }

        public void OrderingEvents()
        {
            _log.Append("done");
            _log.Log("ordering events... ");
        }

        public void WritingEvents()
        {
            _log.Append("done");
            _log.Log("writing events... ");
        }

        public void Finish(IDictionary<string, int> counts)
        {
            _log.Log();
            _log.Log("results:");
            foreach (var k in counts.Keys)
            {
                _log.Log("\t- {0}: {1}", k, counts[k]);
            }
        }
    }
}