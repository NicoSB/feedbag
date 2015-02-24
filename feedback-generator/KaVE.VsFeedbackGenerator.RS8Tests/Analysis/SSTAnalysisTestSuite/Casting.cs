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
using KaVE.Model.SSTs.Declarations;
using KaVE.Model.SSTs.Expressions;
using KaVE.Model.SSTs.Expressions.Basic;
using KaVE.Model.SSTs.Statements;
using NUnit.Framework;

namespace KaVE.VsFeedbackGenerator.RS8Tests.Analysis.SSTAnalysisTestSuite
{
    internal class CastingAndTypeCheckingTest : BaseSSTAnalysisTest
    {
        [Test]
        public void TypeOf()
        {
            CompleteInClass(@"
                public void A()
                {
                    var t = typeof(int);
                    $
                }
            ");

            var mA = NewMethodDeclaration(SSTAnalysisFixture.Void, "A");
            mA.Body.Add(new VariableDeclaration("t", TypeName.Get("System.Type, mscorlib, 4.0.0.0")));
            mA.Body.Add(new Assignment("t", new ConstantValueExpression()));

            AssertAllMethods(mA);
        }

        [Test]
        public void CompositionOfTypeOf()
        {
            CompleteInClass(@"
                public void A()
                {
                    var t = typeof(int) == typeof(string);
                    $
                }
            ");

            var mA = NewMethodDeclaration(SSTAnalysisFixture.Void, "A");
            mA.Body.Add(new VariableDeclaration("t", SSTAnalysisFixture.Bool));
            mA.Body.Add(new Assignment("t", new ConstantValueExpression()));

            AssertAllMethods(mA);
        }

        [Test]
        public void Is_Reference()
        {
            CompleteInClass(@"
                public void A(object o)
                {
                    var isInstanceOf = o is string;
                    $
                }
            ");

            var mA = NewMethodDeclaration(
                SSTAnalysisFixture.Void,
                "A",
                string.Format("[{0}] o", SSTAnalysisFixture.Object));
            mA.Body.Add(new VariableDeclaration("isInstanceOf", SSTAnalysisFixture.Bool));
            mA.Body.Add(new Assignment("isInstanceOf", new ComposedExpression {Variables = new[] {"o"}}));

            AssertAllMethods(mA);
        }

        [Test]
        public void Is_Const()
        {
            CompleteInClass(@"
                public void A()
                {
                    var isInstanceOf = 1 is double;
                    $
                }
            ");

            var mA = NewMethodDeclaration(SSTAnalysisFixture.Void, "A");
            mA.Body.Add(new VariableDeclaration("isInstanceOf", SSTAnalysisFixture.Bool));
            mA.Body.Add(new Assignment("isInstanceOf", new ConstantValueExpression()));

            AssertAllMethods(mA);
        }

        [Test]
        public void As_Reference()
        {
            CompleteInClass(@"
                public void A(object o)
                {
                    var cast = o as string;
                    $
                }
            ");

            var mA = NewMethodDeclaration(
                SSTAnalysisFixture.Void,
                "A",
                string.Format("[{0}] o", SSTAnalysisFixture.Object));
            mA.Body.Add(new VariableDeclaration("cast", SSTAnalysisFixture.String));
            mA.Body.Add(new Assignment("cast", new ComposedExpression {Variables = new[] {"o"}}));

            AssertAllMethods(mA);
        }

        [Test]
        public void As_Const()
        {
            CompleteInClass(@"
                public void A()
                {
                    var cast = 1.0 as object;
                    $
                }
            ");

            var mA = NewMethodDeclaration(SSTAnalysisFixture.Void, "A");
            mA.Body.Add(new VariableDeclaration("cast", SSTAnalysisFixture.Object));
            mA.Body.Add(new Assignment("cast", new ConstantValueExpression()));

            AssertAllMethods(mA);
        }

        [Test]
        public void Cast_Const()
        {
            CompleteInClass(@"
                public void A()
                {
                    var i = (int) 1.0;
                    $
                }
            ");

            var mA = NewMethodDeclaration(SSTAnalysisFixture.Void, "A");
            mA.Body.Add(new VariableDeclaration("i", SSTAnalysisFixture.Int));
            mA.Body.Add(new Assignment("i", new ConstantValueExpression()));

            AssertAllMethods(mA);
        }

        [Test]
        public void Cast_Reference()
        {
            CompleteInClass(@"
                public void A(object o)
                {
                    var s = (string) o;
                    $
                }
            ");

            var mA = NewMethodDeclaration(
                SSTAnalysisFixture.Void,
                "A",
                string.Format("[{0}] o", SSTAnalysisFixture.Object));
            mA.Body.Add(new VariableDeclaration("s", SSTAnalysisFixture.String));
            mA.Body.Add(new Assignment("s", ComposedExpression.Create("o")));

            AssertAllMethods(mA);
        }
    }
}