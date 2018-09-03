using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using MemberApi.Models;

namespace MemberApi.Controllers
{
    public class MemberController : Controller
    {
        string Sqlcon = @"Server=(localdb)\Member; DataBase=Member ;Integrated Security=SSPI";

        // 首頁
        public ActionResult Index()
        {


            return View();
        }
        
        //jquery.datatables資料來源
        public ActionResult GetDataTable(string name)
        {
            var Sqlstr = @"select * from Member";
           
            List<MemberID> MemberDate;
          
            using (SqlConnection con = new SqlConnection(Sqlcon))
            {
                if (name != string.Empty)
                {
                    Sqlstr += " where name like N'%" + name + "%'";
                }
                MemberDate = con.Query<MemberID>(Sqlstr).ToList();
            }
        
            return Json(new { data = MemberDate, JsonRequestBehavior.AllowGet });
        }

        //建立頁
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Member Member)
        {
            string valstr = string.Empty;

            #region 為了不寫死之後只改MODEL
            var MemberType = Member.GetType();

            foreach (var item in MemberType.GetProperties())
            {
                valstr += ("@" + item.Name + ",");
            }
            valstr = valstr.TrimEnd(',');
            #endregion
            var strSql = @"INSERT INTO MEMBER VALUES (" + valstr + ")";

            using (SqlConnection con = new SqlConnection(Sqlcon))
            {
                con.Execute(strSql, Member);
            }

            return RedirectToAction("Index");
        }

        //修改頁
        public ActionResult Update(int ID)
        {
            string strSql = "SELECT * from MEMBER WHERE ID=" + ID;
            List<MemberID> MemberDate;
            using (SqlConnection con = new SqlConnection(Sqlcon))
            {
                MemberDate = con.Query<MemberID>(strSql).ToList();
            }
            return View(MemberDate);
        }

        [HttpPost]
        public ActionResult Update(MemberID MemberID)
        {
            string valstr = string.Empty;
            #region 為了不寫死之後只改MODEL
            var MemberType = MemberID.GetType();

            foreach (var item in MemberType.GetProperties())
            {
                //因為ID是Identity  key值 要排除
                if (item.Name != "ID")
                {
                    valstr += item.Name + "=@" + item.Name + ",";
                }               
            }
            valstr = valstr.TrimEnd(',');
            #endregion
            //這邊有空格 因為是語法 所以要空出來 EX:set後 跟 where 前
            var strSql = @"update Member set " + valstr + " where ID = @ID";
            using (SqlConnection con = new SqlConnection(Sqlcon))
            {
                con.Execute(strSql, MemberID);
            }

            return RedirectToAction("Index");
        }

        //刪除(並沒有頁面刪除完直接做跳轉)
        public ActionResult Delete(int ID)
        {
            string strSql = "DELETE MEMBER WHERE ID=" + ID;
            using (SqlConnection con = new SqlConnection(Sqlcon))
            {
                con.Execute(strSql);
            }
            return RedirectToAction("Index");
        }
    }
}