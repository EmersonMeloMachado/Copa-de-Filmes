using System;
using System.Linq;
using System.Threading.Tasks;
using CopaDeFilmes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using CopaDeFilmes.Api.Business;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CopaDeFilmes.Api.Services.Abstracts;

namespace CopaDeFilmes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : Controller
    {
        private readonly IFilmesService _filmesService;
        public ObservableCollection<Filmes> obterCampeao { get; }

        public FilmesController(IFilmesService filmesService)
        {
            _filmesService = filmesService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IList<Filmes>>> GetAsync()
        {
            try
            {
                var Result = await _filmesService.ObterListaFilmesService();
                return Ok(Result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult<IList<Filmes>> Post([FromBody]IList<Filmes> ListaDeFilmes)
        {
            try
            {

                ConfrontoEntreFilmes Vencedor = new ConfrontoEntreFilmes();
                var Result = Vencedor.ObterVencedor(ListaDeFilmes);
                return Ok(Result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}