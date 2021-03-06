﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyTeam.Utils;
using MyTeam.Models;
using PagedList;

namespace MyTeam.Controllers
{
    /// <summary>
    /// 系统管理Controller
    /// </summary>
    public class AttendanceManageController : BaseController
    {
        //
        // GET: /AttendanceManage/

        public ActionResult Index(int pageNum = 1)
        {
            if (this.GetSessionCurrentUser() == null)
            {
                return RedirectToAction("Login", "User", new { ReturnUrl = "/AttendanceManage" });
            }
            // 分页
            var ls = dbContext.Attendances.OrderByDescending(a => a.AID).ToPagedList(pageNum, Constants.PAGE_SIZE);
            return View(ls);
        }

        //
        // GET: /AttendanceManage/Create

        public ActionResult Create()
        {
            SelectList sl = new SelectList(this.GetUserList().Where(a => a.UserType == 2), "UID", "Realname");

            ViewBag.PersonList = sl;

            return View();
        }

        //
        // POST: /AttendanceManage/Create

        [HttpPost]
        public string Create(Attendance attendance)
        {
            try
            {
                dbContext.Attendances.Add(attendance);
                dbContext.SaveChanges();

                return Constants.AJAX_CREATE_SUCCESS_RETURN;

            }
            catch (Exception e1)
            {
                return "<p class='alert alert-danger'>出错了: " + e1.Message + "</p>";
            }

        }

        //
        // GET: /AttendanceManage/Edit/5

        public ActionResult Edit(int id)
        {
            Attendance attendance = dbContext.Attendances.ToList().Find(a => a.AID == id);

            if (attendance == null)
            {
                return View();
            }

            // 用户列表
            SelectList sl = new SelectList(this.GetUserList().Where(a => a.UserType == 2), "UID", "Realname", attendance.PersonID); // 选中当前值

            ViewBag.PersonList = sl;

            return View(attendance);
        }

        //
        // POST: /AttendanceManage/Edit/5

        [HttpPost]
        public string Edit(Attendance attendance)
        {
            try
            {
                dbContext.Entry(attendance).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();

                return Constants.AJAX_EDIT_SUCCESS_RETURN;
            }
            catch (Exception e1)
            {
                return "<p class='alert alert-danger'>出错了: " + e1.Message + "</p>";
            }
        }

        // AJAX调用
        // POST: /AttendanceManage/Delete/5
        [HttpPost]
        public string Delete(int id)
        {
            try
            {
                Attendance sys = dbContext.Attendances.ToList().Find(a => a.AID == id);
                dbContext.Entry(sys).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();

                return "删除成功";
            }
            catch (Exception e1)
            {
                return "出错了: " + e1.Message;
            }
        }

        // 导出excel
        [HttpGet]
        public ActionResult Export()
        {
            // 转换格式
            List<AttendanceResult> r = new List<AttendanceResult>();
            foreach(var a in dbContext.Attendances.ToList<Attendance>() )
            {
                r.Add(new AttendanceResult { LeaveDate = a.LeaveDate.ToString("yyyy/M/d"), PersonName = a.PersonName, LeaveDays = a.LeaveDays });
            }
            return this.MakeExcel<AttendanceResult>("AttendanceReportT", "外协人员考勤统计表＿" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                r);
        }
    }


}
