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

using System.Collections.Generic;
using KaVE.JetBrains.Annotations;

namespace KaVE.Model.TypeShapes
{
    public interface ITypeShape
    {
        /// <summary>
        ///     A description of the enclosing class, including its parent class and implemented interfaces.
        /// </summary>
        [NotNull]
        ITypeHierarchy TypeHierarchy { get; set; }

        /// <summary>
        ///     All Methods that are overridden in the class under edit (including information about the first and super
        ///     declaration).
        /// </summary>
        [NotNull]
        ISet<IMethodHierarchy> MethodHierarchies { get; set; }
    }
}