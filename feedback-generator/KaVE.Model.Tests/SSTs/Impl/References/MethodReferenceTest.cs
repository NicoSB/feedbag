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
 */

using KaVE.Model.Names.CSharp;
using KaVE.Model.SSTs.Impl.References;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Impl.References
{
    public class MethodReferenceTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new MethodReference();
            Assert.Null(sut.MethodName);
            Assert.AreNotEqual(0, sut.GetHashCode());
            Assert.AreNotEqual(1, sut.GetHashCode());
        }

        [Test]
        public void SettingValues()
        {
            var sut = new MethodReference {MethodName = MethodName.UnknownName};
            Assert.AreEqual(MethodName.UnknownName, sut.MethodName);
        }

        [Test]
        public void Equality_Default()
        {
            var a = new MethodReference();
            var b = new MethodReference();
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new MethodReference {MethodName = MethodName.UnknownName};
            var b = new MethodReference {MethodName = MethodName.UnknownName};
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentName()
        {
            var a = new MethodReference {MethodName = MethodName.UnknownName};
            var b = new MethodReference();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void VisitorIsImplemented()
        {
            var sut = new MethodReference();
            sut.Accept(23).Verify(v => v.Visit(sut, 23));
        }

        [Test]
        public void VisitorWithReturnIsImplemented()
        {
            var sut = new MethodReference();
            sut.Accept(23).VerifyWithReturn(v => v.Visit(sut, 23));
        }
    }
}