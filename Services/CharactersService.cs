using Compendium.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compendium.Services
{
    public class CharactersService
    {
        private readonly IMongoCollection<Character> _characters;
        public CharactersService(ICompendiumDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _characters = database.GetCollection<Character>(settings.CharactersCollectionName);
        }

        public async Task<List<Character>> Get() =>
            (await _characters.FindAsync(Character => true)).ToList();

        public async Task<Character> Get(Guid id) =>
            (await _characters.FindAsync<Character>(Character => Character.Id == id)).FirstOrDefault();

        public async Task<Character> Get(string name) =>
            (await _characters.FindAsync<Character>(Character => Character.Name.ToLower() == name.ToLower())).FirstOrDefault();

        public async Task<Character> Create(Character Character)
        {
            await _characters.InsertOneAsync(Character);
            return Character;
        }

        public async Task Update(Guid id, Character CharacterIn) =>
            await _characters.ReplaceOneAsync(Character => Character.Id == id, CharacterIn);

        public async Task Remove(Character CharacterIn) =>
            await _characters.DeleteOneAsync(Character => Character.Id == CharacterIn.Id);

        public async Task Remove(Guid id) =>
            await _characters.DeleteOneAsync(Character => Character.Id == id);
    }
}
