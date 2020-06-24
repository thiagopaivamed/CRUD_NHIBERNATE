using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD_NHIBERNATE.Models;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;

namespace CRUD_NHIBERNATE.Controllers
{
    public class CarrosController : Controller
    {
        private readonly ISession _session;

        public CarrosController(ISession session)
        {
            _session = session;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _session.Query<Carro>().ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Carro carro)
        {
            if(ModelState.IsValid)
            {
                using(ITransaction transaction = _session.BeginTransaction())
                {
                    await _session.SaveAsync(carro);
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(carro);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int carroId)
        {
            return View(await _session.GetAsync<Carro>(carroId));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int carroId, Carro carro)
        {
            if (carroId != carro.CarroId)
                return NotFound();

            if (ModelState.IsValid)
            {
               using(ITransaction transaction = _session.BeginTransaction())
                {
                    await _session.SaveOrUpdateAsync(carro);
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(carro);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int carroId)
        {
            var carro = await _session.GetAsync<Carro>(carroId);

            using(ITransaction transaction = _session.BeginTransaction())
            {
                await _session.DeleteAsync(carro);
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
