using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;

namespace openhack.k8s.api.Models
{
    public static class ServiceModel
    {
        public static V1Service CreateService(string id)
        {

            var spec = new V1ServiceSpec()
            {
                Type = "LoadBalancer",
                Selector = new Dictionary<string, string>() { { "pod-name", $"minecraft-pod-{id}" } },
                Ports = new List<V1ServicePort>() {
                    new V1ServicePort(80, "padrao", protocol: "TCP", targetPort: 25565),
                    new V1ServicePort(443, "ssl", protocol: "TCP", targetPort: 25575),
                    new V1ServicePort(25565, "padrao-mc", protocol: "TCP", targetPort: 25565),
                    new V1ServicePort(25575, "rcon", protocol: "TCP", targetPort: 25575),
                }
            };

            var metadata = new V1ObjectMeta() { Name = $"minecraft-service-{id}" };
            return new V1Service(V1Service.KubeApiVersion, V1Service.KubeKind, metadata, spec);
        }
    }
}