using System.Diagnostics;
using ListaDeTarefas.Data;
using ListaDeTarefas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefas.Controllers
    {
    public class HomeController : Controller
        {
        private readonly AppDBContext _context;
        public HomeController(AppDBContext context)
            {
            _context = context;
            }
        public IActionResult Index(string id)
            {
            var filtros = new FiltrosModel(id);

            ViewBag.Filtros = filtros;
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Status = _context.Statuses.ToList();
            ViewBag.VencimentoValores = FiltrosModel.VencimentoValoresFiltro;

            IQueryable<TabelaModel> consulta = _context.Tarefas.Include(c => c.Categoria).Include(s => s.Status);

            if (filtros.TemCategoria)
                {
                consulta = consulta.Where(t => t.CategoriaId == filtros.CategoriaId);
                }
            if (filtros.TemStatus)
                {
                consulta = consulta.Where(v => v.StatusId == filtros.StatusId);
                }
            if (filtros.TemVencimento)
                {
                var hoje = DateTime.Today;

                if (filtros.EPassado)
                    {
                    consulta = consulta.Where(t => t.DataVencimento < hoje);
                    }
                if (filtros.EHoje)
                    {
                    consulta = consulta.Where(t => t.DataVencimento == hoje);
                    }
                if (filtros.EFuturo)
                    {
                    consulta = consulta.Where(t => t.DataVencimento > hoje);
                    }

                }


            var tarefas = consulta.OrderBy(t => t.DataVencimento).ToList();




            return View(tarefas);
            }

        [HttpPost]
        public IActionResult Filtrar(string[] filtro)
            {
            string id = string.Join('-', filtro);
            return RedirectToAction("Index", new { ID = id });
            }
        }
    }