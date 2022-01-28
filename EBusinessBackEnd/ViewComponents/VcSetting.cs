using EBusinessBackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBusinessBackEnd.ViewComponents
{
    public class VcSetting:ViewComponent
    {
        private readonly AppDbContext _context;

        public VcSetting(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _context.Settings.FirstOrDefaultAsync();


            return View(model);
        }

    }
}
