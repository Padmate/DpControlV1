﻿using DpControl.Domain.Entities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    /// <summary>
    /// Digest Authentication
    /// </summary>
    public class DigestAuthentication:AbstractAuthentication
    {
        public readonly UserManager<ApplicationUser> _userManager;

        public DigestAuthentication(UserManager<ApplicationUser> userManager)
        {
            base._scheme = "Digest ";
            _userManager = userManager;
        }

        protected override async Task<bool> Login(string headParams, HttpContext httpContext)
        {
            bool loginSuccess = false;
            // logic...

            return loginSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override async Task<string> CheckUserInfo(string headParams,HttpContext httpContext)
        {
            
            var header = DigestHeader.Create(headParams, httpContext.Request.Method);
            string userName = header.UserName;

            if (DigestNonce.IsValid(header.Nonce, header.NounceCounter))
            {
                var user = await _userManager.FindByNameAsync(header.UserName);
                //此处密码需要改为明文密码才可校验
                var password = user.PasswordHash;


                var hash1 = String.Format(
                    "{0}:{1}:{2}",
                    header.UserName,
                    header.Realm,
                    password).ToMd5Hash();

                //查询参数中不能有逗号
                var hash2 = String.Format(
                    "{0}:{1}",
                    header.Method,
                    header.Uri).ToMd5Hash();

                var computedResponse = String.Format(
                    "{0}:{1}:{2}:{3}:{4}:{5}",
                    hash1,
                    header.Nonce,
                    header.NounceCounter,
                    header.Cnonce,
                    "auth",
                    hash2).ToMd5Hash();

                return header.Response.Equals(computedResponse, StringComparison.Ordinal)
                ? userName
                : null;
            }
            return null;

        }

        public override void Challenge(HttpContext httpContext)
        {
            var header = DigestHeader.Unauthorized();
            var parameter = header.ToString();
            base.ApplyChallenge(httpContext,parameter);
        }
        

        private string GetPassword(string userName)
        {
            return userName;
        }

        protected override async Task<ApplicationUser> GetUserInfo(string headParams, HttpContext httpContext)
        {
            ApplicationUser currentUser = null;
            var header = DigestHeader.Create(headParams, httpContext.Request.Method);
            string userName = header.UserName;

            if (DigestNonce.IsValid(header.Nonce, header.NounceCounter))
            {
                currentUser = await _userManager.FindByNameAsync(header.UserName);
               
            }
            return currentUser;
        }
    }
}
