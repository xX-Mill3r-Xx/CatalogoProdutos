using CatalogoProdutos.Data;
using CatalogoProdutos.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoProdutos.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly DataContext _contexto;
        private readonly IWebHostEnvironment _webHostEnvi;

        public ProdutosController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _contexto = dataContext;
            _webHostEnvi = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Produtos.ToListAsync());
        }

        [HttpGet]
        public IActionResult NovoProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NovoProduto(Produto produto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if(foto != null)
                {
                    string diretorio = Path.Combine(_webHostEnvi.WebRootPath, "img");
                    string nomeFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (FileStream fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        produto.Foto = "~/img/" + nomeFoto;
                    }
                    await _contexto.Produtos.AddAsync(produto);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarProduto(int produtoId)
        {
            Produto produto = await _contexto.Produtos.FindAsync(produtoId);
            TempData["FotoProduto"] = produto.Foto;
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(Produto produto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string diretorio = Path.Combine(_webHostEnvi.WebRootPath, "img");
                    string nomeFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (FileStream fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        produto.Foto = "~/img/" + nomeFoto;
                    }
                }
                else
                {
                    produto.Foto = TempData["FotoProduto"].ToString();
                }
                _contexto.Produtos.Update(produto);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        [HttpPost]
        public async Task<JsonResult> Excluir(int id)
        {
            Produto produto = await _contexto.Produtos.FindAsync(id);
            var passos = _contexto.Passos.Where(p => p.ProdutoId == id);
            if (await passos.CountAsync()>0)
            {
                _contexto.Passos.RemoveRange(passos);
            }
            _contexto.Produtos.Remove(produto);
            await _contexto.SaveChangesAsync();
            return Json(new { });
        }
    }
}
