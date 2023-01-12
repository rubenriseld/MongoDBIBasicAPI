
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection.Metadata;

namespace MongoDBBasicAPI;

internal class DataAccess
{
    private readonly string ConnectionString;
    private const string DatabaseName = "art_db";
    private const string ArtworksCollection = "artworks";

    public DataAccess()
    {
        var builder = new ConfigurationBuilder();
        builder.AddUserSecrets<DataAccess>();
        var configurationRoot = builder.Build();

        ConnectionString = configurationRoot["MongoDBConnectionString"];
    }

    private IMongoCollection<BsonDocument> ConnectToMongo()
    {
        MongoClient dbClient = new MongoClient(ConnectionString);
        var database = dbClient.GetDatabase(DatabaseName);
        var collection = database.GetCollection<BsonDocument>(ArtworksCollection);
        return collection;
    }

    public void AddArtwork()
    {
        try
        {
            Console.Write("Enter artwork index: ");
            int index = int.Parse(Console.ReadLine());
            if(CheckIndex(index) == true)
            {
                Console.WriteLine("an artwork with this index already exists, please try again");
                return;
            }

            Console.Write("Enter artwork title: ");
            string title = (Console.ReadLine());

            Console.Write("Enter artwork description: ");
            string desc = (Console.ReadLine());

            Console.Write("Enter artwork price: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Enter artwork sale status (true/false): ");
            bool sold = bool.Parse(Console.ReadLine());

            var document = new BsonDocument
            {
                {"index", index },
                {"title", title },
                {"description", desc },
                {"price", price },
                {"sold", sold }
            };

            var collection = ConnectToMongo();
            collection.InsertOne(document);

            Console.WriteLine("added artwork successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("wrong input, please try again");
            return;
        }

    }
    public void GetArtworkByIndex(int index)
    {
        if (CheckIndex(index) == true)
        {
            try
            {
                var indexFilter = Builders<BsonDocument>.Filter.Eq("index", index);
                var collection = ConnectToMongo();
                var document = collection.Find(indexFilter).FirstOrDefault();
                Console.WriteLine(document.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("could not find artwork with input index, please try again");
                return;
            }
        }
        else Console.WriteLine("index not found, please try again");

    }
    public void GetAllArtworks()
    {//Task<List<BsonDocument>>
        var collection = ConnectToMongo();

        var documents = collection.Find(new BsonDocument()).ToList();
        foreach (var item in documents)
        {
            Console.WriteLine(item.ToString());
        }
    }

    public void UpdateArtworkByIndex(int index)
    {
        if (CheckIndex(index) == true)
        {
            try
            {
                Console.Write("Enter artwork title: ");
                string title = (Console.ReadLine());

                Console.Write("Enter artwork description: ");
                string desc = (Console.ReadLine());

                Console.Write("Enter artwork price: ");
                double price = double.Parse(Console.ReadLine());

                Console.Write("Enter artwork sale status (true/false): ");
                bool sold = bool.Parse(Console.ReadLine());


                var update = Builders<BsonDocument>.Update.Set("title", title)
                    .Set("description", desc)
                    .Set("price", price)
                    .Set("sold", sold);

                var collection = ConnectToMongo();

                var updateFilter = Builders<BsonDocument>.Filter.Eq("index", index);
                collection.UpdateOne(updateFilter, update);

                Console.WriteLine("updated artwork successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("wrong input, please try again");
                return;
            }
        }
        else Console.WriteLine("index not found, please try again");
    }

    public void DeleteArtworkByIndex(int index)
    {
        if (CheckIndex(index) == true)
        {
            var collection = ConnectToMongo();
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("index", index);
            collection.DeleteOne(deleteFilter);
        }
        else Console.WriteLine("index not found, please try again");
    }

    public bool CheckIndex(int index)
    {
        var checkFilter = Builders<BsonDocument>.Filter.Eq("index", index);
        var collection = ConnectToMongo();
        int count = (int)collection.Find(checkFilter).CountDocuments();

        if (count > 0) return true;
        else return false;
    }
}
