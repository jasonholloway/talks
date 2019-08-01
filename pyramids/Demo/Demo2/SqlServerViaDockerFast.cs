using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Sorted.Tracking.Whippet.Test.SqlServerViaDocker;

namespace Demo2
{
    public class SqlServerViaDockerFast
    {
        private static Lazy<DockerDeduction> _lzDeduced;
        private static Lazy<DockerClient> _lzDocker;
        
        private string _contId;
        private Uri _contUri;

        public string ConnectionString { get; private set; }
        
        static SqlServerViaDockerFast()
        {
            _lzDeduced = new Lazy<DockerDeduction>(DockerDeducer.Deduce);
            _lzDocker = new Lazy<DockerClient>(
                () => new DockerClientConfiguration(_lzDeduced.Value.DockerApi)
                        .CreateClient());
        }
        
        public async Task Start(CancellationToken cancel)
        {
            var docker = _lzDocker.Value;
            
            _contId = await RunContainer();
            _contUri = await GetContainerUri();
            
            ConnectionString =
                $"Server={_contUri.Host},{_contUri.Port};Database=electio_tracking;User Id=sa;Password=Wibble123!;MultipleActiveResultSets=True";
            
            async Task<string> RunContainer()
            {
                var contParams = new CreateContainerParameters
                {
                    Image = "sqlservr-tracking",
                    HostConfig = new HostConfig
                    {
                        AutoRemove = true,
                        Privileged = true,
                        Tmpfs = new Dictionary<string, string>
                            { ["/run"] = "rw" },
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                            { ["1433/tcp"] = new[] { new PortBinding() } }
                    },
                    ExposedPorts =new Dictionary<string, EmptyStruct>
                        { ["1433/tcp"] = default(EmptyStruct) },
                    Env = new[] { "STOP_AFTER=1m", "RUN_FAST=1" },
                };
                
                var cont = await docker.Containers
                    .CreateContainerAsync(contParams, cancel);
                
                await docker.Containers
                    .StartContainerAsync(cont.ID, new ContainerStartParameters(), cancel);
                
                return cont.ID;
            }
            
            async Task<Uri> GetContainerUri()
            {
                var info = await docker.Containers.InspectContainerAsync(_contId, cancel);
                var net = info.NetworkSettings;
                
                var exposedHost = _lzDeduced.Value.ExposedHost;
                
                return new Uri($"tcp://{exposedHost}:{net.Ports["1433/tcp"].First().HostPort}");
            }
        }
        
        public void Close()
        {
            if (_contId != null && _lzDocker.IsValueCreated)
            {
                var docker = _lzDocker.Value;
                docker.Containers
                    .KillContainerAsync(_contId, new ContainerKillParameters());
            }
        }
    }
}