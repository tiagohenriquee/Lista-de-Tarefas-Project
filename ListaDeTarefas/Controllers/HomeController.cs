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

        [HttpGet]
        public IActionResult Adicionar()
            {
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Status = _context.Statuses.ToList();

            var tarefa = new TabelaModel { StatusId = "aberto" };

            return View(tarefa);
            }

        [HttpPost]
        public IActionResult Adicionar(TabelaModel tarefa)
            {
            if (ModelState.IsValid)
                {
                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();

                return RedirectToAction("Index");

                }
            else
                {
                ViewBag.Categorias = _context.Categorias.ToList();
                ViewBag.Status = _context.Statuses.ToList();

                return View(tarefa);

                }
            }

        [HttpPost]
        public IActionResult Filtrar(string[] filtro)
            {
            string id = string.Join('-', filtro);
            return RedirectToAction("Index", new { ID = id });
            }

        [HttpPost]
        public IActionResult MarcarCompleto([FromRoute] string id, TabelaModel tarefaselecionada)
            {
            tarefaselecionada = _context.Tarefas.Find(tarefaselecionada.Id);

            if (tarefaselecionada != null)
                {
                tarefaselecionada.StatusId = "completo";
                _context.SaveChanges();
                }
            return RedirectToAction("Index", new { ID = id });
            }

        [HttpPost]
        public IActionResult DeletarCompletos(string id)
            {
            var paraDeletar = _context.Tarefas.Where(s => s.StatusId == "completo").ToList();

            foreach (var tarefa in paraDeletar)
                {
                _context.Tarefas.Remove(tarefa);
                }
            _context.SaveChanges();
            return RedirectToAction("Index", new { ID = id });
            }
        }
    }