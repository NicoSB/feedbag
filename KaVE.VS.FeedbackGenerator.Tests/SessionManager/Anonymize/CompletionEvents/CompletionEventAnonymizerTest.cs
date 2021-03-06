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
using JetBrains.Util;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.TypeShapes;
using KaVE.Commons.Utils.Naming;
using NUnit.Framework;

namespace KaVE.VS.FeedbackGenerator.Tests.SessionManager.Anonymize.CompletionEvents
{
    internal class CompletionEventAnonymizerTest : IDEEventAnonymizerTestBase<CompletionEvent>
    {
        protected override CompletionEvent CreateEventWithAllAnonymizablePropertiesSet()
        {
            return new CompletionEvent
            {
                ProposalCollection = new ProposalCollection(
                    new[]
                    {
                        new Proposal {Name = Names.Type("MyType, EnclosingProject")},
                        new Proposal {Name = Names.Type("OtherType, Assembly, 1.2.3.4")},
                        new Proposal {Name = Names.Namespace("Some.Namepsace")}
                    }),
                Selections =
                {
                    new ProposalSelection(new Proposal {Name = Names.Type("MyType, EnclosingProject")})
                    {
                        SelectedAfter = TimeSpan.FromSeconds(0)
                    },
                    new ProposalSelection(new Proposal {Name = Names.Type("OtherType, Assembly, 1.2.3.4")})
                    {
                        SelectedAfter = TimeSpan.FromSeconds(2)
                    }
                },
                Context2 = CreateSimpleContext()
            };
        }

        private static Context CreateSimpleContext()
        {
            return new Context
            {
                TypeShape = new TypeShape {TypeHierarchy = new TypeHierarchy("C, P")},
                SST = new SST {EnclosingType = Names.Type("T,P")}
            };
        }

        [Test]
        public void ShouldRemoveSelectionOffsetWhenRemoveDurationsIsSet()
        {
            AnonymizationSettings.RemoveDurations = true;

            var actual = WhenEventIsAnonymized();

            actual.Selections.ForEach(selection => Assert.IsNull(selection.SelectedAfter));
        }

        [Test]
        public void ShouldAnonymizeProposalNamesWhenRemoveNamesIsSet()
        {
            AnonymizationSettings.RemoveCodeNames = true;
            var expected = new[]
            {
                new Proposal {Name = Names.Type("Q-vTVCo_g8yayGGoDdH7BA==, qfFVtSOtve-XEFJXWTbfXw==")},
                new Proposal {Name = Names.Type("OtherType, Assembly, 1.2.3.4")},
                new Proposal {Name = Names.Namespace("A_SHMh611J-1vRjtIJDirA==")}
            };

            var actual = WhenEventIsAnonymized();

            CollectionAssert.AreEqual(expected, actual.ProposalCollection.Proposals);
        }

        [Test]
        public void ShouldAnonymizeProposalNamesInSelectionsWhenRemoveNamesIsSet()
        {
            AnonymizationSettings.RemoveCodeNames = true;
            var expected = new[]
            {
                new ProposalSelection(
                    new Proposal {Name = Names.Type("Q-vTVCo_g8yayGGoDdH7BA==, qfFVtSOtve-XEFJXWTbfXw==")})
                {
                    SelectedAfter = TimeSpan.FromSeconds(0)
                },
                new ProposalSelection(new Proposal {Name = Names.Type("OtherType, Assembly, 1.2.3.4")})
                {
                    SelectedAfter = TimeSpan.FromSeconds(2)
                }
            };

            var actual = WhenEventIsAnonymized();

            CollectionAssert.AreEqual(expected, actual.Selections);
        }

        [Test]
        public void ShouldAnonymizeContext()
        {
            AnonymizationSettings.RemoveCodeNames = true;
            var expected = new Context
            {
                TypeShape = new TypeShape
                {
                    TypeHierarchy = new TypeHierarchy("3Rx860ySZTppa3kHpN1N8Q==, aUaDMpYpDqsiSh5nQjiWFw==")
                },
                SST = new SST
                {
                    EnclosingType = Names.Type("T,P").ToAnonymousName()
                }
            };

            var actual = WhenEventIsAnonymized().Context2;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldNotFailIfPropertiesAreNotSet()
        {
            AnonymizationSettings.RemoveCodeNames = true;
            AnonymizationSettings.RemoveDurations = true;
            AnonymizationSettings.RemoveStartTimes = true;

            WhenEventIsAnonymized();
        }

        protected override void AssertThatPropertiesThatAreNotTouchedByAnonymizationAreUnchanged(
            CompletionEvent original,
            CompletionEvent anonymized) {}
    }
}