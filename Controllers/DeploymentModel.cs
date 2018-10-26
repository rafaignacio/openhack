using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;

namespace openhack.k8s.api.Models
{
    public static class DeploymentModel
    {
        public static V1Deployment CreateDeployment(string id)
        {

            var spec = new V1DeploymentSpec()
            {
                Selector = new V1LabelSelector()
                {
                    MatchLabels = new Dictionary<string, string>() { { "pod-name", $"minecraft-pod-{id}" } }
                },
                Template = new V1PodTemplateSpec()
                {
                    Metadata = new V1ObjectMeta()
                    {
                        Labels = new Dictionary<string, string>() { { "pod-name", $"minecraft-pod-{id}" } },
                    },
                    Spec = new V1PodSpec()
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
                    },
                },
            };

            var metadata = new V1ObjectMeta() { Name = $"minecraft-deploy-{id}" };
            return new V1Deployment(V1Deployment.KubeApiVersion, V1Deployment.KubeKind, metadata, spec);
        }
    }
}