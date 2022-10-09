using System.Collections.Generic;
using System.Linq;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projeto_biblioteca_inicial.Models;

namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {
            if (string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        //CRIAR UMA FUNÇÃO PARA VERIFICAR SE O USUÁRIO E A SENHA DIGITADAS ESTÃO NO BANCO DE DADOS
        public static bool verificaLoginSenha(string Login, string senha, Controller controller)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {

                verificaSeUsuarioAdminExiste();

                senha = Criptografia.TextoCriptografado(senha);

                IQueryable<Usuario> UsuarioEncontrado = bc.Usuarios.Where(u => u.Login == Login && u.Senha == senha);

                List<Usuario> ListaUsuarios = UsuarioEncontrado.ToList();

                if (ListaUsuarios.Count == 0)
                {
                    return false;
                }
                else
                {
                    controller.HttpContext.Session.SetString("Login", ListaUsuarios[0].Login);
                    controller.HttpContext.Session.SetInt32("Tipo", ListaUsuarios[0].Tipo);
                    return true;
                }

            }

        }

        public static void verificaSeUsuarioEAdmin(Controller controller)
        {
            if (!(controller.HttpContext.Session.GetInt32("Tipo") == Usuario.ADMIN))
            {
                //REDIRECIONAMENTO DE PÁGINA, CRIAR PÁGINA AVISANDO QUE É NECESSÁRIO LOGIN DE
                //ADMINISTRADOR PARA APLICAR ALTERAÇÕES E COLOCAR UM BOTÃO QUE REDIRECIONAMENTO
                //PARA A PÁGINA DE LOGIN
                controller.Request.HttpContext.Response.Redirect("/Usuario/Permissao");
            }
        }

        public static void verificaSeUsuarioAdminExiste()
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> userAdmin = bc.Usuarios.Where(u => u.Login == "admin");

                if (userAdmin.ToList().Count == 0)
                {
                    Usuario novoAdm = new Usuario();
                    novoAdm.Nome = "Administrador";
                    novoAdm.Login = "admin";
                    novoAdm.Senha = Criptografia.TextoCriptografado("123");
                    novoAdm.Tipo = Usuario.ADMIN;

                    bc.Usuarios.Add(novoAdm);
                    bc.SaveChanges();

                }

            }
        }
    }
}