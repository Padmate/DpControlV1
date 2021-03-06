﻿using DpControl.Domain.Entities;
using DpControl.Models;
using DpControl.Utility;
using DpControl.ViewModels.Manage;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManageController:BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public ManageController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        #region User
        // GET: /Manage/Index
        [HttpGet]
        public IActionResult IndexForUser()
        {
            return View();
        }

        /// <summary>
        /// Get User Paging Data
        /// </summary>
        /// <returns></returns>
        public IActionResult GetUserPageData(JQueryDataTableParams jqueryTableParams)
        {
            
            var allUsers = _userManager.Users.ToList();
            //GET ALL
            if (jqueryTableParams.iDisplayLength == -1)
            {
                jqueryTableParams.iDisplayLength = allUsers.Count;
            }
            var pageUserData = _userManager.Users.Skip(jqueryTableParams.iDisplayStart).Take(jqueryTableParams.iDisplayLength).ToList();
            PageResult<ApplicationUser> pageResult = new PageResult<ApplicationUser>(jqueryTableParams.sEcho,allUsers.Count, pageUserData);

            return Json(pageResult);
        }

        public async Task<ActionResult> GetRolesByUserName(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            var roles = await _userManager.GetRolesAsync(user);
            return Json(roles);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            Message message = new Message();
            message.Success = true;

            if (!ModelState.IsValid)
            {
                message.Success = false;
                message.Content = ModelStateError();
                return Json(message);
            }
            
            //用户的角色
            string[] roles = model.Roles.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries);
            //判断这些角色是否属于角色表
            foreach (string role in roles)
            {
                bool isRoleExisted = await _roleManager.RoleExistsAsync(role);
                if (!isRoleExisted)
                {
                    message.Success = false;
                    message.Content = "Role：" + role + "is not exist in system!";
                    return Json(message);
                    
                }
            }

            //新增用户
            var user = new ApplicationUser { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                message.Success = false;
                message.Content = result.Errors.First().Description;
                    return Json(message);

            }
            //把用户添加到角色
            var curruser = await _userManager.FindByNameAsync(model.UserName);
            result = await _userManager.AddToRolesAsync(curruser, roles);

            
            return Json(message);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public async Task<IActionResult> BachDeleteByUserId(string UserIds)
        {
            Message message = new Message();
            message.Success = true;

            List<string> userIds = JsonHandler.UnJson<List<string>>(UserIds);
            foreach (string userid in userIds)
            {
                var user = await _userManager.FindByIdAsync(userid);
                var result = await _userManager.DeleteAsync(user);

            }

            return Json(message);
        }
        #endregion
        #region Role
        // GET: /Manage/Index
        [HttpGet]
        public IActionResult IndexForRole()
        {
            return View();
        }

        /// <summary>
        /// Get Role Paging Data
        /// </summary>
        /// <returns></returns>
        public IActionResult GetRolePageData(JQueryDataTableParams jqueryTableParams)
        {
            //HttpRequest rq = Request;
            //StreamReader srRequest = new StreamReader(rq.Body);
            //String strReqStream = srRequest.ReadToEnd();
            //JQueryDataTableParams jqueryTableParam = JsonHandler.UnJson<JQueryDataTableParams>(strReqStream);

            var allRoles = _roleManager.Roles.ToList();
            //GET ALL
            if (jqueryTableParams.iDisplayLength == -1)
            {
                jqueryTableParams.iDisplayLength = allRoles.Count;
            }

            var pageRoleData = _roleManager.Roles.Skip(jqueryTableParams.iDisplayStart).Take(jqueryTableParams.iDisplayLength).ToList();
            PageResult<IdentityRole> pageResult = new PageResult<IdentityRole>(jqueryTableParams.sEcho,allRoles.Count, pageRoleData);
            

            return Json(pageResult);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> CreateRole(string RoleName)
        {
            
            Message message = new Message();
            message.Success = true;
            if (string.IsNullOrEmpty(RoleName))
            {
                message.Success = false;
                message.Content = "Role Name is required";
                return Json(message);
            }

            //新增角色
            IdentityRole adminRole = new IdentityRole { Name = RoleName, NormalizedName = RoleName.ToUpper() };
            var result = await _roleManager.CreateAsync(adminRole);
            if (!result.Succeeded)
            {
                message.Success = false;
                message.Content = result.Errors.First().Description;
                return Json(message);

            }
            return Json(message);
            
        }

        public async Task<IActionResult> BachDeleteByRoleId(string RoleIds)
        {
            Message message = new Message();
            message.Success = true;

            List<string> roleIds = JsonHandler.UnJson<List<string>>(RoleIds);
            foreach (string roleid in roleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleid);
                var result = await _roleManager.DeleteAsync(role);

            }

            return Json(message);
        }

        #endregion
    }
}
