using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Demo2
{
    public class SqlServerViaDockerCRIU : IDisposable
    {
        private readonly string _imageName;
        private readonly DockerClient _docker;
        private string _contId;
        
        public SqlServerViaDockerCRIU(string imageName) 
        {
            _imageName = imageName;
            _docker = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"))
                        .CreateClient();
        }
        
        public async Task Start()
        {
            _contId = await RunContainer();
            
            async Task<string> RunContainer()
            {
                var contParams = new CreateContainerParameters
                {
                    Image = _imageName,
                    HostConfig = new HostConfig
                    {
                        AutoRemove = true,
                        Privileged = true,
                        Tmpfs = new Dictionary<string, string>
                            { ["/run"] = "rw" },
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                            { ["1433/tcp"] = new[] { new PortBinding { HostPort = "1433" } } }
                    },
                    ExposedPorts =new Dictionary<string, EmptyStruct>
                        { ["1433/tcp"] = default },
                    Env = new[] { "STOP_AFTER=1m", "RUN_FAST=1" },
                };
                
                var cont = await _docker.Containers
                    .CreateContainerAsync(contParams);
                
                await _docker.Containers
                    .StartContainerAsync(cont.ID, new ContainerStartParameters());
                
                return cont.ID;
            }
        }
        
        public void Dispose() 
        {
            if (_contId != null)
            {
                _docker.Containers
                    .KillContainerAsync(_contId, new ContainerKillParameters());
            }
        }
    }
}