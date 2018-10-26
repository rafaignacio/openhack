using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using openhack.k8s.api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;

namespace openhack.k8s.api.Controllers
{
    [Route("api/servers")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        [HttpGet]
        public ActionResult List()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);

            var list = client.ListNamespacedService("minecraft");

            if (list?.Items?.Count == 0) return NotFound();

            var output = new List<OutputModel>();

            foreach (var item in list.Items)
            {
                var o = new OutputModel();

                o.Nome = item.Metadata.Name;

                if (item.Status?.LoadBalancer?.Ingress?.Count > 0)
                {
                    o.Enderecos = new List<EnderecoModel>();

                    foreach (var i in item.Status.LoadBalancer.Ingress)
                    {
                        using (var wc = new WebClient())
                        {
                            var status = wc.DownloadString($"https://mcapi.us/server/status?ip={i.Ip}");
                            dynamic sta = JObject.Parse(status);

                            var a = sta.status;

                            o.Enderecos.Add(new EnderecoModel()
                            {
                                IP = i.Ip,
                                Nome = i.Hostname,
                                MaxCapacity = sta.players.max,
                                Online = sta.players.now,
                            });
                        }
                    }
                }

                foreach (var e in o.Enderecos)
                {
                    if (item.Spec?.Ports?.Count > 0)
                    {
                        foreach (var p in item.Spec.Ports)
                        {
                            e.Ports += $"{p.Port},";
                        }

                        e.Ports = e.Ports.Substring(0, e.Ports.Length - 2);
                    }
                }

                output.Add(o);
            }

            return new JsonResult(output);
        }

        [HttpPost]
        public ActionResult Create()
        {
            var id = Guid.NewGuid().ToString("N").Substring(0, 5);
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);

            var service = client.CreateNamespacedService(ServiceModel.CreateService(id), "minecraft");
            service.Validate();

            try
            {
                client.CreateNamespacedStatefulSet(StatefulSetModel.CreateStatefulSet(id), "minecraft");
            }
            catch { }

            var pod = client.CreateNamespacedPod(PodModel.CreatePod(id), "minecraft");
            pod.Validate();

            return Created("/servers", new object[] { service, pod });
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);

            client.DeleteNamespacedPod(new V1DeleteOptions(), $"minecraft-pod-{id}", "minecraft");
            client.DeleteNamespacedService(new V1DeleteOptions(), $"minecraft-service-{id}", "minecraft");

            return NoContent();
        }
    }
}