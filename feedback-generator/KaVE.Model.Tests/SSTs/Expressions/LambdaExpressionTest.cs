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

using KaVE.Model.Collections;
using KaVE.Model.SSTs;
using KaVE.Model.SSTs.Expressions;
using KaVE.Model.SSTs.Statements;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Expressions
{
    public class LambdaExpressionTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new LambdaExpression();
            Assert.AreEqual(Lists.NewList<Statement>(), sut.Body);
        }

        [Test]
        public void SettingValues()
        {
            var sut = new LambdaExpression();
            sut.Body.Add(new GotoStatement());

            var expected = Lists.NewList<Statement>();
            expected.Add(new GotoStatement());

            Assert.AreEqual(expected, sut.Body);
        }

        [Test]
        public void Equality_Default()
        {
            var a = new LambdaExpression();
            var b = new LambdaExpression();

            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new LambdaExpression();
            a.Body.Add(new GotoStatement());
            var b = new LambdaExpression();
            b.Body.Add(new GotoStatement());

            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentBody()
        {
            var a = new LambdaExpression();
            a.Body.Add(new GotoStatement());
            var b = new LambdaExpression();

            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}