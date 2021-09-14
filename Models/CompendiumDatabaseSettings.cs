namespace Compendium.Models
{
    public class CompendiumDatabaseSettings : ICompendiumDatabaseSettings
    {
        public string CharactersCollectionName { get; set; }
        public string InteractionsCollectionName { get; set; }
        public string SourcesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICompendiumDatabaseSettings
    {
        string CharactersCollectionName { get; set; }
        string InteractionsCollectionName { get; set; }
        string SourcesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
