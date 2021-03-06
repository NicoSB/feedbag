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
using JetBrains.ReSharper.Psi;
using KaVE.RS.Commons.Tests_Unit.TestFactories;
using KaVE.RS.Commons.Utils.Naming;
using NUnit.Framework;

namespace KaVE.RS.Commons.Tests_Unit.Utils.Naming
{
    [Ignore("right now, mocking does not work for built-in types")]
    public class ReSharperTypeNameFactoryTest
    {
        [TestCase("System.String", "mscore", "4.0.0.0", "System.String, mscore, 4.0.0.0"),
         TestCase("Some.Outer+Inner", "Assembly", "5.4.3.2", "Some.Outer+Inner, Assembly, 5.4.3.2"),
         TestCase("ValueType[,,]", "A", "9.8.7.6", "ValueType[,,], A, 9.8.7.6"),
         TestCase("ValueType[][]", "B", "5.4.3.2", "ValueType[][], B, 5.4.3.2")]
        public void ShouldGetNameForIType(string typeFqn, string assemblyName, string assemblyVersion, string identifier)
        {
            var type = TypeMockUtils.MockIType(typeFqn, assemblyName, assemblyVersion);

            AssertNameIdentifier(type, identifier);
        }

        [Test]
        public void ShouldGetNameForITypeWithTwoTypeParameters()
        {
            var type = TypeMockUtils.MockIType(
                "System.Collections.IDictionary`2",
                new Dictionary<string, IType>
                {
                    {"TKey", TypeMockUtils.MockIType("System.String", "mscore", "1.2.3.4")},
                    {"TValue", TypeMockUtils.MockIType("System.Object", "mscore", "5.6.7.8")}
                },
                "mscore",
                "9.10.11.12");

            AssertNameIdentifier(
                type,
                "System.Collections.IDictionary`2[[TKey -> System.String, mscore, 1.2.3.4],[TValue -> System.Object, mscore, 5.6.7.8]], mscore, 9.10.11.12");
        }

        private static void AssertNameIdentifier(IType type, string identifier)
        {
            var name = type.GetName();
            Assert.AreEqual(identifier, name.Identifier);
        }
    }
}