//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Movie.Models;
//using Movie.Repository;

//namespace Movie.ControllerWed
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DirectorsController : ControllerBase
//    {
//        private readonly IDirectorsRepository _directorRepository;

//        public DirectorsController(IDirectorsRepository directorRepository)
//        {
//            _directorRepository = directorRepository;
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetDirectorById(int id)
//        {
//            var director = await _directorRepository.GetDirectorByIdAsync(id);
//            if (director == null)
//                return NotFound(new { message = "Director not found" });

//            return Ok(director);
//        }
//    }
//}
