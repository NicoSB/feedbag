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
 *    - Dennis Albrecht
 */

namespace KaVE.Commons.Model.ObjectUsage
{
    public class CoReFieldName : CoReName
    {
        public CoReFieldName(string name) : base(name, ValidationPattern()) {}

        private static string ValidationPattern()
        {
            return string.Format(@"{0}\.[_a-zA-Z0-9]+;{0}", CoReTypeName.ValidationPattern());
        }
    }
}