/*
 * Copyright 2014 Technische Universitšt Darmstadt
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
 *    - Sebastian Proksch
 */

using System;
using System.IO;
using System.Net.Http;
using JetBrains.Util;
using KaVE.JetBrains.Annotations;
using KaVE.Utils.Assertion;
using Newtonsoft.Json;
using Messages = KaVE.VsFeedbackGenerator.Properties.SessionManager;

namespace KaVE.VsFeedbackGenerator.Utils
{
    public class HttpPublisher : IPublisher
    {
        private readonly Uri _hostAddress;
        private readonly IIoUtils _ioUtils;

        public HttpPublisher([NotNull] Uri hostAddress)
        {
            _hostAddress = hostAddress;
            _ioUtils = Registry.GetComponent<IIoUtils>();
        }

        public void Publish(MemoryStream stream)
        {
            Asserts.That(stream.CanRead);
            var content = CreateMultipartContent(stream, "tmp.zip");
            var response = _ioUtils.TransferByHttp(content, _hostAddress);
            var json = response.Content.ReadAsStringAsync().Result;

            Asserts.Not(json.IsNullOrEmpty(), Messages.ServerResponseEmpty);

            ExportResult<object> res = null;
            try
            {
                res = Deserialize(json);
            }
            catch (Exception)
            {
                Asserts.Fail(Messages.ServerResponseIncorrentFormat, json);
            }

            Asserts.NotNull(res, Messages.ServerResponseIncompatible);
            Asserts.That(res.Status == State.Ok, Messages.ServerResponseRequestFailure, res.Message);
        }

        private static HttpContent CreateMultipartContent([NotNull] MemoryStream stream, [NotNull] string name)
        {
            return new MultipartFormDataContent
            {
                {new ByteArrayContent(stream.ToArray()), "file", name}
            };
        }

        private static ExportResult<object> Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<ExportResult<object>>(json);
        }

        internal enum State
        {
            Ok,
            Fail
        }

        private class ExportResult<T>
        {
            [UsedImplicitly]
            public State Status;

            [UsedImplicitly]
            public string Message;

            [UsedImplicitly]
            public T Data;
        }
    }
}