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
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Utils.Collections;

namespace KaVE.RS.SolutionAnalysis.CompletionEventToUsageHistory
{
    public class CompletionEventToUsageHistoryRunner
    {
        private readonly ITupleGenerator _tupleGenerator;
        private readonly IIoHelper _ioHelper;

        private int _numTotalEvents;
        private int _numTotalTuples;
        private Dictionary<string, IList<CompletionEvent>> _sortedEvents;

        public CompletionEventToUsageHistoryRunner(ITupleGenerator tupleGenerator, IIoHelper ioHelper)
        {
            _tupleGenerator = tupleGenerator;
            _ioHelper = ioHelper;
        }

        public void Run()
        {
            var startedAt = DateTime.Now;
            _sortedEvents = new Dictionary<string, IList<CompletionEvent>>();

            var exports = _ioHelper.FindExports();

            Log("found {0} exports:", exports.Count);
            foreach (var export in exports)
            {
                Log("     - {0}", export);
            }
            Log("");

            var total = exports.Count;
            var current = 1;
            foreach (var exportFile in exports)
            {
                ReadAndSortEvents(exportFile, current++, total);
            }


            _ioHelper.OpenCache();
            total = _sortedEvents.Keys.Count;
            current = 1;
            foreach (var key in _sortedEvents.Keys)
            {
                ProcessSortedEvents(key, current++, total);
            }
            _ioHelper.CloseCache();

            Log("");
            Log("finished! (started at {0})", startedAt);
            Log("found {0} events in total, extracted {1} tuples in total", _numTotalEvents, _numTotalTuples);
        }

        private void ReadAndSortEvents(string exportFile, int current, int total)
        {
            Log("reading {0}/{1}: {2}", current, total, exportFile);

            var keys = new HashSet<string>();
            var numEvents = 0;

            foreach (var @event in _ioHelper.ReadCompletionEvents(exportFile))
            {
                var idx = _tupleGenerator.GetTemporalIndex(@event);
                keys.Add(idx);
                SortedEvents(idx).Add(@event);
                numEvents++;
            }

            _numTotalEvents += numEvents;
            Log("     {0:#####} completion events, {1:#####} keys", numEvents, keys.Count);
        }

        private IList<CompletionEvent> SortedEvents(string idx)
        {
            if (!_sortedEvents.ContainsKey(idx))
            {
                _sortedEvents[idx] = Lists.NewList<CompletionEvent>();
            }
            return _sortedEvents[idx];
        }

        private void ProcessSortedEvents(string key, int current, int total)
        {
            Log("processing {0}/{1}: {2}", current, total, key);
            var events = _sortedEvents[key];

            var numTuples = 0;
            if (events.Count < 2)
            {
                return;
            }

            var tuple = _tupleGenerator.FindFirstAndLast(events);

            var queryTuples = _tupleGenerator.GenerateTuples(tuple.Item1, tuple.Item2);
            foreach (var qt in queryTuples)
            {
                _ioHelper.AddTuple(qt);
                numTuples++;
            }

            _numTotalTuples += numTuples;
            Log("     --> {0} tuples", numTuples);
        }

        private static void Log(string message, params object[] args)
        {
            Console.Write(@"{0} | ", DateTime.Now);
            Console.WriteLine(message, args);
        }
    }
}