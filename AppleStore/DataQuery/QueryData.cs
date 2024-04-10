using AppleStore.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System;
using AppleStore.DataAcess;

namespace AppleStore.DataQuery
{
    public class QueryData
    {
        Func f = new Func();
        private static HttpClient httpClient = new HttpClient();
        private readonly ApplicationDbContext? _context;

        
        // Hàm Login trả về: -1: Tài khoản không có / 0: Sai mật khẩu / >=1: Đăng nhập thành công!
        public int Login(string email, string pass, out User user)
        {
            user = null;
            string pass_md5 = f.CreateMD5(pass);
            using (var dbContext = _context)
            {
                User x = _context.Users.SingleOrDefault(s => s.user_email == email);
                if (x == null) return -1;
                else
                {
                    if (x.user_password.Equals(pass_md5))
                    {
                        user = x;
                        return x.user_id;
                    }
                    else return 0;
                }
            }
        }

        public int Reg(string fullname, string email, string pass)
        {
            using (var dbContext = _context)
            {
                User x = dbContext.Users.SingleOrDefault(s => s.user_email == email);
                if (x == null)
                {
                    x = new User();
                    x.user_fullname = fullname;
                    x.user_email = email.ToLower();
                    x.user_password = f.CreateMD5(pass);
                    x.user_created_at = DateTime.Now;
                    dbContext.Users.Add(x);
                    dbContext.SaveChanges();
                    return x.user_id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool ChangePass(int user_id, string pass_old, string pass_new, out string err)
        {
            err = string.Empty;
            try
            {
                string pass_md5 = f.CreateMD5(pass_old);
                string pass_md5_ = f.CreateMD5(pass_new);
                using (var dbContext = _context)
                {
                    User x = dbContext.Users.SingleOrDefault(s => s.user_id == user_id);
                    if (x == null)
                    {
                        err = "Tài khoản không tìm thấy!";
                        return false;
                    }
                    else
                    {
                        if (x.user_password.Equals(pass_md5))
                        {
                            x.user_password = pass_md5_;
                            dbContext.SaveChanges();
                            return true;
                        }
                        else
                        {
                            err = "Mật khẩu cũ không chính xác!";
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                return false;
            }
        }

        public User GetUser(int user_id)
        {
            using (var dbContext = _context)
            {
                User x = dbContext.Users.SingleOrDefault(s => s.user_id == user_id);
                return x;
            }
        }
        public List<User> GetUsers()
        {
            using (var dbContext = _context)
            {
                return dbContext.Users.OrderByDescending(o => o.user_id).ToList();
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                using (var dbContext = _context)
                {
                    var userUpdate = dbContext.Users.SingleOrDefault(s => s.user_id == user.user_id);
                    if (userUpdate == null)
                    {
                        return false;
                    }
                    if (userUpdate.user_fullname != user.user_fullname) userUpdate.user_fullname = user.user_fullname;
                    //if (userUpdate.user_email != user.user_email) userUpdate.user_email = user.user_email;
                    if (userUpdate.user_phone_number != user.user_phone_number) userUpdate.user_phone_number = user.user_phone_number;
                    if (userUpdate.user_address != user.user_address) userUpdate.user_address = user.user_address;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
             