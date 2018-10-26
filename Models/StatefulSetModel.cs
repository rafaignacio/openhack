using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;

namespace openhack.k8s.api.Models
{
    public static class StatefulSetModel
    {
        public static V1StatefulSet CreateStatefulSet(string id)
        {

            var spec = new V1StatefulSetSpec()
            {
                ServiceName = $"minecraft-ss-{id}",
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
                                VolumeMounts = new List<V1VolumeMount>() {
                                    new V1VolumeMount() {
                                        MountPath = "/data",
                                        Name = "dados",
                                    }
                                },
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
                VolumeClaimTemplates = new List<V1PersistentVolumeClaim>() {
                    new V1PersistentVolumeClaim() {
                        Metadata = new V1ObjectMeta(name: "dados"),
                        Spec = new V1PersistentVolumeClaimSpec() {
                            AccessModes = new []{ "ReadWriteOnce" },
                            StorageClassName = "minecraft-storage-class",
                            Resources = new V1ResourceRequirements() {
                                Requests = new Dictionary<string, ResourceQuantity>() {
                                    { "storage", new ResourceQuantity("1Gi") }
                                }
                            }
                        }
                    }
                }
            };

            var metadata = new V1ObjectMeta() { Name = $"minecraft-sta-set-{id}" };
            return new V1StatefulSet(V1StatefulSet.KubeApiVersion, V1StatefulSet.KubeKind, metadata, spec);
        }
    }
}