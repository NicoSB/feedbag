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

using KaVE.Model.Names;
using KaVE.Model.Names.CSharp;
using KaVE.Model.SSTs.Expressions;
using KaVE.Model.SSTs.Statements;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Statements
{
    public class InvocationStatementTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new InvocationStatement();
            Assert.Null(sut.Target);
            Assert.AreNotEqual(0, sut.GetHashCode());
            Assert.AreNotEqual(1, sut.GetHashCode());
        }

        [Test]
        public void CustomConstructorNonStatic()
        {
            var sut = new InvocationStatement("a", GetMethod("A"), "b");
            Assert.AreEqual(new InvocationExpression("a", GetMethod("A"), "b"), sut.Target);
        }

        [Test]
        public void CustomConstructorStatic()
        {
            var sut = new InvocationStatement(GetStaticMethod("A"), "b");
            Assert.AreEqual(new InvocationExpression(GetStaticMethod("A"), "b"), sut.Target);
        }

        [Test]
        public void Equality()
        {
            var a = new InvocationStatement(GetStaticMethod("A"), "b");
            var b = new InvocationStatement(GetStaticMethod("A"), "b");
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentTarget()
        {
            var a = new InvocationStatement(GetStaticMethod("A"), "b");
            var b = new InvocationStatement(GetStaticMethod("B"), "b");
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        private static IMethodName GetMethod(string simpleName)
        {
            var methodName = string.Format(
                "[System.String, mscore, 4.0.0.0] [System.String, mscore, 4.0.0.0].{0}()",
                simpleName);
            return MethodName.Get(methodName);
        }

        private static IMethodName GetStaticMethod(string simpleName)
        {
            var methodName = string.Format(
                "static [System.String, mscore, 4.0.0.0] [System.String, mscore, 4.0.0.0].{0}()",
                simpleName);
            return MethodName.Get(methodName);
        }
    }
}