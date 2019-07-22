using System;
using System.IO.Pipes;

namespace Sorted.Tracking.Whippet.Test.SqlServerViaDocker
{
    public static class DockerDeducer
    {
        public static DockerDeduction Deduce()
            => TryDockerHostEnvVar()
               ?? TryWindowsPipe()
               ?? ThrowNothingFound();

        private static DockerDeduction TryDockerHostEnvVar()
        {
            var dockerHostVar = Environment.GetEnvironmentVariable("DOCKER_HOST");
            if (dockerHostVar == null) return null;
            
            var apiUri = new Uri(dockerHostVar);
            return new DockerDeduction(
                dockerApi: apiUri,
                exposedHost: apiUri.Host);
        }

        private static DockerDeduction TryWindowsPipe()
        {
            try
            {
                const string WindowsPipeName = "npipe://./pipe/docker_engine";
                using (var str = new NamedPipeClientStream(WindowsPipeName))
                {
                    str.Connect(100);
                }
                return new DockerDeduction(
                    dockerApi: new Uri(WindowsPipeName), 
                    exposedHost: "127.0.0.1");
            }
            catch (TimeoutException ex)
            {
                return null;
            }
        }

        private static DockerDeduction ThrowNothingFound()
        {
            throw new ApplicationException("Can't find available Docker API");
        }
    }

    public class DockerDeduction
    {
        public Uri DockerApi { get; }
        public string ExposedHost { get; }
        
        public DockerDeduction(Uri dockerApi, string exposedHost)
        {
            DockerApi = dockerApi;
            ExposedHost = exposedHost;
        }
    }
}