using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaManager_X.Models.Cinema;

namespace CinemaManager_X.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cinemaDbContext = _context.Movies.Include(m => m.Producer);
            return View(await cinemaDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public IActionResult Create()
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", movie.ProducerId);
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", movie.ProducerId);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", movie.ProducerId);
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByTitle(string searchTitle)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Producer);

            if (!string.IsNullOrEmpty(searchTitle))
            {
                movies = movies.Where(m => m.Title.Contains(searchTitle));
            }

            return View(await movies.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> SearchByGenre(string searchGenre)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Producer);

            if (!string.IsNullOrEmpty(searchGenre))
            {
                movies = movies.Where(m => m.Genre.Contains(searchGenre));
            }

            return View(await movies.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> SearchByYear2(string searchGenre, string searchTitle)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Producer);

            if (!string.IsNullOrEmpty(searchGenre) && searchGenre != "All")
            {
                movies = movies.Where(m => m.Genre.Contains(searchGenre));
            }

            if (!string.IsNullOrEmpty(searchTitle))
            {
                movies = movies.Where(m => m.Title.Contains(searchTitle));
            }

            var genres = await _context.Movies
                .Select(m => m.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            ViewData["Genres"] = new SelectList(genres);
            ViewData["SearchGenre"] = searchGenre;
            ViewData["SearchTitle"] = searchTitle;

            return View(await movies.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MoviesAndTheirProds()
        {
            var moviesWithProds = from m in _context.Movies
                                 join p in _context.Producers on m.ProducerId equals p.Id
                                 select new { Movie = m, Producer = p };

            return View(await moviesWithProds.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MoviesAndTheirProds_UsingModel()
        {
            var result = from m in _context.Movies
                        join p in _context.Producers on m.ProducerId equals p.Id
                        select new
                        {
                            MovieId = m.Id,
                            MovieTitle = m.Title,
                            MovieGenre = m.Genre,
                            ProducerId = p.Id,
                            ProducerName = p.Name,
                            ProducerNationality = p.Nationality,
                            ProducerEmail = p.Email
                        };

            return View(await result.ToListAsync());
        }
    }
}
