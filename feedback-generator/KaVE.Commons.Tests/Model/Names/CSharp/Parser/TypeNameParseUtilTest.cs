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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KaVE.Commons.Model.Names.CSharp.Parser;
using Antlr4.Runtime;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Utils.Assertion;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Names.CSharp.Parser
{
    [TestFixture]
    class TypeNameParseUtilTest
    {
        [TestCase("?.?")]
        public void ValidateWrongTypeName(string input)
        {
            Assert.Catch(delegate { TypeNameParseUtil.ValidateTypeName(input); });
        }

        [TestCase("n.T,a")]
        public void ValidateTypeName(string input)
        {
            Assert.DoesNotThrow(delegate { TypeNameParseUtil.ValidateTypeName(input); });
        }

        [TestCase("n.T+T1,a", "n:n.T+T1,a"),
         TestCase("n.T+T1+T2,a", "n:n:n.T+T1+T2,a"),
         TestCase("T`1[[T1 -> i:T`1[[T -> T]], a, 4.0.0.0]], a, 4.0.0.0",
             "T'1[[T1 -> i:T'1[[T -> T]], a, 4.0.0.0]], a, 4.0.0.0")]
        public void HandleOldNamesSimpleNested(string input, string expected)
        {
            Assert.AreEqual(expected, TypeNameParseUtil.HandleOldTypeNames(input));
        }
    }
}