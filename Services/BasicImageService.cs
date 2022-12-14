using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NeoMovie.Services.Interfaces;

namespace NeoMovie.Services
{
    public class BasicImageService : IImageService
    {
        private readonly IHttpClientFactory _httpClient;

        public BasicImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory;
        }

        public async Task<byte[]> EncodeImageURLAsync(string imageURL)
        {
            var client = _httpClient.CreateClient();
            var response = await client.GetAsync(imageURL);
            using Stream stream = await response.Content.ReadAsStreamAsync();

            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile poster)
        {
            if (poster == null) return null;

            using var ms = new MemoryStream();
            await poster.CopyToAsync(ms);
            return ms.ToArray();
        }

        public string DecodeImage(byte[] poster, string contentType)
        {
            if (poster == null) return null;

            var posterImage = Convert.ToBase64String(poster);
            return $"data:{contentType};base64,{posterImage}";
        }
    }
}