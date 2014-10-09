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
 *    - Dennis Albrecht
 */

using System;
using KaVE.Model.Events.CompletionEvent;
using KaVE.TestUtils.Model.Events.CompletionEvent;
using NUnit.Framework;

namespace KaVE.VsFeedbackGenerator.Tests.Utils.Json.JsonSerializationSuite
{
    [TestFixture]
    internal class ProposalSelectionSerializationTest : SerializationTestBase
    {
        [Test]
        public void ShouldSerializeProposalSelection()
        {
            var uut = new ProposalSelection(CompletionEventTestFactory.CreateAnonymousProposal())
            {
                SelectedAfter = TimeSpan.FromMinutes(23)
            };

            JsonAssert.SerializationPreservesData(uut);
        }

        [Test]
        public void ShouldSerializeToString()
        {
            var proposal = new ProposalSelection(CompletionEventTestFactory.CreatePredictableProposal())
            {
                SelectedAfter = TimeSpan.FromSeconds(4)
            };
            const string expected =
                "{\"$type\":\"KaVE.Model.Events.CompletionEvent.ProposalSelection, KaVE.Model\",\"SelectedAfter\":\"00:00:04\",\"Proposal\":{\"$type\":\"KaVE.Model.Events.CompletionEvent.Proposal, KaVE.Model\",\"Name\":\"CSharp.Name:1\"}}";

            JsonAssert.SerializesTo(proposal, expected);
        }
    }
}