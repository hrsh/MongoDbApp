using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MongoDbApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Mongo("AddressBook");
            //db.InsertRecord("Person", new Person
            //{
            //    FirstName = "Maryam",
            //    LastName = "Shojaie"
            //});

            //Console.WriteLine("Inserted!");

            var list = db.GetAll<Person>("Person");
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Id:D}: {item.FirstName} {item.LastName}");
            }

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

        public List<T> GetAll<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T FindById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return collection.Find(filter).FirstOrDefault();
        }

        public void Update<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.ReplaceOne(Builders<T>.Filter.Eq("Id", id), record, new ReplaceOptions { IsUpsert = true });
        }


    }
}
