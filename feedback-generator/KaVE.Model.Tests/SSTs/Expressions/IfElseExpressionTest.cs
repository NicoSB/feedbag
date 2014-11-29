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

using KaVE.Model.SSTs.Expressions;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Expressions
{
    public class IfElseExpressionTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new IfElseExpression();
            Assert.IsNull(sut.Condition);
            Assert.IsNull(sut.ThenExpression);
            Assert.IsNull(sut.ElseExpression);
            Assert.AreNotEqual(0, sut.GetHashCode());
            Assert.AreNotEqual(1, sut.GetHashCode());
        }

        [Test]
        public void SettingValues()
        {
            var sut = new IfElseExpression
            {
                Condition = new ComposedExpression(),
                ThenExpression = new ConstantExpression(),
                ElseExpression = new InvocationExpression()
            };
            Assert.AreEqual(new ComposedExpression(), sut.Condition);
            Assert.AreEqual(new ConstantExpression(), sut.ThenExpression);
            Assert.AreEqual(new InvocationExpression(), sut.ElseExpression);
        }

        [Test]
        public void Equality_Default()
        {
            var a = new IfElseExpression();
            var b = new IfElseExpression();
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new IfElseExpression
            {
                Condition = new ComposedExpression(),
                ThenExpression = new ConstantExpression(),
                ElseExpression = new InvocationExpression()
            };
            var b = new IfElseExpression
            {
                Condition = new ComposedExpression(),
                ThenExpression = new ConstantExpression(),
                ElseExpression = new InvocationExpression()
            };
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentCondition()
        {
            var a = new IfElseExpression {Condition = new ComposedExpression()};
            var b = new IfElseExpression();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentIf()
        {
            var a = new IfElseExpression {ThenExpression = new ComposedExpression()};
            var b = new IfElseExpression();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentElse()
        {
            var a = new IfElseExpression {ElseExpression = new ComposedExpression()};
            var b = new IfElseExpression();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}