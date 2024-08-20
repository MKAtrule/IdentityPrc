using IdentityPrc.Models;
using IdentityPrc.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityPrc.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AdministratorController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public AdministratorController(RoleManager<Role> role, UserManager<User> userManager)
        {
            _roleManager = role;
            _userManager = userManager;
        }
        public async Task<IActionResult> ListRoles()
        {
           var roles= await _roleManager.Roles.ToListAsync();  
            return View(roles); 
        }
        public IActionResult Create()
        {
            return View();  
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool existRole = await _roleManager.RoleExistsAsync(model.RoleName);
                if (existRole)
                {
                    ModelState.AddModelError("", "Role already exist");
                }
                else
                {
                    Role role = new Role
                    { 
                    Name=model.RoleName,
                    Description=model.Description,  
                    };
                   var result= await _roleManager.CreateAsync(role);
                    if(result.Succeeded)
                    {
                        ModelState.Clear();
                        return RedirectToAction("ListRoles");
                    }                    
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("",item.Description);
                        }
                        return View();
                    


                }
              
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var roleExist= await _roleManager.FindByIdAsync(Id);
            if(roleExist is null)
            {
                return View("Error");
            }
            var model = new EditRoleViewModel
            {
                    Id=roleExist.Id,
                    RoleName=roleExist.Name,
                    Description=roleExist.Description,
            };
            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ModelState.AddModelError("", "role with this id not exist");
                }
                role.Name=model.RoleName;
                role.Description=model.Description;
                var result= await _roleManager.UpdateAsync(role);   
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }


            }
            return View(model);

        }
        public async Task<IActionResult> Delete(string Id)
        {
            var role= await _roleManager.FindByIdAsync(Id); 
            if(role!=null)
            {
            var result= await   _roleManager.DeleteAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
            }
            else
            {
                ModelState.AddModelError("", "role with this id not exist");
            }
            return View(); 
        }
      
    }
}
