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

using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.TypeShapes;
using KaVE.Commons.Utils.Collections;
using NUnit.Framework;
using Fix = KaVE.Commons.TestUtils.Model.Naming.NameFixture;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.TypeShapeAnalysisTestSuite
{
    internal class NestedTypesTest : BaseCSharpCodeCompletionTest
    {
        [Test]
        public void NestedTypesEmptyWithoutNestedTypes()
        {
            CompleteInNamespace(@"
                public class C
                {
                    public void M()
                    {
                        $
                    }
                }
            ");
            Assert.IsEmpty(ResultContext.TypeShape.NestedTypes);
        }

        [Test]
        public void ShouldRetrieveAllNestedTypes()
        {
            CompleteInCSharpFile(@"
                namespace TestNamespace
                {
                    public interface AnInterface {}

                    public class SuperClass {}

                    public class TestClass
                    {
                        public void Doit()
                        {
                            this.Doit();
                            $
                        }

                        public class N1 {}
                        public class N2 {}
                        public class N3 {}
                    }
                }");
            Assert.AreEqual(
                Sets.NewHashSet(
                    Names.Type("TestNamespace.TestClass+N1, TestProject"),
                    Names.Type("TestNamespace.TestClass+N2, TestProject"),
                    Names.Type("TestNamespace.TestClass+N3, TestProject")),
                ResultContext.TypeShape.NestedTypes);
        }

        [Test]
        public void DoesNotStepIntoNestedTypes()
        {
            CompleteInNamespace(@"
                class C
                    {
                        $

                        public class N
                        {
                            // should not include any of these
                            public delegate void D();
                            public event Action E;
                            public int F;
                            public void M() {}
                            public int P { get; set; }
                        }
                    }
                ");

            var actual = ResultContext.TypeShape;
            var expected = new TypeShape
            {
                TypeHierarchy = new TypeHierarchy("N.C, TestProject"),
                NestedTypes = {Names.Type("N.C+N, TestProject")}
            };
            Assert.AreEqual(expected, actual);
        }
    }
}