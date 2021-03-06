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
using KaVE.Commons.Model.SSTs.Declarations;
using KaVE.Commons.Model.SSTs.Impl.Declarations;
using KaVE.Commons.Utils.Collections;
using NUnit.Framework;
using Fix = KaVE.Commons.TestUtils.Model.Naming.NameFixture;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.Declarations
{
    internal class MemberDeclarationTest : BaseSSTAnalysisTest
    {
        [Test]
        public void DelegateDeclaration()
        {
            CompleteInClass(@"
                public delegate void D(object o);
                $
            ");

            var expected = Sets.NewHashSet(
                new DelegateDeclaration
                {
                    Name =
                        Names.Type(
                            "d:[{0}] [N.C+D, TestProject].([{1}] o)",
                            Fix.Void,
                            Fix.Object)
                             .AsDelegateTypeName
                });
            Assert.AreEqual(expected, ResultSST.Delegates);
        }

        [Test]
        public void EventDeclaration()
        {
            CompleteInClass(@"
                public event int E;
                $
            ");

            var expected = Sets.NewHashSet(
                new EventDeclaration
                {
                    Name = Names.Event("[{0}] [N.C, TestProject].E", Fix.Int)
                });
            Assert.AreEqual(expected, ResultSST.Events);
        }

        [Test]
        public void FieldDeclaration()
        {
            CompleteInClass(@"
                public int _f;
                $
            ");

            var expected =
                Sets.NewHashSet(
                    new FieldDeclaration
                    {
                        Name = Names.Field("[{0}] [N.C, TestProject]._f", Fix.Int)
                    });
            Assert.AreEqual(expected, ResultSST.Fields);
        }

        [Test]
        public void MethodDeclaration()
        {
            CompleteInClass(@"
                public void M() {}
                private void N() {}
                $
            ");

            var m = new MethodDeclaration
            {
                Name = Names.Method("[{0}] [N.C, TestProject].M()", Fix.Void),
                IsEntryPoint = true
            };
            var n = new MethodDeclaration
            {
                Name = Names.Method("[{0}] [N.C, TestProject].N()", Fix.Void),
                IsEntryPoint = false
            };
            var expected = Sets.NewHashSet(m, n);
            Assert.AreEqual(expected, ResultSST.Methods);
        }

        [Test]
        public void PropertyDeclaration()
        {
            CompleteInClass(@"
                public int P {get;set;}
                $
            ");

            var expected =
                Sets.NewHashSet(
                    new PropertyDeclaration
                    {
                        Name = Names.Property("set get [{0}] [N.C, TestProject].P()", Fix.Int)
                    });
            Assert.AreEqual(expected, ResultSST.Properties);
        }

        [Test]
        public void NestedClass_Methods()
        {
            CompleteInNamespace(@"
                class C
                {
                    $
                    class Nested
                    {
                        public void M() {}
                    }
                }
            ");

            var actual = ResultSST.Methods;
            var expected = Lists.NewList<IMethodDeclaration>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedClass_Fields()
        {
            CompleteInNamespace(@"
                class C
                {
                    $
                    class Nested
                    {
                        public int _f;
                    }
                }
            ");

            var actual = ResultSST.Fields;
            var expected = Lists.NewList<IFieldDeclaration>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedClass_Properties()
        {
            CompleteInNamespace(@"
                class C
                {
                    $
                    class Nested
                    {
                        public int P { get; set; }
                    }
                }
            ");

            var actual = ResultSST.Properties;
            var expected = Lists.NewList<IPropertyDeclaration>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedClass_Events()
        {
            CompleteInNamespace(@"
                class C
                {
                    $
                    class Nested
                    {
                        public event Action _e;
                    }
                }
            ");

            var actual = ResultSST.Events;
            var expected = Lists.NewList<IEventDeclaration>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedClass_Delegates()
        {
            CompleteInNamespace(@"
                class C
                {
                    $
                    class Nested
                    {
                        public delegate int D(int i);
                    }
                }
            ");

            var actual = ResultSST.Delegates;
            var expected = Lists.NewList<IDelegateDeclaration>();
            Assert.AreEqual(expected, actual);
        }
    }
}