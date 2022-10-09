using System.Collections.Generic;
using System.Linq;
using Biblioteca.Models;

namespace projeto_biblioteca_inicial.Models
{
    public class UsuarioService
    {
        public List<Usuario> Listar()
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.ToList();
            }
        }


        //LISTAR PELA ID
        public Usuario Listar(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }
        public void Inserir(Usuario usuario)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Add(usuario);
                bc.SaveChanges();
            }
        }
        public void Editar(Usuario usuario)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario usuarios = bc.Usuarios.Find(usuario.Id);

                usuarios.Nome = usuarios.Nome;
                usuarios.Login = usuarios.Login;
                if (usuarios.Senha != usuario.Senha)
                {
                    usuario.Senha = Criptografia.TextoCriptografado(usuario.Senha);
                }
                else
                {
                    usuarios.Senha = usuarios.Senha;
                }
                usuarios.Tipo = usuarios.Tipo;

                bc.SaveChanges();
            }
        }
        public void Excluir(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Remove(bc.Usuarios.Find(id));
                bc.SaveChanges();
            }
        }
    }
}