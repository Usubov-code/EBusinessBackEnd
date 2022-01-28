using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBusinessBackEnd.Data;
using EBusinessBackEnd.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EBusinessBackEnd.Areas.admin.Controllers
{
    [Area("admin")]
    public class TeamsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Teams
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Teams.Include(t => t.Social);
            return View(await appDbContext.ToListAsync());
        }

        // GET: admin/Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Social)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: admin/Teams/Create
        public IActionResult Create()
        {
            ViewData["SocialId"] = new SelectList(_context.Socials, "Id", "Icon");
            return View();
        }

        // POST: admin/Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (ModelState.IsValid)
            {
                if (team.ImageFile.ContentType == "image/jpeg" || team.ImageFile.ContentType=="image/png")
                {
                    if (team.ImageFile.Length<=3104478)
                    {
                        string fileName = Guid.NewGuid() + "-" + team.ImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream=new FileStream(filePath,FileMode.Create))
                        {


                            team.ImageFile.CopyTo(stream);
                            team.Image = fileName;
                                   _context.Add(team);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                        }
                        
                    else
                    {
                        ModelState.AddModelError("", "3mb dan boyuk olmaz!");
                        return View();

                    }


                }
                else
                {

                    ModelState.AddModelError("", "Yalniz png ve jpg");
                    return View();



                }


            }
            ViewData["SocialId"] = new SelectList(_context.Socials, "Id", "Icon", team.SocialId);
            return View(team);
        }

        // GET: admin/Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var team = await _context.Teams.FindAsync(id);

        
            if (team == null)
            {
                return NotFound();
            }
            ViewData["SocialId"] = new SelectList(_context.Socials, "Id", "Icon", team.SocialId);
            return View(team);
        }

        // POST: admin/Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = Guid.NewGuid() + "-" + team.ImageFile.FileName;
                    string Oldfilepath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                    if (System.IO.File.Exists(Oldfilepath))
                    {
                        System.IO.File.Delete(Oldfilepath);
                    }

                    if (team.ImageFile.ContentType == "image/jpeg" || team.ImageFile.ContentType == "image/png")
                    {
                        if (team.ImageFile.Length <= 3104478)
                        {
                            //string fileName = Guid.NewGuid() + "-" + team.ImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {

                                await team.ImageFile.CopyToAsync(stream);
                                team.Image = fileName;

 _context.Update(team);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                            }
                           
                        }
                        else
                        {
                            ModelState.AddModelError("", "3mb dan boyuk olmaz!");
                            return View();

                        }


                    }
                    else
                    {

                        ModelState.AddModelError("", "Yalniz png ve jpg");
                        return View();



                    }



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
              
            }
            ViewData["SocialId"] = new SelectList(_context.Socials, "Id", "Icon", team.SocialId);
            return View(team);
        }

        // GET: admin/Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Social)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: admin/Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var team = await _context.Teams.FindAsync(id);
       
            string filepath= Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", team.Image);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

           

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
