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
    public class CharactersController : Controller
    {
        private readonly CharactersService _charactersService;
        private readonly InteractionsService _interactionsService;

        private Dictionary<Guid, string> CharacterGuidToName;

        public CharactersController(CharactersService charactersService, InteractionsService interactionsService)
        {
            _charactersService = charactersService;
            _interactionsService = interactionsService;

            var characters = _charactersService.Get().Result.ToList();
            CharacterGuidToName = characters.ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task<ActionResult> Index()
        {
            return View(await _charactersService.Get());
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var character = await _charactersService.Get(id);
            ViewBag.Interactions = await _interactionsService.Get(character);
            ViewData["CharacterGuidToName"] = CharacterGuidToName;
            return View(character);
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var character = await _charactersService.Create(new Character()
                {
                    Id = Guid.NewGuid(),
                    Ability = collection["Ability"],
                    Details = collection["Details"],
                    Edition = (Character.GameEdition)Enum.Parse(typeof(Character.GameEdition), collection["Edition"]),
                    Name = collection["Name"],
                    Type = (Character.CharacterType)Enum.Parse(typeof(Character.CharacterType), collection["Type"]),
                });

                var characters = _charactersService.Get().Result.ToList();
                CharacterGuidToName = characters.ToDictionary(x => x.Id, x => x.Name);

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
            return View(await _charactersService.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(Guid id, [Bind("Id,Ability,Details,Edition,Name,Type")] Character character)
        {
            try
            {
                await _charactersService.Update(id, character);

                var characters = _charactersService.Get().Result.ToList();
                CharacterGuidToName = characters.ToDictionary(x => x.Id, x => x.Name);

                return await Task.FromResult(RedirectToAction(nameof(Index)));
            }
            catch
            {
                return await Task.FromResult(View());
            }
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await _charactersService.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id, IFormCollection collection)
        {
            try
            {
                await _charactersService.Remove(id);

                var characters = _charactersService.Get().Result.ToList();
                CharacterGuidToName = characters.ToDictionary(x => x.Id, x => x.Name);

                return await Task.FromResult(RedirectToAction(nameof(Index)));
            }
            catch
            {
                return await Task.FromResult(View());
            }
        }
    }
}
