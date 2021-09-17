using Compendium.Models;
using Compendium.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compendium.Controllers
{
    public class InteractionsController : Controller
    {
        private readonly CharactersService _charactersService;
        private readonly InteractionsService _interactionsService;
        private readonly SourcesService _sourcesService;

        private Dictionary<Guid, string> CharacterGuidToName;

        public InteractionsController(CharactersService charactersService, InteractionsService interactionsService, SourcesService sourcesService)
        {
            _charactersService = charactersService;
            _interactionsService = interactionsService;
            _sourcesService = sourcesService;

            var characters = _charactersService.Get().Result.ToList();
            CharacterGuidToName = characters.ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task<ActionResult> Index()
        {
            ViewData["CharacterGuidToName"] = CharacterGuidToName;
            return View(await _interactionsService.Get());
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var interaction = await _interactionsService.Get(id);
            ViewBag.AffectedCharacter1 = CharacterGuidToName[interaction.AffectedCharacter1];
            ViewBag.AffectedCharacter2 = CharacterGuidToName[interaction.AffectedCharacter2];
            ViewBag.Source = (await _sourcesService.Get(interaction.Source)).Name;
            return View(interaction);
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            ViewBag.Characters = (await _charactersService.Get()).OrderBy(x => x.Name);
            ViewBag.Sources = (await _sourcesService.Get()).OrderBy(x => x.Name);
            return await Task.FromResult(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var interaction = await _interactionsService.Create(new Interaction()
                {
                    Id = Guid.NewGuid(),
                    Explanation = collection["Explanation"],
                    AffectedCharacter1 = Guid.Parse(collection["AffectedCharacter1"]),
                    AffectedCharacter2 = Guid.Parse(collection["AffectedCharacter2"]),
                    Source = (await _sourcesService.Get(collection["Source"])).Id
                });

                return await Task.FromResult(RedirectToAction(nameof(Index)));
            }
            catch
            {
                return await Task.FromResult(View());
            }
        }

        [Authorize]
        public async Task<ActionResult> Edit(Guid id)
        {
            var interaction = await _interactionsService.Get(id);
            ViewBag.AffectedCharacter1 = interaction.AffectedCharacter1;
            ViewBag.AffectedCharacter2 = interaction.AffectedCharacter2;
            ViewBag.Characters = (await _charactersService.Get()).OrderBy(x => x.Name);
            ViewBag.Sources = (await _sourcesService.Get()).OrderBy(x => x.Name);
            ViewBag.SelectedSource = interaction.Source;
            return View(interaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var interaction = await _interactionsService.Get(id);
                interaction.AffectedCharacter1 = Guid.Parse(collection["AffectedCharacter1"]);
                interaction.AffectedCharacter2 = Guid.Parse(collection["AffectedCharacter2"]);
                interaction.Explanation = collection["Explanation"];
                interaction.Source = Guid.Parse(collection["Source"]);

                await _interactionsService.Update(id, interaction);
                return await Task.FromResult(RedirectToAction(nameof(Index))) ;
            }
            catch
            {
                return await Task.FromResult(View());
            }
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            var interaction = await _interactionsService.Get(id);
            ViewBag.AffectedCharacter1 = CharacterGuidToName[interaction.AffectedCharacter1];
            ViewBag.AffectedCharacter2 = CharacterGuidToName[interaction.AffectedCharacter2];
            ViewBag.Source = (await _sourcesService.Get(interaction.Source)).Name;
            return View(interaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id, IFormCollection collection)
        {
            try
            {
                await _interactionsService.Remove(id);
                return await Task.FromResult(RedirectToAction(nameof(Index)));
            }
            catch
            {
                return await Task.FromResult(View());
            }
        }
    }
}
