using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;

namespace AVSSalesExplorer.Services
{
    public class ImageResizeService
    {
        private readonly ImageFormatManager _formatManager;
        private readonly IDictionary<string, string> _mimeTypeMappings
            = new Dictionary<string, string>()
            {
                ["png"] = "image/png",
                ["jpeg"] = "image/jpeg",
                ["jpg"] = "image/jpeg",
                ["bmp"] = "image/bmp",
                ["gif"] = "image/gif"                
            };

        public ImageResizeService()
        {
            _formatManager = new ImageFormatManager();
            _formatManager.AddImageFormat(PngFormat.Instance);
            _formatManager.AddImageFormat(JpegFormat.Instance);
            _formatManager.AddImageFormat(BmpFormat.Instance);
            _formatManager.AddImageFormat(GifFormat.Instance);
        }

        public async Task<byte[]> GetResizedImageStreamAsync(Stream inputStream, string fileExtension, int maxWidth)
        {
            string contentType = GetMimeTypeByExtension(fileExtension);
            if (string.IsNullOrEmpty(contentType))
            {
                throw new InvalidOperationException("Unsupported image type.");
            }

            var imageFormat = _formatManager.FindFormatByMimeType(contentType);
            if (imageFormat == null)
            {
                throw new InvalidOperationException("Invalid image type.");
            }

            var buffer = new MemoryStream();
            await inputStream.CopyToAsync(buffer).ConfigureAwait(false);
            buffer.Position = 0;

            var currentImage = await Image.LoadWithFormatAsync(buffer).ConfigureAwait(false);
            if (currentImage.Image.Width > maxWidth)
            {
                // resize
                var aspect = (decimal)currentImage.Image.Width / currentImage.Image.Height;
                var newWidth = maxWidth;
                var newHeight = (int)Math.Round(newWidth / aspect, 0);
                currentImage.Image.Mutate(o => o.Resize(new ResizeOptions { Size = new Size(newWidth, newHeight) }));

                var ms = new MemoryStream();
                currentImage.Image.Save(ms, imageFormat);
                ms.Position = 0;
                buffer.Dispose();

                return ms.ToArray();
            }

            buffer.Position = 0;

            return buffer.ToArray();
        }

        private string GetMimeTypeByExtension(string fileExtension)
        {
            if (_mimeTypeMappings.ContainsKey(fileExtension))
            {
                return _mimeTypeMappings[fileExtension];
            }
            
            return string.Empty;
        }
    }
}