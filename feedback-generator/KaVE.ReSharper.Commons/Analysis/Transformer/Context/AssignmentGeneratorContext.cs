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

using KaVE.RS.Commons.Analysis.Util;

namespace KaVE.RS.Commons.Analysis.Transformer.Context
{
    public class AssignmentGeneratorContext : ITransformerContext
    {
        public ISSTFactory Factory { get; private set; }
        public IUniqueVariableNameGenerator Generator { get; private set; }
        public IScope Scope { get; private set; }
        public readonly string Dest;

        public AssignmentGeneratorContext(ITransformerContext context, string dest)
            : this(context.Factory, context.Generator, context.Scope, dest) {}

        private AssignmentGeneratorContext(ISSTFactory factory,
            IUniqueVariableNameGenerator generator,
            IScope scope,
            string dest)
        {
            Factory = factory;
            Generator = generator;
            Scope = scope;
            Dest = dest;
        }
    }
}