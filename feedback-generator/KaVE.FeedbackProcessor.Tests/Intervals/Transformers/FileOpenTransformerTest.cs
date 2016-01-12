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

using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Model.Names.VisualStudio;
using KaVE.FeedbackProcessor.Intervals.Model;
using KaVE.FeedbackProcessor.Intervals.Transformers;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.Intervals.Transformers
{
    internal class FileOpenTransformerTest : TransformerTestBase<FileOpenInterval>
    {
        private DocumentEvent TestDocumentEvent(int startOffset,
            int endOffset,
            string filename,
            bool isFileOpen,
            string sessionId = "")
        {
            return new DocumentEvent
            {
                TriggeredAt = TestTime(startOffset),
                TerminatedAt = TestTime(endOffset),
                Action = isFileOpen ? DocumentEvent.DocumentAction.Opened : DocumentEvent.DocumentAction.Closing,
                Document = DocumentName.Get("CSharp " + filename),
                IDESessionUUID = sessionId
            };
        }

        private FileOpenInterval ExpectedInterval(int startOffset, int endOffset, string filename)
        {
            var interval = ExpectedInterval(startOffset, endOffset);
            interval.Filename = filename;
            return interval;
        }

        [Test]
        public void SingleInterval()
        {
            var sut = new FileOpenTransformer();

            sut.ProcessEvent(TestDocumentEvent(0, 0, "File1.cs", true));
            sut.ProcessEvent(TestDocumentEvent(1, 1, "File1.cs", false));

            var expected = ExpectedInterval(0, 1, "File1.cs");
            CollectionAssert.AreEquivalent(new[] {expected}, sut.SignalEndOfEventStream());
        }

        [Test]
        public void MultipleIntervalsWithOverlaps()
        {
            var sut = new FileOpenTransformer();

            sut.ProcessEvent(TestDocumentEvent(0, 0, "File1.cs", true));
            sut.ProcessEvent(TestDocumentEvent(4, 4, "File1.cs", false));

            sut.ProcessEvent(TestDocumentEvent(2, 2, "File2.cs", true));
            sut.ProcessEvent(TestDocumentEvent(6, 6, "File2.cs", false));

            sut.ProcessEvent(TestDocumentEvent(8, 8, "File1.cs", true));
            sut.ProcessEvent(TestDocumentEvent(10, 10, "File1.cs", false));

            var expected = new[]
            {
                ExpectedInterval(0, 4, "File1.cs"),
                ExpectedInterval(2, 6, "File2.cs"),
                ExpectedInterval(8, 10, "File1.cs")
            };

            CollectionAssert.AreEquivalent(expected, sut.SignalEndOfEventStream());
        }

        [Test]
        public void CreatesNewIntervalForNewIdeSession()
        {
            var sut = new FileOpenTransformer();

            sut.ProcessEvent(TestDocumentEvent(0, 1, "File1.cs", true, "a")); 
            // missing close!
            sut.ProcessEvent(TestDocumentEvent(8, 8, "File1.cs", true, "b"));
            sut.ProcessEvent(TestDocumentEvent(10, 10, "File1.cs", false, "b"));

            var expected = new[]
            {
                ExpectedInterval(0, 1, "File1.cs"),
                ExpectedInterval(8, 10, "File1.cs")
            };

            CollectionAssert.AreEquivalent(expected, sut.SignalEndOfEventStream());
        }
    }
}