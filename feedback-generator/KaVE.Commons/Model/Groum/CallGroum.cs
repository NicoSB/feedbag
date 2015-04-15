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
 */
using System.Collections.Generic;
using KaVE.Commons.Model.Names;

namespace KaVE.Commons.Model.Groum
{
    public class CallGroum : GroumBase
    {
        public CallGroum(IMethodName calledMethod)
        {
            CalledMethod = calledMethod;
        }

        public IMethodName CalledMethod { get; private set; }

        public List<Instance> Parameter { get; set; }

        public Instance Return { get; set; }
    }
}