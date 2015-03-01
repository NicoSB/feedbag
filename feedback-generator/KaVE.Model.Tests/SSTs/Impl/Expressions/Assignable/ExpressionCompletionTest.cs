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
using KaVE.Model.SSTs.Expressions.Assignable;
using KaVE.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.Model.SSTs.Impl.References;
using KaVE.Model.SSTs.Impl.Visitor;
using KaVE.Model.SSTs.References;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Impl.Expressions.Assignable
{
    public class ExpressionCompletionTest
    {
        private TestVisitor _visitor;

        [SetUp]
        public void Setup()
        {
            _visitor = new TestVisitor();
        }

        [Test]
        public void DefaultValues()
        {
            var sut = new ExpressionCompletion();
            Assert.Null(sut.Token);
            Assert.Null(sut.ObjectReference);
            Assert.Null(sut.TypeReference);
            Assert.AreNotEqual(0, sut.GetHashCode());
            Assert.AreNotEqual(1, sut.GetHashCode());
        }

        [Test]
        public void SettingValues()
        {
            var sut = new ExpressionCompletion
            {
                ObjectReference = Ref("i"),
                TypeReference = TypeName.UnknownName,
                Token = "t"
            };
            Assert.AreEqual(Ref("i"), sut.ObjectReference);
            Assert.AreEqual(TypeName.UnknownName, sut.TypeReference);
            Assert.AreEqual("t", sut.Token);
        }

        private IVariableReference Ref(string id)
        {
            return new VariableReference {Identifier = id};
        }

        [Test]
        public void Equality_Default()
        {
            var a = new ExpressionCompletion();
            var b = new ExpressionCompletion();
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new ExpressionCompletion
            {
                ObjectReference = Ref("i"),
                Token = "t",
                TypeReference = TypeName.UnknownName
            };
            var b = new ExpressionCompletion
            {
                ObjectReference = Ref("i"),
                Token = "t",
                TypeReference = TypeName.UnknownName
            };
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentObjectReference()
        {
            var a = new ExpressionCompletion {ObjectReference = Ref("i")};
            var b = new ExpressionCompletion {ObjectReference = Ref("j")};
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }


        [Test]
        public void Equality_DifferentToken()
        {
            var a = new ExpressionCompletion {Token = "t"};
            var b = new ExpressionCompletion {Token = "u"};
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentTypeReference()
        {
            var a = new ExpressionCompletion {TypeReference = TypeName.UnknownName};
            var b = new ExpressionCompletion {TypeReference = TypeName.Get("System.Int32, mscore, 4.0.0.0")};
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void VisitorIsImplemented()
        {
            var sut = new ExpressionCompletion();
            sut.Accept(_visitor, 13);
            Assert.AreEqual(sut, _visitor.Expr);
            Assert.AreEqual(13, _visitor.Context);
        }

        internal class TestVisitor : AbstractNodeVisitor<int>
        {
            public IExpressionCompletion Expr { get; private set; }
            public int Context { get; private set; }

            public override void Visit(IExpressionCompletion expr, int context)
            {
                Expr = expr;
                Context = context;
            }
        }
    }
}