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

using System;
using System.Collections.Generic;
using KaVE.Commons.Model.Events.CompletionEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KaVE.Commons.Utils.Json
{
    internal class ProposalCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var proposalCollection = JObject.Load(reader);
                var proposals = proposalCollection.GetValue("Proposals");
                return new ProposalCollection(proposals.ToObject<IEnumerable<Proposal>>());
            }
            if (reader.TokenType == JsonToken.StartArray)
            {
                return new ProposalCollection(serializer.Deserialize<IEnumerable<Proposal>>(reader));
            }
            throw new JsonSerializationException("expected either array or object to deserialize proposal collection");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IProposalCollection).IsAssignableFrom(objectType);
        }
    }
}