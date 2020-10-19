using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace DockerReTagImageMcr
{
    class Program
    {
        private static DockerClient DockerClient;

        static void Main(string[] args)
        {
            /*var _credentials = new BasicAuthCredentials("puvasude", "Cre@tiveme13");
            DockerClientConfiguration _config = new DockerClientConfiguration(_credentials);
            DockerClient _client = _config.CreateClient();

            Console.WriteLine("Created client");

            IList<ImagesListResponse> imageList = ListImages(_client).Result;

            foreach(ImagesListResponse res in imageList)
            {
                Console.WriteLine(res.ID);
            }*/

            string[] imageNames = { "appsvcorg/wordpress-alpine-php", "appsvcorg/wordpress-apache-php", "appsvcorg/wordpress-multi-container", };
            DockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();

            foreach(string imageName in imageNames)
            {
                StartCompose(imageName);
            }


           /* if (OperatingSystem.is)
            {
                DockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            }
            else if (OperatingSystem.IsLinux())
            {
                DockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
            }
            else
            {
                throw new Exception("unknown operation system");
            }*/


        }

        private async static Task<IList<ImagesListResponse>> ListImages(DockerClient _client)
        {
            IList<ImagesListResponse> myList = await _client.Images.ListImagesAsync(
                     new Docker.DotNet.Models.ImagesListParameters() { All = true }
             );

            return myList;
        }

       /* public DockerService()
        {
            if (OperatingSystem.IsWindows())
            {
                DockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            }
            else if (OperatingSystem.IsLinux())
            {
                DockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
            }
            else
            {
                throw new Exception("unknown operation system");
            }
        }*/

        public static void StartCompose(string imageName)
        {
            var progress = new Progress<JSONMessage>();
            var task = PullImage(
                new ImagesCreateParameters()
                {
                    FromImage = imageName
                    //Tag = "latest"
                }, null,
                progress);
            task.Wait();

        }


        private static Task PullImage(ImagesCreateParameters imagesCreateParameters, AuthConfig authConfig,
            Progress<JSONMessage> progress)
        {
            return DockerClient.Images.CreateImageAsync(imagesCreateParameters, authConfig, progress);
        }
    }
}
