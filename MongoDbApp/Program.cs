using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace MongoDbApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Mongo("AddressBook");
            db.InsertRecord("Person", new Person
            {
                FirstName = "Maryam",
                LastName = "Shojaie"
            });

            Console.WriteLine("Inserted!");
            Console.ReadLine();
        }
    }

    public class Person
    {
        [BsonId]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Mongo
    {
        private IMongoDatabase db;

        public Mongo(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }
    }
}
