namespace Catalog.API.Entities
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")] // To custom element name in mongoDb
        public string Name { get; set; }

        public string Category { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string ImageFile { get; set; }

        public decimal Price { get; set; }
    }
}
