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
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.UserProfiles;
using KaVE.Commons.Utils.Collections;
using KaVE.RS.SolutionAnalysis.CompletionEventToMicroCommits;
using KaVE.RS.SolutionAnalysis.UserProfileExports;
using Moq;
using NUnit.Framework;

namespace KaVE.RS.SolutionAnalysis.Tests.UserProfileExports
{
    internal class UserProfileExportRunnerTest
    {
        private IIoHelper _io;
        private UserProfileExportHelper _helper;

        private UserProfileExportRunner _sut;
        private UserProfileEvent _userProfileEvent;

        [SetUp]
        public void Setup()
        {
            _io = Mock.Of<IIoHelper>();
            _helper = Mock.Of<UserProfileExportHelper>();
            _sut = new UserProfileExportRunner(_io, _helper);

            _userProfileEvent = new UserProfileEvent
            {
                TriggeredAt = DateTime.Now,
                Id = "some id to make it non default"
            };

            Mock.Get(_io).Setup(io => io.FindExports()).Returns(Lists.NewList("a.zip", "b.zip"));
            Mock.Get(_io).Setup(io => io.ReadEvents("a.zip")).Returns(Lists.NewList(NewCommandEvent(1)));
            Mock.Get(_io).Setup(io => io.ReadEvents("b.zip")).Returns(Lists.NewList(_userProfileEvent));
        }

        [Test]
        public void NumExportsIsLogged()
        {
            _sut.Export("SomePath");
            Mock.Get(_helper).Verify(h => h.LogFoundExports(2));
        }

        [Test]
        public void AllExportsAreIterated()
        {
            _sut.Export("SomePath");
            Mock.Get(_helper).Verify(h => h.LogOpenExport("a.zip"));
            Mock.Get(_helper).Verify(h => h.LogOpenExport("b.zip"));
        }

        [Test]
        public void ResultIsLogged()
        {
            _sut.Export("SomePath");
            Mock.Get(_helper).Verify(h => h.LogResult(false));
            Mock.Get(_helper).Verify(h => h.LogResult(true));
        }

        [Test]
        public void UserProfilesAreLogged()
        {
            _sut.Export("SomePath");
            Mock.Get(_helper).Verify(h => h.LogUserProfiles(Lists.NewList(_userProfileEvent)));
        }

        [Test]
        public void NumDaysAreLogged_SameDayDifferentUpload()
        {
            _sut.Export("SomePath");
            Mock.Get(_helper).Verify(h => h.LogNumberDays(2, 2));
        }

        [Test]
        public void NumDaysAreLogged_SameUploadMutipleDays()
        {
            Mock.Get(_io).Setup(io => io.FindExports()).Returns(new[] {"a"});
            Mock.Get(_io).Setup(io => io.ReadEvents("a")).Returns(new[] {NewCommandEvent(1), NewCommandEvent(2)});

            _sut.Export("SomePath");
            Mock.Get(_helper).Verify(h => h.LogNumberDays(1, 2));
        }

        private static IDEEvent NewCommandEvent(int i)
        {
            return new CommandEvent
            {
                TriggeredAt = DateTime.Now.AddDays(-10 + i)
            };
        }
    }
}