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
 *    - Uli Fahrer
 */

using KaVE.Model.ObjectUsage;
using NUnit.Framework;

namespace KaVE.Model.Tests.ObjectUsage
{
    [TestFixture]
    internal class DefinitionSitesTest
    {
        [Test]
        public void MethodReturnDefinitionIsCorrectInitialized()
        {
            var actual = DefinitionSites.CreateDefinitionByReturn("LStrangeType.M()LType;");
            var expected = new DefinitionSite
            {
                kind = DefinitionSiteKind.RETURN,
                method = new CoReMethodName("LStrangeType.M()LType;")
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FieldDefinitionIsCorrectInitialized()
        {
            var actual = DefinitionSites.CreateDefinitionByField("LType.object;LType");
            var expected = new DefinitionSite
            {
                kind = DefinitionSiteKind.FIELD,
                field = new CoReFieldName("LType.object;LType")
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InitDefinitionIsCorrectInitialized()
        {
            var actual = DefinitionSites.CreateDefinitionByConstructor("LType.Create()LType;");
            var expected = new DefinitionSite
            {
                kind = DefinitionSiteKind.NEW,
                method = new CoReMethodName("LType.Create()LType;")
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParameterDefinitionIsCorrectInitialized()
        {
            var actual = DefinitionSites.CreateDefinitionByParam("LType.M(LOtherType;)LType;", 0);
            var expected = new DefinitionSite
            {
                kind = DefinitionSiteKind.PARAM,
                method = new CoReMethodName("LType.M(LOtherType;)LType;"),
                argIndex = 0
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UnknownDefinitionIsCorrectInitialized()
        {
            var actual = DefinitionSites.CreateUnknownDefinitionSite();
            var expected = new DefinitionSite
            {
                kind = DefinitionSiteKind.UNKNOWN
            };

            Assert.AreEqual(expected, actual);
        }
    }
}