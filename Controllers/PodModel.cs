using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;

namespace openhack.k8s.api.Models
{
    public static class PodModel
    {
        public static V1Pod CreatePod(string id)
        {

            var spec = new V1PodSpec()
            {
                Containers = new List<V1Container>() {
                    new V1Container() {
                        Name = $"minecraft-pod-{id}",
                        Image = "openhack/minecraft-server:2.0",
                        Ports = new List<V1ContainerPort>() {
                            new V1ContainerPort() { ContainerPort = 25565},
                            new V1ContainerPort() { ContainerPort = 25575},
                        },
                        Env = new List<V1EnvVar>() {
                            new V1EnvVar() { Name = "EULA", Value = "TRUE" },
                        }
                    }
                }
            };

            var metadata = new V1ObjectMeta()
            {
                Name = $"minecraft-pod-{id}",
                Labels = new Dictionary<string, string>() { { "pod-name", $"minecraft-pod-{id}" } }
            };
            return new V1Pod(V1Pod.KubeApiVersion, V1Pod.KubeKind, metadata, spec);
        }
    }
}