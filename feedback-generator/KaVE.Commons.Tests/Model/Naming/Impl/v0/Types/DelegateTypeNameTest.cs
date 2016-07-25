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

using System.Collections.Generic;
using KaVE.Commons.Model.Naming.Impl.v0.CodeElements;
using KaVE.Commons.Model.Naming.Impl.v0.Types;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.Impl.v0.Types
{
    internal class DelegateTypeNameTest
    {
        private static readonly IDelegateTypeName ParameterlessDelegateName =
            new DelegateTypeName("d:[R, A, 1.0.0.0] [Some.DelegateType, A, 1.0.0.0].()");

        private static readonly IDelegateTypeName ParameterizedDelegateName =
            new DelegateTypeName(
                "d:[R, A, 1.0.0.0] [Some.DelegateType, A, 1.0.0.0].([C, A, 1.2.3.4] p1, [D, A, 1.2.3.4] p2)");

        private static IEnumerable<string[]> DelegateTypeNames()
        {
            var delegateIds = Sets.NewHashSet<string[]>();
            foreach (var typeId in TestUtils.TypeSource())
            {
                delegateIds.Add(new[] {"d:[{0}] [{0}].()".FormatEx(typeId), typeId});
                delegateIds.Add(new[] {"d:[{0}] [{0}].([{0}] p1)".FormatEx(typeId), typeId});
                delegateIds.Add(new[] {"d:[{0}] [{0}].([{0}] p1, [{0}] p2)".FormatEx(typeId), typeId});
            }
            return delegateIds;
        }

        [Test]
        public void DefaultValues()
        {
            Assert.IsTrue(new DelegateTypeName().IsUnknown);
            Assert.IsTrue(new DelegateTypeName("d:[?] [?].()").IsUnknown);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void TypeClassification(string delegateId, string delegateTypeId)
        {
            var delegateType = new DelegateTypeName(delegateId);
            Assert.IsTrue(delegateType.IsDelegateType);
            Assert.IsTrue(delegateType.IsReferenceType);

            if (!delegateType.Identifier.StartsWith("d:[?]"))
            {
                Assert.IsFalse(delegateType.IsUnknown);
            }
            Assert.IsFalse(delegateType.IsHashed);

            Assert.IsFalse(delegateType.HasTypeParameters);

            Assert.IsFalse(delegateType.IsArray);
            Assert.IsFalse(delegateType.IsClassType);
            Assert.IsFalse(delegateType.IsEnumType);
            Assert.IsFalse(delegateType.IsInterfaceType);
            Assert.IsFalse(delegateType.IsNullableType);
            Assert.IsFalse(delegateType.IsSimpleType);
            Assert.IsFalse(delegateType.IsStructType);
            Assert.IsFalse(delegateType.IsTypeParameter);
            Assert.IsFalse(delegateType.IsValueType);
            Assert.IsFalse(delegateType.IsVoidType);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldRecognizeDelegateNames(string delegateId, string delegateTypeId)
        {
            Assert.IsTrue(DelegateTypeName.IsDelegateTypeNameIdentifier(delegateId));
            Assert.IsFalse(TypeUtils.IsUnknownTypeIdentifier(delegateId));
            Assert.IsFalse(ArrayTypeName.IsArrayTypeNameIdentifier(delegateId));
            Assert.IsFalse(TypeParameterName.IsTypeParameterNameIdentifier(delegateId));
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldRecognizeNonDelegateNames(string delegateId, string delegateTypeId)
        {
            if (DelegateTypeName.IsDelegateTypeNameIdentifier(delegateTypeId))
            {
                Assert.Ignore();
            }
            else
            {
                Assert.IsFalse(DelegateTypeName.IsDelegateTypeNameIdentifier(delegateTypeId));
            }
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldParseDelegateType(string delegateId, string delegateTypeId)
        {
            var actual = new DelegateTypeName(delegateId).DelegateType;
            var expected = TypeUtils.CreateTypeName(delegateTypeId);
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldParseFullName(string delegateId, string delegateTypeId)
        {
            var actual = new DelegateTypeName(delegateId).FullName;
            var expected = TypeUtils.CreateTypeName(delegateTypeId).FullName;
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldParseName(string delegateId, string delegateTypeId)
        {
            var actual = new DelegateTypeName(delegateId).Name;
            var expected = TypeUtils.CreateTypeName(delegateTypeId).Name;
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldParseNamespace(string delegateId, string delegateTypeId)
        {
            var actual = new DelegateTypeName(delegateId).Namespace;
            var expected = TypeUtils.CreateTypeName(delegateTypeId).Namespace;
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldParseAssembly(string delegateId, string delegateTypeId)
        {
            var actual = new DelegateTypeName(delegateId).Assembly;
            var expected = TypeUtils.CreateTypeName(delegateTypeId).Assembly;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsNested()
        {
            Assert.IsTrue(new DelegateTypeName("d:[R, P] [O+D, P].()").IsNestedType);
        }

        [Test]
        public void IsNotNested()
        {
            Assert.IsFalse(new DelegateTypeName("d:[R, P] [D, P].()").IsNestedType);
        }

        [Test]
        public void HasNoParameters()
        {
            Assert.IsFalse(ParameterlessDelegateName.HasParameters);
        }

        [Test]
        public void HasParameters()
        {
            Assert.IsTrue(ParameterizedDelegateName.HasParameters);
        }

        [TestCaseSource("DelegateTypeNames")]
        public void ShouldParseParameters(string delegateId, string delegateTypeId)
        {
            if (!delegateId.EndsWith("()"))
            {
                var sut = new DelegateTypeName(delegateId);
                Assert.IsTrue(sut.HasParameters);
            }
            else
            {
                Assert.Ignore();
            }
        }

        [Test]
        public void ParsesParameters()
        {
            CollectionAssert.IsEmpty(ParameterlessDelegateName.Parameters);
            CollectionAssert.AreEqual(
                new[] {new ParameterName("[C, A, 1.2.3.4] p1"), new ParameterName("[D, A, 1.2.3.4] p2")},
                ParameterizedDelegateName.Parameters);
        }

        [Test]
        public void TypeParameterParsingIsCached()
        {
            var sut = new DelegateTypeName("d:[?] [n.C+D`1[[T]]].()");
            var a = sut.TypeParameters;
            var b = sut.TypeParameters;
            Assert.AreSame(a, b);
        }
    }
}