using Compendium.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compendium.Services
{
    public class SourcesService
    {
        private readonly IMongoCollection<Source> _sources;
        public SourcesService(ICompendiumDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _sources = database.GetCollection<Source>(settings.SourcesCollectionName);
        }

        public async Task<List<Source>> Get() =>
            (await _sources.FindAsync(Source => true)).ToList();

        public async Task<Source> Get(Guid id) =>
            (await _sources.FindAsync<Source>(Source => Source.Id == id)).FirstOrDefault();

        public async Task<Source> Get(string name) =>
            (await _sources.FindAsync<Source>(Source => Source.Name.ToLower() == name.ToLower())).FirstOrDefault();

        public async Task<Source> Create(Source Source)
        {
            await _sources.InsertOneAsync(Source);
            return Source;
        }

        public async Task Update(Guid id, Source SourceIn) =>
            await _sources.ReplaceOneAsync(Source => Source.Id == id, SourceIn);

        public async Task Remove(Source SourceIn) =>
            await _sources.DeleteOneAsync(Source => Source.Id == SourceIn.Id);

        public async Task Remove(Guid id) =>
            await _sources.DeleteOneAsync(Source => Source.Id == id);
    }
}
