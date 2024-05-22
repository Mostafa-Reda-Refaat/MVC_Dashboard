using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.Models;
using System.Collections.Generic;

namespace PL.Controllers
{
	//// And Roles
	//[Authorize(Roles = "Admin")]
	//[Authorize(Roles = "HR")]

	// Or Roles
	[Authorize(Roles = "Admin , HR")]
    public class RolesController : Controller
	{
		private readonly RoleManager<ApplicationRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ILogger<RolesController> logger;

		public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<RolesController> logger)
        {
			this.roleManager = roleManager;
			this.userManager = userManager;
			this.logger = logger;
		}
        public async Task<IActionResult> Index(string SearchValue)
		{
			List<ApplicationRole> roles;
			if(string.IsNullOrEmpty(SearchValue))
				roles = await roleManager.Roles.ToListAsync();
			else
				roles = await roleManager.Roles.Where(role => role.Name.Trim().ToLower().Contains(SearchValue.Trim().ToLower())).ToListAsync();

			return View(roles);
		}

		public IActionResult Create()
		{
			return View(new ApplicationRole());
		}

		[HttpPost]
		public async Task<IActionResult> Create(ApplicationRole role)
		{
			if(ModelState.IsValid)
			{
				var result = await roleManager.CreateAsync(role);

				if(result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                {
                    //logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
            }

			return View(role);
        }

		public async Task<IActionResult> Details(string id, string viewName = "Details")
		{
			if (id is null)
				return BadRequest();

			var user = await roleManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			return View(viewName, user);
		}

		public async Task<IActionResult> Update(string id)
		{
			return await Details(id, "Update");
		}

		[HttpPost]
		public async Task<IActionResult> Update(string id, ApplicationRole applicationRole)
		{
			if (id != applicationRole.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				var role = await roleManager.FindByIdAsync(id);

				role.Name = applicationRole.Name;
				role.NormalizedName = applicationRole.Name.ToUpper();

				var result = await roleManager.UpdateAsync(role);

				if (result.Succeeded)
					return RedirectToAction("Index");

				foreach (var error in result.Errors)
				{
					//logger.LogError(error.Description);
					ModelState.AddModelError("", error.Description);
				}
			}

			return View(applicationRole);
		}

		public async Task<IActionResult> Delete(string id)
		{
			var user = await roleManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			var result = await roleManager.DeleteAsync(user);

			if (result.Succeeded)
				return RedirectToAction("Index");

			foreach (var error in result.Errors)
			{
				//logger.LogError(error.Description);
				ModelState.AddModelError("", error.Description);
			}

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> AddOrRemoveUsers(string roleId)
		{
			var role = await roleManager.FindByIdAsync(roleId);

			if (role is null) 
				return NotFound();

			ViewBag.RoleId = roleId;

			var usersInRole = new List<UserInRoleViewModel>();

			var users = await userManager.Users.ToListAsync();

			foreach(var user in users)
			{
				var userInRole = new UserInRoleViewModel
				{
					UserId = user.Id,
					UserName = user.UserName
				};

				if (await userManager.IsInRoleAsync(user, role.Name))
					userInRole.IsSelected = true;
				else
					userInRole.IsSelected = false;

				usersInRole.Add(userInRole);
			}
			
			return View(usersInRole);
		}

		[HttpPost]
		public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewModel> users)
		{
			var role = await roleManager.FindByIdAsync (roleId);

			if(role is null)
				return NotFound();

			if(ModelState.IsValid)
			{
				foreach(var user in users)
				{
					var appUser = await userManager.FindByIdAsync(user.UserId);

					if(appUser != null)
					{
						if (user.IsSelected && !await userManager.IsInRoleAsync(appUser, role.Name))
							await userManager.AddToRoleAsync(appUser, role.Name);
						else if (!user.IsSelected && await userManager.IsInRoleAsync(appUser, role.Name))
							await userManager.RemoveFromRoleAsync(appUser, role.Name);
					}
				}
				return RedirectToAction("Update", new { id = roleId });
			}

			return View(users);
		}

	}
}
