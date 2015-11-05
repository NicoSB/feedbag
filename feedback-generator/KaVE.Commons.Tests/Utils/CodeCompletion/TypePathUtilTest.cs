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

using KaVE.Commons.Model.ObjectUsage;
using KaVE.Commons.Utils.CodeCompletion.Impl;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.CodeCompletion
{
    internal class TypePathUtilTest
    {
        private static readonly CoReTypeName ExpectedTypeName = new CoReTypeName(@"LSystem\Collections\List");
        private static readonly string PathWithVersion = ExpectedTypeName + ".1.zip";
        private static readonly string PathWithoutVersion = ExpectedTypeName + ".zip";

        private const string BasePath = "C:\\";
        private static readonly string ExpectedPathWithVersion = BasePath + PathWithVersion;
        private static readonly string ExpectedPathWithoutVersion = BasePath + PathWithoutVersion;

        private TypePathUtil _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new TypePathUtil();
        }

        [Test]
        public void TypeAsNested()
        {
            var name = new CoReTypeName("La/b/C");
            var actual = _sut.ToNestedPath(name);
            const string expected = "La/b/C";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TypeAsFlat()
        {
            var name = new CoReTypeName("La/b/C");
            var actual = _sut.ToFlatPath(name);
            const string expected = "La_b_C";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldGetTypeNameWithVersionNumber()
        {
            Assert.AreEqual(ExpectedTypeName, _sut.GetTypeName(PathWithVersion));
        }

        [Test]
        public void ShouldGetTypeNameWithoutVersionNumber()
        {
            Assert.AreEqual(ExpectedTypeName, _sut.GetTypeName(PathWithoutVersion));
        }

        [Test]
        public void ShouldGetVersionNumber()
        {
            Assert.AreEqual(1, _sut.GetVersionNumber(PathWithVersion));
        }

        [Test]
        public void ShouldUseVersionZeroAsFallback()
        {
            Assert.AreEqual(0, _sut.GetVersionNumber(PathWithoutVersion));
        }

        [Test]
        public void ShouldGetCorrectNestedPath()
        {
            Assert.AreEqual(ExpectedPathWithVersion, _sut.GetNestedFileName(BasePath, ExpectedTypeName, 1, "zip"));
        }

        [Test]
        public void ShouldGetCorrectNestedPathWithoutVersion()
        {
            Assert.AreEqual(ExpectedPathWithoutVersion, _sut.GetNestedFileName(BasePath, ExpectedTypeName, "zip"));
        }
    }
}