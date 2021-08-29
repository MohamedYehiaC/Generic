using Microsoft.AspNetCore.Mvc;
using App.Common.Interfaces.Logger;
using App.Core.Interfaces.Services;
using System.Threading.Tasks;
using App.Core.Models;
using System;
using App.Common.Services.ExceptionHandler;
using App.Common.Resources;
using App.Core.Utility;
using Microsoft.AspNetCore.Authorization;

namespace App.API.Controllers.AdminAPI
{
    [Authorize]
    public class UsageController : ControllerBase
	{
        private readonly Ilogger logger;
        private readonly IUsageService usageService;
        private readonly IUserRoleService userRoleService;


        public UsageController(Ilogger logger, IUsageService usageService, IUserRoleService userRoleService)
        {
            this.logger = logger;
            this.usageService = usageService;
            this.userRoleService = userRoleService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                logger.Info($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: GetAll");
                var models = await usageService.GetAll<UsageModel>();
                return Ok(new { Result = models.Result, Count = models.TotalItems });
            }
            catch (Exception ex)
            {
                logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Get Failed with the following error {ex.ToString()}");
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UsageModel model)
        {
            try
            {
                if (await this.userRoleService.UserIsInRole(Roles.ITTeam))
                {
                    logger.Info($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Add(Name:{model.UsageDisplayName})");
                var models = await usageService.Add(model);
                return Ok(models);
                }
                else
                {
                    throw new ExceptionService(System.Net.HttpStatusCode.Unauthorized, ExceptionResource.UserUnAuthorized);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Get Failed with the following error {ex.ToString()}");
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UsageModel model)
        {
            try
            {
                if (await this.userRoleService.UserIsInRole(Roles.ITTeam))
                {
                    logger.Info($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Edit(Id:{model.Id},Name:{model.UsageDisplayName}");
                var models = await usageService.Update(model);
                return Ok(models);
                }
                else
                {
                    throw new ExceptionService(System.Net.HttpStatusCode.Unauthorized, ExceptionResource.UserUnAuthorized);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Get Failed with the following error {ex.ToString()}");
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                logger.Info($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Delete(Id:{id})");

                bool notDeletable = usageService.GetById<UsageModel>(id).Result.NotDeletable;
                if (notDeletable)
                {
                    return BadRequest("Cannot delete system value.");
                }

                await usageService.SoftDelete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Get Failed with the following error {ex.ToString()}");
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByKey(int key)
        {
            try
            {
                if (key != null)
                {

                    logger.Info($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: GetAllByKey");
                    var model = await usageService.FindSingle<UsageModel>(a => a.Id == key);
                    return Ok(new { Result = model });
                }
                else
                {
                    return BadRequest("No defined key");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: GetAllByKey Failed with the following error {ex.ToString()}");
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByName(string UsageName)
        {
            try
            {
                logger.Info($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: GetByName");
                var model = await usageService.GetByName<UsageModel>(UsageName);
                return Ok(new { ID = model.Id });
            }
            catch (Exception ex)
            {
                logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}: Get Failed with the following error {ex.ToString()}");
                throw ex;
            }
        }
    }
}
