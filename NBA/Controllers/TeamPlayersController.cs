using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBA.Models;

namespace NBA.Controllers
{
    public class TeamPlayersController : Controller
    {
        private readonly NbaContext _context;

        public TeamPlayersController(NbaContext context)
        {
            _context = context;
        }

        // GET: TeamPlayers
        public async Task<IActionResult> Index()
        {
            var nbaContext = _context.TeamPlayers.Include(t => t.Player).Include(t => t.Team);
            return View(await nbaContext.ToListAsync());
        }

        // GET: TeamPlayers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeamPlayers == null)
            {
                return NotFound();
            }

            var teamPlayer = await _context.TeamPlayers
                .Include(t => t.Player)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            return View(teamPlayer);
        }

        // GET: TeamPlayers/Create
        public IActionResult Create()
        {
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerId");
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId");
            return View();
        }

        // POST: TeamPlayers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamId,PlayerId")] TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamPlayer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerId", teamPlayer.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", teamPlayer.TeamId);
            return View(teamPlayer);
        }

        // GET: TeamPlayers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeamPlayers == null)
            {
                return NotFound();
            }

            var teamPlayer = await _context.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return NotFound();
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerId", teamPlayer.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", teamPlayer.TeamId);
            return View(teamPlayer);
        }

        // POST: TeamPlayers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,PlayerId")] TeamPlayer teamPlayer)
        {
            if (id != teamPlayer.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamPlayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamPlayerExists(teamPlayer.PlayerId))
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
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerId", teamPlayer.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", teamPlayer.TeamId);
            return View(teamPlayer);
        }

        // GET: TeamPlayers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeamPlayers == null)
            {
                return NotFound();
            }

            var teamPlayer = await _context.TeamPlayers
                .Include(t => t.Player)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            return View(teamPlayer);
        }

        // POST: TeamPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeamPlayers == null)
            {
                return Problem("Entity set 'NbaContext.TeamPlayers'  is null.");
            }
            var teamPlayer = await _context.TeamPlayers.FindAsync(id);
            if (teamPlayer != null)
            {
                _context.TeamPlayers.Remove(teamPlayer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamPlayerExists(int id)
        {
          return (_context.TeamPlayers?.Any(e => e.PlayerId == id)).GetValueOrDefault();
        }
    }
}
