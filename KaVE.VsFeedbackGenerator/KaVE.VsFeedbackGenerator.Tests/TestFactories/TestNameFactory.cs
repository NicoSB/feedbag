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
using KaVE.Model.Names;
using KaVE.Model.Names.CSharp;

namespace KaVE.VsFeedbackGenerator.Tests.TestFactories
{
    internal static class TestNameFactory
    {
        private static int _counter;

        private static int NextCounter()
        {
            return ++_counter;
        }

        public static IMethodName GetAnonymousMethodName()
        {
            return
                MethodName.Get(
                    string.Format(
                        "[{0}] [{1}].Method{2}()",
                        GetAnonymousTypeName(),
                        GetAnonymousTypeName(),
                        NextCounter()));
        }

        public static ITypeName GetAnonymousTypeName()
        {
            return
                TypeName.Get(
                    string.Format("SomeType{0}, SomeAssembly{1}, 9.8.7.6", NextCounter(), NextCounter()));
        }

        public static INamespaceName GetAnonymousNamespace()
        {
            return NamespaceName.Get(string.Format("A.N{0}", NextCounter()));
        }
    }
}