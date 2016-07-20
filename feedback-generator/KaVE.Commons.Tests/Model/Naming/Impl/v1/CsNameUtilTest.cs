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
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Impl.v0.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Model.Naming.Types.Organization;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.Impl.v1
{
    public class CsNameUtilTest
    {
        [TestCase("CSharp.AliasName:???", typeof(AliasName)),
         TestCase("CSharp.AssemblyName:", typeof(IAssemblyName)),
         TestCase("CSharp.EventName:???", typeof(IEventName)),
         TestCase("CSharp.FieldName:???", typeof(IFieldName)),
         TestCase("CSharp.LambdaName:?", typeof(ILambdaName)),
         TestCase("CSharp.LocalVariableName:???", typeof(LocalVariableName)),
         TestCase("CSharp.MethodName:???", typeof(IMethodName)),
         TestCase("CSharp.Name:???", typeof(IName)),
         TestCase("CSharp.NamespaceName:???", typeof(INamespaceName)),
         TestCase("CSharp.ParameterName:???", typeof(IParameterName)),
         TestCase("CSharp.PropertyName:?", typeof(IPropertyName)),
         TestCase("CSharp.TypeName:T, A", typeof(ITypeName))]
        public void ParseJsonOldNames(string input, Type expectedType)
        {
            Assert.True(expectedType.IsInstanceOfType(Names.ParseJson(input)));
        }

        [TestCase("1a:???", typeof(AliasName)),
         TestCase("1b:A, 0.0.0.0", typeof(IAssemblyName)),
         TestCase("1c:[?] [?].m", typeof(IEventName)),
         TestCase("1d:???", typeof(IFieldName)),
         TestCase("1e:?", typeof(ILambdaName)),
         TestCase("1f:???", typeof(LocalVariableName)),
         TestCase("1g:???", typeof(IMethodName)),
         TestCase("1h:???", typeof(IName)),
         TestCase("1i:???", typeof(INamespaceName)),
         TestCase("1j:???", typeof(IParameterName)),
         TestCase("1k:???", typeof(IPropertyName)),
         TestCase("1l:???", typeof(ITypeName))]
        public void ParseJsonVersionOne(string input, Type expectedType)
        {
            Assert.True(expectedType.IsInstanceOfType(Names.ParseJson(input)));
        }

        [TestCase("1a:?", "1a:?"),
         TestCase("1b:?", "1b:?"),
         TestCase("1c:?", "1c:?"),
         TestCase("1d:?", "1d:?"),
         TestCase("1e:?", "1e:?"),
         TestCase("1f:?", "1f:?"),
         TestCase("1g:?", "1g:?"),
         TestCase("1h:?", "1h:?"),
         TestCase("1i:?", "1i:?"),
         TestCase("1j:?", "1j:?"),
         TestCase("1k:?", "1k:?"),
         TestCase("1l:?", "1l:?")]
        public void NameToJson(string input, string expected)
        {
            var name = Names.ParseJson(input);
            Console.WriteLine(name);
            Assert.AreEqual(expected, Names.ToJson(name));
        }
    }
}