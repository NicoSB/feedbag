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
 */

using KaVE.Model.Names.CSharp;
using NUnit.Framework;

namespace KaVE.Model.Tests.Names.CSharp
{
    [TestFixture]
    class AssemblyNameTest
    {
        [Test]
        public void ShouldBeMSCorLibAssembly()
        {
            const string identifier = "mscorlib, 4.0.0.0";
            var mscoreAssembly = AssemblyName.Get(identifier);

            Assert.AreEqual("mscorlib", mscoreAssembly.Name);
            Assert.AreEqual("4.0.0.0", mscoreAssembly.AssemblyVersion.Identifier);
            Assert.AreEqual(identifier, mscoreAssembly.Identifier);
        }

        [Test]
        public void ShouldBeVersionlessAssembly()
        {
            const string identifier = "assembly";
            var assemblyName = AssemblyName.Get(identifier);

            Assert.AreEqual("assembly", assemblyName.Name);
            Assert.AreSame(AssemblyVersion.UnknownName, assemblyName.AssemblyVersion);
            Assert.AreEqual(identifier, assemblyName.Identifier);
        }

        [Test]
        public void ShouldHaveUnknownVersionIfUnknown()
        {
            var uut = AssemblyName.UnknownName;

            Assert.AreEqual("???", uut.Name);
            Assert.AreSame(AssemblyVersion.UnknownName, uut.AssemblyVersion);
        }
    }
}
