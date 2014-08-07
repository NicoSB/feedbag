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
 *    - Sven Amann
 */

using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Util;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.LiveTemplates.Templates;
using KaVE.JetBrains.Annotations;
using KaVE.Model.Events.CompletionEvent;
using KaVE.Model.Names;
using KaVE.Model.Names.CSharp;
using KaVE.Model.Names.ReSharper;
using KaVE.Utils.Assertion;
using KaVE.VsFeedbackGenerator.Utils.Names;

namespace KaVE.VsFeedbackGenerator.Utils
{
    public static class LookupItemUtils
    {
        [NotNull]
        public static ProposalCollection ToProposalCollection([NotNull] this IEnumerable<ILookupItem> items)
        {
            return new ProposalCollection(items.Select(ToProposal).ToList());
        }

        [NotNull]
        public static Proposal ToProposal([CanBeNull] this ILookupItem lookupItem)
        {
            var name = lookupItem == null ? Name.UnknownName : lookupItem.GetName();
            return new Proposal {Name = name};
        }

        private static IName GetName([NotNull] this ILookupItem lookupItem)
        {
            return TryGetNameFromDeclaredElementLookupItem(lookupItem) ??
                   TryGetNameFromWrappedLookupItem(lookupItem) ??
                   TryGetNameFromKeywordOrTextualLookupItem(lookupItem) ??
                   TryGetNameFromTemplateLookupItem(lookupItem) ??
                   Asserts.Fail<IName>("unknown kind of lookup item: {0}", lookupItem.GetType());
        }

        private static IName TryGetNameFromDeclaredElementLookupItem(ILookupItem lookupItem)
        {
            var declaredElementLookupItem = lookupItem as IDeclaredElementLookupItem;
            if (declaredElementLookupItem == null || declaredElementLookupItem.PreferredDeclaredElement == null)
            {
                return null;
            }
            // Only the lookup-item type tells whether this is a proposal for a constructor call or not.
            // In fact, ConstructorLookupItem is derived from TypeLookupItem and the additional internface
            // IConstructorLookupItem doesnot provide anything new. Hence, the special treatment.
            var constructorLookupItem = declaredElementLookupItem as ConstructorLookupItem;
            return constructorLookupItem != null
                ? constructorLookupItem.GetName()
                : declaredElementLookupItem.PreferredDeclaredElement.GetName();
        }

        private static IMethodName GetName(this ConstructorLookupItem constructor)
        {
            var typeName = constructor.PreferredDeclaredElement.GetName();
            var identifier = new StringBuilder();
            identifier.Append('[')
                .Append(typeName.Identifier)
                .Append("] [")
                .Append(typeName.Identifier)
                .Append("]..ctor()");
            return MethodName.Get(identifier.ToString());
        }

        private static IName TryGetNameFromWrappedLookupItem(ILookupItem lookupItem)
        {
            var wrappedLookupItem = lookupItem as IWrappedLookupItem;
            // TODO find example of wrapped case and decide whether or not to include wrapping information
            // there are no implementations of this interface found by hierarchy inspection...
            return wrappedLookupItem != null ? wrappedLookupItem.Item.GetName() : null;
        }

        private static IName TryGetNameFromKeywordOrTextualLookupItem(ILookupItem lookupItem)
        {
            // TODO implement specific name subclasses?
            return (lookupItem is IKeywordLookupItem || lookupItem is ITextualLookupItem)
                ? Name.Get(lookupItem.GetType().Name + ":" + lookupItem.DisplayName)
                : null;
        }

        private static IName TryGetNameFromTemplateLookupItem(ILookupItem lookupItem)
        {
            var templateLookupItem = lookupItem as TemplateLookupItem;
            return templateLookupItem == null ? null : templateLookupItem.Template.GetName();
        }

        private static LiveTemplateName GetName(this Template template)
        {
            return LiveTemplateName.Get(template.Shortcut + ":" + template.Description);
        }
    }
}