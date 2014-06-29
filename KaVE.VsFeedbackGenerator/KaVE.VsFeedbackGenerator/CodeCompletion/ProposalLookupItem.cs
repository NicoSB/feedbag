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

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace KaVE.VsFeedbackGenerator.CodeCompletion
{
    internal class ProposalLookupItem : IWrappedLookupItem
    {
        private readonly ILookupItem _wrappedItem;
        private readonly string _prefix;
        private readonly RichText _postfix;

        public ProposalLookupItem(ILookupItem wrappedItem, string prefix, RichText postfix)
        {
            _wrappedItem = wrappedItem;
            _prefix = prefix;
            _postfix = postfix;
        }

        public ILookupItem Item
        {
            get { return _wrappedItem; }
        }

        public RichText DisplayName
        {
            get { return _wrappedItem.DisplayName + _postfix; }
        }

        public string OrderingString
        {
            get { return _prefix + (_wrappedItem.OrderingString ?? DisplayName.ToString()); }
        }

        public bool AcceptIfOnlyMatched(LookupItemAcceptanceContext itemAcceptanceContext)
        {
            return _wrappedItem.AcceptIfOnlyMatched(itemAcceptanceContext);
        }

        public MatchingResult Match(string prefix, ITextControl textControl)
        {
            return _wrappedItem.Match(prefix, textControl);
        }

        public void Accept(ITextControl textControl,
            TextRange nameRange,
            LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution,
            bool keepCaretStill)
        {
            _wrappedItem.Accept(textControl, nameRange, lookupItemInsertType, suffix, solution, keepCaretStill);
        }

        public TextRange GetVisualReplaceRange(ITextControl textControl, TextRange nameRange)
        {
            return _wrappedItem.GetVisualReplaceRange(textControl, nameRange);
        }

        public bool Shrink()
        {
            return _wrappedItem.Shrink();
        }

        public void Unshrink()
        {
            _wrappedItem.Unshrink();
        }

        public IconId Image
        {
            get { return _wrappedItem.Image; }
        }

        public RichText DisplayTypeName
        {
            get { return _wrappedItem.DisplayTypeName; }
        }

        public bool CanShrink
        {
            get { return _wrappedItem.CanShrink; }
        }

        public int Multiplier
        {
            get { return _wrappedItem.Multiplier; }
            set { _wrappedItem.Multiplier = value; }
        }

        public bool IsDynamic
        {
            get { return _wrappedItem.IsDynamic; }
        }

        public bool IgnoreSoftOnSpace
        {
            get { return _wrappedItem.IgnoreSoftOnSpace; }
            set { _wrappedItem.IgnoreSoftOnSpace = value; }
        }

        public string Identity
        {
            get { return _wrappedItem.Identity; }
        }
    }
}