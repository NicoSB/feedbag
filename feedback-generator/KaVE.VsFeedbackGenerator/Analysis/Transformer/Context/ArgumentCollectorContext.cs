/*
 * Copyright 2014 Technische Universit�t Darmstadt
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
 *    - Dennis Albrecht
 */

using System.Collections.Generic;
using KaVE.VsFeedbackGenerator.Analysis.Util;

namespace KaVE.VsFeedbackGenerator.Analysis.Transformer.Context
{
    public class ArgumentCollectorContext : ITransformerContext
    {
        public ISSTFactory Factory { get; private set; }
        public ITempVariableGenerator Generator { get; private set; }
        public IScope Scope { get; private set; }
        public readonly IList<string> Arguments;

        public ArgumentCollectorContext(ITransformerContext context)
            : this(context.Factory, context.Generator, context.Scope) {}

        private ArgumentCollectorContext(ISSTFactory factory,
            ITempVariableGenerator generator,
            IScope scope)
        {
            Factory = factory;
            Generator = generator;
            Scope = scope;
            Arguments = new List<string>();
        }
    }
}