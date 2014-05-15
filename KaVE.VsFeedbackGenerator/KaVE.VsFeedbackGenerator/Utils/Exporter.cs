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
 *    - Sven Amann
 *    - Dennis Albrecht
 */

using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using JetBrains.Application;
using KaVE.Model.Events;
using KaVE.VsFeedbackGenerator.Utils.Json;

namespace KaVE.VsFeedbackGenerator.Utils
{
    public interface IExporter
    {
        void Export(IEnumerable<IDEEvent> events, IPublisher publisher);
    }

    [ShellComponent]
    internal class Exporter : IExporter
    {
        public void Export(IEnumerable<IDEEvent> events, IPublisher publisher)
        {
            using (var stream = new MemoryStream())
            {
                CreateZipFile(events, stream);

                publisher.Publish(stream);
            }
        }

        private static void CreateZipFile(IEnumerable<IDEEvent> events, Stream stream)
        {
            using (var zipFile = new ZipFile())
            {
                int i = 0;
                foreach (var e in events)
                {
                    var fileName = (i++) + "-" + e.GetType().Name + ".json";
                    var json = e.ToFormattedJson();
                    zipFile.AddEntry(fileName, json);
                }
                zipFile.Save(stream);
            }
        }
    }
}