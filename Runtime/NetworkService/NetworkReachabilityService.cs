using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using StripedArts.Unity.Core.Logging.Core;
using StripedArts.Unity.Core.Logging.Core.Factory;
using StripedArts.Unity.Core.Logging.Core.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace TestCore.Code.Test.Runtime.NetworkService
{
    public static class NetworkReachabilityService
    {
        private static readonly ILogProvider Logger = LogFactory.Produce(typeof(NetworkReachabilityService), new DChannel("Network"));
        
        private static readonly string[] TestUrls =
        {
            "https://www.google.com",
            "https://www.yahoo.com",
            "https://www.bing.com",
            "https://www.wikipedia.org",
            "https://github.com"
        };

        /// <summary>
        /// Checks if the internet is available by pinging a set of predefined URLs.
        /// </summary>
        /// <returns>Returns true if at least one URL responds successfully; otherwise, false.</returns>
        public static async UniTask<bool> IsInternetAvailable()
        {
            using var cts = new CancellationTokenSource(5000);
            var tasks = TestUrls.Select(url => PingUrlAsync(url, cts.Token)).ToList();
        
            await foreach (var result in UniTask.WhenEach(tasks))
            {
                if (result.IsCompletedSuccessfully)
                {
                    if (result.Result)
                    {
                        Logger.LogInfo("Ping successful from URL.");
                        cts.Cancel();
                        return true;
                    }
                }
                else if (result.IsFaulted)
                {
                    Logger.LogWarning($"Ping failed with exception: {result.Exception}");
                }
            }
        
            Logger.LogInfo("All ping attempts failed.");
            return false;
        }

        /// <summary>
        /// Attempts to ping a given URL to check for network availability.
        /// </summary>
        /// <param name="url">The URL to ping.</param>
        /// <param name="token">Cancellation token to abort the request if necessary.</param>
        /// <returns>Returns true if the request succeeds; otherwise, false.</returns>
        private static async UniTask<bool> PingUrlAsync(string url, CancellationToken token)
        {
            using var request = UnityWebRequest.Get(url);
            request.timeout = 3;

            try
            {
                await request.SendWebRequest().WithCancellation(token);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Logger.LogInfo($"Ping to {url} success.");
                    return true;
                }

                Logger.LogWarning($"Ping to {url} failed. Error: {request.error}");
                return false;
            }
            catch (OperationCanceledException)
            {
                Logger.LogWarning($"Ping to {url} was canceled.");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Ping to {url} failed: {ex.Message}");
                return false;
            }
        }
    }
}