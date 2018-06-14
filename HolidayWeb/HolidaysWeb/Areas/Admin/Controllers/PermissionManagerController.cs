using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Holidays.Web.Areas.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class PermissionManagerController : Controller
    {
        //
        // GET: /Admin/PermissionManage/
        #region 用户操作
        

        public ActionResult UserManagerView()
        {
            return View();
        }
        /// <summary>
        /// 判断是否存在同名用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool ExistRepeatNameUser(string loginName,long userId, out string password)
        {
            bool exist = false;
            password = null;
            Expression<Func<User, bool>> expression = h => h.IsDeleted == false && h.LoginName == loginName&&h.Id!=userId;
            IList<User> users = OperateContext.Current.BLLSession.IUserBLL.GetListBy(expression);
            if (users.Count() > 0||loginName=="Admin")
            {
                exist = true;
                password = users.Single().Password;
            }

            return exist;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser(UserViewModel userViewModel)
        {
            var currentUserId = userViewModel.Id;
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            //验证传入的数据
            string msg = Validate.ValidateString(
                new CustomValidate() { FieldName = "用户名", FieldValue = userViewModel.LoginName, MinLength = 1, MaxLength = 25, IsRequired = true },
                new CustomValidate() { FieldName = "密码", FieldValue = userViewModel.Password, MinLength = 6, IsRequired = true },
                new CustomValidate() { FieldName = "用户真实姓名", FieldValue = userViewModel.UserRealName, MinLength = 1, MaxLength = 25 });

            if (msg == null)
            {
                string tmpPassword;
                int addSuccess;
                if (ExistRepeatNameUser(userViewModel.LoginName,userViewModel.Id, out tmpPassword))
                {
                    msg = "已经存在同名用户！";
                    state = HolidaysWebConst.HolidaysWebConst.FAIL;
                    return OperateContext.Current.RedirectAjax(state, msg, null, null);
                }

                else if (userViewModel.Id > 0)//编辑用户
                {
                    var user = OperateContext.Current.BLLSession.IUserBLL.GetListBy(h => h.Id == userViewModel.Id).FirstOrDefault();
                    //房东不能编辑自己,只能编辑子账号
                    if (user.ParentId==-1&&OperateContext.Current.User!=null)
                    {
                        return OperateContext.Current.RedirectAjax(HolidaysWebConst.HolidaysWebConst.FAIL, "您没有权限对自己进行修改，请联系管Admin用户！", null, null);

                    }
                    user.LoginName = userViewModel.LoginName;
                    user.Password = userViewModel.Password==user.Password?userViewModel.Password:Common.Encrypt.MD5Encrypt32( userViewModel.Password).ToUpper();
                    user.UserRealName = userViewModel.UserRealName;
                    user.Email = userViewModel.Email;
                    user.Description = userViewModel.Description;
                    addSuccess = OperateContext.Current.BLLSession.IUserBLL.Modify(user, "LoginName", "Password", "UserRealName", "Description", "Email");
                    if (addSuccess != 1)//添加失败
                    {
                        state = HolidaysWebConst.HolidaysWebConst.FAIL;
                        msg = "添加失败";
                    }
                    else
                    {
                        msg = "addSuccess";
                    }
                }
                else//添加用户
                {
                    //user.Role = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(o => o.IsDeleted == false &&
                    //(from a in user.Role
                    // select a.Id).ToList().Contains(o.Id));
                    var user = new User()
                    {

                        GUIID = Guid.NewGuid(),
                        LoginName = userViewModel.LoginName,
                        Password = Common.Encrypt.MD5Encrypt32(userViewModel.Password).ToUpper(),
                        UserRealName = userViewModel.UserRealName,
                        Description = userViewModel.Description,
                        Email = userViewModel.Email,
                        ParentId = OperateContext.Current.User==null?-1:OperateContext.Current.User.Id,
                        CreateTime = DateTime.Now,
                        IsDeleted = false
                    };
                    addSuccess = OperateContext.Current.BLLSession.IUserBLL.Add(user);
                    if (addSuccess != 1)//添加失败
                    {
                        state = HolidaysWebConst.HolidaysWebConst.FAIL;
                        msg = "添加失败";
                    }
                    else
                    {
                        currentUserId = user.Id;
                        msg = "addSuccess";
                    }
                }
               
                currentUserId = Convert.ToInt64(currentUserId);
                //获取当前用户的所有用户角色表中的记录并删除
                var currentUser = OperateContext.Current.BLLSession.IUserBLL.GetListBy(s => s.IsDeleted == false && s.Id == currentUserId).FirstOrDefault();
                //当前用户下的所有角色记录
                var allUserRoleWithCurrentUser = OperateContext.Current.BLLSession.IUserRoleBLL.GetListBy(s => s.UserId == currentUser.GUIID);
                //没有选中角色则把所有中间表记录删除
                if (string.IsNullOrEmpty(userViewModel.RoleIds))
                {
                   int delRes= OperateContext.Current.BLLSession.IUserRoleBLL.DelBy(s => s.UserId==currentUser.GUIID);

                }
                else
                {
                    //要添加的角色列表
                    List<Guid> listIds = new List<Guid>();
                    foreach (var tmpRoleId in userViewModel.RoleIds.Split(','))
                    {
                        long tmpId = Convert.ToInt64(tmpRoleId);
                        listIds.Add(OperateContext.Current.BLLSession.IRoleBLL.GetListBy(s => s.IsDeleted == false && s.Id == tmpId).FirstOrDefault().GUID);
                    }
                    //删除所有当前用户的UserRole中间表记录
                    List<Guid> toDeleteIds = (from a in allUserRoleWithCurrentUser
                                              select a.RoleId).ToList();
                    int delResult = OperateContext.Current.BLLSession.IUserRoleBLL.DelBy(s => toDeleteIds.Contains(s.RoleId) && s.UserId == currentUser.GUIID);

                    //添加用户角色
                    foreach (var roleId in listIds)
                    {
                        UserRole userRole = new UserRole();
                        userRole.UserId = currentUser.GUIID;
                        userRole.RoleId = roleId;
                        userRole.MemberType = (int)MemberEnum.MemberEnum.User;
                        var result = OperateContext.Current.BLLSession.IUserRoleBLL.Add(userRole);
                        if (result != 1)
                        {
                            state = HolidaysWebConst.HolidaysWebConst.FAIL;
                            msg = "添加角色失败";
                            return OperateContext.Current.RedirectAjax(state, msg, null, null);
                        }
                    }
                }
              

            }
            return OperateContext.Current.RedirectAjax(state, msg, null, null);
        }
        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult QueryUserById(long Id)
        {
           var test= OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(s => s.ID > -1);

            
            foreach (var item in test)
            {
                if (string.IsNullOrEmpty(item.LoginAccount))
                {
                    continue;
                }
                User user = new User();
                user.GUIID = Guid.NewGuid();
                user.IsDeleted = false;
                user.LoginName = item.LoginAccount;
                user.Password = item.LoginPwd;
                user.UserRealName = item.Username;
                user.ParentId = -1;
                user.AccountId = item.ID;
                user.CreateTime = item.CreateTime;
                user.Description = "主账号，该账号可以分配子账号！";
                user.Email = item.Email;
                OperateContext.Current.BLLSession.IUserBLL.Add(user);
            }
            return Content("ok");
            //string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            //var msg = string.Empty;
            //Expression<Func<User, bool>> expression = h => h.IsDeleted == false && h.Id == Id;
            //var user = OperateContext.Current.BLLSession.IUserBLL.GetListBy(expression);


            //if (user.Count <= 0)
            //{
            //    state = HolidaysWebConst.HolidaysWebConst.FAIL;
            //    msg = "未能查询到该用户";
            //}
            ////获取所有角色并标记当前用户所拥有角色
            //var allRoles = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(s => s.IsDeleted == false);
            //List<RoleWithSelectedViewModel> rvList = new List<RoleWithSelectedViewModel>();
            //foreach (var role in allRoles)
            //{
            //    RoleWithSelectedViewModel rv = new RoleWithSelectedViewModel();
            //    rv.CreateTime = role.CreateTime;
            //    rv.Description = role.Description;
            //    rv.Name = role.Name;
            //    rv.RoleId = role.Id;
            //    //var isSelected = OperateContext.Current.BLLSession.IUserRoleBLL.GetListBy(s => s.UserId == user.SingleOrDefault().GUIID && s.RoleId == role.GUID).Count>0;
            //    bool isSelected = false;
            //    var includesRoles = OperateContext.Current.BLLSession.IUserRoleBLL.GetListBy(s => s.RoleId == role.GUID);
            //    foreach (var includeRole in includesRoles)
            //    {
            //        if (includeRole.UserId == user.FirstOrDefault().GUIID)
            //        {
            //            isSelected = true;
            //            rv.IsSelected = isSelected;
            //        }
            //    }
            //    rvList.Add(rv);
            //}
            //QueryUserByIdViewModel qv = new QueryUserByIdViewModel();
            //qv.User = user.FirstOrDefault();
            //qv.RoleWithSelectedViewModels = rvList;
            //return OperateContext.Current.RedirectAjax(state, msg, qv, null);
        }
        /// <summary>
        /// 删除单个用户
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUser(long Id)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            var msg = "删除成功";
            #region 删除用户条件
            var currentUser = OperateContext.Current.User;
            if (currentUser!=null&& currentUser.ParentId==-1)
            {
                return OperateContext.Current.RedirectAjax(HolidaysWebConst.HolidaysWebConst.FAIL, "你没有删除自己的权限，请联系Admin管理员", null, null);
            }
            Expression<Func<User, bool>> expression = h => h.Id == Id;
            #endregion
            int result = OperateContext.Current.BLLSession.IUserBLL.DelBy(expression);
            if (result != 1)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                msg = "删除失败";
            }
            return OperateContext.Current.RedirectAjax(state, msg, null, null);
        }
        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="userIds">多用户Id字符串，用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchDeleteUsers(string userIds)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            string msg = string.Empty;
            List<long> userIdList = new List<long>();
            foreach (var userId in userIds.Split(','))
            {
                if (string.IsNullOrEmpty(userId))
                {
                    continue;
                }
                userIdList.Add(Convert.ToInt64(userId));
            }
            if (userIdList.Count() <= 0)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                msg = "请选择要删除的用户！";
            }
            Expression<Func<User, bool>> whereExpression = h => h.IsDeleted == false;
            #region 删除用户条件

            foreach (var Id in userIdList)
            {
                whereExpression.And(h => h.Id == Id);
            }

            #endregion
            int result = OperateContext.Current.BLLSession.IUserBLL.DelBy(whereExpression);
            if (result != 1)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                msg = "删除失败";
            }
            return OperateContext.Current.RedirectAjax(state, string.Empty, null, null);
        }
        /// <summary>
        /// 根据登录名或用户真实姓名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserList(string userName, int page = 1, int row = 10)
        {
            var whereString = PredicateBuilder.True<User>();
            #region 查询条件
            Expression<Func<User, bool>> stateExpression = h => h.IsDeleted == false;
            whereString = whereString.And(stateExpression);
            if (!string.IsNullOrEmpty(userName))
            {
                Expression<Func<User, bool>> userNameExpression = h => h.LoginName.Contains(userName) || h.UserRealName.Contains(userName);
                whereString = whereString.And(userNameExpression);
            }
            #endregion
            int rowCount = 0;
            var currentPermUser = OperateContext.Current.User;
            IList<User> users = OperateContext.Current.BLLSession.IUserBLL.GetPagedList(page, row, ref rowCount, whereString, m => m.Id);
            if (currentPermUser!=null)//非admin
            {
                users = users.Where(s => s.IsDeleted == false && s.ParentId == currentPermUser.Id).ToList();
                users.Add(currentPermUser);
            }
        
            PageModel<User> model = PageModel<User>.GetPageModel(users, page, rowCount, row);
            
            //var tkvemp = model.Data.Select(o => new { o.CreateTime, o.Description, o.Email, o.Id, o.IsDeleted, o.LoginName, o.Password, o.RoleId, o.UserRealName, Role = o.Role.Select(h => new { h.Name, h.Id }) });
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None
            };

            var ret = JsonConvert.SerializeObject(model, setting);
            //JavaScriptSerializer js = new JavaScriptSerializer()
            return Json(ret);
        }
        #endregion
        #region 角色操作
        

        public ActionResult RoleManagerView()
        {
            return View();
        }
        /// <summary>
        /// 判断是否存在同名角色
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool ExistRepeatNameRole(string roleName,long roleId)
        {
            bool exist = false;
            Expression<Func<Role, bool>> expression = h => h.IsDeleted == false && h.Name == roleName&&h.Id!=roleId;
            IList<Role> roles = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(expression);
            if (roles.Count() > 0)
            {
                exist = true;
            }

            return exist;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="userInfo">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRole(AddRoleViewModel addRoleView)
        {
            var CurrentRoleId = addRoleView.Id;
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            //验证传入的数据
            string msg = Validate.ValidateString(
                new CustomValidate() { FieldName = "角色名", FieldValue = addRoleView.Name, MinLength = 1, MaxLength = 25, IsRequired = true });

            if (msg == null)
            {
                int addSuccess;
                if (ExistRepeatNameRole(addRoleView.Name,addRoleView.Id))
                {
                    msg = "已经存在同名角色！";
                    state = HolidaysWebConst.HolidaysWebConst.FAIL;
                    return OperateContext.Current.RedirectAjax(state,msg, null, null);
                }

                else if (addRoleView.Id > 0)//编辑角色
                {
                    Role role = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(s => s.IsDeleted == false && s.Id == addRoleView.Id).FirstOrDefault();
                    role.Name = addRoleView.Name;
                    role.Description = addRoleView.Description;
                    addSuccess = OperateContext.Current.BLLSession.IRoleBLL.Modify(role, "Name", "Description");
                    if (addSuccess != 1)//添加失败
                    {
                        state = HolidaysWebConst.HolidaysWebConst.FAIL;
                        msg = "添加失败";
                        return OperateContext.Current.RedirectAjax(msg, state, null, null);

                    }
                }
                else//添加角色
                {
                    Role role = new Role()
                    {
                        GUID = Guid.NewGuid(),
                        Name = addRoleView.Name,
                        Description = addRoleView.Description,
                        CreateTime = DateTime.Now,
                        IsDeleted = false
                    };
                    addSuccess = OperateContext.Current.BLLSession.IRoleBLL.Add(role);
                    if (addSuccess != 1)//添加失败
                    {
                        state = HolidaysWebConst.HolidaysWebConst.FAIL;
                        msg = "添加失败";
                        return OperateContext.Current.RedirectAjax(msg, state, null, null);

                    }
                    else
                    {
                        CurrentRoleId = role.Id;
                    }
                }
                if (!string.IsNullOrEmpty(addRoleView.MenuIds))
                {
                    //当前角色
                    var currentRole = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(s => s.IsDeleted == false && s.Id == CurrentRoleId).FirstOrDefault();
                    //找到当前角色的所有角色菜单关系表中的所有记录并删除
                    var RolePermWithCurrentRole = OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s => s.MemberId == currentRole.GUID);
                    List<Guid> funcIds = (from a in RolePermWithCurrentRole
                                          select a.FuncId).ToList();
                    int delResult = OperateContext.Current.BLLSession.IFuncPermissionBLL.DelBy(s => s.MemberId == currentRole.GUID && funcIds.Contains(s.FuncId));


                    //添加当前角色菜单权限关系
                    foreach (var menuId in addRoleView.MenuIds.Split(','))
                    {
                        long tmpMenuId = Convert.ToInt64(menuId);
                        //当前菜单
                       var currentMenu= OperateContext.Current.BLLSession.IFunctionBLL.GetListBy(s => s.IsDeleted == false && s.Id == tmpMenuId).FirstOrDefault();
                        string menuPermName = $"{currentMenu.MenuName}|{currentRole.Name}|权限";
                        //当前菜单权限
                       var currentMenuPerm= OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.Name==menuPermName).FirstOrDefault();
                     
                        //如果对于这个菜单的菜单权限还不存在，添加一个菜单权限
                        if (currentMenuPerm==null)
                        {
                            Permission pm = new Permission();
                            pm.Name = $"{currentMenu.MenuName}|{currentRole.Name}|权限";
                            pm.GUID = Guid.NewGuid();
                            pm.ParentId = currentMenu.ParentId;
                            pm.PermValue = 1;
                            pm.Type = 1;//菜单权限
                            pm.Url = currentMenu.Url;
                            pm.CreateTime = DateTime.Now;
                            pm.Description = string.Empty;
                            pm.IsDeleted = false;
                           int addPermResult= OperateContext.Current.BLLSession.IPermissionBLL.Add(pm);
                            currentMenuPerm = addPermResult == 1 ? pm : currentMenuPerm;
                            state = addPermResult != 1 ? HolidaysWebConst.HolidaysWebConst.FAIL : HolidaysWebConst.HolidaysWebConst.SUCCESS;
                            msg = addPermResult != 1 ? "添加失败" : "添加成功";
                        }
                        //给当前用户添加角色
                        //todo:这里没有菜单权限，必须要先添加菜单权限，然后在添加到关系表中
                        //查看菜单权限关系表中是否有该记录
                      bool menuPermIsExist=  OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s => s.MemberId == currentRole.GUID && s.FuncId == currentMenuPerm.GUID).Count>0;
                        if (!menuPermIsExist)//如果不存在
                        {
                            FuncPermission fp= new FuncPermission();
                            fp.MemberId = currentRole.GUID;
                            fp.FuncId = currentMenuPerm.GUID;
                            fp.PermValue = 1;
                            fp.MemberType = (int)MemberEnum.MemberEnum.Role;
                            var addMiddleTableResult = OperateContext.Current.BLLSession.IFuncPermissionBLL.Add(fp);
                            if (addMiddleTableResult != 1)
                            {
                                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                                msg = "添加失败";
                            }
                        }
                       
                       
                    }
                }
            }
            return OperateContext.Current.RedirectAjax(state, msg, null, null);
        }
        /// <summary>
        /// 根据Id获取角色信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult QueryRoleById(long Id)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            var msg = string.Empty;
            Expression<Func<Role, bool>> expression = h => h.IsDeleted == false && h.Id == Id;
            var role = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(expression).FirstOrDefault();
            if (role==null)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                msg = "未能查询到该角色";
            }
            //获取所有菜单
           var allMenu= OperateContext.Current.BLLSession.IFunctionBLL.GetListBy(s => s.IsDeleted == false);
         
            RolePermViewModel rolePermView = new RolePermViewModel();
            List<MenuFuncViewModel> menuLIst = new List<MenuFuncViewModel>();
            //获取所有菜单权限
            //var allMenuPerm = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false);
            foreach (var menu in allMenu)
            {
                MenuFuncViewModel mv = new MenuFuncViewModel();
                mv.Id = menu.Id;
                mv.MenuName = menu.MenuName;
                mv.Url = menu.Url;
                mv.ParentId = menu.ParentId;
                var menuPermName = $"{menu.MenuName}|{role.Name}|权限";
                var currentMenuPerm = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false && s.Name == menuPermName).FirstOrDefault();
                if (currentMenuPerm==null)
                {
                    mv.IsSelected = false;
                }
                else
                {
                    mv.IsSelected = OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s => s.MemberId == role.GUID && s.FuncId == currentMenuPerm.GUID&&s.PermValue==1).Count > 0;
                }
                menuLIst.Add(mv);
            }
            rolePermView.MenuFuncViewModels = menuLIst;
            rolePermView.Role = role;
            return OperateContext.Current.RedirectAjax(state, msg, rolePermView, null);
        }
        /// <summary>
        /// 删除单个角色
        /// </summary>
        /// <param name="Id">角色Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRole(long Id)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            #region 删除角色条件
            Expression<Func<Role, bool>> expression = h => h.Id == Id;
            #endregion
            int result = OperateContext.Current.BLLSession.IRoleBLL.DelBy(expression);
            if (result != 1)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
            }
            return OperateContext.Current.RedirectAjax(state, string.Empty, null, null);
        }
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="userIds">多角色Id字符串，用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchDeleteRoles(string roleIds)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            string msg = string.Empty;
            List<long> roleIdList = new List<long>();
            foreach (var userId in roleIds.Split(','))
            {
                if (string.IsNullOrEmpty(userId))
                {
                    continue;
                }
                roleIdList.Add(Convert.ToInt64(userId));
            }
            if (roleIdList.Count() <= 0)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                msg = "请选择要删除的角色！";
            }
            Expression<Func<User, bool>> whereExpression = h => h.IsDeleted == false;
            #region 删除角色条件

            foreach (var Id in roleIdList)
            {
                whereExpression.And(h => h.Id == Id);
            }

            #endregion
            int result = OperateContext.Current.BLLSession.IUserBLL.DelBy(whereExpression);
            if (result != 1)
            {
                state = HolidaysWebConst.HolidaysWebConst.FAIL;
                msg = "删除失败";
            }
            return OperateContext.Current.RedirectAjax(state, string.Empty, null, null);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoleList(string roleName, int page = 1, int row = 10)
        {
            var whereString = PredicateBuilder.True<Role>();
            #region 查询条件
            Expression<Func<Role, bool>> stateExpression = h => h.IsDeleted == false;
            whereString = whereString.And(stateExpression);
            if (!string.IsNullOrEmpty(roleName))
            {
                Expression<Func<Role, bool>> roleNameExpression = h => h.Name.Contains(roleName);
                whereString = whereString.And(roleNameExpression);
            }
            #endregion
            int rowCount = 0;
            IList<Role> roles = OperateContext.Current.BLLSession.IRoleBLL.GetPagedList(page, row, ref rowCount, whereString, m => m.Id);
            List<RolePermViewModel> rolePermViewList = new List<RolePermViewModel>();
            PageModel<Role> model = PageModel<Role>.GetPageModel(roles, page, rowCount, row);
            return new JsonResult { Data = model };
        }
        [HttpPost]
       public ActionResult GetRoleListWithSelected(string menuName)
        {
            List<RoleWithSelectedViewModel> rvList = new List<RoleWithSelectedViewModel>();
           var allRoles= OperateContext.Current.BLLSession.IRoleBLL.GetListBy(s => s.IsDeleted == false);
            if (allRoles.Count<=0)
            {
                rvList = null;
            }
            foreach (var role in allRoles)
            {
                RoleWithSelectedViewModel rv = new RoleWithSelectedViewModel();
                rv.RoleId = role.Id;
                rv.Name = role.Name;
                rv.Description = role.Description;
                rv.CreateTime = role.CreateTime;
                string menuPermName = $"{menuName}|{role.Name}|权限";
                var currentPerm = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false && s.Name == menuPermName).FirstOrDefault();
                bool isExist = false;
                if (currentPerm==null)
                {
                   isExist = false;
                }
                else
                {
                 isExist    = OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s => s.FuncId == currentPerm.GUID && s.MemberId == role.GUID&&s.PermValue==1).Count > 0;
                }
                //中间表中查询是否有该记录
           
                rv.IsSelected = isExist;
                rvList.Add(rv);
            }
            return new JsonResult { Data = rvList };
        }
        public ActionResult GetRoleListby()
        {
            var whereString = PredicateBuilder.True<Role>();
            #region 查询条件
            Expression<Func<Role, bool>> stateExpression = h => h.IsDeleted == false;
            whereString = whereString.And(stateExpression);
            #endregion
            int rowCount = 0;
            IList<Role> roles = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(whereString).ToList();
            return OperateContext.Current.RedirectAjax("", "", roles, null);
            //JsonSerializerSettings setting = new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    Formatting = Formatting.None
            //};

            //var ret = JsonConvert.SerializeObject(roles, setting);
        }
        #endregion
        #region 权限操作
        

        public ActionResult PermManagerView()
        {
            return View();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPermission(PermissionModel permissionMdeol)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            //验证传入的数据
            string msg = Validate.ValidateString(
                new CustomValidate() { FieldName = "权限名称", FieldValue = permissionMdeol.Name, MinLength = 1, MaxLength = 100, IsRequired = true },
                new CustomValidate() { FieldName = "权限描述", FieldValue = permissionMdeol.Description, MinLength = 1, MaxLength = 100, IsRequired = true }
                );
            if (msg == null)
            {
                Permission permission = new Permission()
                {
                    CreateTime = DateTime.Now,
                    Description = permissionMdeol.Description,
                    Name = permissionMdeol.Name,
                    Type = permissionMdeol.Type,
                    ParentId = 0
                };
                int addSuccess = OperateContext.Current.BLLSession.IPermissionBLL.Add(permission);
                if (addSuccess != 1)//添加失败
                {
                    state = HolidaysWebConst.HolidaysWebConst.FAIL;
                    msg = "添加失败";
                }
            }
            return OperateContext.Current.RedirectAjax(state, msg, null, null);
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuList(int pageIndex, int pageSize = 10, long Id = -1, string menuName = null)
        {
            string state = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            Expression<Func<Function, bool>> whereExpression = h => h.IsDeleted == false;
            if (Id != -1)
            {
                Expression<Func<Function, bool>> expression = h => h.Id == Id;
                whereExpression = whereExpression.And(expression);
            }
            if (menuName != null && menuName != string.Empty)
            {
                Expression<Func<Function, bool>> expression = h => h.MenuName == menuName;
                whereExpression = whereExpression.And(expression);
            }
            int totalCount = OperateContext.Current.BLLSession.IFunctionBLL.GetListBy(s => s.IsDeleted == false).Count;
            List<Function> functions = OperateContext.Current.BLLSession.IFunctionBLL.GetPagedList(pageIndex, pageSize, whereExpression, h => h.Id);
            PageModel<Function> page = PageModel<Function>.GetPageModel(functions, pageIndex, totalCount, pageSize);
         

            return new JsonResult() { Data = page };
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public ActionResult GetRoleList()
        {
            //获取所有角色
            Expression<Func<Role, bool>> roleExpression = h => h.IsDeleted == false;
            var roleList = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(roleExpression);
            List<RoleModel> list = new List<RoleModel>();
            foreach (var role in roleList)
            {
                RoleModel roleModel = new RoleModel();
                roleModel.Id = role.Id;
                roleModel.Name = role.Name;
                list.Add(roleModel);
            }
            return new JsonResult { Data = list };
        }
        /// <summary>
        /// 添加菜单权限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult AddMenuPermission(string roleIdsAndpermValues, long funcId)
        {
            bool addSuccess = true;
            List<long> roleList = new List<long>();
            List<int> permList = new List<int>();
            if (string.IsNullOrEmpty(roleIdsAndpermValues))
            {
                return OperateContext.Current.RedirectAjax(HolidaysWebConst.HolidaysWebConst.FAIL, "请先添加角色", null, null);
            }
            //获取角色Id和对应的权限值
            foreach (var data in roleIdsAndpermValues.Split(';'))
            {
                roleList.Add(Convert.ToInt64(data.Split(',')[0]));
                permList.Add(Convert.ToInt32(data.Split(',')[1]));
            }
            //获取选中菜单信息
            var func = OperateContext.Current.BLLSession.IFunctionBLL.GetListBy(h => h.IsDeleted == false && h.Id == funcId).FirstOrDefault();
            //获取所有的角色
          var allRoles=  OperateContext.Current.BLLSession.IRoleBLL.GetListBy(s => s.IsDeleted == false);
            //遍历所有角色
            foreach (var role in allRoles)
            {
                string menuPermName = $"{func.MenuName}|{role.Name}|权限";
                if (roleList.Contains(role.Id))//如果角色被选中，则添加权限，并且记录角色权限关系表中
                {
                    //查看当前权限以及中间表中记录是否已经存在
                    bool permIsExist = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false && s.Name == menuPermName).Count > 0;
                    Guid permGUID = Guid.NewGuid();
                    if (permIsExist)
                    {
                       permGUID= OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false && s.Name == menuPermName).FirstOrDefault().GUID;
                    }
                 bool middleTableIsExist=   OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s=>s.MemberId==role.GUID&&s.FuncId==permGUID).Count>0;
                    int permValue =(int) permList.FirstOrDefault();
                    bool changeIsPermValue =  OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s => s.MemberId == role.GUID && s.FuncId == permGUID&&s.PermValue!=permValue).Count>0;
                   
                  if (permIsExist==true&& middleTableIsExist==false)//中间表中记录不存在
                    {
                       
                      var currentPerm=  OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.Name == menuPermName).FirstOrDefault();
                          //给当前角色加上当前权限
                          FuncPermission fp = new FuncPermission();
                        fp.FuncId = currentPerm.GUID;
                        fp.MemberId = role.GUID;
                        fp.MemberType = (int)MemberEnum.MemberEnum.Role;
                        fp.PermValue = permList.FirstOrDefault();
                       var result= OperateContext.Current.BLLSession.IFuncPermissionBLL.Add(fp);
                        if (result!=1)
                        {
                            addSuccess = false;
                        }
                    }
                    else if (permIsExist && middleTableIsExist && changeIsPermValue)//改变的是权限值
                    {
                        var currentPerm = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false && s.Name == menuPermName).FirstOrDefault();
                        var currentFuncPerm = OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s => s.MemberId == role.GUID && s.FuncId == currentPerm.GUID).FirstOrDefault();
                        if (currentFuncPerm==null)
                        {
                            //向中间表插入数据
                            FuncPermission fp = new FuncPermission();
                            fp.FuncId = currentPerm.GUID;
                            fp.MemberId = role.GUID;
                            fp.MemberType = (int)MemberEnum.MemberEnum.Role;
                            fp.PermValue = permList.FirstOrDefault();
                            addSuccess = OperateContext.Current.BLLSession.IFuncPermissionBLL.Add(fp)==1;
                            currentFuncPerm = fp;
                        }
                        currentFuncPerm.PermValue = permList.FirstOrDefault();
                       var result= OperateContext.Current.BLLSession.IFuncPermissionBLL.Modify(currentFuncPerm, "PermValue");
                        if (result != 1)
                        {
                            addSuccess = false;
                        }
                    }
                  else  if (permIsExist && middleTableIsExist && !changeIsPermValue)
                    {
                        continue;
                    }
                    else
                    {
                        Permission perm = new Permission();
                        perm.GUID = Guid.NewGuid();
                        perm.Name = menuPermName;
                        perm.ParentId = 0;
                        perm.Type = 1;//菜单权限
                        perm.Url = func.Url;
                        perm.CreateTime = DateTime.Now;
                        perm.Description = string.Empty;
                        perm.PermValue = permList.FirstOrDefault();
                        perm.IsDeleted = false;
                        //添加权限
                        int addPermResult = OperateContext.Current.BLLSession.IPermissionBLL.Add(perm);
                        //向中间表插入数据
                        FuncPermission fp = new FuncPermission();
                        fp.FuncId = perm.GUID;
                        fp.MemberId = role.GUID;
                        fp.MemberType = (int)MemberEnum.MemberEnum.Role;
                        fp.PermValue = permList.FirstOrDefault();
                        int addMiddleTableResult = OperateContext.Current.BLLSession.IFuncPermissionBLL.Add(fp);
                        if (addPermResult != 1 || addMiddleTableResult != 1)
                        {
                            addSuccess = false;
                        }
                    }
                  
                }
                else//否则直接删除关于这些角色的中间表中的记录
                {
                    OperateContext.Current.BLLSession.IFuncPermissionBLL.DelBy(s => s.MemberId == role.GUID);
                }
            }
          
            if (!addSuccess)
            {
                return OperateContext.Current.RedirectAjax(HolidaysWebConst.HolidaysWebConst.FAIL, "添加失败", null, null);
            }
            return OperateContext.Current.RedirectAjax(HolidaysWebConst.HolidaysWebConst.SUCCESS, "添加成功", null, null);
        }
        #endregion

    }
}
