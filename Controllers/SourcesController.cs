using Compendium.Models;
using Compendium.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Compendium.Controllers
{
    public class SourcesController : Controller
    {
        private readonly SourcesService _sourcesService;

        public SourcesController(SourcesService sourcesService)
        {
            _sourcesService = sourcesService;
        }

        public async Task<ActionResult> Index()
        {
            return View(await _sourcesService.Get());
        }

        public async Task<ActionResult> Details(Guid id)
        {
            return View(await _sourcesService.Get(id));
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
                var source = await _sourcesService.Create(new Source()
                {
                    Id = Guid.NewGuid(),
                    Name = collection["Name"],
                    Description = collection["Description"],
                    Link = collection["Link"]
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
            return View(await _sourcesService.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(Guid id, [Bind("Id,Name,Description,Link")] Source source)
        {
            try
            {
                await _sourcesService.Update(id, source);
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
            return View(await _sourcesService.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id, IFormCollection collection)
        {
            try
            {
                await _sourcesService.Remove(id);
                return await Task.FromResult(RedirectToAction(nameof(Index)));
            }
            catch
            {
                return await Task.FromResult(View());
            }
        }
    }
}
