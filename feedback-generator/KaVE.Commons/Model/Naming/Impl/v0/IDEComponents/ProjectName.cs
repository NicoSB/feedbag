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

using KaVE.Commons.Model.Naming.IDEComponents;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.Naming.Impl.v0.IDEComponents
{
    public class ProjectName : Name, IProjectName
    {
        private static readonly WeakNameCache<IProjectName> Registry =
            WeakNameCache<IProjectName>.Get(id => new ProjectName(id));

        public new static IProjectName UnknownName
        {
            get { return Get("?"); }
        }

        public new static IProjectName Get(string identifier)
        {
            return Registry.GetOrCreate(identifier);
        }

        private ProjectName(string identifier) : base(identifier) {}

        public string Type
        {
            get { return Identifier.Split(new[] {' '}, 2)[0]; }
        }

        public string Name
        {
            get { return Identifier.Split(new[] {' '}, 2)[1]; }
        }
    }
}