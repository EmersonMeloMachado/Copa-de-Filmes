using CopaDeFilmes.Api.Models;
using CopaDeFilmes.Api.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CopaDeFilmes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : Controller
    {

        private readonly IFilmesService _filmesService;

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
                return  Ok(Result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult<IList<Filmes>>> PostAsync(String[] id)
        {
            try
            {
                var Result = await _filmesService.ObterListaFilmesService();
                return Ok(Result.Take(2));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
