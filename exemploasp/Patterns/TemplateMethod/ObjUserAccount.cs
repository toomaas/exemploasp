using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.InteractDB;
using exemploasp.Models;

namespace exemploasp.Patterns.TemplateMethod
{
    class ObjUserAccount : ObjetoMuseu
    {
        private UserAccount userAccount { get; set; }

        public override string Validar()
        {
            OurDBContext db = new OurDBContext();
            DateTime data18 = DateTime.Now;
            data18 = data18.AddYears(-18);
            if (userAccount.Idade < data18)
            {
                if (!db.UserAccount.Any(n => n.Email == userAccount.Email))
                {
                    return null;
                }
                    return "Email já existente.";
            }
            return "Idade não permitida.";
        }

        public override void SalvarBd(OurDBContext db)
        {
            MuseuInteractDb museuDB = new MuseuInteractDb();
            var encrypt = museuDB.Encrypt(userAccount.Password);
            userAccount.Password = encrypt;
            encrypt = museuDB.Encrypt(userAccount.ConfirmPassword);
            userAccount.ConfirmPassword = encrypt;
            db.UserAccount.Add(userAccount);
            db.SaveChanges();
        }

        public ObjUserAccount(object objeto)
        {
            userAccount = (UserAccount)objeto;
        }
    }
}