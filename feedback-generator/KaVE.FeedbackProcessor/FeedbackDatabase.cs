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
 *    - 
 */
using KaVE.Commons.Model.Events;
using KaVE.Commons.Utils.Reflection;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace KaVE.FeedbackProcessor
{
    internal class FeedbackDatabase {
        private readonly MongoDatabase _database;

        public FeedbackDatabase(string databaseUrl, string databaseName)
        {
            var client = new MongoClient(databaseUrl);
            var server = client.GetServer();
            _database = server.GetDatabase(databaseName);
        }

        public MongoCollection<Developer> GetDeveloperCollection()
        {
            return GetCollection<Developer>();
        } 

        public MongoCollection<IDEEvent> GetEventsCollection()
        {
            var eventsCollection = GetCollection<IDEEvent>();
            EnsureEventIndex(eventsCollection);
            return eventsCollection;
        }

        private MongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T).Name;
            if (!_database.CollectionExists(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
            return _database.GetCollection<T>(collectionName);
        }

        private static void EnsureEventIndex(MongoCollection<IDEEvent> eventsCollection)
        {
            var evtIndex = IndexKeys.Ascending(TypeExtensions<IDEEvent>.GetPropertyName(evt => evt.IDESessionUUID))
                .Ascending(TypeExtensions<IDEEvent>.GetPropertyName(evt => evt.TriggeredAt))
                .Ascending("_t");
            if (!eventsCollection.IndexExists(evtIndex))
            {
                eventsCollection.CreateIndex(
                    evtIndex,
                    // the index is unique, but there can be multiple instance with missing index fields (anonymization)
                    IndexOptions<IDEEvent>.SetUnique(true)
                        .SetSparse(true));
            }
        }
    }
}