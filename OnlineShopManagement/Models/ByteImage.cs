using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Libmongocrypt;
using MongoDB.Bson.Serialization.Attributes;
using System.Windows.Media;
using SE104_OnlineShopManagement.Models.ModelEntity;

namespace SE104_OnlineShopManagement.Models
{
   
    public class ByteImage:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string ID { get; set; }
        [BsonElement("obID")]
        public string obID { get; private set; }
        [BsonElement("data")]
        public Byte[] data { get; private set; }

        public ByteImage(string id, Byte[] stringdata, string privateid ="")
        {
            this.obID = id;
            this.data = stringdata;
            this.ID = privateid;
        }

        public ByteImage(string id, BitmapImage bitmapImage, string privateid = "")
        {
            obID = id;
            this.ID = privateid;
            Byte[] bytedata;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                bytedata = ms.ToArray();

            }

            data = bytedata;
        }

        public void convertImageToByte(BitmapImage bitmapImage)
        {
            Byte[] bytedata;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                bytedata = ms.ToArray();
                
            }

            data = bytedata;
        }

        public BitmapImage convertByteToImage()
        {
            if(data != null && data.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(data))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            else
            {
                Console.WriteLine("Data is null");
                return null;
            }
        }
    }
}
