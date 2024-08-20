using IdentityPrc.Models;
using IdentityPrc.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityPrc.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public UserController(RoleManager<Role> role, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _roleManager = role;
            _userManager = userManager;
        }
        public async Task<IActionResult> UserList()
        {
            var usersList = await _userManager.Users.ToListAsync();
            return View(usersList);
        }
        public async Task<IActionResult> Edit(string UserId)
        {
            var user= await _userManager.FindByIdAsync(UserId); 
            if(user == null)
            {
                return View("Error");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims= await _userManager.GetClaimsAsync(user);
            var model = new EditUserViewModel
             { 
            Id = user.Id,
            UserName=user.UserName,
            Bio=user.Bio,
            Claims=userClaims.Select(c=>c.Value).ToList(),
            Roles=userRoles            
             };
            return View(model);

        }
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string UserId)
        {
            var user= await _userManager.FindByIdAsync(UserId);
            if(user == null )
            {

            return View("Error");
            }
            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;
            var model = new List<UserRolesViewModel>();
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Description = role.Description
                };
                if(await _userManager.IsInRoleAsync(user,role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected= false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            List<string> roleNames=model.Where(x=>x.IsSelected).Select(r=>r.RoleName).ToList();
            if (roleNames.Any())
            {
              

                result = await _userManager.AddToRolesAsync(user, roleNames);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot Add Selected Roles to User");
                    return View(model);
                }
            }
            return RedirectToAction("Edit", new { UserId = UserId });
        }
        public async Task<IActionResult> ManageUserClaims(string UserId)
        {
            var user= await _userManager.FindByIdAsync(UserId);
            if(user == null)
            {
                return View("Error");
            }
            ViewBag.UserName = user.UserName;
            var model = new UserClaimsViewModel
            {
                UserId = UserId
            };
            var existingClaims= await _userManager.GetClaimsAsync(user);
            foreach (Claim claim in ClaimsStore.GetAllClaims())
            {
                var userClaim = new UserClaim
                {
                ClaimType = claim.Type,
                };
                if(existingClaims.Any(c=>c.Type==claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);              

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user= await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return View("Error");
            }
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }
            var allSelectedClaims=  model.Claims.Where(c=>c.IsSelected)
                                               .Select(c=>new Claim(c.ClaimType,c.ClaimType))
                                               .ToList();
            if (allSelectedClaims.Any())
            {
                result = await _userManager.AddClaimsAsync(user, allSelectedClaims);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected claims to user");
                    return View(model);
                }
            }
            return RedirectToAction("Edit", new { UserId = model.UserId });
        }
        
    }
}
