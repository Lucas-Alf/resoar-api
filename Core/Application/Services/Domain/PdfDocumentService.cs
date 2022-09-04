using System.Runtime.InteropServices;
using Application.Interfaces.Services.Domain;
using Docnet.Core;
using Docnet.Core.Models;
using SkiaSharp;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace Application.Services.Domain
{
    public class PdfDocumentService : IPdfDocumentService
    {
        public string ExtractText(byte[] file)
        {
            using (PdfDocument document = PdfDocument.Open(file))
            {
                string documentText = "";
                var textExtractor = NearestNeighbourWordExtractor.Instance;
                foreach (Page page in document.GetPages())
                {
                    documentText += (String.Join(" ", page.GetWords(textExtractor).Select(x => x.Text)).Replace("- ", "") + " ");
                }

                return documentText;
            }
        }

        public byte[] GenerateThumbnail(byte[] file)
        {
            using (var docNet = DocLib.Instance)
            using (var docReader = docNet.GetDocReader(file, new PageDimensions(1080, 1920)))
            using (var pageReader = docReader.GetPageReader(0))
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();
                var rawBytes = pageReader.GetImage();

                using (var inStream = new MemoryStream(rawBytes))
                using (var imgStream = new SKManagedStream(inStream))
                using (var skData = SKData.Create(inStream))
                {
                    var bitmap = ArrayToImage(rawBytes, width, height);
                    SKData png = bitmap.Encode(SKEncodedImageFormat.Png, 100);
                    return png.ToArray();
                }
            }
        }

        private static SKBitmap ArrayToImage(byte[] pixelArray, int width, int height)
        {
            SKBitmap bitmap = new();
            GCHandle gcHandle = GCHandle.Alloc(pixelArray, GCHandleType.Pinned);
            SKImageInfo info = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

            IntPtr ptr = gcHandle.AddrOfPinnedObject();
            int rowBytes = info.RowBytes;
            bitmap.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });

            return bitmap;
        }
    }
}