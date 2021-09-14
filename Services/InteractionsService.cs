using Compendium.Models;
using Microsoft.CodeAnalysis.CSharp;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compendium.Services
{
    public class InteractionsService
    {
        private readonly IMongoCollection<Interaction> _interactions;

        public InteractionsService(ICompendiumDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _interactions = database.GetCollection<Interaction>(settings.InteractionsCollectionName);
        }

        public async Task<List<Interaction>> Get() =>
            (await _interactions.FindAsync(interaction => true)).ToList();

        public async Task<Interaction> Get(Guid id) =>
            (await _interactions.FindAsync<Interaction>(interaction => interaction.Id == id)).FirstOrDefault();

        public async Task<List<Interaction>> Get(Character character) =>
            (await _interactions.FindAsync(interaction => interaction.AffectedCharacter1 == character.Id || interaction.AffectedCharacter2 == character.Id)).ToList();

        public async Task<Interaction> Create(Interaction interaction)
        {
            await _interactions.InsertOneAsync(interaction);
            return interaction;
        }

        public async Task Update(Guid id, Interaction interactionIn) =>
            await _interactions.ReplaceOneAsync(interaction => interaction.Id == id, interactionIn);

        public async Task Remove(Interaction interactionIn) =>
            await _interactions.DeleteOneAsync(interaction => interaction.Id == interactionIn.Id);

        public async Task Remove(Guid id) =>
            await _interactions.DeleteOneAsync(interaction => interaction.Id == id);
    }
}
